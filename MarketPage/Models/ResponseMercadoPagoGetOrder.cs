using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MarketPage.Models
{
    public class ResponseMercadoPagoGetOrder
    {
        public class Payment
        {
            [JsonProperty("id")]
            public long Id { get; set; }

            [JsonProperty("transaction_amount")]
            public double TransactionAmount { get; set; }

            [JsonProperty("total_paid_amount")]
            public double TotalPaidAmount { get; set; }

            [JsonProperty("shipping_cost")]
            public int ShippingCost { get; set; }

            [JsonProperty("currency_id")]
            public string CurrencyId { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("status_detail")]
            public string StatusDetail { get; set; }

            [JsonProperty("operation_type")]
            public string OperationType { get; set; }

            [JsonProperty("date_approved")]
            public DateTime DateApproved { get; set; }

            [JsonProperty("date_created")]
            public DateTime DateCreated { get; set; }

            [JsonProperty("last_modified")]
            public DateTime LastModified { get; set; }

            [JsonProperty("amount_refunded")]
            public int AmountRefunded { get; set; }
        }

        public class Collector
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("email")]
            public string Email { get; set; }

            [JsonProperty("nickname")]
            public string Nickname { get; set; }
        }

        public class Payer
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("email")]
            public string Email { get; set; }
        }

        public class Item
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("category_id")]
            public string CategoryId { get; set; }

            [JsonProperty("currency_id")]
            public string CurrencyId { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; }

            [JsonProperty("picture_url")]
            public object PictureUrl { get; set; }

            [JsonProperty("title")]
            public string Title { get; set; }

            [JsonProperty("quantity")]
            public int Quantity { get; set; }

            [JsonProperty("unit_price")]
            public double UnitPrice { get; set; }
        }

        public class Element
        {
            [JsonProperty("id")]
            public long Id { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("external_reference")]
            public string ExternalReference { get; set; }

            [JsonProperty("preference_id")]
            public string PreferenceId { get; set; }

            [JsonProperty("payments")]
            public List<Payment> Payments { get; set; }

            [JsonProperty("shipments")]
            public List<object> Shipments { get; set; }

            [JsonProperty("payouts")]
            public List<object> Payouts { get; set; }

            [JsonProperty("collector")]
            public Collector Collector { get; set; }

            [JsonProperty("marketplace")]
            public string Marketplace { get; set; }

            [JsonProperty("notification_url")]
            public object NotificationUrl { get; set; }

            [JsonProperty("date_created")]
            public string DateCreated { get; set; }

            [JsonProperty("last_updated")]
            public string LastUpdated { get; set; }

            [JsonProperty("sponsor_id")]
            public object SponsorId { get; set; }

            [JsonProperty("shipping_cost")]
            public int ShippingCost { get; set; }

            [JsonProperty("total_amount")]
            public double TotalAmount { get; set; }

            [JsonProperty("site_id")]
            public string SiteId { get; set; }

            [JsonProperty("paid_amount")]
            public double PaidAmount { get; set; }

            [JsonProperty("refunded_amount")]
            public int RefundedAmount { get; set; }

            [JsonProperty("payer")]
            public Payer Payer { get; set; }

            [JsonProperty("items")]
            public List<Item> Items { get; set; }

            [JsonProperty("cancelled")]
            public bool Cancelled { get; set; }

            [JsonProperty("additional_info")]
            public string AdditionalInfo { get; set; }

            [JsonProperty("application_id")]
            public object ApplicationId { get; set; }

            [JsonProperty("order_status")]
            public string OrderStatus { get; set; }
        }

        public class Root
        {
            [JsonProperty("elements")]
            public List<Element> Elements { get; set; }

            [JsonProperty("next_offset")]
            public int NextOffset { get; set; }

            [JsonProperty("total")]
            public int Total { get; set; }
        }
    }
}
