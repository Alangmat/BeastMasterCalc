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
    public class Attack : INotifyPropertyChanged
    {
        private double timeDelay = 0;
        public double TimeDelay
        {
            get => timeDelay;
            set { timeDelay = value; NotifyPropertyChanged("TimeDelay"); }
        }
        private bool isStaff = false;
        public bool IsStaff
        {
            get => isStaff;
            set { isStaff = value; NotifyPropertyChanged("IsStaff"); }
        }
        public int Formula(int magicaldd, int physicaldd)
        {
            if (IsStaff)
            {
                return magicaldd;
            }
            return physicaldd;
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
