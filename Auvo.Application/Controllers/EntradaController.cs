using Auvo.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Auvo.Domain.Interfaces.Services;
using Auvo.Domain.DTOs.Request;
using System.Text.RegularExpressions;
using System.Globalization;
using Auvo.Domain.DTOs.Response;
using Newtonsoft.Json;
using System.Threading;
using Auvo.Domain.Log;
namespace Auvo.Application.Controllers
{
    public class EntradaController : Controller
    {
        private readonly ILogger<EntradaController> _logger;
        private readonly ICSVService _csvService;

        public EntradaController(ILogger<EntradaController> logger, ICSVService csvService)
        {
            _logger = logger;
            _csvService = csvService; 
        }

        public IActionResult Index()
        {
            return View();
        }

        public async void Processamento(string param)
        {
            
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Arquivos\\Entrada", param);
            string outputFolder = Path.Combine(Directory.GetCurrentDirectory(), $"Arquivos\\Saida");
            List<SaidaJson> finalResult = new List<SaidaJson>();
            if (!System.IO.File.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }
            
            string[] files = Directory.GetFiles(path, "*.csv", SearchOption.TopDirectoryOnly);
            Log.GravaLog(param, $"Processamento Pasta iniciado");
            string logPath = Path.Combine(Directory.GetCurrentDirectory(), "Logs", $"{param}");

            foreach (var file in files)
            {           
                string fileName = Path.GetFileNameWithoutExtension(file);
                Log.AdicionarTxt(logPath, $"Processamento CSV {fileName} iniciado");
                FileStream stream = System.IO.File.Open(file, FileMode.Open);
                List<CSVRequest> employees = _csvService.ReadCSV<CSVRequest>(stream).ToList();
                stream.Close();
                IEnumerable<CSVFormatted> csvFinal = await _csvService.CSVFormat(employees);
                SaidaJson _saidaJson = await _csvService.GenerateJson(csvFinal, fileName, logPath);
                finalResult.Add(_saidaJson);
            }
            string json = JsonConvert.SerializeObject(finalResult);
            string filePath = Path.Combine(outputFolder, $"{param}.json");
            System.IO.File.WriteAllText(filePath, json);
            Log.AdicionarTxt(Path.Combine(Directory.GetCurrentDirectory(), "Logs", $"{param}"), $"Processamento pasta finalizado");


        }

        public UserCallBack IniciarProcessamento(string param)
        {
            UserCallBack retorno = new UserCallBack();
            string directory;

            if (param == null)
            {
                retorno.Success = false;
                retorno.Message = "Informe o nome da pasta!";
                return retorno;
            }
            else
            {
                directory = Path.Combine(Directory.GetCurrentDirectory(), "Arquivos\\Entrada", param);
            }

            if (System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "Arquivos\\Saida", $"{param}.json")))
            {
                retorno.Success = false;
                retorno.Message = "Pasta já Processada!";
                return retorno;
            }

            if (!Directory.Exists(directory))
            {
                retorno.Success = false;
                retorno.Message = "Pasta informada nao existe!";
                return retorno;
            }

            string[] files = Directory.GetFiles(directory, "*.csv", SearchOption.TopDirectoryOnly);
            if (files.Length == 0)
            {
                retorno.Success = false;
                retorno.Message = "Pasta nao contem arquivos CSV's!";
                return retorno;
            }

            Thread thread = new Thread(() => Processamento(param));
            thread.Start();

            retorno.Success = true;
            retorno.Message = "Processamento iniciado!";
            return retorno;
        }


    }
}
