using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auvo.Domain.DTOs.Response
{
    public class SaidaJson
    {
        public string Departamento { get; set; }
        public string MesVigencia { get; set; }
        public string AnoVigencia { get; set; }
        public decimal TotalPagar { get; set; }
        public decimal TotalDescontos { get; set; }
        public decimal TotalExtras { get; set; }
        public IEnumerable<Funcionario> funcionarios { get; set; }

}
}
