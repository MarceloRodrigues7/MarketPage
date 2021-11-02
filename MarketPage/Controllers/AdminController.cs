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

        public AdminController(ICategoriaRepository categoria, IItemRepository item, IImagemRepository imgItem, ICodPromocionalRepository codPromocional)
        {
            _Categoria = categoria;
            _Item = item;
            _ImgItem = imgItem;
            _codPromocional = codPromocional;
        }

        [Authorize]
        public IActionResult Categoria()
        {
            return View(_Categoria.GetCategorias());
        }
        [Authorize]
        public IActionResult AdicionarCategoria()
        {
            return View();
        }
        [Authorize]
        public IActionResult EditarCategoria(Categoria item)
        {
            return View(item);
        }
        [Authorize]
        public IActionResult DeletarCategoria(Categoria item)
        {
            return View(item);
        }
        [Authorize]
        public IActionResult PostCategoria(Categoria categoria)
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
        [Authorize]
        public IActionResult PutCategoria(Categoria categoria)
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
        [Authorize]
        public IActionResult DeleteCategoria(Categoria categoria)
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
        [Authorize]
        public IActionResult CodPromocional()
        {
            var data = _codPromocional.GetCodPromocoes();
            return View(data);
        }
        [Authorize]
        public IActionResult AddCodPromo()
        {
            return View();
        }
        [Authorize]
        public IActionResult EditarCodPromo(CodPromocao codPromocao)
        {
            return View(codPromocao);
        }
        [Authorize]
        public IActionResult PutCodPromo(CodPromocao codPromocao)
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
        [Authorize]
        public IActionResult DeletarCodPromo(CodPromocao codPromocao)
        {
            return View(codPromocao);
        }
        public IActionResult DeleteCodPromo(CodPromocao codPromocao)
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
        [Authorize]
        public IActionResult PostCodPromo(CodPromocao codPromocao)
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
        [Authorize]
        public IActionResult Produto()
        {
            var categorias = _Categoria.GetCategorias();
            var itens = _Item.GetItens();
            var data = NovoItemViewAdmin(itens, categorias);
            return View(data);
        }
        [Authorize]
        public IActionResult AdicionarProduto()
        {
            ViewBag.Categorias = _Categoria.GetCategorias();
            return View();
        }
        [Authorize]
        public IActionResult EditarProduto(ItemViewAdmin item)
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
        [Authorize]
        public IActionResult DeletarProduto(ItemViewAdmin item)
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
        [Authorize]
        public IActionResult PostProduto(ViewItemAdmAddEEdit produto)
        {
            try
            {
                var novoProduto = _Item.GeraItem(produto);
                _Item.PostItem(novoProduto);

                var novaImagem = _ImgItem.GeraImgItemPrincipal(produto.Nome);
                novaImagem.Img = _ImgItem.GeraImgByte(produto.ImageUploadMain);
                _ImgItem.PostImgItem(novaImagem);
                foreach (var img in produto.ImageUpload)
                {
                    var novaImagemPadrao = _ImgItem.GeraImgItemPadrao(produto.Nome);
                    novaImagemPadrao.Img = _ImgItem.GeraImgByte(img);
                    _ImgItem.PostImgItem(novaImagemPadrao);
                }
                return RedirectToAction("Produto", "Admin");
            }
            catch (Exception e)
            {
                TempData["Message"] = "Ocorreu algum erro, tente novamente! " + e.Message;
                return RedirectToAction("AdicionarProduto", "Admin");
            }
        }
        [Authorize]
        public IActionResult DeleteProduto(ItemViewAdmin item)
        {
            try
            {
                _ImgItem.DeleteImgItem(item.Id);
                _Item.DeleteItem(item.Id);
                return RedirectToAction("Produto", "Admin");
            }
            catch (Exception e)
            {
                TempData["Message"] = "Ocorreu algum erro, tente novamente! " + e.Message;
                return RedirectToAction("DeletarProduto", "Admin");
            }

        }
        [Authorize]
        public IActionResult PutProduto(ViewItemAdmAddEEdit produto)
        {
            try
            {
                if (produto.ImageUploadMain != null)
                {
                    _ImgItem.DeletaItemImgMain(produto);
                    var novaImagem = _ImgItem.GeraImgItemPrincipal(produto.Nome);
                    novaImagem.Img = _ImgItem.GeraImgByte(produto.ImageUploadMain);
                    _ImgItem.PostImgItem(novaImagem);
                }
                if (produto.ImageUpload != null)
                {
                    _ImgItem.DeletaItemImgPadrao(produto);
                    foreach (var img in produto.ImageUpload)
                    {
                        var novaImagemPadrao = _ImgItem.GeraImgItemPadrao(produto.Nome);
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
        [Authorize]
        public IActionResult Painel()
        {
            var context = new ContextEF();
            var pedidos = context.PedidosUsuario.ToList();
            ViewBag.ResumoPedidos = ResumoTotalPedidos(pedidos);

            return View();
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
            return new List<int>
            {
                pedidos.Count(),pedidosPendentes,pedidosAprovados,pedidosAutorizado,pedidosEmProcesso,pedidosEmMediacao,pedidosRejeitado,pedidosCancelado,pedidosDevolvido,pedidosCobradoVolta
            };
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
