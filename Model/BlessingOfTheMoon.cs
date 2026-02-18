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
    public class BlessingOfTheMoon : INotifyPropertyChanged
    {
        // ===== Конфиг (кэш на всё приложение) =====
        private static JObject _cfg;
        private static JArray _levels;
        private static int TalentPlusPenetrationCfg = 1;
        private static int TalentPlusCriticalHitCfg = 2;
        private static double BaseCooldownCfg = 25;

        static BlessingOfTheMoon()
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var path = Path.Combine(baseDir, "config", "skills", "blessing_of_the_moon.json");
            if (!File.Exists(path))
                throw new FileNotFoundException("Не найден конфиг blessing_of_the_moon.json", path);

            var json = File.ReadAllText(path);
            _cfg = JObject.Parse(json);

            BaseCooldownCfg = (double?)_cfg["BaseCooldown"] ?? 25.0;

            var talents = _cfg["Talents"] as JObject;
            TalentPlusPenetrationCfg = (int?)talents?["PlusPenetration"] ?? 1;
            TalentPlusCriticalHitCfg = (int?)talents?["PlusCriticalHit"] ?? 2;

            _levels = (JArray)_cfg["Levels"];
            if (_levels == null || _levels.Count == 0)
                throw new InvalidOperationException("В конфиге BlessingOfTheMoon пустой Levels.");
        }

        // ===== Публичные поля/свойства (имена как в исходнике) =====

        /// <summary>Прибавка шанса крита</summary>
        [JsonIgnore] public int AdditionCriticalHit = 8;

        /// <summary>Прибавка пробива</summary>
        [JsonIgnore] public int AdditionPenetration = 5;

        /// <summary>Базовое значение длительности перезарядки навыка</summary>
        [JsonIgnore] public double BaseTimeCooldown = 25;

        private bool hasTalantPlusPenetration = false;
        /// <summary>Талант левой ветки: +1 пробив</summary>
        public bool HasTalantPlusPenetration
        {
            get => hasTalantPlusPenetration;
            set { hasTalantPlusPenetration = value; Level = level; NotifyPropertyChanged(nameof(HasTalantPlusPenetration)); }
        }

        private bool hasTalantPlusCriticalHit = false;
        /// <summary>Талант центральной ветки: +2 крита</summary>
        public bool HasTalantPlusCriticalHit
        {
            get => hasTalantPlusCriticalHit;
            set { hasTalantPlusCriticalHit = value; Level = level; NotifyPropertyChanged(nameof(HasTalantPlusCriticalHit)); }
        }

        private int level = 1;
        /// <summary>Обновление уровня; пересчитывает прибавки</summary>
        public int Level
        {
            get => level;
            set
            {
                if (value < 1 || value > 4) return;
                level = value;

                ApplyLevelFromConfig(level);          // базовые значения из JSON

                // Приоритет у тебя был: если Penetration — то +1 PEN, иначе +2 CRIT
                if (HasTalantPlusPenetration)
                    AdditionPenetration += TalentPlusPenetrationCfg;
                else if (HasTalantPlusCriticalHit)
                    AdditionCriticalHit += TalentPlusCriticalHitCfg;

                NotifyPropertyChanged(nameof(Level));
            }
        }

        // ===== Внутренняя загрузка уровня из JSON =====
        private void ApplyLevelFromConfig(int lvl)
        {
            var row = _levels.FirstOrDefault(x => (int)x["Level"] == lvl) as JObject
                   ?? _levels.FirstOrDefault(x => (int)x["Level"] == 1) as JObject;

            AdditionCriticalHit = (int?)row?["AdditionCriticalHit"] ?? AdditionCriticalHit;
            AdditionPenetration = (int?)row?["AdditionPenetration"] ?? AdditionPenetration;
            BaseTimeCooldown = BaseCooldownCfg;
        }

        // ===== INotifyPropertyChanged =====
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string prop = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
