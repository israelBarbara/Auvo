using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auvo.Domain.DTOs.Request
{
    public class CSVRequest
    {
        public int Codigo { get; set; }
        public string Nome { get; set; }
        public string ValorHora { get; set; }
        public string Data { get; set; }
        public string Entrada { get; set; }
        public string Saida { get; set; }
        public string Almoco { get; set; }

    }
}
