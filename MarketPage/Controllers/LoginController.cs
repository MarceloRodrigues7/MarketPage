using MarketPage.Context;
using MarketPage.Models;
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

        [Authorize]
        public IActionResult Endereco()
        {
            using (var context = new ContextEF())
            {
                var data = context.EnderecosUsuario.Where(e => e.IdUsuario == int.Parse(User.Identity.Name)).FirstOrDefault();
                return View(data);
            };
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

        public IActionResult PostEndereco(Endereco endereco)
        {
            try
            {
                using (var context = new ContextEF())
                {
                    endereco.IdUsuario = int.Parse(User.Identity.Name);
                    var res = context.EnderecosUsuario.Where(u => u.IdUsuario == endereco.IdUsuario).FirstOrDefault();
                    if (res==null)
                    {
                        context.EnderecosUsuario.Add(endereco);
                    }
                    else
                    {
                        res.Pais = endereco.Pais;
                        res.Estado = endereco.Estado;
                        res.Cidade = endereco.Cidade;
                        res.Bairro = endereco.Bairro;
                        res.Numero = endereco.Numero;
                        context.EnderecosUsuario.Update(res);
                    }
                    context.SaveChanges();
                    return RedirectToAction("Usuario");
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TempData["Message"] = "Ocorreu algum erro, tente novamente! " + ex.Message;
                return View("Endereco");
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

        [Authorize]
        public IActionResult Usuario()
        {
            if (User.IsInRole("Usuario_Comum") || User.IsInRole("Usuario_Admin"))
            {
                using (var context = new ContextEF())
                {
                    var data = context.Usuarios.Where(u => u.Id == int.Parse(User.Identity.Name)).First();
                    ViewBag.Endereco = context.EnderecosUsuario.Where(e => e.IdUsuario == int.Parse(User.Identity.Name)).FirstOrDefault();
                    return View(data);
                };
            }
            return RedirectToAction("Index", "Home");
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

        [Authorize]
        public IActionResult Carrinho()
        {
            var carrinho = GetItensCarrinho();
            return View(carrinho);
        }

        public IActionResult PostItemCarrinho(ItemViewProduto item)
        {
            using (var context = new ContextEF())
            {
                var i = new Carrinho
                {
                    IdItem = item.Id,
                    IdUsuario = int.Parse(User.Identity.Name),
                    Quantidade = item.Quantidade,
                    Tamanhos = item.Tamanhos,
                    Valor = item.Valor,
                    DataHora = DateTime.Now
                };
                context.CarrinhoItem.Add(i);
                context.SaveChanges();
                return RedirectToAction("Carrinho");
            };
        }

        public IActionResult DeleteItemCarrinho(long item)
        {
            using (var context = new ContextEF())
            {
                context.CarrinhoItem.Remove(context.CarrinhoItem.Where(c => c.Id == item).First());
                context.SaveChanges();
                return RedirectToAction("Carrinho");
            };
        }

        public List<ItemViewProduto> GetItensCarrinho()
        {
            var data = new List<Carrinho>();
            var lista = new List<ItemViewProduto>();

            using (var context = new ContextEF())
            {
                data = context.CarrinhoItem.Where(c => c.IdUsuario == int.Parse(User.Identity.Name)).ToList();
                foreach (var item in data)
                {
                    lista.Add(new ItemViewProduto
                    {
                        Id = item.Id,
                        Nome = context.Itens.Where(i => i.Id == item.IdItem).First().Nome,
                        Descricao = context.Itens.Where(i => i.Id == item.IdItem).First().Descricao,
                        Valor = item.Valor,
                        Tamanhos = item.Tamanhos,
                        Quantidade = item.Quantidade,
                        Img = context.ImagensItem.Where(i => i.IdItem == item.IdItem).First().Img
                    });
                }
            };
            return lista;
        }
    }
}