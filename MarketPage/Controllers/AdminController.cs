using MarketPage.Context;
using MarketPage.Models;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public IActionResult Categoria()
        {
            using (var context = new ContextEF())
            {
                return View(context.Categorias.ToList());
            };
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
                using (var context = new ContextEF())
                {
                    categoria.DataAdicao = DateTime.Now;
                    context.Categorias.Add(categoria);
                    context.SaveChanges();
                    return RedirectToAction("Categoria", "Admin");
                };
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
                using (var context = new ContextEF())
                {
                    categoria.DataAdicao = DateTime.Now;
                    context.Categorias.Update(categoria);
                    context.SaveChanges();
                    return RedirectToAction("Categoria", "Admin");
                };
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
                using (var context = new ContextEF())
                {
                    context.Itens.RemoveRange(context.Itens.Where(i => i.IdCategoria == categoria.Id));
                    context.Categorias.Remove(categoria);
                    context.SaveChanges();
                    return RedirectToAction("Categoria", "Admin");
                };
            }
            catch (Exception e)
            {
                TempData["Message"] = "Ocorreu algum erro, tente novamente! " + e.Message;
                return RedirectToAction("DeleteCategoria", "Admin");
            }

        }

        [Authorize]
        public IActionResult Produto()
        {
            using (var context = new ContextEF())
            {
                var categorias = context.Categorias.ToList();
                var itens = context.Itens.ToList();
                var data = NovoItemViewAdmin(itens, categorias);
                return View(data);
            };
        }
        [Authorize]
        public IActionResult AdicionarProduto()
        {
            using (var context = new ContextEF())
            {
                ViewBag.Categorias = context.Categorias.ToList();
                return View();
            };
        }
        [Authorize]
        public IActionResult PostProduto(ItemImagem produto)
        {
            try
            {
                var novoProduto = NovoProduto(produto);
                SalvarNovoItem(novoProduto);
                var novaImagem = NovoImgItem(produto.Nome);
                var res = new BinaryReader(produto.ImageUpload.OpenReadStream());
                novaImagem.Img = res.ReadBytes((int)produto.ImageUpload.Length);
                SalvarNovoImgItem(novaImagem);
                return RedirectToAction("Produto", "Admin");
            }
            catch (Exception e)
            {
                TempData["Message"] = "Ocorreu algum erro, tente novamente! " + e.Message;
                return RedirectToAction("AdicionarProduto", "Admin");
            }

        }

        private static Item NovoProduto(ItemImagem produto)
        {
            return new Item
            {
                Nome = produto.Nome,
                Descricao = produto.Descricao,
                Valor = produto.Valor,
                Quantidade = produto.Quantidade,
                Destaque = produto.Destaque,
                DataAdicao = DateTime.Now,
                IdCategoria = produto.Categoria
            };
        }
        private static void SalvarNovoItem(Item item)
        {
            using (var context = new ContextEF())
            {
                context.Itens.Add(item);
                context.SaveChanges();
            };
        }
        private static long GetIdNovoItem(string nomeItem)
        {
            using (var context = new ContextEF())
            {
                return context.Itens.Where(i => i.Nome == nomeItem).First().Id;
            };
        }
        private static ImgItem NovoImgItem(string nomeItem)
        {
            return new ImgItem
            {
                IdItem = GetIdNovoItem(nomeItem),
                Principal = true,
                DataAdicao = DateTime.Now
            };
        }
        private static void SalvarNovoImgItem(ImgItem imgItem)
        {
            using (var context = new ContextEF())
            {
                context.ImagensItem.Add(imgItem);
                context.SaveChanges();
            };
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


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
