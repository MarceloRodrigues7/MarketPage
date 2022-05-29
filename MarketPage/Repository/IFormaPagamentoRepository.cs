using ADO;

namespace MarketPage.Repository
{
    public interface IFormaPagamentoRepository
    {
        FormaPagamento GetFormaPagamento();
        FormaPagamento GetFormaPagamentoTeste();
        void Update(FormaPagamento formaPagamento);
    }
}
