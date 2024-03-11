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
    public class Moonlight : INotifyPropertyChanged
    {
        private int level = 1;
        public int Level
        {
            get => level;
            set { level = value;
                switch (level)
                {
                    case 1:
                        coefficientMagicalDD = 0.2;
                        break;
                    case 2:
                        coefficientMagicalDD = 0.25;
                        break;
                    case 3:
                        coefficientMagicalDD = 0.3;
                        break;
                    case 4:
                        coefficientMagicalDD = 0.4;
                        break;
                    default:
                        coefficientMagicalDD = 0.2;
                        break;
                }
                switch (LvlTalant)
                {
                    case 1:
                        coefficientMagicalDD += 0.03;
                        break;
                    case 2:
                        coefficientMagicalDD += 0.06;
                        break;
                    case 3:
                        coefficientMagicalDD += 0.09;
                        break;
                }
                NotifyPropertyChanged("Level"); }
        }
        public double BaseTimeCooldown = 14;
        private double coefficientMagicalDD = 0.2;
        private int lvlTalant = 0;
        public int LvlTalant
        {
            get => lvlTalant;
            set { lvlTalant = value; NotifyPropertyChanged("LvlTalant"); }
        }

        public int Formula(int magicaldd)
        {
            Level = level;
            int result = (int)(magicaldd * coefficientMagicalDD);
            return result;
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
