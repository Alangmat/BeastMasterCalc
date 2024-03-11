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
    public class OrderToAttack : INotifyPropertyChanged
    {
        public OrderToAttack(BeastAwakening luna) 
        {
            Luna = luna;
        }

        public int _level;
        public int Level 
        {
            get => _level;
            set
            {
                _level = value;
                switch (_level)
                {
                    case 1:
                        coefficient = 0.05;
                        break;
                    case 2:
                        coefficient = 0.1;
                        break;
                    case 3:
                        coefficient = 0.15;
                        break;
                    case 4:
                        coefficient = 0.2;
                        break;
                    case 5:
                        coefficient = 0.25;
                        break;
                    default:
                        break;
                }
                if (LvlTalant <= 3) 
                {
                    coefficient += 0.01 * LvlTalant;
                }
                NotifyPropertyChanged(nameof(Level));
            }
        }
        private double coefficient = 0.05;
        public double BaseTimeCooldown = 10;
        public BeastAwakening Luna;

        private int lvlTalant = 0;
        public int LvlTalant
        {
            get => lvlTalant;
            set { lvlTalant = value; NotifyPropertyChanged("LvlTalant"); }
        }


        public int Formula(int magedd, int physdd)
        {
            Level = Level;
            double result = Luna.Formula(magedd, physdd) * coefficient; 

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
