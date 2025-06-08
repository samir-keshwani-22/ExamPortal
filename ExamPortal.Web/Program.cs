using System.Net;
using System.Text;
using ExamPortal.BusinessLogic.Implementations;
using ExamPortal.BusinessLogic.Interfaces;
using ExamPortal.DataAccess.DataContext;
using ExamPortal.DataAccess.Implementations;
using ExamPortal.DataAccess.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var webHostEnviroment = builder.Services.BuildServiceProvider().GetRequiredService<IWebHostEnvironment>();

// var webHostEnviroment = builder.Environment;

var conn = builder.Configuration.GetConnectionString("my_connection");
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));


builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IExaminationService, ExaminationService>();


// i need to pass the root folder path so used this way injecion

builder.Services.AddScoped<IEmailService>(provider =>
    new EmailService(provider.GetRequiredService<IConfiguration>(), webHostEnviroment.WebRootPath));

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddDbContext<ExamPortalContext>(q => q.UseNpgsql(conn));

var key = Encoding.ASCII.GetBytes(builder.Configuration["JwtSettings:SecretKey"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };

    // Read token from cookie
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            context.Token = context.Request.Cookies["AuthToken"];
            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            if (context.Response.StatusCode == (int)HttpStatusCode.Forbidden)
            {
                context.Response.Redirect("/Error/AccessDeniedPage");
            }
            return Task.CompletedTask;
        }
    };
});


builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new ResponseCacheAttribute
    {
        NoStore = true,
        Location = ResponseCacheLocation.None,
    });
});


builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});


builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
});

var app = builder.Build();


app.UseStatusCodePages(async context =>
            {
                var response = context.HttpContext.Response;

                if (response.StatusCode == (int)HttpStatusCode.Unauthorized)
                    response.Redirect("/Account/Login");
                else if (response.StatusCode == (int)HttpStatusCode.Forbidden)
                    response.Redirect("/Error/AccessDeniedPage");
            });


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
