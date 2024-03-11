using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Serializable]
    public class MoonTouch : INotifyPropertyChanged
    {

        public int Level {
            get => _level;
            set
            {
                _level = value;
                switch (_level)
                {
                    case 1:
                        coefficient = 1.3;
                        baseDmg = 40;
                        DurationMoonTouch = 4;
                        CoefficientDD = 0.05;
                        break;
                    case 2:
                        coefficient = 1.4;
                        baseDmg = 55;
                        DurationMoonTouch = 5;
                        CoefficientDD = 0.07;
                        break;
                    case 3:
                        coefficient = 1.55;
                        baseDmg = 70;
                        DurationMoonTouch = 5;
                        CoefficientDD = 0.1;
                        break;
                    case 4:
                        coefficient = 1.65;
                        baseDmg = 100;
                        DurationMoonTouch = 6;
                        CoefficientDD = 0.13;
                        break;
                    case 5:
                        coefficient = 1.75;
                        baseDmg = 120;
                        DurationMoonTouch = 6;
                        CoefficientDD = 0.15;
                        break;
                    default:
                        break;
                }
                if (HasTalantPlus)
                    coefficient += 0.05;
                NotifyPropertyChanged(nameof(Level));
            }
        }
        public int _level; 
        private double coefficient = 1.3;
        private int baseDmg = 40;
        public double BaseTimeCooldown = 11;
        public int DurationMoonTouch = 4;
        public double CoefficientDD = 0.05;
        public BeastAwakening Luna;

        public bool HasTalantPlus
        {
            get => hasTalantPlus;
            set {
                hasTalantPlus = value; NotifyPropertyChanged("HasTalantPlus"); 
            }
        }
        private bool hasTalantPlus = false;
        private bool hasRelic;
        public bool HasRelic
        {
            get => hasRelic;
            set { hasRelic = value; NotifyPropertyChanged("HasRelic"); }
        }
        public int Formula(int dmg)
        {
            Level = Level;
            double result = baseDmg + coefficient * dmg;
            if (HasRelic) result *= 1.12;
            return (int)result;
        }



        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Метод для вызова события PropertyChanged
        /// </summary>
        /// <param name="prop">Имя свойства, которое изменилось</param>
        public void NotifyPropertyChanged([CallerMemberName] string prop = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
