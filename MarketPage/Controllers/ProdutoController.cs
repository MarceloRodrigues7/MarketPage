using MarketPage.Context;
using MarketPage.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPage.Controllers
{
    public class ProdutoController : Controller
    {
        [HttpGet]
        public IActionResult Index(long Id)
        {
            var data = GetProduto(Id);
            return View(data);
        }

        private static ItemViewProduto GetProduto(long idProduto)
        {
            using (var context = new ContextEF())
            {
                var item = context.Itens.Where(i => i.Id == idProduto).First();
                var img = context.ImagensItem.Where(i => i.IdItem == idProduto && i.Principal == true).First();
                var imgsPadrao = context.ImagensItem.Where(i => i.IdItem == idProduto && i.Principal == false).Select(i=>i.Img).Take(4).ToList();
                return NovoItemViewProduto(item, img, imgsPadrao);
            };
        }

        private static ItemViewProduto NovoItemViewProduto(Item item, ImgItem img, List<byte[]> imgsPadrao)
        {
            return new ItemViewProduto
            {
                Id = item.Id,
                Nome = item.Nome,
                Descricao = item.Descricao,
                Valor = item.Valor,
                Tamanhos = item.Tamanhos,
                Quantidade = item.Quantidade,
                IdCategoria = item.IdCategoria,
                Img = img.Img,
                ImgsPadrao=imgsPadrao
            };
        }
    }
}
