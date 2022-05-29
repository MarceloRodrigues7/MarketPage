using ADO;
using MarketPage.Context;
using System.Linq;

namespace MarketPage.Repository
{
    public class FormaPagamentoRepository : IFormaPagamentoRepository
    {
        public FormaPagamento GetFormaPagamento()
        {
            using (var context = new ContextEF())
            {
                return context.PlataformasPagamento.First(p => p.Nome == "MercadoPago");
            };
        }
        public FormaPagamento GetFormaPagamentoTeste()
        {
            using (var context = new ContextEF())
            {
                return context.PlataformasPagamento.First(p => p.Nome == "Teste");
            };
        }

        public void Update(FormaPagamento formaPagamento)
        {
            using (var context = new ContextEF())
            {
                var data = context.PlataformasPagamento.First();
                context.PlataformasPagamento.Remove(data);
                context.PlataformasPagamento.Add(formaPagamento);
                context.SaveChanges();
            };
        }
    }
}
