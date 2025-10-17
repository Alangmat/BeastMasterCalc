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
    public class MoonTouch : INotifyPropertyChanged
    {
        // ====== Конфигурация ======
        private static JObject _cfg;
        private static JArray _levels;

        private static double BaseCooldownCfg;
        private static double RelicBonusPercentCfg;
        private static double TalentPlusBonusCfg;

        static MoonTouch()
        {
            // читаем конфиг один раз при старте
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var path = Path.Combine(baseDir, "config", "skills", "moon_touch.json");
            if (!File.Exists(path))
                throw new FileNotFoundException("Не найден конфиг moon_touch.json", path);

            var json = File.ReadAllText(path);
            _cfg = JObject.Parse(json);
            _levels = (JArray)_cfg["Levels"];

            BaseCooldownCfg = (double?)_cfg["BaseCooldown"] ?? 100.0;
            RelicBonusPercentCfg = (double?)_cfg["RelicBonusPercent"] ?? 12.0;
            TalentPlusBonusCfg = (double?)_cfg["TalentPlusBonus"] ?? 5.0;
        }

        // ====== Публичные поля, как в оригинале ======
        [JsonIgnore] public int _level;
        [JsonIgnore] public double coefficient = 1.3;
        [JsonIgnore] public int baseDmg = 40;
        [JsonIgnore] public double BaseTimeCooldown = 11;
        [JsonIgnore] public int DurationMoonTouch = 4;
        [JsonIgnore] public double CoefficientDD = 0.07;
        [JsonIgnore] public BeastAwakening Luna;

        private bool hasTalantPlus;
        private bool hasRelic;

        // ====== Публичные свойства, имена не меняем ======

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

        public bool HasTalantPlus
        {
            get => hasTalantPlus;
            set
            {
                hasTalantPlus = value;
                NotifyPropertyChanged(nameof(HasTalantPlus));
            }
        }

        public bool HasRelic
        {
            get => hasRelic;
            set
            {
                hasRelic = value;
                NotifyPropertyChanged(nameof(HasRelic));
            }
        }

        // ====== Подгрузка данных уровня из JSON ======
        private void ApplyLevelFromConfig(int level)
        {
            var lvl = _levels.FirstOrDefault(l => (int)l["Level"] == level) as JObject;
            if (lvl == null) return;

            // подставляем значения в публичные поля
            coefficient = ((double?)lvl["PercentDamage"] ?? 100.0) / 100.0;
            baseDmg = (int?)lvl["BaseDamage"] ?? 0;
            DurationMoonTouch = (int?)lvl["Duration"] ?? 0;
            CoefficientDD = ((double?)lvl["PercentDamageLuna"] ?? 0.0) / 100.0;

            BaseTimeCooldown = BaseCooldownCfg;

            // бонус таланта (прибавляем %)
            if (HasTalantPlus)
                coefficient += (TalentPlusBonusCfg / 100.0);
        }

        // ====== Основная формула ======
        public int Formula(int dmg)
        {
            ApplyLevelFromConfig(Level);

            double result = baseDmg + dmg * coefficient;

            if (HasRelic)
                result *= 1.0 + (RelicBonusPercentCfg / 100.0);

            return (int)Math.Round(result);
        }

        // ====== PropertyChanged ======
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string prop = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
