using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlataformaCursos.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace PlataformaCursos.Controllers
{
    public class LoginController : Controller
    {
        private readonly PlataformaCursosContext _context;

        public LoginController(PlataformaCursosContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string nombreUsuario, string contraseña)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario && u.Contraseña == contraseña);

            if (usuario == null)
            {
                ViewBag.Error = "Nombre de usuario o contraseña incorrectos.";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.NombreUsuario),
                new Claim(ClaimTypes.Role, usuario.Rol.NombreRol)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties();

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            // Redirigir según el rol del usuario
            return usuario.Rol.NombreRol switch
            {
                "Admin" => RedirectToAction("Index", "Admin"),
                "Profesor" => RedirectToAction("Index", "Profesor"),
                "Alumno" => RedirectToAction("Index", "Alumno"),
                _ => RedirectToAction("Index", "Home")
            };
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string nombreUsuario, string contraseña)
        {
            // Verificar si el usuario ya existe
            if (_context.Usuarios.Any(u => u.NombreUsuario == nombreUsuario))
            {
                ViewBag.Error = "El nombre de usuario ya está en uso.";
                return View();
            }

            // Obtener el rol de alumno por defecto
            var rolAlumno = await _context.Roles.FirstOrDefaultAsync(r => r.NombreRol == "Alumno");
            if (rolAlumno == null)
            {
                ViewBag.Error = "El rol de Alumno no está configurado en el sistema.";
                return View();
            }

            // Crear un nuevo usuario con el rol de alumno
            var nuevoUsuario = new Usuario
            {
                NombreUsuario = nombreUsuario,
                Contraseña = contraseña,
                RolId = rolAlumno.RolId
            };

            _context.Usuarios.Add(nuevoUsuario);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Login");
        }
    }
}
