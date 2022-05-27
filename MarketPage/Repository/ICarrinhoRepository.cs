using ADO;
using MarketPage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPage.Repository
{
    public interface ICarrinhoRepository
    {
        List<Carrinho> GetCarrinhos(long idPedido);
        List<Carrinho> GetCarrinho(int idUsuario);
        List<ItemViewProduto> GetItensCarrinhoView(int idUsuario);
        void UpdateItensCarrinhoRealizado(long idPedido, List<Carrinho> carrinho);
        void DeleteItemCarrinho(long item);
        void DeleteItemCarrinhoAdmin(long idItem);
    }
}
