﻿using MarketPage.Context;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MarketPage.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataAdicao { get; set; }
        
        public List<Categoria> GetCategorias()
        {
            using (var context = new ContextEF())
            {
                return context.Categorias.ToList();
            };
        }
    }
}
