using MarketPage.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MarketPage.Models.ResponseMercadoPagoGetOrder;

namespace MarketPage.Controllers
{
    public class RefitRepository
    {
        public Root GetPedidoMercadoPago(string id)
        {
            var req = RestService.For<IRefitRepository>("https://api.mercadopago.com");
            var root = req.GetPedido(id);
            return root.Result;
        }

        public interface IRefitRepository
        {
            [Get("/merchant_orders/search?preference_id={id}")]
            [Headers("Authorization: Bearer APP_USR-1223540178250481-092615-8aee4b2461ec8e00fd5f066bcbd83d26-194500220")]
            Task<Root> GetPedido(string id);
        }
    }
}
