using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO
{
    public class MessageContato
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Texto { get; set; }
        public DateTime DataEnvio { get; set; }
        public bool Visualizado { get; set; }
        public DateTime? DataVisualizado { get; set; }
    }
}
