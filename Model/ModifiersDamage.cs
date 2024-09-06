using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared;

namespace Model
{
    /// <summary>
    /// Класс, содержащий списки модификаторов урона и преобразователь ConvertInModifiers
    /// </summary>
    public static class ModifiersDamage
    {
        #region Константы

        public const int DD_PROCENT_PASSIVE = 4;
        public const int DD_GUILD = 10;
        public const double DD_TALENTS = 4.75;
        public const double DD_CASTLE = 7.5;


        #endregion



        public static List<string> Amulets = new List<string>() { 
            "0%",
            "6% маг",
            "10% маг",
            "15% маг",
            "4% физ",
            "7% физ"
        };
        public static List<string> Cloaks = new List<string>() {
            "0%",
            "5% маг",
            "10% маг",
            "15% маг",
            "4% физ",
            "7% физ"
        };
        public static List<string> Rings = new List<string>() {
            "0%",
            "5% маг",
            "9% маг",
            "10% маг",
            "3% физ",
            "6% физ"
        };
        public static List<string> Bracelets = new List<string>() {
            "0%",
            "6% маг",
            "7.5% маг",
            "4% физ",
            "5% физ"
        };
        public static List<string> Sets = new List<string>() {
            "0%",
            "12% маг",
            "8% физ"
        };
        /// <summary>
        /// Дурак это баф на урон от скиллов
        /// </summary>
        public static List<string> Castle = new List<string>() {
            "Без замка 0%",
            "1 сектор, 5%",
            "2 сектор, 7.5%",
            "3 сектор, 10%",
            "4 сектор, 12.5%",
            "5 сектор, 15%",
        };

        public static List<string> Equipments = new List<string>
        {
            "Empty",
            "Cloth",
            "Leather"
        };

        /// <summary>
        /// Конвертирует полученный элемент в прибавку урона
        /// </summary>
        /// <param name="inp"></param>
        /// <returns>Словарь с ключами Magical и Physical</returns>
        public static Dictionary<TypesDamage, double> ConvertInModifiers(string inp)
        {
            var result = new Dictionary<TypesDamage, double>() { {TypesDamage.Magical, 0 }, { TypesDamage.Physical, 0} };
            if (inp == "0%")
                return result;
            double mod = 0;

            switch (inp.Split().Length)
            {
                case 1:
                    if (inp == "Cloth")
                    {
                        result[TypesDamage.Magical] = 3;
                        result[TypesDamage.Physical] = 2;
                    }
                    else if (inp == "Leather")
                    {
                        result[TypesDamage.Physical] = 3;
                        result[TypesDamage.Magical] = 2;
                    }
                    break;

                case 2:
                    if (double.TryParse(inp.Split()[0].Trim('%'), out mod))
                    { 
                        if (inp.Split()[1] == "маг")
                        {
                                result[TypesDamage.Magical] = mod;
                        }
                        else if (inp.Split()[1] == "физ")
                        {
                                result[TypesDamage.Magical] = mod;
                        }
                    }
                    break;

                case 3:
                    var sp = inp.Split();
                    if (double.TryParse(sp[0].Trim('%'), out mod))
                    {
                        result[TypesDamage.Magical] = mod;
                        result[TypesDamage.Physical] = mod;
                    }
                    break;
            }

            return result;
        }
    }
}
