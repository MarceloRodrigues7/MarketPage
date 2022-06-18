using ADO;
using MarketPage.Models;
using MarketPage.Repository;
using Microsoft.AspNetCore.Mvc;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MarketPage.Controllers
{
    public class ShopController : Controller
    {
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IImagemRepository _imagemRepository;

        public ShopController(ICategoriaRepository categoria, IItemRepository itemRepository, IImagemRepository imagemRepository)
        {
            _categoriaRepository = categoria;
            _itemRepository = itemRepository;
            _imagemRepository = imagemRepository;
        }

        [HttpGet("Shop")]
        public IActionResult Index(int? pagina, [FromQuery] Pesquisa item = null)
        {
            var itens = new List<Item>();

            if (item == null || string.IsNullOrEmpty(item.Nome))
            {
                itens = GetItens();
            }
            else
            {
                itens = GetItens(item.Nome);
            }

            var imagens = GetImgs(itens.Select(i => i.Id));
            var data = GeraListaItemImagem(itens, imagens);

            int paginaTamanho = 20;
            int paginaNumero = (pagina ?? 1);

            if (data.Count <= 0)
            {
                TempData["Message"] = "Nenhum produto localizado";
            }

            return View(data.ToPagedList(paginaNumero, paginaTamanho));
        }

        [HttpGet("Shop/{categoria}")]
        public IActionResult Index(string categoria)
        {
            var idCategoria = _categoriaRepository.GetCategoria(categoria).Id;
            var itens = GetItens(idCategoria);
            var imagens = GetImgs(itens.Select(i => i.Id));
            var data = GeraListaItemImagem(itens, imagens);
            return View(data);
        }

        private List<Item> GetItens()
        {
            try
            {
                var itens = _itemRepository.GetItens().Where(i => i.Quantidade > 0);
                return itens.ToList();
            }
            catch (Exception)
            {
                return new List<Item>();
            }
        }

        private List<Item> GetItens(int categoria)
        {
            try
            {
                var itens = _itemRepository.GetItens().Where(i => i.IdCategoria == categoria && i.Quantidade > 0);
                return itens.ToList();
            }
            catch (Exception)
            {
                return new List<Item>();
            }
        }
        private List<Item> GetItens(string nome)
        {
            try
            {
                var itens = _itemRepository.GetItens().Where(i => i.Nome.Contains(nome));
                return itens.ToList();
            }
            catch (Exception)
            {
                return new List<Item>();
            }
        }

        private List<ImgItem> GetImgs(IEnumerable<long> listIdItens)
        {
            try
            {
                var newList = new List<ImgItem>();
                foreach (var idItem in listIdItens)
                {
                    var img = _imagemRepository.GetImgPrincipalPorId(idItem);
                    newList.Add(img);
                }
                return newList;
            }
            catch (Exception)
            {
                return new List<ImgItem>();
            }
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
                    Img = imgs.First(i => i.IdItem == item.Id).Img
                };
                lista.Add(itemViewShop);
            }
            return lista;
        }
    }
}
