using ADO;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
using MercadoPago.Resource.Preference;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;
using static MarketPage.Models.ResponseMercadoPagoGetOrder;

namespace MarketPage.Services
{
    public class MercadoPagoServices
    {
        private readonly string _urlMercadoPago = $"https://api.mercadopago.com/merchant_orders/search?preference_id=";

        public Root GetPedidoMercadoPago(string id, string token)
        {
            var webRequest = WebRequest.CreateHttp(_urlMercadoPago + id);
            var webHeaderCollection = GeraWebHeaderCollection(token);

            webRequest.Method = "GET";
            webRequest.Headers = webHeaderCollection;

            return GetPedido(webRequest);
        }

        public string MercadoPagoRequest(Pedido pedido, string token)
        {
            MercadoPagoConfig.AccessToken = token;
            PreferenceRequest request = new();
            Preference preference;

            request.Items = GeraPedido(pedido);
            preference = GeraPreference(request);

            return preference.Id;
        }

        public string GetIdPedido(Pedido pedido, string token)
        {
            if (pedido.IdMercadoPago == null)
            {
                return MercadoPagoRequest(pedido, token);
            }
            else
            {
                return pedido.IdMercadoPago;
            }
        }

        private static WebHeaderCollection GeraWebHeaderCollection(string token)
        {
            WebHeaderCollection webHeader = new();
            webHeader.Add("Authorization", $"Bearer {token}");
            return webHeader;
        }

        private static Root GetPedido(WebRequest webRequest)
        {
            using (var webResponse = webRequest.GetResponse())
            {
                var responseStream = webResponse.GetResponseStream();
                try
                {
                    StreamReader reader = new(responseStream);
                    object response = reader.ReadToEnd();
                    return JsonConvert.DeserializeObject<Root>(response.ToString());
                }
                finally
                {
                    responseStream.Close();
                    webResponse.Close();
                }
            };
        }

        private static List<PreferenceItemRequest> GeraPedido(Pedido pedido)
        {
            return new List<PreferenceItemRequest>()
            {
                new PreferenceItemRequest
                {
                    Title = $"Pedido nº{pedido.IdUsuario}{pedido.Id}",
                    Quantity = 1,
                    CurrencyId = "BRL",
                    UnitPrice = pedido.ValorTotal,
                    Id = pedido.Id.ToString()
                }
            };
        }

        private static Preference GeraPreference(PreferenceRequest preferenceRequest)
        {
            var client = new PreferenceClient();
            return client.Create(preferenceRequest);
        }

    }
}
