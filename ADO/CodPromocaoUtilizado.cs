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

        public static CodPromocaoUtilizado GeraObj(CodPromocao codPromocao, Carrinho carrinho, int idUsuario)
        {
            return new CodPromocaoUtilizado
            {
                IdUsuario = idUsuario,
                IdCarrinho = carrinho.Id,
                IdCodPromocao = codPromocao.Id,
                DataUtilizacao = DateTime.UtcNow.AddHours(-3)
            };
        }
    }
}
