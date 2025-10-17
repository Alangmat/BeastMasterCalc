using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Model
{
    [Serializable]
    public class BeastAwakening : INotifyPropertyChanged
    {
        // ===== Конфиг (кэшируется один раз) =====
        private static JObject _cfg;
        private static JArray _levels;

        private static double BaseDelayCfg = 2.5;
        private static double TalentMageBonusCfg = 0.03;       // уже в долях (3% -> 0.03)
        private static readonly double[] TalentPhysBonusByLevel = new double[4]; // 1..3 (в долях)

        static BeastAwakening()
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var path = Path.Combine(baseDir, "config", "skills", "beast_awakening.json");
            if (!File.Exists(path))
                throw new FileNotFoundException("Не найден конфиг beast_awakening.json", path);

            var json = File.ReadAllText(path);
            _cfg = JObject.Parse(json);

            BaseDelayCfg = (double?)_cfg["BaseDelay"] ?? 2.5;

            // в конфиге проценты (3 => 3%), конвертируем в доли
            TalentMageBonusCfg = ((double?)_cfg["TalentMageBonus"] ?? 3.0) / 100.0;

            var phys = _cfg["TalentPhysBonuses"] as JObject;
            TalentPhysBonusByLevel[1] = ((double?)phys?["1"] ?? 1.5) / 100.0;
            TalentPhysBonusByLevel[2] = ((double?)phys?["2"] ?? 3.0) / 100.0;
            TalentPhysBonusByLevel[3] = ((double?)phys?["3"] ?? 4.5) / 100.0;

            _levels = (JArray)_cfg["Levels"];
            if (_levels == null || _levels.Count == 0)
                throw new InvalidOperationException("В конфиге BeastAwakening пустой массив Levels.");
        }

        // ===== Публичный API (имена сохранены) =====
        private int level = 1;
        public int Level
        {
            get => level;
            set
            {
                level = value;
                ApplyLevelFromConfig(level); // базовые значения из конфига
                ApplyTalentBonuses();        // добавляем бонусы талантов
                NotifyPropertyChanged("Level");
            }
        }

        [JsonIgnore] public double BaseDelay = 2.5;
        private int baseDmg = 20;

        private double coefficientMageDD = 0.35;
        //[JsonIgnore]
        //public double CoefficientMageDD
        //{
        //    get => coefficientMageDD;
        //    set { coefficientMageDD = value; NotifyPropertyChanged("MageDD"); }
        //}

        private double coefficientPhysicalDD = 0.7;
        //[JsonIgnore]
        //public double CoefficientPhysicalDD
        //{
        //    get => coefficientPhysicalDD;
        //    set { coefficientPhysicalDD = value; NotifyPropertyChanged("PhysicalDD"); }
        //}

        public int Formula(int mageDD, int physicalDD)
        {
            // на всякий случай освежаем из конфига (если менялся файл/уровень/таланты)
            ApplyLevelFromConfig(level);
            ApplyTalentBonuses();

            int result = (int)(baseDmg + coefficientMageDD * mageDD + coefficientPhysicalDD * physicalDD);
            return result;
        }

        #region Таланты
        public bool HasTalantMage
        {
            get => hasTalantMage;
            set { hasTalantMage = value; NotifyPropertyChanged(nameof(HasTalantMage)); }
        }
        private bool hasTalantMage = false;

        public int LvlTalantPhys
        {
            get => lvlTalantPhys;
            set { lvlTalantPhys = value; NotifyPropertyChanged(nameof(LvlTalantPhys)); }
        }
        private int lvlTalantPhys = 0;
        #endregion

        // ===== Внутренняя логика =====
        private void ApplyLevelFromConfig(int lvl)
        {
            var row = _levels.FirstOrDefault(x => (int)x["Level"] == lvl) as JObject;
            if (row == null) return;

            // В КОНФИГЕ ПРОЦЕНТЫ (35 => 35%), переводим в коэффициенты (0.35)
            coefficientMageDD = ((double?)row["CoefficientMagicalDamage"] ?? 35.0) / 100.0;
            coefficientPhysicalDD = ((double?)row["CoefficientPhysicalDamage"] ?? 70.0) / 100.0;

            baseDmg = (int?)row["BaseDamage"] ?? baseDmg;
            BaseDelay = BaseDelayCfg;
        }

        private void ApplyTalentBonuses()
        {
            // Сначала гарантия: базовые значения от уровня уже подставлены.
            // Теперь добавляем бонусы талантов в АБСОЛЮТНЫХ долях (как было у тебя).
            if (HasTalantMage)
            {
                coefficientMageDD = coefficientMageDD + TalentMageBonusCfg; // +3 п.п. => +0.03
            }
            else
            {
                int t = Math.Max(0, Math.Min(3, LvlTalantPhys));
                if (t > 0)
                    coefficientPhysicalDD = coefficientPhysicalDD + TalentPhysBonusByLevel[t]; // +1.5/3/4.5 п.п.
            }
        }

        // ===== INotifyPropertyChanged =====
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string prop = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
