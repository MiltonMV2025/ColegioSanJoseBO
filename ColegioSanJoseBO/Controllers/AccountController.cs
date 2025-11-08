using ColegioSanJoseBO.Data;
using DASALUD.Helpers;
using DASALUD.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ColegioSanJoseBO.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            // Si ya está autenticado, redirigir al Home
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                string hashedPassword = PasswordHelper.HashPassword(model.Password);

                var login = await _context.Login
                    .Include(l => l.Teacher)
                    .FirstOrDefaultAsync(l => 
                        l.Username == model.Username 
                        && l.Password == hashedPassword 
                        && l.Status);

                if (login != null && login.Teacher != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, login.Id.ToString()),
                        new Claim(ClaimTypes.Name, login.Username),
                        new Claim(ClaimTypes.GivenName, login.Teacher.Name),
                        new Claim(ClaimTypes.Surname, login.Teacher.Surname),
                        new Claim("TeacherId", login.TeacherId.ToString() ?? "")
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    TempData["SuccessMessage"] = $"¡Bienvenido {login.Teacher.Name}!";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Usuario o contraseña incorrectos.");
                }
            }
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["InfoMessage"] = "Sesión cerrada correctamente.";
            return RedirectToAction("Login", "Account");
        }
    }
}
