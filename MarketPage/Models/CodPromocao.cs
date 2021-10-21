using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPage.Models
{
    public class CodPromocao
    {
        public long Id { get; set; }
        public string Codigo { get; set; }
        public decimal Desconto { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFinal { get; set; }
        public bool Ativo { get; set; }
        public int Utilizacoes { get; set; }
    }
    public class CodPromocaoUtilizado
    {
        public long Id { get; set; }
        public long IdCodPromocao { get; set; }
        public int IdUsuario { get; set; }
        public long IdCarrinho { get; set; }
        public DateTime DataUtilizacao { get; set; }
    }
}
