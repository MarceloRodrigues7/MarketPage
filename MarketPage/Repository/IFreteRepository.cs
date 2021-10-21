using MarketPage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPage.Repository
{
    public interface IFreteRepository
    {
        FretePedidoUsuario GetFretePedido(int idUsuario, long idCarrinho);
        public void PostFrete(FretePedidoUsuario frete);
    }
}
