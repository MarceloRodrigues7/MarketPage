using MarketPage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPage.Repository
{
    public interface IFreteRepository
    {
        public void PostFrete(FretePedidoUsuario frete);
    }
}
