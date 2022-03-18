using Microsoft.AspNetCore.Mvc;
using Refit;
using System.Threading.Tasks;
using static MarketPage.Models.ResponseMercadoPagoGetOrder;

namespace MarketPage.Services
{
    public class MercadoPagoService
    {
        private const string token = "APP_USR-1223540178250481-092615-8aee4b2461ec8e00fd5f066bcbd83d26-194500220";
        public Root GetPedidoMercadoPago(string id)
        {
            var req = RestService.For<IMercadoPagoService>("https://api.mercadopago.com");
            var root = req.GetPedido(id);
            var task = root.GetAwaiter();
            return task.GetResult();
        }

        public interface IMercadoPagoService
        {
            [Get("/merchant_orders/search?preference_id={id}")]
            [Headers("Authorization: Bearer " + token)]
            Task<Root> GetPedido(string id);
        }
    }
}
