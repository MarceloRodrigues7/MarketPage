using ADO;
using MarketPage.Models;
using System.Collections.Generic;

namespace MarketPage.Repository
{
    public interface ICarrinhoRepository
    {
        List<Carrinho> GetCarrinhos(long idPedido);
        List<Carrinho> GetCarrinho(int idUsuario);
        List<ItemViewProduto> GetItensCarrinhoView(int idUsuario);
        void PostItemCarrinho(Carrinho carrinho);
        void UpdateItensCarrinhoRealizado(long idPedido, List<Carrinho> carrinho);
        void DeleteItemCarrinho(long item);
        void DeleteItemCarrinhoAdmin(long idItem);
    }
}
