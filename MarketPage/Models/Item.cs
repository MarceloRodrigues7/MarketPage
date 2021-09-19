using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPage.Models
{
    public class Item
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public int Quantidade { get; set; }
        public bool Destaque { get; set; }
        public DateTime DataAdicao { get; set; }
        public int IdCategoria { get; set; }
    }

    public class ItemViewShop
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public decimal Valor { get; set; }
        public byte[] Img { get; set; }
        public int IdCategoria { get; set; }
    }
}
