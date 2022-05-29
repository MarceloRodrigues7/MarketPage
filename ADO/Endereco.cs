using System.Text.RegularExpressions;

namespace ADO
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

        public static bool ValidaCEP(string cep)
        {
            var cepvalid = FormataCEP(cep);
            Regex Rgx = new(@"^\d{8}$");

            if (!Rgx.IsMatch(cepvalid))

                return false;

            else

                return true;
        }

        public static string FormataCEP(string cep)
        {
            return cep.Replace("-", "").Replace(".", "").Replace(" ", "").Replace("/", "").Replace(@"\", "");
        }
    }
}
