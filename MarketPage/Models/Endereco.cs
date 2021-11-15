﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MarketPage.Models
{
    public class Endereco
    {
        public long Id { get; set; }
        public int IdUsuario { get; set; }
        public string Pais { get; set; }
        public string Estado { get; set; }
        public string Cidade { get; set; }
        public string Bairro { get; set; }
        public int Numero { get; set; }
        public string Cep { get; set; }

        public bool ValidaCEP(string cep)
        {
            var cepvalid = FormataCEP(cep);
            Regex Rgx = new Regex(@"^\d{8}$");

            if (!Rgx.IsMatch(cepvalid))

                return false;

            else

                return true;
        }

        public string FormataCEP(string cep)
        {
            return cep.Replace("-", "").Replace(".", "").Replace(" ", "").Replace("/","").Replace(@"\","");
        }
    }


}
