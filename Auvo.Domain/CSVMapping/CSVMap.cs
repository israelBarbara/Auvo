using Auvo.Domain.DTOs.Request;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auvo.Domain.CSVMapping
{
    internal sealed class MyCsvMap : ClassMap<CSVRequest>
    {
        public MyCsvMap()
        {
            Map(x => x.Codigo).Name("Codigo");
            Map(x => x.Nome).Name("Nome");
            Map(x => x.ValorHora).Name("ValorHora");
            Map(x => x.Data).Name("Data");
            Map(x => x.Entrada).Name("Entrada");
            Map(x => x.Saida).Name("Saida");
            Map(x => x.Almoco).Name("Almoco");
        }
    }
}
