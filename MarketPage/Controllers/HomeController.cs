using MarketPage.Context;
using MarketPage.Models;
using MarketPage.Repository;
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
        private readonly ICategoriaRepository _Categoria;

        public HomeController(ILogger<HomeController> logger, ICategoriaRepository categoria)
        {
            _logger = logger;
            _Categoria = categoria;
        }

        public IActionResult Index()
        {
            var itens = GetItems().Where(i => i.Destaque == true).ToList();
            var imgs = GetImgs();
            ViewBag.Itens = GeraListaItemImagem(itens, imgs);
            ViewBag.Categorias = _Categoria.GetCategorias();
            return View();
        }

        public IActionResult Termos()
        {
            return View();
        }

        public IActionResult FormasPagamento()
        {
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
                MessageContato message = new()
                {
                    Nome = usuario.Nome,
                    Email = usuario.Email
                };
                return View(message);
            }
            return View();
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
                    Img = imgs.First(i => i.IdItem == item.Id && i.Principal == true).Img,
                    IdCategoria = item.IdCategoria

                };
                lista.Add(itemViewShop);
            }
            return lista;
        }

        public IActionResult PostMessageContato(MessageContato message)
        {
            using (var context = new ContextEF())
            {
                message.DataEnvio = DateTime.Now;
                message.Visualizado = false;
                context.MessagesContato.Add(message);
                context.SaveChanges();
                TempData["Message"] = "Mensagem enviada com sucesso, aguarde retorno.";
                return RedirectToAction("Index");
            };
        }
    }
}
