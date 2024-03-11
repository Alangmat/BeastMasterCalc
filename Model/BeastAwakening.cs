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
    public class BeastAwakening : INotifyPropertyChanged
    {
        private int level = 1;
        public int Level
        {
            get => level;
            set
            {
                level = value;
                switch (level)
                {
                    case 1:
                        CoefficientMageDD = 0.35;
                        CoefficientPhysicalDD = 0.7;
                        baseDmg = 20;
                        break;
                    case 2:
                        CoefficientMageDD = 0.45;
                        CoefficientPhysicalDD = 0.8;
                        baseDmg = 30;
                        break;
                    case 3:
                        CoefficientMageDD = 0.55;
                        CoefficientPhysicalDD = 0.9;
                        baseDmg = 40;
                        break;
                    case 4:
                        CoefficientMageDD = 0.65;
                        CoefficientPhysicalDD = 1;
                        baseDmg = 50;
                        break;
                    case 5:
                        CoefficientMageDD = 0.75;
                        CoefficientPhysicalDD = 1.1;
                        baseDmg = 60;
                        break;
                }
                if (HasTalantMage) 
                {
                    coefficientMageDD += 0.03;
                }
                else switch (LvlTalantPhys)
                    {
                        case 1:
                            coefficientPhysicalDD += 0.015;
                            break;
                        case 2:
                            coefficientPhysicalDD += 0.03;
                            break;
                        case 3:
                            coefficientPhysicalDD += 0.045;
                            break;
                    }
                            
                NotifyPropertyChanged("Level");
            }
        }
        public double BaseDelay = 2.5;
        private int baseDmg = 20;
        private double coefficientMageDD = 0.35;
        public double CoefficientMageDD
        {
            get => coefficientMageDD;
            set { coefficientMageDD = value; NotifyPropertyChanged("MageDD"); }
        }
        private double coefficientPhysicalDD = 0.7;
        public double CoefficientPhysicalDD
        {
            get => coefficientPhysicalDD;
            set { coefficientPhysicalDD = value; NotifyPropertyChanged("PhysicalDD"); }
        }
        public int Formula(int mageDD, int physicalDD)
        {
            Level = level;
            int result = (int)(baseDmg + coefficientMageDD * mageDD + coefficientPhysicalDD * physicalDD);
            return result;
        }



        #region Таланты
        public bool HasTalantMage
        {
            get => hasTalantMage;
            set
            {
                hasTalantMage = value; NotifyPropertyChanged("HasTalantMage");
            }
        }
        private bool hasTalantMage = false;
        public int LvlTalantPhys
        {
            get => lvlTalantPhys;
            set
            {
                lvlTalantPhys = value; NotifyPropertyChanged("LvlTalantPhys");
            }
        }
        private int lvlTalantPhys = 0;
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Метод для вызова события PropertyChanged
        /// </summary>
        /// <param name="prop">Имя свойства, которое изменилось</param>
        public void NotifyPropertyChanged([CallerMemberName] string prop = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
