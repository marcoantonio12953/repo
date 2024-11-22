using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlataformaCursos.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace PlataformaCursos.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private readonly PlataformaCursosContext _context;

        public RolesController(PlataformaCursosContext context)
        {
            _context = context;
        }

        // Acción para mostrar la lista de usuarios con sus roles actuales
        public async Task<IActionResult> Index()
        {
            var usuarios = await _context.Usuarios.Include(u => u.Rol).ToListAsync();
            var roles = await _context.Roles.ToListAsync();
            ViewBag.Roles = roles;
            return View(usuarios);
        }

        // Acción para actualizar el rol de un usuario
        [HttpPost]
        public async Task<IActionResult> AsignarRol(int usuarioId, int rolId)
        {
            var usuario = await _context.Usuarios.FindAsync(usuarioId);
            if (usuario == null) return NotFound();

            usuario.RolId = rolId;
            _context.Update(usuario);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
