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
    public class BestialRampage : INotifyPropertyChanged
    {
        public BestialRampage(BeastAwakening luna) 
        {
            Luna = luna;
        }
        private int level = 1;
        public int Level
        {
            get => level;
            set { level = value;
                switch (level)
                {
                    case 1:
                        TimeActive = 8;
                        IncreaseDD = 1.1;
                        IncreaseAttackSpeed = 10;
                        break;
                    case 2:
                        TimeActive = 9;
                        IncreaseDD = 1.15;
                        IncreaseAttackSpeed = 15;
                        break;
                    case 3:
                        TimeActive = 9;
                        IncreaseDD = 1.2;
                        IncreaseAttackSpeed = 20;
                        break;
                    case 4:
                        TimeActive = 10;
                        IncreaseDD = 1.25;
                        IncreaseAttackSpeed = 30;
                        break;
                }
                if (HasTalant) IncreaseAttackSpeed += 0.02;
                NotifyPropertyChanged(nameof(Level)); }
        }
        public double BaseTimeCooldown = 26;
        public int TimeActive = 8;
        public double IncreaseDD = 1.1;
        public double IncreaseAttackSpeed = 10;
        public BeastAwakening Luna;
        private bool hasTalant = false;
        public bool HasTalant
        {
            get => hasTalant;
            set { hasTalant = value; NotifyPropertyChanged(nameof(HasTalant)); }
        }

        public int Formula(int magedd, int physdd)
        {
            Level = level;
            int resultDD = (int)(Luna.Formula(magedd, physdd) * IncreaseDD);
            return resultDD;
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
