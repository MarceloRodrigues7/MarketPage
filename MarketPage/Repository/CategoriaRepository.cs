using ADO;
using MarketPage.Context;
using MarketPage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPage.Repository
{
    public class CategoriaRepository : ICategoriaRepository
    {
        public List<Categoria> GetCategorias()
        {
            using (var db = new ContextEF())
            {
                return db.Categorias.ToList();
            };
        }

        public List<Categoria> GetCategoriasComItens()
        {
            using (var db = new ContextEF())
            {
                var catItens = db.Itens.Where(i => i.Quantidade > 0).Select(i => i.IdCategoria).Distinct();
                var data = db.Categorias.Where(c => c.Ativo == true && catItens.Contains(c.Id));
                return data.ToList();
            };
        }

        public Categoria GetCategoria(int id)
        {
            using (var db = new ContextEF())
            {
                return db.Categorias.Where(c => c.Id == id).FirstOrDefault();
            };
        }
        public Categoria GetCategoria(string nome)
        {
            using (var db = new ContextEF())
            {
                return db.Categorias.Where(c => c.Nome == nome).FirstOrDefault();
            };
        }
        public void PostCategoria(Categoria categoria)
        {
            using (var db = new ContextEF())
            {
                categoria.DataAdicao = DateTime.Now;
                db.Categorias.Add(categoria);
                db.SaveChanges();
            };
        }
        public void PutCategoria(Categoria categoria)
        {
            using (var db = new ContextEF())
            {
                categoria.DataAdicao = DateTime.Now;
                db.Categorias.Update(categoria);
                db.SaveChanges();
            };
        }
        public void DeleteCategoria(Categoria categoria)
        {
            using (var db = new ContextEF())
            {
                db.Itens.RemoveRange(db.Itens.Where(i => i.IdCategoria == categoria.Id));
                db.Categorias.Remove(categoria);
                db.SaveChanges();
            };
        }
    }
}
