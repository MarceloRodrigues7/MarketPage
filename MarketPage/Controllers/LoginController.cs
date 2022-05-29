using ADO;
using MarketPage.Repository;
using MarketPage.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MarketPage.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IEnderecoRepository _enderecoRepository;
        private readonly AuthService _authService;

        public LoginController(IUsuarioRepository usuarioRepository, IEnderecoRepository enderecoRepository)
        {
            _usuarioRepository = usuarioRepository;
            _enderecoRepository = enderecoRepository;

            _authService = new AuthService();
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
                TempData["Message"] = "Ocorreu algum erro, tente novamente! " + ex.Message;
                return View("Cadastrar");
            }
        }

        public IActionResult AutenticacaoUsuario(Usuario usuario)
        {
            try
            {
                usuario = _usuarioRepository.GetUsuario(usuario.Username, usuario.Password);
                if (usuario != null)
                {
                    _authService.GeraIdentity(usuario, HttpContext);
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

        [Authorize]
        public IActionResult Endereco()
        {
            var data = _enderecoRepository.GetEndereco(int.Parse(User.Identity.Name));
            return View(data);
        }

        [Authorize]
        public IActionResult Usuario()
        {
            var idUsuario = int.Parse(User.Identity.Name);
            var data = _usuarioRepository.GetUsuario(idUsuario);
            ViewBag.Endereco = _enderecoRepository.GetEndereco(idUsuario);
            return View(data);
        }

        [Authorize]
        public IActionResult PostEndereco(Endereco endereco)
        {
            try
            {
                var validaCep = ADO.Endereco.ValidaCEP(endereco.Cep);
                if (validaCep)
                {
                    endereco.Cep = ADO.Endereco.FormataCEP(endereco.Cep);
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
                TempData["Message"] = "Ocorreu algum erro, tente novamente! " + ex.Message;
                return RedirectToAction("Usuario");
            }
        }

        [Authorize]
        public IActionResult Editar()
        {
            var data = _usuarioRepository.GetUsuario(int.Parse(User.Identity.Name));
            return View(data);
        }

        [Authorize]
        public IActionResult PutUsuario(Usuario usuario)
        {
            _usuarioRepository.PutUsuario(usuario);
            return RedirectToAction("Usuario");
        }
    }
}