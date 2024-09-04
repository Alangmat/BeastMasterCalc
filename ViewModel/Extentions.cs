using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public static class Extentions
    {
        /// <summary>
        /// Метод преобразует число в коэффициент 1.[value]
        /// </summary>
        /// <param name="value"></param>
        /// <returns>double число в формате 1.[value]</returns>
        public static double ConvertToCoefficient(this double value)
        {
            return (1 + value.ConvertToFraction());
        }
        /// <summary>
        /// Метод преобразует число в десятичную дробь 0.[value]
        /// </summary>
        /// <param name="value"></param>
        /// <returns>double число в формате 0.[value]</returns>
        public static double ConvertToFraction(this double value)
        {
            return (value / 100);
        }
    }
}
