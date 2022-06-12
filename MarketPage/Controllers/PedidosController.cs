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
        #region Propriedades
        private readonly IPedidoRepository _pedidoRepository;
        private readonly ICarrinhoRepository _carrinhoRepository;
        private readonly IFreteRepository _freteRepository;
        private readonly IEnderecoRepository _enderecoRepository;
        private readonly ICodPromocionalRepository _codPromocional;
        private readonly IItemRepository _itemRepository;
        private readonly IFormaPagamentoRepository _formaPagamentoRepository;

        private readonly MercadoPagoServices _mercadoPagoService;
        private readonly PedidoServices _pedidoServices;
        #endregion

        #region Construtores
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
            _pedidoServices = new();
        }
        #endregion

        [Authorize]
        public IActionResult Index()
        {            
            var data = _pedidoRepository.GetPedidos(int.Parse(User.Identity.Name));
            AtualizarListaPedidos(data);
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
            var idUsuario = int.Parse(User.Identity.Name);
            var carrinho = _carrinhoRepository.GetCarrinho(idUsuario);            
            var frete = _freteRepository.GetFretePedido(idUsuario, carrinho.FirstOrDefault().Id);
            var endereco = _enderecoRepository.GetEndereco(idUsuario);
            var codPromo = _codPromocional.GetPromocaoUtilizada(carrinho.FirstOrDefault().Id);

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

            var valorCarrinho = carrinho.Sum(c => c.Valor * c.Quantidade);

            if (codPromo != null)
            {
                valorCarrinho -= valorCarrinho * codPromo.Desconto;
            }

            var valorTotalPedido = valorCarrinho + frete.ValorTotal;

            var idPedido = GeraPedido(idUsuario, endereco, valorTotalPedido);

            _carrinhoRepository.UpdateItensCarrinhoRealizado(idPedido, carrinho);
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        public IActionResult ValidarCodPromo(string CodPromocional)
        {
            var codPromocao = _codPromocional.GetPromocaoValida(CodPromocional);

            if (codPromocao == null)
            {
                TempData["Message"] = "Codigo promocional inválido";
                return RedirectToAction("Carrinho");
            }

            var idUsuario = int.Parse(User.Identity.Name);
            var carrinho = _carrinhoRepository.GetCarrinho(idUsuario);
            
            AtualizarCarrinhoComCodPromo(codPromocao, idUsuario, carrinho);
            
            AtualizarUtilizacaoCodPromo(codPromocao);

            return RedirectToAction("Carrinho");
        }

        [Authorize]
        [HttpPost]
        public IActionResult SelecionaTipoFrete(string TipoFrete)
        {
            if (TipoFrete == null)
            {
                TempData["Message"] = "Selecione o frete";
                return RedirectToAction("Carrinho");
            }

            var valorFrete = TipoFrete.Replace(" ", "").Split("-");
            var idUsuario = int.Parse(User.Identity.Name);

            var carrinho = _carrinhoRepository.GetCarrinho(idUsuario);

            AtualizarCarrinhoComValorFrete(valorFrete, idUsuario, carrinho);
            return RedirectToAction("Carrinho");
        }

        [Authorize]
        public IActionResult PostItemCarrinho(ItemViewProduto item)
        {
            var itemCarrinho = item.GeraObj();
            var idUsuario = int.Parse(User.Identity.Name);

            var carrinho = ADO.Carrinho.GeraObj(itemCarrinho, idUsuario);
            _carrinhoRepository.PostItemCarrinho(carrinho);
            return RedirectToAction("Carrinho");
        }

        [Authorize]
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

        private void AtualizarListaPedidos(List<Pedido> pedidos)
        {
            var token = _formaPagamentoRepository.GetFormaPagamento();
            foreach (var pedido in pedidos)
            {
                if (!_pedidoServices.StatusIgnore.Contains(pedido.StatusAtual))
                {
                    AtualizarStatusPedido(token, pedido);
                }
            }
        }

        private long GeraPedido(int idUsuario, Endereco endereco, decimal valorTotalPedido)
        {
            Pedido pedido = Pedido.GerarObj(idUsuario);
            pedido.ValorTotal = valorTotalPedido;
            pedido = AdicionarEndereco(pedido, endereco);
            var formaPagamento = _formaPagamentoRepository.GetFormaPagamentoTeste();
            pedido = AdicionarIdMercadoPago(pedido, formaPagamento);

            return _pedidoRepository.PostPedido(pedido);
        }

        private Pedido AdicionarIdMercadoPago(Pedido pedido, FormaPagamento formaPagamento)
        {
            var idMercadoLivre = _mercadoPagoService.MercadoPagoRequest(pedido, formaPagamento.TokenService);
            pedido.IdMercadoPago = idMercadoLivre;
            return pedido;
        }

        private static Pedido AdicionarEndereco(Pedido pedido, Endereco endereco)
        {
            pedido.Pais = endereco.Pais;
            pedido.Estado = endereco.Estado;
            pedido.Cidade = endereco.Cidade;
            pedido.Bairro = endereco.Bairro;
            pedido.Numero = endereco.Numero;
            return pedido;
        }

        private List<ViewBaseValorFrete> GeraViewValorFrete(string cep)
        {
            var list = new List<ViewBaseValorFrete>();
            var valoresFrete = _freteRepository.GetFreteValores(cep);
            foreach (var valorFrete in valoresFrete)
            {
                list.Add(ViewBaseValorFrete.GeraObj(valorFrete));
            }
            return list;
        }

        private void AtualizarStatusPedido(FormaPagamento token, Pedido item)
        {
            try
            {
                var res = _mercadoPagoService.GetPedidoMercadoPago(item.IdMercadoPago, token.TokenService);
                if (res.Elements != null)
                {
                    item.StatusAtual = _pedidoServices.GetStatusPagamentoPedido(res);
                    item.DataAtualizacao = DateTime.UtcNow.AddHours(-3);
                    _pedidoRepository.PutStatusPedido(item);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void AtualizarCarrinhoComValorFrete(string[] valorFrete, int idUsuario, List<Carrinho> carrinho)
        {
            foreach (var item in carrinho)
            {
                var frete = FretePedidoUsuario.GeraObj(valorFrete, item, idUsuario);
                _freteRepository.PostFrete(frete);
            }
        }

        private void AtualizarCarrinhoComCodPromo(CodPromocao data, int idUsuario, List<Carrinho> carrinho)
        {
            foreach (var item in carrinho)
            {
                var codPromoUtilizado = CodPromocaoUtilizado.GeraObj(data, item, idUsuario);
                _codPromocional.PostCodPromoUsuario(codPromoUtilizado);
            }
        }

        private void AtualizarUtilizacaoCodPromo(CodPromocao data)
        {
            data.Utilizacoes++;
            _codPromocional.Put(data);
        }

    }
}
