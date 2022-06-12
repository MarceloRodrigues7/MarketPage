using ADO;
using MarketPage.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace MarketPage.Controllers
{
    public class CategoriaController : Controller
    {
        #region Propriedades
        private readonly ICategoriaRepository _categoria;
        #endregion

        #region Construtores
        public CategoriaController(ICategoriaRepository categoria)
        {
            _categoria = categoria;
        }
        #endregion

        [Authorize]
        public IActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                return View(_categoria.GetCategorias());
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
        public IActionResult Editar(Categoria item)
        {
            if (User.IsInRole("Admin"))
            {
                return View(item);
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult Deletar(Categoria item)
        {
            if (User.IsInRole("Admin"))
            {
                return View(item);
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult PostCategoria(Categoria categoria)
        {
            if (User.IsInRole("Admin"))
            {
                try
                {
                    _categoria.PostCategoria(categoria);
                    return RedirectToAction("Index", "Categoria");
                }
                catch (Exception e)
                {
                    TempData["Message"] = "Ocorreu algum erro, tente novamente! " + e.Message;
                    return RedirectToAction("Adicionar", "Categoria");
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult PutCategoria(Categoria categoria)
        {
            if (User.IsInRole("Admin"))
            {
                try
                {
                    _categoria.PutCategoria(categoria);
                    return RedirectToAction("Index", "Categoria");
                }
                catch (Exception e)
                {
                    TempData["Message"] = "Ocorreu algum erro, tente novamente! " + e.Message;
                    return RedirectToAction("Editar", "Categoria", categoria);
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult DeleteCategoria(Categoria categoria)
        {
            if (User.IsInRole("Admin"))
            {
                try
                {
                    _categoria.DeleteCategoria(categoria);
                    return RedirectToAction("Index", "Categoria");
                }
                catch (Exception e)
                {
                    TempData["Message"] = "Ocorreu algum erro, tente novamente! " + e.Message;
                    return RedirectToAction("Delete", "Categoria");
                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
