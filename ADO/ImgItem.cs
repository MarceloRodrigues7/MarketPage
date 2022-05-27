using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO
{
    public class ImgItem
    {
        public int Id { get; set; }
        public long IdItem { get; set; }
        public byte[] Img { get; set; }
        public bool Principal { get; set; }
        public string Tamanhos { get; set; }
        public DateTime DataAdicao { get; set; }
    }
}
