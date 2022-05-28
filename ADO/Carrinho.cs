using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO
{
    public class Carrinho
    {
        public long Id { get; set; }
        public int IdUsuario { get; set; }
        public long IdItem { get; set; }
        public decimal Valor { get; set; }
        public string Tamanhos { get; set; }
        public int Quantidade { get; set; }
        public DateTime DataHora { get; set; }
        public long? IdPedido { get; set; }

        public static Carrinho GeraObj(Item item, int idUsuario)
        {
            return new Carrinho
            {
                IdItem = item.Id,
                IdUsuario = idUsuario,
                Quantidade = item.Quantidade,
                Tamanhos = item.Tamanhos,
                Valor = item.Valor,
                DataHora = DateTime.Now
            };
        }
    }
}
