using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

    public class ItemImagem
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public int Quantidade { get; set; }
        public bool Destaque { get; set; }
        public int Categoria { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile ImageUpload { get; set; }
    }

    public class ItemViewShop
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public decimal Valor { get; set; }
        public int Quantidade { get; set; }
        public byte[] Img { get; set; }
        public int IdCategoria { get; set; }
    }

    public class ItemViewProduto
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public int Quantidade { get; set; }
        public int IdCategoria { get; set; }
        public byte[] Img { get; set; }
    }

    public class Carrinho
    {
        public long Id { get; set; }
        public int IdUsuario { get; set; }
        public long IdItem { get; set; }
        public decimal Valor { get; set; }
        public int Quantidade { get; set; }
        public DateTime DataHora { get; set; }
    }

    public class ItemViewAdmin
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public int Quantidade { get; set; }
        public bool Destaque { get; set; }
        public DateTime DataAdicao { get; set; }
        public string Categoria { get; set; }
    }

}
