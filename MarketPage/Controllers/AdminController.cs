using MarketPage.Context;
using MarketPage.Models;
using MarketPage.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPage.Controllers
{
    public class AdminController : Controller
    {
        private readonly ICategoriaRepository _Categoria;
        private readonly IItemRepository _Item;
        private readonly IImagemRepository _ImgItem;
        private readonly ICodPromocionalRepository _codPromocional;
        private readonly IMessagesContatoRepository _messagesContato;
        private readonly IPedidoRepository _pedidoRepository;
        private readonly ICarrinhoRepository _carrinhoRepository;

        public AdminController(ICategoriaRepository categoria, IItemRepository item, IImagemRepository imgItem, ICodPromocionalRepository codPromocional, IMessagesContatoRepository messagesContato, IPedidoRepository pedidoRepository, ICarrinhoRepository carrinhoRepository)
        {
            _Categoria = categoria;
            _Item = item;
            _ImgItem = imgItem;
            _codPromocional = codPromocional;
            _messagesContato = messagesContato;
            _pedidoRepository = pedidoRepository;
            _carrinhoRepository = carrinhoRepository;
        }

        [Authorize]
        public IActionResult Categoria()
        {
            if (User.IsInRole("Usuario_Admin"))
            {
                return View(_Categoria.GetCategorias());
            }
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public IActionResult AdicionarCategoria()
        {
            if (User.IsInRole("Usuario_Admin"))
            {
                return View();
            }
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public IActionResult EditarCategoria(Categoria item)
        {
            if (User.IsInRole("Usuario_Admin"))
            {
                return View(item);
            }
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public IActionResult DeletarCategoria(Categoria item)
        {
            if (User.IsInRole("Usuario_Admin"))
            {
                return View(item);
            }
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public IActionResult PostCategoria(Categoria categoria)
        {
            if (User.IsInRole("Usuario_Admin"))
            {
                try
                {
                    _Categoria.PostCategoria(categoria);
                    return RedirectToAction("Categoria", "Admin");
                }
                catch (Exception e)
                {
                    TempData["Message"] = "Ocorreu algum erro, tente novamente! " + e.Message;
                    return RedirectToAction("AdicionarCategoria", "Admin");
                }
            }
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public IActionResult PutCategoria(Categoria categoria)
        {
            if (User.IsInRole("Usuario_Admin"))
            {
                try
                {
                    _Categoria.PutCategoria(categoria);
                    return RedirectToAction("Categoria", "Admin");
                }
                catch (Exception e)
                {
                    TempData["Message"] = "Ocorreu algum erro, tente novamente! " + e.Message;
                    return RedirectToAction("EditarCategoria", "Admin", categoria);
                }
            }
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public IActionResult DeleteCategoria(Categoria categoria)
        {
            if (User.IsInRole("Usuario_Admin"))
            {
                try
                {
                    _Categoria.DeleteCategoria(categoria);
                    return RedirectToAction("Categoria", "Admin");
                }
                catch (Exception e)
                {
                    TempData["Message"] = "Ocorreu algum erro, tente novamente! " + e.Message;
                    return RedirectToAction("DeleteCategoria", "Admin");
                }
            }
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public IActionResult CodPromocional()
        {
            if (User.IsInRole("Usuario_Admin"))
            {
                var data = _codPromocional.GetCodPromocoes();
                return View(data);
            }
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public IActionResult AddCodPromo()
        {
            if (User.IsInRole("Usuario_Admin"))
            {
                return View();
            }
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public IActionResult EditarCodPromo(CodPromocao codPromocao)
        {
            if (User.IsInRole("Usuario_Admin"))
            {
                return View(codPromocao);
            }
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public IActionResult PutCodPromo(CodPromocao codPromocao)
        {
            if (User.IsInRole("Usuario_Admin"))
            {
                try
                {
                    codPromocao.Desconto *= 0.01m;
                    _codPromocional.PutCodPromocao(codPromocao);
                    return RedirectToAction("CodPromocional");
                }
                catch (Exception e)
                {
                    TempData["Message"] = "Ocorreu algum erro, tente novamente! " + e.Message;
                    return RedirectToAction("EditarCodPromo");
                }
            }
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public IActionResult DeletarCodPromo(CodPromocao codPromocao)
        {
            if (User.IsInRole("Usuario_Admin"))
            {
                return View(codPromocao);
            }
            return RedirectToAction("Index", "Home");
        }
        public IActionResult DeleteCodPromo(CodPromocao codPromocao)
        {
            if (User.IsInRole("Usuario_Admin"))
            {
                try
                {
                    _codPromocional.DeleteCodPromocao(codPromocao.Codigo);
                    return RedirectToAction("CodPromocional");
                }
                catch (Exception e)
                {
                    TempData["Message"] = "Ocorreu algum erro, tente novamente! " + e.Message;
                    return RedirectToAction("DeletarCodPromo");
                }
            }
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public IActionResult PostCodPromo(CodPromocao codPromocao)
        {
            if (User.IsInRole("Usuario_Admin"))
            {
                try
                {
                    codPromocao.Utilizacoes = 0;
                    codPromocao.Desconto *= 0.01m;
                    _codPromocional.PostCodPromocao(codPromocao);
                    return RedirectToAction("CodPromocional");
                }
                catch (Exception e)
                {
                    TempData["Message"] = "Ocorreu algum erro, tente novamente! " + e.Message;
                    return RedirectToAction("AddCodPromo");
                }
            }
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public IActionResult Produto()
        {
            if (User.IsInRole("Usuario_Admin"))
            {
                var categorias = _Categoria.GetCategorias();
                var itens = _Item.GetItens();
                var data = NovoItemViewAdmin(itens, categorias);
                return View(data);
            }
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public IActionResult AdicionarProduto()
        {
            if (User.IsInRole("Usuario_Admin"))
            {
                ViewBag.Categorias = _Categoria.GetCategorias();
                return View();
            }
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public IActionResult EditarProduto(ItemViewAdmin item)
        {
            if (User.IsInRole("Usuario_Admin"))
            {
                var produto = _Item.GetItem(item.Id);
                var itemImg = new ViewItemAdmAddEEdit
                {
                    Id = produto.Id,
                    Nome = produto.Nome,
                    Descricao = produto.Descricao,
                    ValorString = produto.Valor.ToString(),
                    Tamanhos = produto.Tamanhos,
                    Quantidade = produto.Quantidade,
                    Destaque = produto.Destaque,
                    IdCategoria = produto.IdCategoria,
                    PesoString = produto.Peso.ToString()
                };
                ViewBag.Categorias = _Categoria.GetCategorias();
                return View(itemImg);
            }
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public IActionResult DeletarProduto(ItemViewAdmin item)
        {
            if (User.IsInRole("Usuario_Admin"))
            {
                var produto = _Item.GetItem(item.Id);
                var itemImg = new ViewItemAdmAddEEdit
                {
                    Id = produto.Id,
                    Nome = produto.Nome,
                    Descricao = produto.Descricao,
                    Valor = produto.Valor,
                    Tamanhos = produto.Tamanhos,
                    Quantidade = produto.Quantidade,
                    Destaque = produto.Destaque,
                    IdCategoria = produto.IdCategoria,
                    Peso = produto.Peso
                };
                ViewBag.Categorias = _Categoria.GetCategorias();
                return View(itemImg);
            }
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public IActionResult PostProduto(ViewItemAdmAddEEdit produto)
        {
            if (User.IsInRole("Usuario_Admin"))
            {
                try
                {
                    var validaNome = _Item.GetIdItem(produto.Nome);
                    if (validaNome == 0)
                    {
                        var novoProduto = _Item.GeraItem(produto);
                        var idProduto = _Item.PostItem(novoProduto);

                        var novaImagem = _ImgItem.GeraImgItemPrincipal(idProduto);
                        novaImagem.Img = _ImgItem.GeraImgByte(produto.ImageUploadMain);
                        _ImgItem.PostImgItem(novaImagem);
                        foreach (var img in produto.ImageUpload)
                        {
                            var novaImagemPadrao = _ImgItem.GeraImgItemPadrao(idProduto);
                            novaImagemPadrao.Img = _ImgItem.GeraImgByte(img);
                            _ImgItem.PostImgItem(novaImagemPadrao);
                        }
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
            if (User.IsInRole("Usuario_Admin"))
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
            if (User.IsInRole("Usuario_Admin"))
            {
                try
                {
                    if (produto.ImageUploadMain != null)
                    {
                        _ImgItem.DeletaItemImgMain(produto);
                        var novaImagem = _ImgItem.GeraImgItemPrincipal(produto.Id);
                        novaImagem.Img = _ImgItem.GeraImgByte(produto.ImageUploadMain);
                        _ImgItem.PostImgItem(novaImagem);
                    }
                    if (produto.ImageUpload != null)
                    {
                        _ImgItem.DeletaItemImgPadrao(produto);
                        foreach (var img in produto.ImageUpload)
                        {
                            var novaImagemPadrao = _ImgItem.GeraImgItemPadrao(produto.Id);
                            novaImagemPadrao.Img = _ImgItem.GeraImgByte(img);
                            _ImgItem.PostImgItem(novaImagemPadrao);
                        }
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
            if (User.IsInRole("Usuario_Admin"))
            {
                var context = new ContextEF();
                var pedidos = context.PedidosUsuario.ToList();
                ViewBag.ResumoPedidos = ResumoTotalPedidos(pedidos);

                return View();
            }
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public ActionResult MensagensContato()
        {
            if (User.IsInRole("Usuario_Admin"))
            {
                var data = _messagesContato.GetMessageContatos();
                return View(data);
            }
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public ActionResult Pedidos(string status)
        {
            if (User.IsInRole("Usuario_Admin"))
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
            if (User.IsInRole("Usuario_Admin"))
            {
                using (var context = new ContextEF())
                {
                    List<ItemViewDescAdmin> items = new();
                    var data = context.PedidosUsuario.Where(p => p.Id == pedido.Id).FirstOrDefault();
                    var carrinho = context.CarrinhoItem.Where(c => c.IdPedido == pedido.Id).ToList();
                    foreach (var item in carrinho)
                    {
                        var i = context.Itens.Where(i => i.Id == item.IdItem).FirstOrDefault();
                        var tamanho = item.Tamanhos;
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
                    ViewBag.PedidosStatus = context.PedidosStatus.OrderBy(p => p.Nome).ToList();
                    return View(data);
                };
            }
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public IActionResult PutPedido(Pedido pedido)
        {
            if (User.IsInRole("Usuario_Admin"))
            {
                var data = _pedidoRepository.GetPedido(pedido.Id);
                data.StatusAtual = pedido.StatusAtual;
                data.CodRastreio = pedido.CodRastreio;
                data.PrazoEntrega = pedido.PrazoEntrega;
                data.DataAtualizacao = DateTime.UtcNow.AddHours(-3);
                if (data.StatusAtual == "Finalizado")
                {
                    data.DateFinalizacao = DateTime.UtcNow.AddHours(-3);
                }
                _pedidoRepository.PutPedido(data);
                return RedirectToAction("Pedidos");
            }
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public ActionResult MensageContato(long Id)
        {
            if (User.IsInRole("Usuario_Admin"))
            {
                var data = _messagesContato.GetMessageContatos(Id);
                return View(data);
            }
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public ActionResult ConfirmaVisualizacaoMensageContato(MessageContato message)
        {
            if (User.IsInRole("Usuario_Admin"))
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

        private static List<ItemViewAdmin> NovoItemViewAdmin(List<Item> item, List<Categoria> categoria)
        {
            List<ItemViewAdmin> list = new();
            foreach (var i in item)
            {
                list.Add(new ItemViewAdmin
                {
                    Id = i.Id,
                    Nome = i.Nome,
                    Descricao = i.Descricao,
                    Valor = i.Valor,
                    Quantidade = i.Quantidade,
                    Destaque = i.Destaque,
                    DataAdicao = i.DataAdicao,
                    Categoria = categoria.Where(c => c.Id == i.IdCategoria).First().Nome
                });
            }
            return list;
        }
        private static List<int> ResumoTotalPedidos(List<Pedido> pedidos)
        {
            var pedidosPendentes = pedidos.Where(p => p.StatusAtual == "Pendente").Count();
            var pedidosAprovados = pedidos.Where(p => p.StatusAtual == "Aprovado").Count();
            var pedidosAutorizado = pedidos.Where(p => p.StatusAtual == "Autorizado").Count();
            var pedidosEmProcesso = pedidos.Where(p => p.StatusAtual == "Em Processo").Count();
            var pedidosEmMediacao = pedidos.Where(p => p.StatusAtual == "Em Mediação").Count();
            var pedidosRejeitado = pedidos.Where(p => p.StatusAtual == "Rejeitado").Count();
            var pedidosCancelado = pedidos.Where(p => p.StatusAtual == "Cancelado").Count();
            var pedidosDevolvido = pedidos.Where(p => p.StatusAtual == "Devolvido").Count();
            var pedidosCobradoVolta = pedidos.Where(p => p.StatusAtual == "Cobrado de Volta").Count();
            var pedidosFinalizado = pedidos.Where(p => p.StatusAtual == "Finalizado").Count();
            var pedidosPreparando = pedidos.Where(p => p.StatusAtual == "Preparando").Count();
            var pedidosEnviado = pedidos.Where(p => p.StatusAtual == "Enviado").Count();
            var pedidosEntregue = pedidos.Where(p => p.StatusAtual == "Entregue").Count();
            return new List<int>
            {
                pedidos.Count(),pedidosPendentes,pedidosAprovados,pedidosAutorizado,pedidosEmProcesso,pedidosEmMediacao,pedidosRejeitado,pedidosCancelado,pedidosDevolvido,pedidosCobradoVolta,pedidosFinalizado,pedidosPreparando,pedidosEnviado,pedidosEntregue
            };
        }
    }
}
