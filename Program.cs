using Agenda.Models.Domain;
using Agenda.Service.Impl;
using Agenda.Service.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CalendarEvents.Models;

var builder = WebApplication.CreateBuilder(args);

// agregue las configuraciones para la conexion y autenticacion
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("conn"))); 
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<DatabaseContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options => options.LoginPath = "/UserAuthentication/Login");

// Agregue los servicios y el IFileService para cargar la imagen
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();
builder.Services.AddScoped<ICalendarEventRepository, CalendarEventRepository>();
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseStatusCodePagesWithReExecute("/Home/Error/{0}"); //agregue pero no es necesario
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication(); //agregue la autenticacion
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
