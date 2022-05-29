using ADO;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MarketPage.Models
{
    public class ViewItemAdmAddEEdit : Item
    {
        [DataType(DataType.Upload)]
        public IFormFile ImageUploadMain { get; set; }
        [DataType(DataType.Upload)]
        public IFormFile[] ImageUpload { get; set; }
        public string ValorString { get; set; }
        public string PesoString { get; set; }

        public static ViewItemAdmAddEEdit GeraObj(ADO.Item item)
        {
            return new ViewItemAdmAddEEdit
            {
                Id = item.Id,
                Nome = item.Nome,
                Descricao = item.Descricao,
                ValorString = item.Valor.ToString(),
                Tamanhos = item.Tamanhos,
                Quantidade = item.Quantidade,
                Destaque = item.Destaque,
                IdCategoria = item.IdCategoria,
                PesoString = item.Peso.ToString()
            };
        }
    }
}
