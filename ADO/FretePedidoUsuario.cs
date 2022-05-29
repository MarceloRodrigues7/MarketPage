namespace ADO
{
    public class FretePedidoUsuario
    {
        public long Id { get; set; }
        public int IdUsuario { get; set; }
        public long IdCarrinho { get; set; }
        public string TipoFrete { get; set; }
        public decimal ValorTotal { get; set; }

        public static FretePedidoUsuario GeraObj(string[] valorFrete, Carrinho carrinho, int idUsuario)
        {
            return new FretePedidoUsuario
            {
                IdUsuario = idUsuario,
                IdCarrinho = carrinho.Id,
                TipoFrete = valorFrete[0],
                ValorTotal = decimal.Parse(valorFrete[1].Replace("R$", ""))
            };
        }
    }
}
