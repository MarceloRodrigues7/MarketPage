using ADO;
using System.Collections.Generic;

namespace MarketPage.Repository
{
    public interface IFreteRepository
    {
        FretePedidoUsuario GetFretePedido(int idUsuario, long idCarrinho);
        public void PostFrete(FretePedidoUsuario frete);
        List<FreteValores> GetFreteValores(string cep);
    }
}
