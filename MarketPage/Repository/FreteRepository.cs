using ADO;
using System.Collections.Generic;
using System.Linq;

namespace MarketPage.Repository
{
    public class FreteRepository : IFreteRepository
    {
        public FretePedidoUsuario GetFretePedido(int idUsuario, long idCarrinho)
        {
            using (var context = new ContextEF())
            {
                return context.FretesPedidosUsuarios.Where(f => f.IdUsuario == idUsuario && f.IdCarrinho == idCarrinho).FirstOrDefault();
            };
        }

        public void PostFrete(FretePedidoUsuario frete)
        {
            using (var context = new ContextEF())
            {
                context.FretesPedidosUsuarios.Add(frete);
                context.SaveChanges();
            };
        }

        public List<FreteValores> GetFreteValores(string cep)
        {
            var cepLong = long.Parse(cep);
            using (var context = new ContextEF())
            {
                var data = context.FreteValores.Where(f => cepLong <= f.CepFinal && cepLong >= f.CepInicio).ToList();
                return data;
            };
        }
    }
}
