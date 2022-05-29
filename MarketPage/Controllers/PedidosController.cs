using ADO;
using MarketPage.Models;
using MarketPage.Repository;
using MarketPage.Services;
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

        private readonly MercadoPagoServices _mercadoPagoService;
        private readonly List<string> _statusIgnore;

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
            _statusIgnore = new() { "aprovado", "rejeitado", "cancelado", "devolvido", "cobrado de volta", "finalizado", "preparando", "enviado", "entregue" };
        }

        [Authorize]
        public IActionResult Index()
        {
            var token = _formaPagamentoRepository.GetFormaPagamento();
            var data = _pedidoRepository.GetPedidos(int.Parse(User.Identity.Name));
            foreach (var item in data)
            {
                if (!_statusIgnore.Contains(item.StatusAtual))
                {
                    try
                    {
                        var res = _mercadoPagoService.GetPedidoMercadoPago(item.IdMercadoPago, token.TokenService);
                        if (res.Elements != null)
                        {
                            var pedido = res.Elements.First();
                            item.StatusAtual = pedido.Payments.Last().Status;

                            item.DataAtualizacao = DateTime.UtcNow.AddHours(-3);
                            _pedidoRepository.PutStatusPedido(item);
                        }
                    }
                    catch (Exception)
                    {
                        return View(data);
                        throw;
                    }
                }
            }
            return View(data);
        }

        [Authorize]
        public IActionResult Desc(Pedido pedido)
        {
            List<ItemViewDescAdmin> items = new();
            var token = _formaPagamentoRepository.GetFormaPagamentoTeste();
            var data = _pedidoRepository.GetPedido(pedido.Id);
            var carrinho = _carrinhoRepository.GetCarrinhos(pedido.Id);

            ViewBag.IdMp = _mercadoPagoService.GetIdPedido(data, token.TokenService);

            foreach (var itemCarrinho in carrinho)
            {
                Item item = _itemRepository.GetItem(itemCarrinho.IdItem);
                var itemViewDescAdmin = ItemViewDescAdmin.GeraObj(item, itemCarrinho);
                items.Add(itemViewDescAdmin);
            }
            ViewBag.ItensPedido = items;
            ViewBag.TokenMp = _formaPagamentoRepository.GetFormaPagamentoTeste().TokenClient;
            return View(data);
        }

        [Authorize]
        public IActionResult Carrinho()
        {
            var enderecoUsuario = _enderecoRepository.GetEndereco(int.Parse(User.Identity.Name));
            if (enderecoUsuario == null)
            {
                return RedirectToAction("Endereco", "Login", new object());
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
            var endereco = _enderecoRepository.GetEndereco(int.Parse(User.Identity.Name));

            if (frete == null)
            {
                TempData["Message"] = "Selecione o tipo de frete";
                return RedirectToAction("Carrinho");
            }

            if (endereco == null)
            {
                TempData["Message"] = "Realize o cadastro do endereço antes de finalizar sua compra.";
                return RedirectToAction("Endereco");
            }

            Pedido pedido = Pedido.GerarObj(int.Parse(User.Identity.Name));
            var token = _formaPagamentoRepository.GetFormaPagamentoTeste();
            var valorCarrinho = carrinho.Sum(c => c.Valor * c.Quantidade);
            if (codPromo != null)
            {
                valorCarrinho -= valorCarrinho * codPromo.Desconto;
            }
            pedido.ValorTotal = valorCarrinho + frete.ValorTotal;
            pedido.Pais = endereco.Pais;
            pedido.Estado = endereco.Estado;
            pedido.Cidade = endereco.Cidade;
            pedido.Bairro = endereco.Bairro;
            pedido.Numero = endereco.Numero;
            var idMercadoLivre = _mercadoPagoService.MercadoPagoRequest(pedido, token.TokenService);
            pedido.IdMercadoPago = idMercadoLivre;
            var idPedido = _pedidoRepository.PostPedido(pedido);

            _carrinhoRepository.UpdateItensCarrinhoRealizado(idPedido, carrinho);
            return RedirectToAction("Index");

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
                var codPromoUtilizado = CodPromocaoUtilizado.GeraObj(data, item, int.Parse(User.Identity.Name));
                _codPromocional.PostCodPromoUsuario(codPromoUtilizado);
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
                var frete = FretePedidoUsuario.GeraObj(valorFrete, item, int.Parse(User.Identity.Name));
                _freteRepository.PostFrete(frete);
            }
            return RedirectToAction("Carrinho");
        }

        [Authorize]
        public IActionResult PostItemCarrinho(ItemViewProduto item)
        {
            var i = item.GeraObj();
            var carrinho = ADO.Carrinho.GeraObj(i, int.Parse(User.Identity.Name));
            _carrinhoRepository.PostItemCarrinho(carrinho);
            return RedirectToAction("Carrinho");
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
