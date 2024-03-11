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
    public class AuraOfTheForest : INotifyPropertyChanged
    {
        private int level = 1;
        public int Level
        {
            get => level;
            set { level = value;
                switch (level)
                {
                    case 1:
                        coefficientMagicalDD = 0.35;
                        break;
                    case 2:
                        coefficientMagicalDD = 0.45;
                        break;
                    case 3:
                        coefficientMagicalDD = 0.6;
                        break;
                    case 4:
                        coefficientMagicalDD = 0.8;
                        break;
                    default:
                        coefficientMagicalDD = 0.35;
                        break;
                }
                NotifyPropertyChanged("Level"); }
        }
        private bool hasTalantPowerOfNature = false;
        public bool HasTalantPowerOfNature
        {
            get => hasTalantPowerOfNature;
            set { hasTalantPowerOfNature = value; 
                NotifyPropertyChanged("HasTalantPowerOfNature"); }
        }
        private double coefficientMagicalDD = 0.35;
        public double BaseTimeCooldown = 24;
        public int Formula(int magicaldd)
        {
            Level = level;
            int result = (int)(magicaldd * coefficientMagicalDD);
            if (HasTalantPowerOfNature)
                result = (int)(result * 1.15 * 1.2);
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
