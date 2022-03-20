using MarketPage.Context;
using MarketPage.Models;
using MarketPage.Repository;
using MarketPage.Services;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
using MercadoPago.Resource.Preference;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IEnderecoRepository _enderecoRepository;

        public LoginController(IUsuarioRepository usuarioRepository, IFreteRepository freteRepository, IEnderecoRepository enderecoRepository)
        {
            _usuarioRepository = usuarioRepository;
            _enderecoRepository = enderecoRepository;
        }

        public IActionResult Index()
        {
            if (User.IsInRole("Usuario_Comum") || User.IsInRole("Admin"))
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

        public IActionResult EsqueciMinhaSenha()
        {
            return View();
        }

        [Authorize]
        public IActionResult Endereco()
        {
            var data = _enderecoRepository.GetEndereco(int.Parse(User.Identity.Name));
            return View(data);
        }

        [Authorize]
        public IActionResult Usuario()
        {
            if (User.IsInRole("Usuario_Comum") || User.IsInRole("Admin"))
            {
                var data = _usuarioRepository.GetUsuario(int.Parse(User.Identity.Name));
                ViewBag.Endereco = _enderecoRepository.GetEndereco(int.Parse(User.Identity.Name));
                return View(data);
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult PostUsuario(Usuario usuario)
        {
            try
            {
                if (!_usuarioRepository.ValidaNovoUsuario(usuario.Username))
                {
                    usuario.Telefone = usuario.Telefone.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
                    usuario.StatusAtivo = true;
                    usuario.RoleAcess = "Usuario_Comum";
                    _usuarioRepository.PostUsuario(usuario);
                    return View("Index");
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

        public IActionResult PostEndereco(Endereco endereco)
        {
            try
            {
                var validaCep = new Endereco().ValidaCEP(endereco.Cep);
                if (validaCep)
                {
                    endereco.Cep = new Endereco().FormataCEP(endereco.Cep);
                    endereco.IdUsuario = int.Parse(User.Identity.Name);
                    _enderecoRepository.InsertOrUpdate(endereco);
                    return RedirectToAction("Usuario");
                }
                else
                {
                    TempData["Message"] = "Cep inválido, tente novamente!";
                    return RedirectToAction("Endereco");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TempData["Message"] = "Ocorreu algum erro, tente novamente! " + ex.Message;
                return RedirectToAction("Usuario");
            }
        }

        public IActionResult AutenticacaoUsuario(Usuario usuario)
        {
            try
            {
                var res = _usuarioRepository.GetUsuario(usuario.Username, usuario.Password);
                if (res != null)
                {
                    GeraIdentity(res);
                    return RedirectToAction("Index", "Home");
                }
                TempData["Message"] = "Login Falhou. Username ou Senha inválido";
                return View("Index");
            }
            catch (Exception e)
            {
                TempData["Message"] = "Ocorreu algum erro, tente novamente! " + e.Message;
                return View("Index");
            }
        }

        [Authorize]
        public IActionResult Editar()
        {
            if (User.IsInRole("Usuario_Comum") || User.IsInRole("Admin"))
            {
                var data = _usuarioRepository.GetUsuario(int.Parse(User.Identity.Name));
                return View(data);
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult PutUsuario(Usuario usuario)
        {
            _usuarioRepository.PutUsuario(usuario);
            return RedirectToAction("Usuario");
        }

        public IActionResult ResetarSenha(Usuario usuario)
        {
            usuario.Telefone = usuario.Telefone.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
            var data = _usuarioRepository.GetUsuario(usuario.Username, usuario.Email, usuario.Telefone);
            if (data == null)
            {
                TempData["Message"] = "Informações de cadastro não localizadas. Tente novamente.";
                return RedirectToAction("EsqueciMinhaSenha");
            }
            data.Password = null;
            return View(data);
        }

        public IActionResult PostNovaSenha(Usuario usuario)
        {
            var data = _usuarioRepository.GetUsuario(usuario.Id);
            data.Password = usuario.Password;
            try
            {
                _usuarioRepository.PutUsuario(data);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["Message"] = "Erro ao alterar nova senha. Tente novamente.";
                return RedirectToAction("EsqueciMinhaSenha");
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
    }
}