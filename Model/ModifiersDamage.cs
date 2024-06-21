using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// Класс, содержащий списки модификаторов урона и преобразователь ConvertInModifiers
    /// </summary>
    public class ModifiersDamage
    {
        public List<string> Amulets = new List<string>() { 
            "0%",
            "6% маг",
            "10% маг",
            "15% маг",
            "4% физ",
            "7% физ"
        };
        public List<string> Cloaks = new List<string>() {
            "0%",
            "5% маг",
            "10% маг",
            "15% маг",
            "4% физ",
            "7% физ"
        };
        public List<string> Rings = new List<string>() {
            "0%",
            "5% маг",
            "9% маг",
            "10% маг",
            "3% физ",
            "6% физ"
        };
        public List<string> Bracelets = new List<string>() {
            "0%",
            "6% маг",
            "7.5% маг",
            "4% физ",
            "5% физ"
        };
        public List<string> Sets = new List<string>() {
            "0%",
            "12% маг",
            "8% физ"
        };
        public List<string> Castle = new List<string>() {
            "Без замка 0%",
            "1 сектор, 5%",
            "2 сектор, 7.5%",
            "3 сектор, 10%",
            "4 сектор, 12.5%",
            "5 сектор, 15%",
        };

        public List<string> Equipments = new List<string>
        {
            "Empty",
            "Cloath",
            "Leather"
        };


        public Dictionary<string, double> ConvertInModifiers(string inp)
        {
            var result = new Dictionary<string, double>() { {"Magical", 0 }, {"Physical", 0} };
            if (inp == "0%")
                return result;
            double mod = 0;

            switch (inp.Split().Length)
            {
                case 1:

                    break;

                case 2:
                    if (double.TryParse(inp.Split()[0].Trim('%'), out mod))
                    { 
                        if (inp.Split()[1] == "маг")
                        {
                                result["Magical"] = mod;
                        }
                        else if (inp.Split()[1] == "физ")
                        {
                                result["Physical"] = mod;
                        }
                    }
                    break;

                case 3:
                    var sp = inp.Split();
                    if (double.TryParse(sp[0].Trim('%'), out mod))
                    {
                        result["Magical"] = mod;
                        result["Physical"] = mod;
                    }
                    break;
            }

            /*if (inp.Split().Length == 2)
            {
                if (inp.Split()[1] == "маг")
                {
                    if (double.TryParse(inp.Split()[0].Trim('%'), out mod))
                    {
                        result["Magical"] = mod;
                    }
                }
                else if (inp.Split()[1] == "физ")
                {
                    if (double.TryParse(inp.Split()[0].Trim('%'), out mod))
                    {
                        result["Physical"] = mod;
                    }
                }

            }*/
            return result;
        }
    }
}
