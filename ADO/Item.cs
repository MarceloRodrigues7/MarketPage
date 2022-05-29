using System;

namespace ADO
{
    public class Item
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public string Tamanhos { get; set; }
        public int Quantidade { get; set; }
        public bool Destaque { get; set; }
        public DateTime DataAdicao { get; set; }
        public int IdCategoria { get; set; }
        public float Peso { get; set; }
    }
}
