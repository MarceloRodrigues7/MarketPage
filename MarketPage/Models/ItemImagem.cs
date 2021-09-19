using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPage.Models
{
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
}
