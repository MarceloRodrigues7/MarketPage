using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO
{
    public class CodPromocaoUtilizado
    {
        public long Id { get; set; }
        public long IdCodPromocao { get; set; }
        public int IdUsuario { get; set; }
        public long IdCarrinho { get; set; }
        public DateTime DataUtilizacao { get; set; }
    }
}
