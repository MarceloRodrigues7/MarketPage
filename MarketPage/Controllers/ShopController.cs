using MarketPage.Context;
using MarketPage.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPage.Controllers
{
    public class ShopController : Controller
    {
        public IActionResult Index()
        {
            var itens = GetItems();
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
                    Tamanhos=item.Tamanhos,
                    Img = imgs.First(i => i.IdItem == item.Id).Img
                };
                lista.Add(itemViewShop);
            }
            return lista;
        }
    }
}
