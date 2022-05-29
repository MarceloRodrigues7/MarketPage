using ADO;
using MarketPage.Models;
using MarketPage.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MarketPage.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IMessagesContatoRepository _messagesContatoRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IImagemRepository _imagemRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public HomeController(ICategoriaRepository categoriaRepository, IMessagesContatoRepository messagesContatoRepository, IItemRepository itemRepository, IImagemRepository imagemRepository, IUsuarioRepository usuarioRepository)
        {
            _categoriaRepository = categoriaRepository;
            _messagesContatoRepository = messagesContatoRepository;
            _itemRepository = itemRepository;
            _imagemRepository = imagemRepository;
            _usuarioRepository = usuarioRepository;
        }

        public IActionResult Index()
        {
            var itens = _itemRepository.GetItens().Where(i => i.Destaque == true && i.Quantidade > 0).ToList();
            var imgs = _imagemRepository.GetImgPrincipalItens();
            ViewBag.Itens = GeraListaItemImagem(itens, imgs);
            ViewBag.Categorias = _categoriaRepository.GetCategoriasComItens();
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
            if (User.IsInRole("Usuario_Comum") || User.IsInRole("Admin"))
            {
                var usuario = _usuarioRepository.GetUsuario(int.Parse(User.Identity.Name));
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

        private List<ItemView> GeraListaItemImagem(List<Item> items, IEnumerable<ImgItem> imgs)
        {
            var lista = new List<ItemView>();
            foreach (var item in items)
            {
                lista.Add(new ItemView
                {
                    Id = item.Id,
                    Nome = item.Nome,
                    Valor = item.Valor,
                    Img = imgs.First(i => i.IdItem == item.Id).Img,
                    IdCategoria = item.IdCategoria
                });
            }
            return lista;
        }

        public IActionResult PostMessageContato(MessageContato message)
        {
            try
            {
                _messagesContatoRepository.PostMensage(message);
                TempData["Message"] = "Mensagem enviada com sucesso, aguarde retorno.";
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["Message"] = "Erro ao enviar mensagem, tente novamente!";
                return Contato();
            }
        }
    }
}
