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
    public class BestialRampage : INotifyPropertyChanged
    {
        // ===== Конфиг (кэш на всё приложение) =====
        private static JObject _cfg;
        private static JArray _levels;

        private static double BaseCooldownCfg = 26;
        private static double TalentAtkSpeedPercCfg = 2; // проценты

        static BestialRampage()
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var path = Path.Combine(baseDir, "config", "skills", "bestial_rampage.json");
            if (!File.Exists(path))
                throw new FileNotFoundException("Не найден конфиг bestial_rampage.json", path);

            var json = File.ReadAllText(path);
            _cfg = JObject.Parse(json);

            BaseCooldownCfg = (double?)_cfg["BaseCooldown"] ?? 26.0;
            TalentAtkSpeedPercCfg = (double?)_cfg["TalentAttackSpeedPercents"] ?? 2.0;

            _levels = (JArray)_cfg["Levels"];
            if (_levels == null || _levels.Count == 0)
                throw new InvalidOperationException("В конфиге BestialRampage пустой Levels.");
        }

        // ===== Конструктор (как у тебя) =====
        public BestialRampage(BeastAwakening luna)
        {
            Luna = luna;
        }
        [JsonConstructor]
        public BestialRampage() { }

        // ===== Публичное API (имена не меняю) =====
        private int level = 1;
        public int Level
        {
            get => level;
            set
            {
                level = value;
                ApplyLevelFromConfig(level);

                // талант даёт +2% к скорости атаки поверх уровневого значения
                if (HasTalant)
                    IncreaseAttackSpeed += TalentAtkSpeedPercCfg;

                NotifyPropertyChanged(nameof(Level));
            }
        }

        [JsonIgnore] public double BaseTimeCooldown = 26;
        [JsonIgnore] public int TimeActive = 8;

        // ВАЖНО: IncreaseDD — множитель урона (1.10, 1.15, ...)
        [JsonIgnore] public double IncreaseDD = 1.1;

        // IncreaseAttackSpeed — проценты (10, 15, 20, 30)
        [JsonIgnore] public double IncreaseAttackSpeed = 10;

        [JsonIgnore]
        public BeastAwakening Luna;

        private bool hasTalant = false;
        public bool HasTalant
        {
            get => hasTalant;
            set { hasTalant = value; NotifyPropertyChanged(nameof(HasTalant)); }
        }

        // ===== Формула урона =====
        public int Formula(int magedd, int physdd)
        {
            // освежим на случай изменений
            Level = level;

            int resultDD = (int)(Luna.Formula(magedd, physdd) * IncreaseDD);
            return resultDD;
        }

        // ===== Подстановка значений уровня из JSON =====
        private void ApplyLevelFromConfig(int lvl)
        {
            var row = _levels.FirstOrDefault(x => (int)x["Level"] == lvl) as JObject
                   ?? _levels.FirstOrDefault(x => (int)x["Level"] == 1) as JObject;

            // проценты → нужные величины
            TimeActive = (int?)row?["TimeActive"] ?? TimeActive;

            // 110% -> 1.10
            IncreaseDD = (((double?)row?["DamagePercents"]) ?? 110.0) / 100.0;

            // скорость атаки храним в процентах, как у тебя (10, 15, ...)
            IncreaseAttackSpeed = ((double?)row?["AttackSpeedPercents"]) ?? 10.0;

            BaseTimeCooldown = BaseCooldownCfg;
        }

        // ===== INotifyPropertyChanged =====
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string prop = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
