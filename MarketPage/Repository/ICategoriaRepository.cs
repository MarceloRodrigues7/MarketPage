using MarketPage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPage.Repository
{
    public interface ICategoriaRepository
    {
        Categoria GetCategoria(int id);
        Categoria GetCategoria(string nome);
        List<Categoria> GetCategorias();
        List<Categoria> GetCategoriasComItens();
        void PostCategoria(Categoria categoria);
        void PutCategoria(Categoria categoria);
        void DeleteCategoria(Categoria categoria);
    }
}
