using Auvo.Domain.DTOs.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auvo.Domain.Interfaces.Repositories
{
    public interface IProcessadosRepository
    {
        public Task<int> InitProcessing(string PastaEntrada);
        public Task<bool> FinishProcessing(int id);
    }
}
