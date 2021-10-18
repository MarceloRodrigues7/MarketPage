using MarketPage.Context;
using MarketPage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPage.Repository
{
    public class FreteRepository : IFreteRepository
    {
        public void PostFrete(FretePedidoUsuario frete)
        {
            using (var context = new ContextEF())
            {
                context.FretesPedidosUsuarios.Add(frete);
            };
        }
    }
}
