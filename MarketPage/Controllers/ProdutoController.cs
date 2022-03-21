using MarketPage.Context;
using MarketPage.Models;
using MarketPage.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPage.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly IItemRepository _itemRepository;
        private readonly IImagemRepository _imagemRepository;

        public ProdutoController(IImagemRepository imagemRepository, IItemRepository itemRepository)
        {
            _imagemRepository = imagemRepository;
            _itemRepository = itemRepository;
        }

        [HttpGet]
        public IActionResult Index(long Id)
        {
            var data = GetProduto(Id);
            return View(data);
        }

        private ItemViewProduto GetProduto(long idProduto)
        {
            var item = _itemRepository.GetItem(idProduto);
            var img = _imagemRepository.GetImgPrincipalPorId(idProduto);
            var imgsPadrao = _imagemRepository.GetDemaisImagensPorId(idProduto);
            return NovoItemViewProduto(item, img, imgsPadrao);
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
                ImgsPadrao = imgsPadrao
            };
        }
    }
}
