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
        List<ItemViewProduto> GetItensCarrinhoView(int idUsuario);
        void DeleteItemCarrinhoAdmin(long idItem);
    }
}
