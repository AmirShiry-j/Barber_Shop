using Application.Interfaces.Contexts;
using Domain.Users;
using Infrastructure.EmailService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Persistence.Contexts;
using WebApi.Tools.PersianError;

var builder = WebApplication.CreateBuilder(args);

IConfiguration Configuration = builder.Configuration;

// Add services to the container.

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

////Services of DB
//Db service
builder.Services.AddScoped<IDataBaseContext, DataBaseContext>();

//Service email
builder.Services.AddScoped<IEmailService, EmailService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
