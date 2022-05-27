using ADO;
using MarketPage.Context;
using MarketPage.Models;
using System;
using System.Linq;

namespace MarketPage.Repository
{
    public class FormaPagamentoRepository : IFormaPagamentoRepository
    {
        public FormaPagamento GetFormaPagamento()
        {
            using (var context = new ContextEF())
            {
                return context.PlataformasPagamento.First();
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
