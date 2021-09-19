using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPage.Models
{
    public class ImgItem
    {
        public int Id { get; set; }
        public long IdItem { get; set; }
        public byte[] Img { get; set; }
        public bool Principal { get; set; }
        public DateTime DataAdicao { get; set; }
    }
}
