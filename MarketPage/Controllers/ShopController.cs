using MarketPage.Context;
using MarketPage.Models;
using MarketPage.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPage.Controllers
{
    public class ShopController : Controller
    {
        private readonly ICategoriaRepository _Categoria;

        public ShopController(ICategoriaRepository categoria)
        {
            _Categoria = categoria;
        }

        [HttpGet("Shop")]
        public IActionResult Index()
        {
            var itens = GetItems();
            var imagens = GetImgs();
            var data = GeraListaItemImagem(itens, imagens);
            return View(data);
        }

        [HttpGet("Shop/{categoria}")]
        public IActionResult Index(string categoria)
        {
            var idCategoria = _Categoria.GetCategoria(categoria).Id;
            var itens = GetItems(idCategoria);
            var imagens = GetImgs();
            var data = GeraListaItemImagem(itens, imagens);
            return View(data);
        }

        private static List<Item> GetItems()
        {
            using (var context = new ContextEF())
            {
                return context.Itens.Where(i => i.Quantidade > 0).ToList();
            };
        }

        private static List<Item> GetItems(int categoria)
        {
            using (var context = new ContextEF())
            {
                return context.Itens.Where(i => i.IdCategoria == categoria && i.Quantidade > 0).ToList();
            };
        }

        private static List<ImgItem> GetImgs()
        {
            using (var context = new ContextEF())
            {
                return context.ImagensItem.ToList();
            };
        }

        private static List<ItemView> GeraListaItemImagem(List<Item> items, List<ImgItem> imgs)
        {
            var lista = new List<ItemView>();
            foreach (var item in items)
            {
                var itemViewShop = new ItemView
                {
                    Id = item.Id,
                    Nome = item.Nome,
                    Valor = item.Valor,
                    Tamanhos = item.Tamanhos,
                    Img = imgs.First(i => i.IdItem == item.Id && i.Principal == true).Img
                };
                lista.Add(itemViewShop);
            }
            return lista;
        }
    }
}
