using MarketPage.Context;
using MarketPage.Models;
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
        private readonly ILogger<ShopController> _logger;
        private readonly IFunc_Categoria _Categoria;

        public ShopController(ILogger<ShopController> logger, IFunc_Categoria categoria)
        {
            _logger = logger;
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
                return context.Itens.ToList();
            };
        }
        private static List<Item> GetItems(int categoria)
        {
            using (var context = new ContextEF())
            {
                return context.Itens.Where(i=>i.IdCategoria==categoria).ToList();
            };
        }

        private static List<ImgItem> GetImgs()
        {
            using (var context = new ContextEF())
            {
                return context.ImagensItem.ToList();
            };
        }

        private static List<ItemViewShop> GeraListaItemImagem(List<Item> items, List<ImgItem> imgs)
        {
            var lista = new List<ItemViewShop>();
            foreach (var item in items)
            {
                var itemViewShop = new ItemViewShop
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
