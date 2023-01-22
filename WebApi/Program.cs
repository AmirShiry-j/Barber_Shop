using Application.AddressesService.Query;
using Application.CustomerService.Command;
using Application.Interfaces.Contexts;
using Application.ProfileService.Command;
using Application.ProfileService.Query;
using Application.SalonsService.Command;
using Application.SalonsService.Query;
using Application.TokenService;
using Application.UserService;
using Domain.Users;
using ExceptionHandling;
using Infrastructure.EmailService;
using Infrastructure.MappingProfile;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog.Web;
using Persistence.Contexts;
using System.Runtime;
using System.Text;
using WebApi.Helpers;
using WebApi.Tools.PersianError;
using WebApi.Tools.TokenValidator;

var builder = WebApplication.CreateBuilder(args);

//Nlog configs
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
builder.Host.UseNLog();

IConfiguration Configuration = builder.Configuration;

//Add CORS configs
//Get origins cores in appsetting
var corsOrigins = Configuration.GetSection("CorsOrigins").Get<string[]>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        b => b.WithOrigins(corsOrigins)
        .AllowAnyHeader()
        .AllowAnyMethod());
});


//Config controller service
builder.Services.AddControllers();

//Config DataBase
#region Connect To DataBase
string connection = Configuration["ConnectionStrings:SqlServer"];
builder.Services.AddDbContext<DataBaseContext>(options => options.UseSqlServer(connection));
#endregion

//Config Vesioning
builder.Services.AddApiVersioning(option =>
{
    option.ReportApiVersions = true;
});

//Config Identity and his option
builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<DataBaseContext>()
    .AddDefaultTokenProviders()
    .AddRoles<Role>()
    .AddErrorDescriber<PersianIdentityErrors>();

//Set Identity's Options
builder.Services.Configure<IdentityOptions>(options =>
{
    options.User.RequireUniqueEmail = true;

    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    //options.Password.RequiredUniqueChars = 6;
    options.Password.RequireUppercase = false;

    options.Lockout.MaxFailedAccessAttempts = 3;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);

    options.SignIn.RequireConfirmedEmail = true;
    options.SignIn.RequireConfirmedPhoneNumber = false;
    options.SignIn.RequireConfirmedAccount = true;
});

//Config Swagger
builder.Services.AddSwaggerGen(c =>
{
    //برای نمایش استرینگی Enum ها
    //c.DescribeAllEnumsAsStrings();

    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Barber Shop", Version = "v1" });

    //برای نمایش Description کنترلر ها
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "WebApi.Barber.xml"), true);

    //For configure Authentication in swaager Ui
    var security = new OpenApiSecurityScheme
    {
        Name = "JWT Auth",
        Description = "توکن را وارد کنید",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    c.AddSecurityDefinition(security.Reference.Id, security);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { security , new string[]{ } }
                });
});



//Config JWT Authenfication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(jwtConfig =>
{
    jwtConfig.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidIssuer = JwtInfo.Issuer,
        ValidAudience = JwtInfo.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtInfo.SecretKey)),
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true
    };
    jwtConfig.SaveToken = true;
    jwtConfig.Events = new JwtBearerEvents
    {
        OnTokenValidated = context =>
        {
            //Dependent service token validator
            var tokenValidatorService = context.HttpContext.RequestServices.GetRequiredService<ITokenValidator>();

            //Enable it
            return tokenValidatorService.Execute(context);
        }
    };
});

////Services of DB
//Db service
builder.Services.AddScoped<IDataBaseContext, DataBaseContext>();

//AuthoMapper Profile
builder.Services.AddAutoMapper(typeof(SalonMppingProfile));
builder.Services.AddAutoMapper(typeof(UserMappingProfile));




//Authorize and token services
builder.Services.AddScoped<IUserTokenService, UserTokenService>();
builder.Services.AddScoped<IUserAuthorizeService, UserAuthorizeService>();

//User services
builder.Services.AddScoped<IGetAllUserService, GetAllUserService>();
builder.Services.AddScoped<IGetProfileService, GetProfileService>();
builder.Services.AddScoped<IEditProfileService, EditProfileService>();


//Customer Service
builder.Services.AddScoped<IAddCustomerService, AddCustomerService>();

//Salon services
builder.Services.AddScoped<IAddSalonService, AddSalonService>();
builder.Services.AddScoped<IEditSalonService,EditSalonService>();
builder.Services.AddScoped<IDeleteSalonService, DeleteSalonService>();
builder.Services.AddScoped<IGetSalonByIdService, GetSalonByIdService>();
builder.Services.AddScoped<IGetAllSalonsService, GetAllSalonsService>();

//Address services
builder.Services.AddScoped<IGetUnitedsService, GetUnitedsService>();
builder.Services.AddScoped<IGetCitiesService, GetCitiesService>();

//Service email
builder.Services.AddScoped<IEmailService, EmailService>();
//Service Handler
builder.Services.AddSingleton<HandlerOptions>();



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Services of token validator
builder.Services.AddScoped<ITokenValidator, TokenValidator>();

var app = builder.Build();

//Swagger config
app.UseSwagger(c =>
{
    c.SerializeAsV2 = false;
});
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi v1");
    c.RoutePrefix = string.Empty;
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseDeveloperExceptionPage();
}
else
if (app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    //For takes exeption
    app.UseMiddleware<ExceptionHandlerMiddleware>();
}


app.UseHsts();
app.UseHttpsRedirection();

app.UseCors("CorsPolicy");
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
