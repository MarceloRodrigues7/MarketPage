using MarketPage.Models;
using MarketPage.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarketPage.Controllers
{
    public class FormasPagamentoController : Controller
    {
        private readonly IFormaPagamentoRepository _formaPagamentoRepository;

        public FormasPagamentoController(IFormaPagamentoRepository formaPagamentoRepository)
        {
            _formaPagamentoRepository = formaPagamentoRepository;
        }

        [Authorize]
        public IActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                var data = _formaPagamentoRepository.GetFormaPagamento();
                return View(data);
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult EditarDados(FormaPagamento formaPagamento)
        {
            if (User.IsInRole("Admin"))
            {
                TempData["Message"] = "Dados alterados com sucesso.";
                _formaPagamentoRepository.Update(formaPagamento);
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
