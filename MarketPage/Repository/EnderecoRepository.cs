using ADO;
using MarketPage.Context;
using System.Linq;

namespace MarketPage.Repository
{
    public class EnderecoRepository : IEnderecoRepository
    {
        public Endereco GetEndereco(int idUsuario)
        {
            using (var context = new ContextEF())
            {
                return context.EnderecosUsuario.Where(e => e.IdUsuario == idUsuario).FirstOrDefault();
            };
        }

        public void InsertOrUpdate(Endereco endereco)
        {
            using (var context = new ContextEF())
            {
                var res = context.EnderecosUsuario.Where(u => u.IdUsuario == endereco.IdUsuario).FirstOrDefault();
                if (res == null)
                {
                    context.EnderecosUsuario.Add(endereco);
                }
                else
                {
                    res.Pais = endereco.Pais;
                    res.Estado = endereco.Estado;
                    res.Cidade = endereco.Cidade;
                    res.Bairro = endereco.Bairro;
                    res.Numero = endereco.Numero;
                    res.Cep = endereco.Cep;
                    context.EnderecosUsuario.Update(res);
                }
                context.SaveChanges();
            };
        }
    }
}
