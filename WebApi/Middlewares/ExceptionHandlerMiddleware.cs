using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionHandling
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionHandlerMiddleware> logger;
        private readonly RequestDelegate exceptionHandler;
        private readonly IResult operationResult;
        private readonly long logMaxBodyLength;
        private readonly string overSizeBodyLengthMessage;

        public ExceptionHandlerMiddleware(HandlerOptions options, RequestDelegate next,
            ILogger<ExceptionHandlerMiddleware> logger)
        {
            logMaxBodyLength = options.LogMaxBodyLength;
            overSizeBodyLengthMessage = options.OverSizeBodyLengthMessage;
            operationResult = options.OperationResult;

            this.next = next;
            this.logger = logger;
        }

        public ExceptionHandlerMiddleware(RequestDelegate exceptionHandler, RequestDelegate next)
        {
            this.exceptionHandler = exceptionHandler ?? throw new ArgumentNullException($"{nameof(exceptionHandler)} cannot be null");
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                context.Request.EnableBuffering();
                await next(context);
            }
            catch (Exception e)
            {
                context.Features.Set<IExceptionHandlerPathFeature>(new ExceptionHandlerFeature { Error = e });
                await (exceptionHandler ?? HandleErrorAsync).Invoke(context);
            }
        }

        private async Task HandleErrorAsync(HttpContext context)
        {
            var error = context.Features.Get<IExceptionHandlerPathFeature>().Error;
            if (error == null)
                return;

            string body;
            if (context.Request.HasFormContentType)
            {
                var files = context.Request.Form.Files.Select(f =>
                    new KeyValuePair<string, string>($"{f.Name}(file)", f.FileName));
                var dict = new Dictionary<string, string>(files);
                foreach (var (k, v) in context.Request.Form)
                    dict[k] = v;
                body = JsonConvert.SerializeObject(dict);
            }
            else
            {
                context.Request.EnableBuffering();
                using var reader = new StreamReader(context.Request.Body, context.Request.ContentType == null
                    ? Encoding.UTF8
                    : new MediaType(context.Request.ContentType).Encoding);
                var request = await reader.ReadToEndAsync();
                context.Request.Body.Seek(0, SeekOrigin.Begin);
                body = request.Length > logMaxBodyLength ? overSizeBodyLengthMessage : request;
            }

            var log = JsonConvert.SerializeObject(new
            {
                Url = context.Request.GetEncodedUrl(),
                context.Request.Method,
                context.Request.Headers,
                context.Request.Cookies,
                context.Request.Query,
                Body = body
            });

            string message;
            const string logTemplate = "error:{0}\r\nrequest{1}";
            if (error is OException)
            {
                message = error.Message;
                //context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                logger.LogError(error, logTemplate, error.ToString(), log);
            }
            else
            {
                message = operationResult.ErrorMessage;
                //context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                logger.LogError(error, logTemplate, error.ToString(), log);
            }

            var viewResult = new ViewResult()
            {
                ViewName = "~/Views/Home/Error.cshtml"
            };

            //var viewDataDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(),
            //        new ModelStateDictionary());
            //viewDataDictionary.Model = //your model
            //viewResult.ViewData = viewDataDictionary;

            var executor = context.RequestServices
            .GetRequiredService<IActionResultExecutor<ViewResult>>();
            var routeData = context.GetRouteData() ?? new RouteData();
            var actionContext = new ActionContext(context, routeData,
            new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor());

            await executor.ExecuteAsync(actionContext, viewResult);

        }
    }
}
