using MarketPage.Models;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
using MercadoPago.Resource.Preference;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Refit;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using static MarketPage.Models.ResponseMercadoPagoGetOrder;

namespace MarketPage.Services
{
    public class MercadoPagoServices
    {
        public Root GetPedidoMercadoPago(string id, string token)
        {            
            var root = GetPedidoMG(id, token);
            return root;
        }

        public Root GetPedidoMG(string id, string token)
        {
            var requisicaoWeb = WebRequest.CreateHttp($"https://api.mercadopago.com/merchant_orders/search?preference_id={id}");
            requisicaoWeb.Method = "GET";
            var h = new WebHeaderCollection();
            h.Add(token);
            requisicaoWeb.Headers = h;
            using (var resposta = requisicaoWeb.GetResponse())
            {
                var streamDados = resposta.GetResponseStream();
                StreamReader reader = new StreamReader(streamDados);
                object objResponse = reader.ReadToEnd();
                var post = JsonConvert.DeserializeObject<Root>(objResponse.ToString());
                streamDados.Close();
                resposta.Close();
                return post;
            };
        }

        public string MercadoPagoRequest(Pedido pedido)
        {
            MercadoPagoConfig.AccessToken = "APP_USR-1223540178250481-092615-8aee4b2461ec8e00fd5f066bcbd83d26-194500220";
            var request = new PreferenceRequest
            {
                Items = new List<PreferenceItemRequest>
                {
                    new PreferenceItemRequest
                    {
                        Title=$"Pedido nº{pedido.IdUsuario}{pedido.Id}",
                        Quantity=1,
                        CurrencyId="BRL",
                        UnitPrice=pedido.ValorTotal,
                        Id=pedido.Id.ToString()
                    }
                }
            };
            var client = new PreferenceClient();
            Preference preference = client.Create(request);
            return preference.Id;
        }

        public static Preference GetPreferenceMP(string id)
        {
            MercadoPagoConfig.AccessToken = "APP_USR-1223540178250481-092615-8aee4b2461ec8e00fd5f066bcbd83d26-194500220";
            var client = new PreferenceClient();
            return client.Get(id);
        }
    }
}
