using ADO;
using System.Collections.Generic;

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
