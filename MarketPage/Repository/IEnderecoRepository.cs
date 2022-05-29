using ADO;

namespace MarketPage.Repository
{
    public interface IEnderecoRepository
    {
        Endereco GetEndereco(int idUsuario);
        void InsertOrUpdate(Endereco endereco);
    }
}
