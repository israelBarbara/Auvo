using Auvo.Domain.DTOs.Request;
using Auvo.Domain.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auvo.Domain.Interfaces.Services
{
    public interface ICSVService
    {
        public  IEnumerable<T> ReadCSV<T>(Stream file);
        public  Task<IEnumerable<CSVFormatted>> CSVFormat(IEnumerable<CSVRequest> csv);
        public  Task<SaidaJson> GenerateJson(IEnumerable<CSVFormatted> csv,string fileName, string logPath);
    }
}
