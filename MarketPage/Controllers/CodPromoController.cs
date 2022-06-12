using ADO;
using MarketPage.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace MarketPage.Controllers
{
    public class CodPromoController : Controller
    {
        #region Propriedades
        private readonly ICodPromocionalRepository _codPromocional;
        #endregion

        #region Construtores
        public CodPromoController(ICodPromocionalRepository codPromocional)
        {
            _codPromocional = codPromocional;
        }
        #endregion

        [Authorize]
        public IActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                var data = _codPromocional.GetList();
                return View(data);
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult Adicionar()
        {
            if (User.IsInRole("Admin"))
            {
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult Editar(long id)
        {
            if (User.IsInRole("Admin"))
            {
                var codPromocao = _codPromocional.GetCodPromocao(id);
                codPromocao.Desconto *= 100;
                return View(codPromocao);
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult Deletar(long Id)
        {
            if (User.IsInRole("Admin"))
            {
                var codPromocao = _codPromocional.GetCodPromocao(Id);
                return View(codPromocao);
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult Put(CodPromocao codPromocao)
        {
            if (User.IsInRole("Admin"))
            {
                try
                {
                    AtualizarCodPromocao(codPromocao);
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    TempData["Message"] = "Ocorreu algum erro, tente novamente! " + e.Message;
                    return RedirectToAction("EditarCodPromo");
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Delete(CodPromocao codPromocao)
        {
            if (User.IsInRole("Admin"))
            {
                try
                {
                    _codPromocional.Delete(codPromocao.Id);
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    TempData["Message"] = "Ocorreu algum erro, tente novamente! " + e.Message;
                    return RedirectToAction("DeletarCodPromo");
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult Post(CodPromocao codPromocao)
        {
            if (User.IsInRole("Admin"))
            {
                try
                {
                    AdicionarCodPromocao(codPromocao);
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    TempData["Message"] = "Ocorreu algum erro, tente novamente! " + e.Message;
                    return RedirectToAction("Adicionar");
                }
            }
            return RedirectToAction("Index", "Home");
        }

        private void AtualizarCodPromocao(CodPromocao codPromocao)
        {
            codPromocao.Desconto *= 0.01m;
            _codPromocional.Put(codPromocao);
        }

        private void AdicionarCodPromocao(CodPromocao codPromocao)
        {
            codPromocao.Utilizacoes = 0;
            codPromocao.Desconto *= 0.01m;
            _codPromocional.Post(codPromocao);
        }

    }
}
