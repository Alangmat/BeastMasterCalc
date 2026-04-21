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
    public class OrderToAttack : INotifyPropertyChanged
    {
        // ===== Конфиг =====
        private static JObject _cfg;
        private static JArray _levels;
        private static JArray _talentGuardianLevels;
        private static JArray _talentDualLevels;

        private static double BaseCooldownCfg = 10;

        static OrderToAttack()
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var path = Path.Combine(baseDir, "config", "skills", "order_to_attack.json");
            if (!File.Exists(path))
                throw new FileNotFoundException("Не найден конфиг order_to_attack.json", path);

            var json = File.ReadAllText(path);
            _cfg = JObject.Parse(json);

            BaseCooldownCfg = (double?)_cfg["BaseCooldown"] ?? 10.0;

            _levels = (JArray)_cfg["Levels"];
            if (_levels == null || _levels.Count == 0)
                throw new InvalidOperationException("В конфиге OrderToAttack пустой Levels.");

            var talents = _cfg["Talents"] as JObject;
            _talentGuardianLevels = talents?["GuardianUnity"]?["Levels"] as JArray ?? new JArray();
            _talentDualLevels = talents?["DualRage"]?["Levels"] as JArray ?? new JArray();
        }
        [JsonConstructor] public OrderToAttack() { }

        public OrderToAttack(BeastAwakening luna)
        {
            Luna = luna;
        }

        // ===== Публичные поля/свойства — как у тебя =====
        public string Description { get; set; } = "";

        [JsonIgnore] public int _level;
        public int Level
        {
            get => _level;
            set
            {
                _level = value;
                ApplyLevelFromConfig(_level);
                NotifyPropertyChanged(nameof(Level));
            }
        }

        private double coefficient = 0.05;
        [JsonIgnore] public double BaseTimeCooldown = 10;
        [JsonIgnore] public BeastAwakening Luna;

        private int lvlTalantDualRage = 0;
        public int LvlTalantDualRage
        {
            get => lvlTalantDualRage;
            set { lvlTalantDualRage = value; NotifyPropertyChanged("LvlTalantDualRage"); }
        }

        private int lvlTalantGuardianUnity = 0;
        public int LvlTalantGuardianUnity
        {
            get => lvlTalantGuardianUnity;
            set { lvlTalantGuardianUnity = value; NotifyPropertyChanged("LvlTalantGuardianUnity"); }
        }

        // ===== Формула =====
        public int Formula(int magedd, int physdd)
        {
            // держим всё актуальным
            ApplyLevelFromConfig(Level);

            double result = Luna.Formula(magedd, physdd) * coefficient;
            return (int)result;
        }

        // ===== Внутреннее =====
        private void ApplyLevelFromConfig(int lvl)
        {
            // базовый коэф. по уровню умения
            var row = _levels.FirstOrDefault(x => (int)x["Level"] == lvl) as JObject
                   ?? _levels.FirstOrDefault(x => (int)x["Level"] == 1) as JObject;

            coefficient = (((double?)row?["Percents"]) ?? 5.0) / 100.0;
            BaseTimeCooldown = BaseCooldownCfg;

            // прибавки талантов — читаем «как Levels»
            // приоритет как в твоём коде: сначала GuardianUnity, иначе DualRage
            double addGuardian = GetTalentAdd(_talentGuardianLevels, LvlTalantGuardianUnity);
            if (addGuardian > 0)
            {
                coefficient += addGuardian;
            }
            else
            {
                double addDual = GetTalentAdd(_talentDualLevels, LvlTalantDualRage);
                if (addDual > 0)
                    coefficient += addDual;
            }
        }

        // возвращает добавку к coefficient в ДОЛЯХ для уровня таланта tLvl
        // (например в JSON Percents=2 -> 0.02)
        private static double GetTalentAdd(JArray talentLevels, int tLvl)
        {
            if (tLvl <= 0 || talentLevels == null || talentLevels.Count == 0)
                return 0.0;

            var row = talentLevels.FirstOrDefault(x => (int)x["Level"] == tLvl) as JObject;
            if (row == null)
                // если просили уровень больше, чем описан — берём максимум из массива
                row = talentLevels
                        .OrderBy(x => (int?)((JObject)x)["Level"] ?? 0)
                        .LastOrDefault() as JObject;

            double perc = (double?)row?["Percents"] ?? 0.0;
            return perc / 100.0;
        }

        // ===== INotifyPropertyChanged =====
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string prop = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
