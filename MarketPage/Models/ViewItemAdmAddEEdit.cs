using ADO;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
    }
}
