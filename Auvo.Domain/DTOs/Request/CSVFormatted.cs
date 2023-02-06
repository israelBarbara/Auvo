using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auvo.Domain.DTOs.Request
{
    public class CSVFormatted
    {
        public int Codigo { get; set; }
        public string Nome { get; set; }
        public decimal ValorHora { get; set; }
        public DateTime Data { get; set; }
        public DateTime Entrada { get; set; }
        public DateTime Saida { get; set; }
        public double Almoco { get; set; }

    }
}
