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
    public class ChainLightning : INotifyPropertyChanged
    {
        private int level = 1;
        public int Level
        {
            get => level;
            set { level = value;
                switch (level)
                {
                    case 1:
                        CoefficientMageDD = 1;
                        CoefficientPhysicalDD = 0.55;
                        break;
                    case 2:
                        CoefficientMageDD = 1.15;
                        CoefficientPhysicalDD = 0.65;
                        break;
                    case 3:
                        CoefficientMageDD = 1.3;
                        CoefficientPhysicalDD = 0.75;
                        break;
                    case 4:
                        CoefficientMageDD = 1.45;
                        CoefficientPhysicalDD = 0.85;
                        break;
                    case 5:
                        CoefficientMageDD = 1.6;
                        CoefficientPhysicalDD = 0.95;
                        break;
                }
                NotifyPropertyChanged("Level"); }
        }
        public double BaseTimeCooldown = 19;

        private double coefficientMageDD = 1;
        public double CoefficientMageDD
        {
            get => coefficientMageDD;
            set { coefficientMageDD = value; NotifyPropertyChanged("MageDD"); }
        }
        private double coefficientPhysicalDD = 0.55;
        public double CoefficientPhysicalDD
        {
            get => coefficientPhysicalDD;
            set { coefficientPhysicalDD = value; NotifyPropertyChanged("PhysicalDD"); }
        }
        private bool hasRelic;
        public bool HasRelic
        {
            get => hasRelic;
            set { hasRelic = value; NotifyPropertyChanged("HasRelic"); }
        }

        public int Formula(int mageDD, int physicalDD)
        {
            double result = 0;
            if (mageDD >= physicalDD)
            {
                result += CoefficientMageDD * mageDD;
            }
            else result += CoefficientPhysicalDD * physicalDD;
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
