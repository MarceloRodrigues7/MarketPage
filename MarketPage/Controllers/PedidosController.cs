using MarketPage.Context;
using MarketPage.Models;
using MarketPage.Repository;
using MarketPage.Services;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
using MercadoPago.Resource.Preference;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MarketPage.Controllers
{
    public class PedidosController : Controller
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly ICarrinhoRepository _carrinhoRepository;
        private readonly IFreteRepository _freteRepository;
        private readonly IEnderecoRepository _enderecoRepository;
        private readonly ICodPromocionalRepository _codPromocional;
        private readonly IItemRepository _itemRepository;
        private readonly IFormaPagamentoRepository _formaPagamentoRepository;
        private readonly MercadoPagoService _mercadoPagoService;

        public PedidosController(IPedidoRepository pedidoRepository, ICarrinhoRepository carrinhoRepository, IFreteRepository freteRepository, IEnderecoRepository enderecoRepository, ICodPromocionalRepository codPromocional, IItemRepository itemRepository, IFormaPagamentoRepository formaPagamentoRepository)
        {
            _pedidoRepository = pedidoRepository;
            _carrinhoRepository = carrinhoRepository;
            _freteRepository = freteRepository;
            _enderecoRepository = enderecoRepository;
            _codPromocional = codPromocional;
            _itemRepository = itemRepository;
            _formaPagamentoRepository = formaPagamentoRepository;
            _mercadoPagoService = new();
        }

        [Authorize]
        public IActionResult Index()
        {
            var token = _formaPagamentoRepository.GetFormaPagamento();
            var data = _pedidoRepository.GetPedidos(int.Parse(User.Identity.Name));
            foreach (var item in data)
            {
                List<string> StatusIgnore = new() { "aprovado", "rejeitado", "cancelado", "devolvido", "cobrado de volta", "finalizado", "preparando", "enviado", "entregue" };
                if (!StatusIgnore.Contains(item.StatusAtual))
                {
                    var res = _mercadoPagoService.GetPedidoMercadoPago(item.IdMercadoPago, token.TokenService);
                    if (res.Elements != null)
                    {
                        var teste = res.Elements.First();
                        item.StatusAtual = teste.Payments.Last().Status;

                        item.DataAtualizacao = DateTime.UtcNow.AddHours(-3);
                        _pedidoRepository.PutStatusPedido(item);
                    }
                }
            }
            return View(data);
        }

        [Authorize]
        public IActionResult Desc(Pedido pedido)
        {
            List<ItemViewDescAdmin> items = new();
            var data = _pedidoRepository.GetPedido(pedido.Id);
            var carrinho = _carrinhoRepository.GetCarrinhos(pedido.Id);
            if (data.IdMercadoPago == null)
            {
                ViewBag.IdMp = _mercadoPagoService.MercadoPagoRequest(data);
            }
            else
            {
                ViewBag.IdMp = data.IdMercadoPago;
            }
            foreach (var item in carrinho)
            {
                Item i = _itemRepository.GetItem(item.IdItem);
                items.Add(new ItemViewDescAdmin
                {
                    Id = i.Id,
                    DataAdicao = i.DataAdicao,
                    Descricao = i.Descricao,
                    Destaque = i.Destaque,
                    IdCategoria = i.IdCategoria,
                    Nome = i.Nome,
                    Peso = i.Peso,
                    Quantidade = item.Quantidade,
                    Tamanho = item.Tamanhos,
                    Tamanhos = i.Tamanhos,
                    Valor = i.Valor
                });
            }
            ViewBag.ItensPedido = items;
            ViewBag.TokenMp = _formaPagamentoRepository.GetFormaPagamento().TokenClient;
            return View(data);
        }

        [Authorize]
        public IActionResult Carrinho()
        {
            var enderecoUsuario = _enderecoRepository.GetEndereco(int.Parse(User.Identity.Name));
            if (enderecoUsuario == null)
            {
                TempData["Message"] = "Registre o endereço de entrega";
                return RedirectToAction("Endereco");
            }
            var carrinho = _carrinhoRepository.GetItensCarrinhoView(int.Parse(User.Identity.Name));
            ViewBag.ValoresFrete = GeraViewValorFrete(enderecoUsuario.Cep);
            return View(carrinho);
        }

        [Authorize]
        public IActionResult PostPedido(List<ItemViewProduto> produtos)
        {
            var carrinho = _carrinhoRepository.GetCarrinho(int.Parse(User.Identity.Name));
            var codPromo = _codPromocional.GetPromocaoUtilizada(carrinho.FirstOrDefault().Id);
            var frete = _freteRepository.GetFretePedido(int.Parse(User.Identity.Name), carrinho.FirstOrDefault().Id);
            if (frete == null)
            {
                TempData["Message"] = "Selecione o tipo de frete";
                return RedirectToAction("Carrinho");
            }
            else
            {
                var end = _enderecoRepository.GetEndereco(int.Parse(User.Identity.Name));
                if (end == null)
                {
                    TempData["Message"] = "Realize o cadastro do endereço antes de finalizar sua compra.";
                    return RedirectToAction("Endereco");
                }

                Pedido pedido = new()
                {
                    IdUsuario = int.Parse(User.Identity.Name),
                    DataRealizacao = DateTime.UtcNow.AddHours(-3),
                    StatusAtual = "pendente"
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
                var idMercadoLivre = _mercadoPagoService.MercadoPagoRequest(pedido);
                pedido.IdMercadoPago = idMercadoLivre;
                var idPedido = _pedidoRepository.PostPedido(pedido);

                _carrinhoRepository.UpdateItensCarrinhoRealizado(idPedido, carrinho);
                return RedirectToAction("Index");
            }

        }
        [HttpPost]
        public IActionResult ValidarCodPromo(string CodPromocional)
        {
            var data = _codPromocional.GetPromocaoValida(CodPromocional);
            if (data == null)
            {
                TempData["Message"] = "Codigo promocional inválido";
                return RedirectToAction("Carrinho");
            }
            var carrinho = _carrinhoRepository.GetCarrinho(int.Parse(User.Identity.Name));
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
            _codPromocional.Put(data);
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
            var carrinho = _carrinhoRepository.GetCarrinho(int.Parse(User.Identity.Name));
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
            if (User.IsInRole("Usuario_Comum") || User.IsInRole("Admin"))
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
            TempData["Alert"] = "Realize seu cadastro para continuar.";
            return RedirectToAction("Cadastrar");
        }

        public IActionResult DeleteItemCarrinho(long item)
        {
            try
            {
                _carrinhoRepository.DeleteItemCarrinho(item);
                return RedirectToAction("Carrinho");
            }
            catch (Exception e)
            {
                TempData["Alert"] = e.Message;
                return RedirectToAction("Carrinho");
            }

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

    }
}
