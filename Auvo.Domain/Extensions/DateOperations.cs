using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auvo.Domain.Extensions
{
    public class DateOperations
    {
        public static int GetBusinessDays(DateTime startD, DateTime endD)
        {
            double calcBusinessDays =
                1 + ((endD - startD).TotalDays * 5 -
                (startD.DayOfWeek - endD.DayOfWeek) * 2) / 7;

            if (endD.DayOfWeek == DayOfWeek.Saturday) calcBusinessDays--;
            if (startD.DayOfWeek == DayOfWeek.Sunday) calcBusinessDays--;

            return Convert.ToInt32(calcBusinessDays);
        }
        public static bool verifyIfWeekend(DateTime day)
        {
            if (day.DayOfWeek == DayOfWeek.Saturday) return true;
            if (day.DayOfWeek == DayOfWeek.Sunday) return true;

            return false;
        }
        public static double FormatAlmoco(DateTime dia, string almoco)
        {
            DateTime _entrada;
            DateTime _volta;
            string[] _almoco = almoco.Split("-");
            string[] hrEntrada = _almoco[0].Trim().Split(":");
            string[] hrSaida = _almoco[1].Trim().Split(":");

            _entrada = dia.AddHours(Convert.ToDouble(hrEntrada[0])).AddMinutes(Convert.ToDouble(hrEntrada[1]));
            _volta = dia.AddHours(Convert.ToDouble(hrSaida[0])).AddMinutes(Convert.ToDouble(hrSaida[1]));
            return (_volta - _entrada).TotalHours;
        }

    }
}
