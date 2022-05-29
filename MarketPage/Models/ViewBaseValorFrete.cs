namespace MarketPage.Models
{
    public class ViewBaseValorFrete
    {
        public string Servico { get; set; }
        public decimal Preco { get; set; }
        public int Prazo { get; set; }
    }

    public class ViewFreteCarrinho
    {
        public string Servico { get; set; }
        public string Texto { get; set; }
    }
}
