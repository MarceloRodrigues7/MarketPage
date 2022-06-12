using ADO;
using MarketPage.Models;
using MarketPage.Repository;
using MarketPage.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MarketPage.Controllers
{
    public class AdminController : Controller
    {
        #region Propriedades
        private readonly ICategoriaRepository _categoria;
        private readonly IItemRepository _Item;
        private readonly IImagemRepository _ImgItem;
        private readonly IMessagesContatoRepository _messagesContato;
        private readonly IPedidoRepository _pedidoRepository;
        private readonly ICarrinhoRepository _carrinhoRepository;
        
        private const string _scopeRequired = "Admin";
        
        private readonly PedidoServices _pedidoServices;
        #endregion

        #region Construtores
        public AdminController(ICategoriaRepository categoria, IItemRepository item, IImagemRepository imgItem, IMessagesContatoRepository messagesContato, IPedidoRepository pedidoRepository, ICarrinhoRepository carrinhoRepository)
        {
            _categoria = categoria;
            _Item = item;
            _ImgItem = imgItem;
            _messagesContato = messagesContato;
            _pedidoRepository = pedidoRepository;
            _carrinhoRepository = carrinhoRepository;

            _pedidoServices = new();
        }
        #endregion

        [Authorize]
        public IActionResult Produto()
        {
            if (User.IsInRole(_scopeRequired))
            {
                var categorias = _categoria.GetCategorias();
                var itens = _Item.GetItens();
                var data = NovoItemViewAdmin(itens, categorias);
                return View(data);
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult AdicionarProduto()
        {
            if (User.IsInRole(_scopeRequired))
            {
                ViewBag.Categorias = _categoria.GetCategorias();
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult EditarProduto(ItemViewAdmin item)
        {
            if (User.IsInRole(_scopeRequired))
            {
                var itemDb = _Item.GetItem(item.Id);
                var itemImg = ViewItemAdmAddEEdit.GeraObj(itemDb);

                ViewBag.Categorias = _categoria.GetCategorias();
                return View(itemImg);
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult DeletarProduto(ItemViewAdmin item)
        {
            if (User.IsInRole(_scopeRequired))
            {
                var itemDb = _Item.GetItem(item.Id);
                var itemImg = ViewItemAdmAddEEdit.GeraObj(itemDb);
                ViewBag.Categorias = _categoria.GetCategorias();
                return View(itemImg);
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult PostProduto(ViewItemAdmAddEEdit produto)
        {
            if (User.IsInRole(_scopeRequired))
            {
                try
                {
                    var validaNome = _Item.GetIdItem(produto.Nome);
                    if (validaNome == 0)
                    {
                        AdicionarProduto(produto);
                        return RedirectToAction("Produto", "Admin");
                    }
                    TempData["Message"] = "Já existe um produto com esse mesmo nome, tente novamente!";
                    return RedirectToAction("AdicionarProduto", "Admin");
                }
                catch (Exception e)
                {
                    TempData["Message"] = "Ocorreu algum erro, tente novamente! " + e.Message;
                    return RedirectToAction("AdicionarProduto", "Admin");
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult DeleteProduto(ItemViewAdmin item)
        {
            if (User.IsInRole(_scopeRequired))
            {
                try
                {
                    _ImgItem.DeleteImgItem(item.Id);
                    _Item.DeleteItem(item.Id);
                    _carrinhoRepository.DeleteItemCarrinhoAdmin(item.Id);
                    return RedirectToAction("Produto", "Admin");
                }
                catch (Exception e)
                {
                    TempData["Message"] = "Ocorreu algum erro, tente novamente! " + e.Message;
                    return RedirectToAction("DeletarProduto", "Admin");
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult PutProduto(ViewItemAdmAddEEdit produto)
        {
            if (User.IsInRole(_scopeRequired))
            {
                try
                {
                    if (produto.ImageUploadMain != null)
                    {
                        AtualizarImagemPrincipal(produto);
                    }
                    if (produto.ImageUpload != null)
                    {
                        AtualizarDemaisImagens(produto);
                    }
                    _Item.AtualizaItem(produto);
                    return RedirectToAction("Produto");
                }
                catch (Exception e)
                {
                    TempData["Message"] = "Ocorreu algum erro, tente novamente! " + e.Message;
                    return RedirectToAction("EditarProduto", new ItemViewAdmin { Id = produto.Id });
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult Painel()
        {
            if (User.IsInRole(_scopeRequired))
            {
                var pedidos = _pedidoRepository.GetPedidos();
                ViewBag.ResumoPedidos = _pedidoServices.ResumoTotalPedidos(pedidos);

                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult MensagensContato()
        {
            if (User.IsInRole(_scopeRequired))
            {
                var data = _messagesContato.GetMessageContatos();
                return View(data);
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult Pedidos(string status)
        {
            if (User.IsInRole(_scopeRequired))
            {
                if (status == null)
                {
                    return View(_pedidoRepository.GetPedidos());
                }
                return View(_pedidoRepository.GetPedidos(status));
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult DescPedido(Pedido pedido)
        {
            if (User.IsInRole(_scopeRequired))
            {
                List<ItemViewDescAdmin> itens = new();
                var data = _pedidoRepository.GetPedidos().Where(p => p.Id == pedido.Id).FirstOrDefault();
                var carrinho = _carrinhoRepository.GetCarrinhos(pedido.Id);
                itens = GerarListaItensDeCarrinhos(carrinho);
                ViewBag.ItensPedido = itens;
                ViewBag.PedidosStatus = _pedidoRepository.GetPedidosStatus();
                return View(data);
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult PutPedido(Pedido pedido)
        {
            if (User.IsInRole(_scopeRequired))
            {
                var pedidoDb = _pedidoRepository.GetPedido(pedido.Id);
                pedidoDb = AtualizarDadosPedido(pedido, pedidoDb);
                _pedidoRepository.PutPedido(pedidoDb);
                return RedirectToAction("Pedidos");
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult MensageContato(long Id)
        {
            if (User.IsInRole(_scopeRequired))
            {
                var message = _messagesContato.GetMessageContatos(Id);
                return View(message);
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult ConfirmaVisualizacaoMensageContato(MessageContato message)
        {
            if (User.IsInRole(_scopeRequired))
            {
                _messagesContato.PutConfirmaVisualizacao(message);
                return RedirectToAction("MensagensContato");
            }
            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private void AtualizarDemaisImagens(ViewItemAdmAddEEdit produto)
        {
            _ImgItem.DeletaItemImgPadrao(produto);
            foreach (var img in produto.ImageUpload)
            {
                var novaImagemPadrao = _ImgItem.GeraImgItemPadrao(produto.Id);
                novaImagemPadrao.Img = _ImgItem.GeraImgByte(img);
                _ImgItem.PostImgItem(novaImagemPadrao);
            }
        }

        private void AtualizarImagemPrincipal(ViewItemAdmAddEEdit produto)
        {
            _ImgItem.DeletaItemImgMain(produto.Id);
            var novaImagem = _ImgItem.GeraImgItemPrincipal(produto.Id);
            novaImagem.Img = _ImgItem.GeraImgByte(produto.ImageUploadMain);
            _ImgItem.PostImgItem(novaImagem);
        }

        private static List<ItemViewAdmin> NovoItemViewAdmin(List<Item> item, List<Categoria> categoria)
        {
            List<ItemViewAdmin> list = new();
            foreach (var i in item)
            {
                var itemViewAdmin = ItemViewAdmin.GeraObj(i, categoria);
                list.Add(itemViewAdmin);
            }
            return list;
        }

        private void AdicionarProduto(ViewItemAdmAddEEdit produto)
        {
            var novoProduto = _Item.GeraItem(produto);
            var idProduto = _Item.PostItem(novoProduto);
            AdicionarImagemPrincipal(produto, idProduto);
            if (produto.ImageUpload != null)
            {
                AdicionarDemaisImagens(produto, idProduto);
            }
        }

        private void AdicionarImagemPrincipal(ViewItemAdmAddEEdit produto, long idProduto)
        {
            var novaImagem = _ImgItem.GeraImgItemPrincipal(idProduto);
            novaImagem.Img = _ImgItem.GeraImgByte(produto.ImageUploadMain);
            _ImgItem.PostImgItem(novaImagem);
        }

        private void AdicionarDemaisImagens(ViewItemAdmAddEEdit produto, long idProduto)
        {
            foreach (var img in produto.ImageUpload)
            {
                var novaImagemPadrao = _ImgItem.GeraImgItemPadrao(idProduto);
                novaImagemPadrao.Img = _ImgItem.GeraImgByte(img);
                _ImgItem.PostImgItem(novaImagemPadrao);
            }
        }

        private static Pedido AtualizarDadosPedido(Pedido pedido, Pedido pedidoDb)
        {
            pedidoDb.StatusAtual = pedido.StatusAtual;
            pedidoDb.CodRastreio = pedido.CodRastreio;
            pedidoDb.PrazoEntrega = pedido.PrazoEntrega;
            pedidoDb.DataAtualizacao = DateTime.UtcNow.AddHours(-3);
            if (pedidoDb.StatusAtual == "Finalizado")
            {
                pedidoDb.DateFinalizacao = DateTime.UtcNow.AddHours(-3);
            }

            return pedidoDb;
        }

        private List<ItemViewDescAdmin> GerarListaItensDeCarrinhos(List<Carrinho> carrinhos)
        {
            List<ItemViewDescAdmin> itens = new();

            foreach (var carrinho in carrinhos)
            {
                var item = _Item.GetItem(carrinho.IdItem);
                itens.Add(new ItemViewDescAdmin
                {
                    Id = item.Id,
                    DataAdicao = item.DataAdicao,
                    Descricao = item.Descricao,
                    Destaque = item.Destaque,
                    IdCategoria = item.IdCategoria,
                    Nome = item.Nome,
                    Peso = item.Peso,
                    Quantidade = carrinho.Quantidade,
                    Tamanho = carrinho.Tamanhos,
                    Tamanhos = item.Tamanhos,
                    Valor = item.Valor
                });
            }
            return itens;
        }

    }
}
