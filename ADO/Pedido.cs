﻿using System;

namespace ADO
{
    public class Pedido
    {
        public long Id { get; set; }
        public int IdUsuario { get; set; }
        public decimal ValorTotal { get; set; }
        public string Pais { get; set; }
        public string Estado { get; set; }
        public string Cidade { get; set; }
        public string Bairro { get; set; }
        public int Numero { get; set; }
        public DateTime DataRealizacao { get; set; }
        public string StatusAtual { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public DateTime? DateFinalizacao { get; set; }
        public string IdMercadoPago { get; set; }
        public string CodRastreio { get; set; }
        public int PrazoEntrega { get; set; }

        public static Pedido GerarObj(int idUsuario)
        {
            return new Pedido()
            {
                IdUsuario = idUsuario,
                DataRealizacao = DateTime.UtcNow.AddHours(-3),
                StatusAtual = "Pendente"
            };
        }
    }
}
