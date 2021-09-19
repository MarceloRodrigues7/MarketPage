using MarketPage.Context;
using MarketPage.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MarketPage.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            if (User.IsInRole("Usuario_Comum") || User.IsInRole("Usuario_Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public IActionResult Cadastrar()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }

        public IActionResult PostUsuario(Usuario usuario)
        {
            try
            {
                if (ValidaNovoUsuario(usuario.Username))
                {
                    using (var context = new ContextEF())
                    {
                        usuario.StatusAtivo = true;
                        usuario.RoleAcess = "Usuario_Comum";
                        context.Usuarios.Add(usuario);
                        context.SaveChanges();
                        return View("Index");
                    };
                }
                TempData["Message"] = "Username já cadastrado, tente novamente!";
                return View("Cadastrar");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TempData["Message"] = "Ocorreu algum erro, tente novamente! " + ex.Message;
                return View("Cadastrar");
            }
        }

        public IActionResult AutenticacaoUsuario(Usuario usuario)
        {
            try
            {
                using (var context = new ContextEF())
                {
                    var res = context.Usuarios.Where(u => u.Username == usuario.Username && u.Password == usuario.Password).FirstOrDefault();
                    if (res != null)
                    {
                        GeraIdentity(res);
                        return RedirectToAction("Index", "Home");
                    }
                };
                TempData["Message"] = "Login Falhou. Username ou Senha inválido";
                return View("Index");
            }
            catch (Exception e)
            {
                TempData["Message"] = "Ocorreu algum erro, tente novamente! " + e.Message;
                return View("Index");
            }
        }

        private void GeraIdentity(Usuario usuario)
        {
            var claims = new List<Claim> { new(ClaimTypes.Name, usuario.Id.ToString()), new(ClaimTypes.Role, usuario.RoleAcess) };        
            var identidadeDeUsuario = new ClaimsIdentity(claims, "Login");
            ClaimsPrincipal claimPrincipal = new(identidadeDeUsuario);
            var propriedadesDeAutenticacao = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTime.Now.ToLocalTime().AddHours(2),
                IsPersistent = true,
            };
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimPrincipal, propriedadesDeAutenticacao);
        }

        private static bool ValidaNovoUsuario(string username)
        {
            using (var context = new ContextEF())
            {
                return context.Usuarios.Where(l => l.Username == username).Any();
            };
        }
    }
}
