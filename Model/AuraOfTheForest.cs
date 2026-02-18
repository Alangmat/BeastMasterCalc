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
    public class AuraOfTheForest : INotifyPropertyChanged
    {
        // ===== Конфиг =====
        private static JObject _cfg;
        private static JArray _levels;

        private static double BaseCooldownCfg = 24;
        private static int TimeActiveCfg = 10;
        private static int DelayCfg = 2;

        private static double BaseIncreaseCfg = 0.20;      // 20% -> 0.20
        private static double TalentPowerOfNatureCfg = 0.15; // 15% -> 0.15

        static AuraOfTheForest()
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var path = Path.Combine(baseDir, "config", "skills", "aura_of_the_forest.json");
            if (!File.Exists(path))
                throw new FileNotFoundException("Не найден конфиг aura_of_the_forest.json", path);

            var json = File.ReadAllText(path);
            _cfg = JObject.Parse(json);

            BaseCooldownCfg = (double?)_cfg["BaseCooldown"] ?? 24.0;
            TimeActiveCfg = (int?)_cfg["TimeActive"] ?? 10;
            DelayCfg = (int?)_cfg["Delay"] ?? 2;

            BaseIncreaseCfg = ((double?)_cfg["BaseIncrease"] ?? 20.0) / 100.0;
            TalentPowerOfNatureCfg = ((double?)_cfg["TalentPowerOfNature"] ?? 15.0) / 100.0;

            _levels = (JArray)_cfg["Levels"];
            if (_levels == null || _levels.Count == 0)
                throw new InvalidOperationException("В конфиге AuraOfTheForest пустой Levels.");
        }

        // ===== Публичный API =====
        private int level = 1;
        public int Level
        {
            get => level;
            set
            {
                level = value;
                ApplyLevelFromConfig(level);
                NotifyPropertyChanged("Level");
            }
        }

        private bool hasTalantPowerOfNature = false;
        public const int TimeActive = 10;
        public const int Delay = 2;

        public bool HasTalantPowerOfNature
        {
            get => hasTalantPowerOfNature;
            set { hasTalantPowerOfNature = value; NotifyPropertyChanged("HasTalantPowerOfNature"); }
        }

        private double coefficientMagicalDD = 0.35;
        [JsonIgnore] public double BaseTimeCooldown = 24;

        // ===== Формула =====
        public int Formula(int magicaldd)
        {
            ApplyLevelFromConfig(level);

            // базовое усиление ауры
            double result = magicaldd * coefficientMagicalDD;

            // бонус таланта
            if (HasTalantPowerOfNature)
                result *= (1.0 + TalentPowerOfNatureCfg) * (1 + BaseIncreaseCfg);


            return (int)Math.Round(result);
        }

        // ===== Загрузка уровня из JSON =====
        private void ApplyLevelFromConfig(int lvl)
        {
            var row = _levels.FirstOrDefault(x => (int)x["Level"] == lvl) as JObject;
            if (row == null)
                row = _levels.FirstOrDefault(x => (int)x["Level"] == 1) as JObject;

            // читаем Percents вместо PercentMagicalDD
            coefficientMagicalDD = (((double?)row?["Percents"]) ?? 35.0) / 100.0;

            BaseTimeCooldown = BaseCooldownCfg;
        }

        // ===== INotifyPropertyChanged =====
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string prop = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
