using ProniaNihat.DAL;
using ProniaNihat.Interfaces;
using ProniaNihat.Middlewares;
using ProniaNihat.Models;
using ProniaNihat.Services;
using ProniaNihat.ViewComponents;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(opt =>opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));







builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 7;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;

    options.User.RequireUniqueEmail = true;

    options.Lockout.MaxFailedAccessAttempts = 3;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);

}
   ).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders() ;


builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<LayoutService>();
builder.Services.AddScoped<HeaderViewComponent>();
builder.Services.AddScoped<IEmailService, EmailService>();
var app = builder.Build();


app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.UseStaticFiles();
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.MapControllerRoute(
    "default",
    "{area:exists}/{controller=home}/{action=index}/{id?}"
    );
app.MapControllerRoute(
    "default",
    "{controller=home}/{action=index}/{id?}"
    );

app.Run();

