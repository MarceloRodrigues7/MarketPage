using MarketPage.Context;
using MarketPage.Models;
using MarketPage.Repository;
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
        private readonly ICodPromocionalRepository _codPromocional;
        private readonly IFreteRepository _freteRepository;
        private readonly IEnderecoRepository _enderecoRepository;
        private readonly ICarrinhoRepository _carrinhoRepository;
        private readonly IPedidoRepository _pedidoRepository;

        public LoginController(IUsuarioRepository usuarioRepository, ICodPromocionalRepository codPromocional, IFreteRepository freteRepository, IEnderecoRepository enderecoRepository, ICarrinhoRepository carrinhoRepository, IPedidoRepository pedidoRepository)
        {
            _usuarioRepository = usuarioRepository;
            _codPromocional = codPromocional;
            _freteRepository = freteRepository;
            _enderecoRepository = enderecoRepository;
            _carrinhoRepository = carrinhoRepository;
            _pedidoRepository = pedidoRepository;
        }

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
            if (User.IsInRole("Usuario_Comum") || User.IsInRole("Usuario_Admin"))
            {
                var data = _usuarioRepository.GetUsuario(int.Parse(User.Identity.Name));
                ViewBag.Endereco = _enderecoRepository.GetEndereco(int.Parse(User.Identity.Name));
                return View(data);
            }
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public IActionResult Carrinho()
        {
            var enderecoUsuario = _enderecoRepository.GetEndereco(int.Parse(User.Identity.Name));
            if (enderecoUsuario == null)
            {
                TempData["Message"] = "Registre o endereço de destino";
                return RedirectToAction("Endereco");
            }
            var carrinho = _carrinhoRepository.GetItensCarrinhoView(int.Parse(User.Identity.Name));
            ViewBag.ValoresFrete = GeraViewValorFrete(enderecoUsuario.Cep);
            return View(carrinho);
        }
        [Authorize]
        public IActionResult Pedidos()
        {
            var data = _pedidoRepository.GetPedidos(int.Parse(User.Identity.Name));
            foreach (var item in data)
            {
                if (!string.IsNullOrEmpty(item.IdMercadoPago) && item.StatusAtual != "Aprovado")
                {
                    var res = new RefitRepository().GetPedidoMercadoPago(item.IdMercadoPago);
                    if (res.Elements != null)
                    {
                        item.StatusAtual = res.Elements.First().Payments.Last().Status;
                        item.DataAtualizacao = DateTime.UtcNow.AddHours(-3);
                        _pedidoRepository.PutStatusPedido(item);
                    }
                }
            }
            return View(data);
        }
        [Authorize]
        public IActionResult DescPedido(Pedido pedido)
        {
            var data = _pedidoRepository.GetPedido(pedido.Id);
            if (data.IdMercadoPago == null)
            {
                ViewBag.IdMp = MercadoPagoRequest(data);
            }
            else
            {
                ViewBag.IdMp = data.IdMercadoPago;
            }
            ViewBag.ItensPedido = _carrinhoRepository.GetCarrinhos(pedido.Id);
            return View(data);
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
                using (var context = new ContextEF())
                {
                    var validaCep = new Endereco().ValidaCEP(endereco.Cep);
                    if (validaCep)
                    {
                        endereco.Cep = new Endereco().FormataCEP(endereco.Cep);
                        endereco.IdUsuario = int.Parse(User.Identity.Name);
                        var res = context.EnderecosUsuario.Where(u => u.IdUsuario == endereco.IdUsuario).FirstOrDefault();
                        if (res == null)
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
                            res.Cep = endereco.Cep;
                            context.EnderecosUsuario.Update(res);
                        }
                        context.SaveChanges();
                        return RedirectToAction("Usuario");
                    }
                    else
                    {
                        TempData["Messege"] = "Cep inválido, tente novamente!";
                        return View("Endereco");
                    }
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
        public IActionResult PostPedido(List<ItemViewProduto> produtos)
        {
            var carrinho = GetCarrinho(int.Parse(User.Identity.Name));
            var codPromo = _codPromocional.GetCodPromocao(carrinho.FirstOrDefault().Id);
            var frete = _freteRepository.GetFretePedido(int.Parse(User.Identity.Name), carrinho.FirstOrDefault().Id);
            if (frete == null)
            {
                TempData["Message"] = "Selecione o tipo de frete";
                return RedirectToAction("Carrinho");
            }
            else
            {
                var end = GetEndereco(int.Parse(User.Identity.Name));
                if (end == null)
                {
                    TempData["Message"] = "Realize o cadastro do endereço antes de finalizar sua compra.";
                    return RedirectToAction("Endereco");
                }

                Pedido pedido = new()
                {
                    IdUsuario = int.Parse(User.Identity.Name),
                    DataRealizacao = DateTime.UtcNow.AddHours(-3),
                    StatusAtual = "Pendente"
                };

                var valorCarrinho = carrinho.Sum(c => c.Valor * c.Quantidade);
                if (codPromo != null)
                {
                    valorCarrinho -= valorCarrinho * codPromo.Desconto;
                }

                pedido.ValorTotal = valorCarrinho + frete.ValorTotal;
                pedido.Pais = end.Pais;
                pedido.Estado = end.Estado;
                pedido.Cidade = end.Cidade;
                pedido.Bairro = end.Bairro;
                pedido.Numero = end.Numero;
                var idPedido = _pedidoRepository.PostPedido(pedido);

                AtualizaItensCarrinhoRealizado(idPedido, carrinho);
                return RedirectToAction("Pedidos");
            }

        }

        private static List<Carrinho> GetCarrinho(int idUsuario)
        {
            using (var context = new ContextEF())
            {
                return context.CarrinhoItem.Where(c => c.IdUsuario == idUsuario && c.IdPedido == null).ToList();
            };
        }

        private static void AtualizaItensCarrinhoRealizado(long idPedido, List<Carrinho> carrinho)
        {
            using (var context = new ContextEF())
            {
                foreach (var item in carrinho)
                {
                    item.IdPedido = idPedido;
                    context.CarrinhoItem.Update(item);
                }
                context.SaveChanges();
            };
        }

        [HttpPost]
        public IActionResult ValidarCodPromo(string CodPromocional)
        {
            var data = _codPromocional.GetCodPromocao(CodPromocional);
            if (data == null)
            {
                TempData["Message"] = "Codigo promocional inválido";
                return RedirectToAction("Carrinho");
            }
            var carrinho = GetCarrinho(int.Parse(User.Identity.Name));
            foreach (var item in carrinho)
            {
                _codPromocional.PostCodPromoUsuario(new CodPromocaoUtilizado
                {
                    IdUsuario = int.Parse(User.Identity.Name),
                    IdCarrinho = item.Id,
                    IdCodPromocao = data.Id,
                    DataUtilizacao = DateTime.UtcNow.AddHours(-3),
                });
            }
            data.Utilizacoes++;
            _codPromocional.PutCodPromocao(data);
            return RedirectToAction("Carrinho");
        }

        [HttpPost]
        public IActionResult SelecionaTipoFrete(string TipoFrete)
        {
            if (TipoFrete == null)
            {
                TempData["Message"] = "Selecione o frete";
                return RedirectToAction("Carrinho");
            }
            var valorFrete = TipoFrete.Replace(" ", "").Split("-");
            var carrinho = GetCarrinho(int.Parse(User.Identity.Name));
            foreach (var item in carrinho)
            {
                var frete = new FretePedidoUsuario
                {
                    IdUsuario = int.Parse(User.Identity.Name),
                    IdCarrinho = item.Id,
                    TipoFrete = valorFrete[0],
                    ValorTotal = decimal.Parse(valorFrete[1].Replace("R$", ""))
                };
                _freteRepository.PostFrete(frete);
            }
            return RedirectToAction("Carrinho");
        }

        public IActionResult PostItemCarrinho(ItemViewProduto item)
        {
            if (User.IsInRole("Usuario_Comum") || User.IsInRole("Usuario_Admin"))
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
            TempData["Message"] = "Atenção! Realize cadastro para continuar!";
            return RedirectToAction("Cadastrar");
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

        private static Endereco GetEndereco(int idUsuario)
        {
            using (var context = new ContextEF())
            {
                return context.EnderecosUsuario.Where(e => e.IdUsuario == idUsuario).FirstOrDefault();
            };
        }
                
        private static string MercadoPagoRequest(Pedido pedido)
        {
            MercadoPagoConfig.AccessToken = "APP_USR-1223540178250481-092615-8aee4b2461ec8e00fd5f066bcbd83d26-194500220";
            var request = new PreferenceRequest
            {
                Items = new List<PreferenceItemRequest>
                {
                    new PreferenceItemRequest
                    {
                        Title=$"Pedido nº{pedido.IdUsuario}{pedido.Id}",
                        Quantity=1,
                        CurrencyId="BRL",
                        UnitPrice=pedido.ValorTotal,
                        Id=pedido.Id.ToString()
                    }
                }
            };
            var client = new PreferenceClient();
            Preference preference = client.Create(request);
            PutIdMercadoPago(pedido, preference.Id);
            return preference.Id;
        }
        private static Preference GetPreferenceMP(string id)
        {
            MercadoPagoConfig.AccessToken = "APP_USR-1223540178250481-092615-8aee4b2461ec8e00fd5f066bcbd83d26-194500220";
            var client = new PreferenceClient();
            return client.Get(id);
        }
        private static void PutIdMercadoPago(Pedido pedido, string mercadoPagoId)
        {
            using (var context = new ContextEF())
            {
                pedido.IdMercadoPago = mercadoPagoId;
                context.PedidosUsuario.Update(pedido);
                context.SaveChanges();
            };
        }
        
        private List<ViewBaseValorFrete> GeraViewValorFrete(string cep)
        {
            var list = new List<ViewBaseValorFrete>();
            var data = _freteRepository.GetFreteValores(cep);
            foreach (var frete in data)
            {
                list.Add(new ViewBaseValorFrete { Preco = frete.PrecoFrete, Prazo = frete.PrazoMin, Servico = frete.Servico });
            }
            return list;
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