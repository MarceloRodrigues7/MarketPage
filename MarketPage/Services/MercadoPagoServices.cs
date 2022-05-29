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

            var request = new PreferenceRequest
            {
                Items = GeraItem(pedido)
            };

            var preference = GeraPreference(request);

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
            using (var resposta = webRequest.GetResponse())
            {
                var streamDados = resposta.GetResponseStream();
                try
                {
                    StreamReader reader = new(streamDados);
                    object objResponse = reader.ReadToEnd();
                    var post = JsonConvert.DeserializeObject<Root>(objResponse.ToString());
                    return post;
                }
                finally
                {
                    streamDados.Close();
                    resposta.Close();
                }
            };
        }

        private static List<PreferenceItemRequest> GeraItem(Pedido pedido)
        {
            List<PreferenceItemRequest> itens = new();

            itens.Add(new PreferenceItemRequest
            {
                Title = $"Pedido nº{pedido.IdUsuario}{pedido.Id}",
                Quantity = 1,
                CurrencyId = "BRL",
                UnitPrice = pedido.ValorTotal,
                Id = pedido.Id.ToString()
            });

            return itens;
        }

        private static Preference GeraPreference(PreferenceRequest preferenceRequest)
        {
            var client = new PreferenceClient();
            return client.Create(preferenceRequest);
        }

    }
}
