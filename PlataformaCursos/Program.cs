using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PlataformaCursos.Models;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<PlataformaCursosContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PlataformaCursosConnection")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index";
        options.LogoutPath = "/Login/Logout";
    });

var app = builder.Build();

// Seed para roles y usuarios iniciales
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<PlataformaCursosContext>();
    context.Database.Migrate();

    // Crear los roles si no existen
    if (!context.Roles.Any(r => r.NombreRol == "Admin"))
    {
        context.Roles.Add(new Rol { NombreRol = "Admin" });
    }
    if (!context.Roles.Any(r => r.NombreRol == "Alumno"))
    {
        context.Roles.Add(new Rol { NombreRol = "Alumno" });
    }
    if (!context.Roles.Any(r => r.NombreRol == "Profesor"))
    {
        context.Roles.Add(new Rol { NombreRol = "Profesor" });
    }

    await context.SaveChangesAsync();

    // Crear un usuario administrador si no existe
    if (!context.Usuarios.Any(u => u.NombreUsuario == "admin"))
    {
        var rolAdmin = context.Roles.FirstOrDefault(r => r.NombreRol == "Admin");
        context.Usuarios.Add(new Usuario
        {
            NombreUsuario = "admin",
            Contraseña = "admin123",  // Contraseña por defecto, puedes cambiarla luego en el sistema
            RolId = rolAdmin.RolId
        });
    }

    // Crear un usuario profesor si no existe
    if (!context.Usuarios.Any(u => u.NombreUsuario == "profesor"))
    {
        var rolProfesor = context.Roles.FirstOrDefault(r => r.NombreRol == "Profesor");
        context.Usuarios.Add(new Usuario
        {
            NombreUsuario = "profesor",
            Contraseña = "profesor123",  // Contraseña por defecto para el profesor
            RolId = rolProfesor.RolId
        });
    }

    // Crear un usuario alumno si no existe
    if (!context.Usuarios.Any(u => u.NombreUsuario == "alumno"))
    {
        var rolAlumno = context.Roles.FirstOrDefault(r => r.NombreRol == "Alumno");
        context.Usuarios.Add(new Usuario
        {
            NombreUsuario = "alumno",
            Contraseña = "alumno123",  // Contraseña por defecto para el alumno
            RolId = rolAlumno.RolId
        });
    }

    await context.SaveChangesAsync();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
