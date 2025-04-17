using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Practica20250411.AppMVCCore.Models;
using Microsoft.AspNetCore.Authorization;

namespace Practica20250411.AppMVCCore.Controllers
{
    
    public class UsuariosController : Controller
    {
        private readonly Practica220250411DbContext _context;

        public UsuariosController(Practica220250411DbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "ADMINISTRADOR")]
        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            return View(await _context.Usuarios.ToListAsync());
        }
        [Authorize(Roles = "ADMINISTRADOR")]
        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }
        [Authorize(Roles = "ADMINISTRADOR")]
        // GET: Usuarios/Create
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "ADMINISTRADOR")]
        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Email,Password,Rol")] Usuario usuario)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    usuario.Password=CalcularHashMD5(usuario.Password);
                    _context.Add(usuario);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }


                return View(usuario);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("",ex.Message);
                return View(usuario);
            }


        }
        [Authorize(Roles = "ADMINISTRADOR")]
        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }
        [Authorize(Roles = "ADMINISTRADOR")]
        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Email,Rol")] Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            try
            {
                var usuarioData= await _context.Usuarios.FirstOrDefaultAsync(s => s.Id==usuario.Id);
                if (usuarioData != null)
                {
                    usuarioData.Email = usuario.Email;
                    usuarioData.Nombre = usuario.Nombre;
                    usuarioData.Rol=usuario.Rol;                 
                    _context.Update(usuarioData);
                    await _context.SaveChangesAsync();

                }

               
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(usuario.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
         
        }
        [Authorize(Roles = "ADMINISTRADOR")]
        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }
        [Authorize(Roles = "ADMINISTRADOR")]
        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }
        [Authorize (Roles ="ADMINISTRADOR,OPERARIO")]
        public async Task<IActionResult> ChancePassword()
        {
            int idUser = int.Parse(User.FindFirst("Id").Value);
            var usuario= await _context.Usuarios.FindAsync(idUser);
            if (usuario == null)
            {
                return NotFound();
            }           
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMINISTRADOR,OPERARIO")]
        public async Task<IActionResult> ChancePassword(int id, [Bind("Id,Password")] Usuario usuario, string passwordAnt)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            try
            {
                var passAnt= CalcularHashMD5(passwordAnt);
                var usuarioData = await _context.Usuarios.FirstOrDefaultAsync(s => s.Id == usuario.Id);
                if (usuarioData != null && usuarioData.Password == passAnt)
                {
                    usuarioData.Password = CalcularHashMD5(usuario.Password);
                    _context.Update(usuarioData);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Login");
                }
                else
                {
                    throw new Exception("El password anterior es incorrecto");
                }
            }
            catch (Exception ex)
            {
                
                {
                   ModelState.AddModelError("", ex.Message);
                    return View(usuario);
                }
                return RedirectToAction(nameof(Index));
            }
          

        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([Bind("Email,Password")] Usuario usuario)
        {
          
            try
            {
                usuario.Password =CalcularHashMD5(usuario.Password);
                var usuarioAuth = await _context.Usuarios.FirstOrDefaultAsync(s =>s.Email == usuario.Email && s.Password==usuario.Password);
                if (usuarioAuth != null && usuarioAuth.Id > 0)
                {
                    var claims = new[] {
                    new Claim(ClaimTypes.Name, usuarioAuth.Nombre),
                    new Claim("Id", usuarioAuth.Id.ToString()),
                     new Claim("Email", usuarioAuth.Email),
                    new Claim(ClaimTypes.Role, usuarioAuth.Rol)
                    };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
                    return RedirectToAction("Index","Home");
                }
                else
                {
                    throw new Exception("El email o contraseña son incorrectos");
                }


            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(usuario);
            }
       

        }
        [AllowAnonymous]
        public async Task<IActionResult> CerrarSession()
        { 
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
        [AllowAnonymous]
        public IActionResult Login()
        { 
        return View();
        }
        private string CalcularHashMD5(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2")); // "x2" convierte el byte en una cadena hexadecimal de dos caracteres.
                }
                return sb.ToString();
            }
        }
    }
}
