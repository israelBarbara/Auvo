using Auvo.Domain.CSVMapping;
using Auvo.Domain.DTOs.Request;
using Auvo.Domain.DTOs.Response;
using Auvo.Domain.Interfaces.Services;
using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Auvo.Domain.Log;
using Auvo.Domain.Extensions;

namespace Auvo.Domain.Services
{
    public class CSVService : ICSVService
    {
        public IEnumerable<T> ReadCSV<T>(Stream file)
        {
            //var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture);
            //csvConfig.Delimiter = ";";

            var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                HasHeaderRecord = true,
                HeaderValidated = null,
                MissingFieldFound = null,
                IgnoreBlankLines = true,
                ShouldSkipRecord = args => args.Row.Parser.Record.All(string.IsNullOrWhiteSpace)
            };

            var reader = new StreamReader(file);
            var csv = new CsvReader(reader, configuration);

            csv.Read();
            csv.ReadHeader();
            var records =  csv.GetRecords<T>();
            return records;
        }
        public async Task<IEnumerable<CSVFormatted>> CSVFormat(IEnumerable<CSVRequest> csv)
        {
            List<CSVFormatted> csvFormatted = new List<CSVFormatted>();

            foreach (var item in csv)
            {

                string[] entrada = item.Entrada.Split(":");
                string[] saida = item.Saida.Split(":");

                CSVFormatted _csv = new CSVFormatted
                {
                    Codigo = item.Codigo,
                    Nome = item.Nome,
                    ValorHora = decimal.Parse(Regex.Replace(item.ValorHora, "[^R$\\d\\,]", ""), NumberStyles.Currency),
                    Data = Convert.ToDateTime(item.Data),
                    Entrada = Convert.ToDateTime(item.Data).AddHours(Convert.ToDouble(entrada[0])).AddMinutes(Convert.ToDouble(entrada[1])).AddSeconds(Convert.ToDouble(entrada[2])),
                    Saida = Convert.ToDateTime(item.Data).AddHours(Convert.ToDouble(saida[0])).AddMinutes(Convert.ToDouble(saida[1])).AddSeconds(Convert.ToDouble(saida[2])),
                    Almoco = DateOperations.FormatAlmoco(Convert.ToDateTime(item.Data), item.Almoco),
                };

                csvFormatted.Add(_csv);

            }



            return csvFormatted;
        }

        public async Task<SaidaJson> GenerateJson(IEnumerable<CSVFormatted> csv, string fileName, string logPath)
        {
            SaidaJson saidaJson = new SaidaJson();

            try
            {
                List<int> codigos = csv.Select(x => x.Codigo).OrderBy(y => y).Distinct().ToList();
                List<Funcionario> _funcList = new List<Funcionario>();

                const int horaDiaria = 8;
                string[] _fileName = fileName.Split("-");

                saidaJson.Departamento = _fileName[0];
                saidaJson.MesVigencia = _fileName[1];
                saidaJson.AnoVigencia = _fileName[2];



                foreach (var item in codigos)
                {
                    List<CSVFormatted> func = csv.Where(x => x.Codigo == item).ToList();
                    Funcionario _func = new Funcionario();
                    decimal valorhr = 0;
                    DateTime firstDayOfMonth = new DateTime(func[0].Data.Year, func[0].Data.Month, 1);
                    DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                    int diasUteis = DateOperations.GetBusinessDays(firstDayOfMonth, lastDayOfMonth);
                    int diasUteisTrabalhados = 0;
                    _func.Nome = func[0].Nome;
                    _func.Codigo = func[0].Codigo;
                    _func.DiasTrabalhados = func.Count();

                    foreach (var itemFunc in func)
                    {

                        decimal horasTrabalhadasNoDia = decimal.Round(Convert.ToDecimal((itemFunc.Saida - itemFunc.Entrada).TotalHours - itemFunc.Almoco), 2, MidpointRounding.ToZero);
                        decimal totalReceberDia = (itemFunc.ValorHora * horasTrabalhadasNoDia);
                        valorhr = itemFunc.ValorHora;
                        if (DateOperations.verifyIfWeekend(itemFunc.Data))
                        {
                            _func.DiasExtras += 1;
                            saidaJson.TotalExtras += totalReceberDia;
                        }
                        else
                        {
                            diasUteisTrabalhados += 1;
                        }
                        
                        _func.TotalReceber += totalReceberDia;
                        if (horasTrabalhadasNoDia - horaDiaria > 0) _func.HorasExtras += horasTrabalhadasNoDia - horaDiaria;
                        if (horasTrabalhadasNoDia - horaDiaria < 0) _func.HorasDebito += horaDiaria - horasTrabalhadasNoDia;
                        saidaJson.TotalPagar += totalReceberDia;

                    }
                    _func.DiasFalta = diasUteis - diasUteisTrabalhados;
                    if(_func.DiasFalta > 0)
                    {
                        decimal vlr = valorhr * 8 * _func.DiasFalta;
                        saidaJson.TotalDescontos += vlr;
                    }
                    diasUteisTrabalhados = 0;
                    _funcList.Add(_func);


                }
                saidaJson.funcionarios = _funcList;



            }
            catch (Exception ex)
            {
                Log.Log.AdicionarTxt(logPath, $"Erro ao processar CSV {fileName} - {ex.Message} ");
            }

            Log.Log.AdicionarTxt(logPath, $"Processamento CSV {fileName} finalizado ");
            return saidaJson;
        }




    }
}
