﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataAdicao { get; set; }
    }
}
