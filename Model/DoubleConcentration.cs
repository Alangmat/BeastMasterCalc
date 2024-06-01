using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// Класс определяющий навык Двойная концентрация. 
    /// Cодержит поля добавляющие статы Крит урон и кд, а так же свойство Level. 
    /// Статы считаются под 4 стаками.
    /// </summary>
    public class DoubleConcentration : INotifyPropertyChanged
    {
        /// <summary>
        /// Прибавка крит урона
        /// </summary>
        public int AdditionCriticalDamage = 8;
        /// <summary>
        /// Прибавка перезарядки навыков
        /// </summary>
        public int AdditionSkillCooldown = 16;
        /// <summary>
        /// Свойство определяющее наличие таланта на скорость атаки вместо кд
        /// </summary>
        public bool HasTalentDeadlyDexterity = false;
        public int BaseTimeCooldown = 22;

        /// <summary>
        /// Метод, для проверки таланта второй ветки и изменения прибавки кд
        /// </summary>
        /// <returns>Прибавляемое значение кд</returns>
        public double AddSkillCooldown()
        {
            return (HasTalentDeadlyDexterity ? 0 : AdditionSkillCooldown);
        }
        /// <summary>
        /// Метод, для проверки таланта второй ветки и изменения прибавки Скорости атаки
        /// </summary>
        /// <returns>Прибавляемое значение Скорости атаки</returns>
        public double AddAttackSpeed()
        {
            return (HasTalentDeadlyDexterity ? (AdditionSkillCooldown * 0.4) : 0);
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
                    switch (level)
                    {
                        case 1:
                            AdditionCriticalDamage = 8;
                            AdditionSkillCooldown = 16;
                            break;
                        case 2:
                            AdditionCriticalDamage = 12;
                            AdditionSkillCooldown = 24;
                            break;
                        case 3:
                            AdditionCriticalDamage = 16;
                            AdditionSkillCooldown = 32;
                            break;
                        case 4:
                            AdditionCriticalDamage = 24;
                            AdditionSkillCooldown = 40;
                            break;
                    }
                }

                NotifyPropertyChanged(nameof(Level));
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
