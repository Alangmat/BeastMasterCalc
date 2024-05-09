using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    // TESTCASE
    // Для проверки работы добавки статов можно после загрузки потыкать +- лвл и посмотреть изменился ли урон

    /// <summary>
    /// Класс определяющий навык Благословение луны. 
    /// Cодержит поля добавляющие статы Крит и пробив, а так же свойство Level
    /// </summary>
    public class BlessingOfTheMoon
    {
        /// <summary>
        /// Прибавка шанса крита
        /// </summary>
        public int AdditionCriticalHit = 8;
        /// <summary>
        /// Прибавка пробива
        /// </summary>
        public int AdditionPenetration = 5;
        /// <summary>
        /// Базовое значение длительности перезарядки навыка
        /// </summary>
        public double BaseTimeCooldown = 25;

        private bool hasTalantPlusPenetration = false;
        /// <summary>
        /// Свойство определяющее наличие таланта на благо с левой ветки, дает +1 пробив
        /// </summary>
        public bool HasTalantPlusPenetration
        {
            get => hasTalantPlusPenetration;
            set
            {
                hasTalantPlusPenetration = value;
                Level = level;
                NotifyPropertyChanged(nameof(HasTalantPlusPenetration));
            }
        }
        private bool hasTalantPlusCriticalHit = false;
        /// <summary>
        /// Свойство определяющее наличие таланта на благо с центральной ветки, дает +2 крита
        /// </summary>
        public bool HasTalantPlusCriticalHit
        {
            get => hasTalantPlusCriticalHit;
            set
            {
                hasTalantPlusCriticalHit = value;
                Level = level;
                NotifyPropertyChanged(nameof(HasTalantPlusCriticalHit));
            }
        }

                private int level = 1;
        /// <summary>
        /// Свойство для обновления уровня навыка, при изменении значения обновляет прибавки статов.
        /// </summary>
        public int Level
        {
            get => level;
            set
            {
                if (value >= 1 && value <= 4)
                {
                    level = value;
                    switch(level)
                    {
                        case 1:
                            AdditionCriticalHit = 8;
                            AdditionPenetration = 5;
                            break;
                        case 2:
                            AdditionCriticalHit = 10;
                            AdditionPenetration = 6;
                            break;
                        case 3:
                            AdditionCriticalHit = 13;
                            AdditionPenetration = 7;
                            break;
                        case 4:
                            AdditionCriticalHit = 16;
                            AdditionPenetration = 8;
                            break;
                    }
                    if (HasTalantPlusPenetration) AdditionPenetration += 1;
                    else if (HasTalantPlusCriticalHit) AdditionCriticalHit += 2;
                    NotifyPropertyChanged(nameof(Level));
                }
            }
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
