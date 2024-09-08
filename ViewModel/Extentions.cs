using Shared;
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
        

        public static double ToPercentInDictionary(this PercentsDamage value, TypesDamage type) 
        {
            Dictionary<TypesDamage, double> result = new Dictionary<TypesDamage, double>();
            double mPercent = 0;
            double pPercent = 0;
            switch (value)
            {
                case PercentsDamage.None: mPercent = 0; pPercent = 0; break;
                case PercentsDamage.Magic5Percent: mPercent = 5; break;
                case PercentsDamage.Magic6Percent: mPercent = 6; break;
                case PercentsDamage.Magic7_5Percent: mPercent = 7.5; break;
                case PercentsDamage.Magic9Percent: mPercent = 9; break;
                case PercentsDamage.Magic10Percent: mPercent = 10; break;
                case PercentsDamage.Magic12Percent: mPercent = 12; break;
                case PercentsDamage.Magic15Percent: mPercent = 15; break;

                case PercentsDamage.Physical3Percent: pPercent = 3; break;
                case PercentsDamage.Physical4Percent: pPercent = 4; break;
                case PercentsDamage.Physical5Percent: pPercent = 5; break;
                case PercentsDamage.Physical6Percent: pPercent = 6; break;
                case PercentsDamage.Physical7Percent: pPercent = 7; break;
                case PercentsDamage.Physical8Percent: pPercent = 8; break;
            }
            result.Add(TypesDamage.Magical, mPercent);
            result.Add(TypesDamage.Physical, pPercent);
            return result[type];
        }
        public static double ToPercentInDictionary(this TypesEquipment value, TypesDamage type)
        {
            Dictionary<TypesDamage, double> result = new Dictionary<TypesDamage, double>();
            double mPercent = 0;
            double pPercent = 0;

            switch (value)
            {
                case TypesEquipment.Cloth:
                    mPercent = 3;
                    pPercent = 2; 
                    break;
                case TypesEquipment.Leather:
                    mPercent = 2;
                    pPercent = 3;
                    break;
            }
            result.Add(TypesDamage.Magical, mPercent);
            result.Add(TypesDamage.Physical, pPercent);

            return result[type];
        }
    }
}
