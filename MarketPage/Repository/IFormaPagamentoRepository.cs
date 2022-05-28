using ADO;
using MarketPage.Models;

namespace MarketPage.Repository
{
    public interface IFormaPagamentoRepository
    {
        FormaPagamento GetFormaPagamento();
        FormaPagamento GetFormaPagamentoTeste();
        void Update(FormaPagamento formaPagamento);
    }
}
