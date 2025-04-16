using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Practica20250411.AppMVCCore.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<Practica220250411DbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Conn"));
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie((o) =>
{
    o.LoginPath = new PathString("/Usuarios/login");//RUTA DONDE VA DIRECCIONAR LA APLICACION CUANDO UN USUARIO INTENTA ACCEDER A UNA ACCION DE UN CONTROLADOR DONDE NO TIENE PERMISO
    o.AccessDeniedPath = new PathString("/Usuarios/login");
    o.ExpireTimeSpan = TimeSpan.FromHours(8);//TIEMPO QUE DURA LA SESION(PARA EL CASO 8 HORAS)
    o.SlidingExpiration = true;// SIRVE PARA ACTUALIZAR LAS 8 HORAS A PARTIR DEL ULTIMO USO EN EL SISTEMA
    o.Cookie.HttpOnly = true;
});

var app = builder.Build();

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
