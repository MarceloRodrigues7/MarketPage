﻿using MarketPage.Context;
using MarketPage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPage.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var itens = GetItems().Where(i => i.Destaque == true).ToList();
            var imgs = GetImgs();
            ViewBag.Itens = GeraListaItemImagem(itens, imgs);
            ViewBag.Categorias = GetCategorias();
            return View();
        }

        public IActionResult Contato()
        {
            if (User.IsInRole("Usuario_Comum") || User.IsInRole("Usuario_Admin"))
            {
                Usuario usuario;
                using (var context = new ContextEF())
                {
                    usuario = context.Usuarios.Where(u => u.Id == int.Parse(User.Identity.Name)).FirstOrDefault();
                };
                ViewBag.Usuario = usuario;
            }
            return View();
        }

        private List<Categoria> GetCategorias()
        {
            using (var context = new ContextEF())
            {
                return context.Categorias.Where(c => c.Ativo == true).ToList();
            };
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private List<Item> GetItems()
        {
            using (var context = new ContextEF())
            {
                return context.Itens.ToList();
            };
        }

        private List<ImgItem> GetImgs()
        {
            using (var context = new ContextEF())
            {
                return context.ImagensItem.ToList();
            };
        }

        private List<ItemViewShop> GeraListaItemImagem(List<Item> items, List<ImgItem> imgs)
        {
            var lista = new List<ItemViewShop>();
            foreach (var item in items)
            {
                var itemViewShop = new ItemViewShop
                {
                    Id = item.Id,
                    Nome = item.Nome,
                    Valor = item.Valor,
                    Img = imgs.First(i => i.IdItem == item.Id).Img,
                    IdCategoria = item.IdCategoria

                };
                lista.Add(itemViewShop);
            }
            return lista;
        }
    }
}