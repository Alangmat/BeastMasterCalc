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
    public class Moonlight : INotifyPropertyChanged
    {
        // ===== Конфиг (кэш на всё приложение) =====
        private static JObject _cfg;
        private static JArray _levels;
        private static JArray _talentLevels;

        private static double BaseCooldownCfg = 14;

        static Moonlight()
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var path = Path.Combine(baseDir, "config", "skills", "moonlight.json");
            if (!File.Exists(path))
                throw new FileNotFoundException("Не найден конфиг moonlight.json", path);

            var json = File.ReadAllText(path);
            _cfg = JObject.Parse(json);

            BaseCooldownCfg = (double?)_cfg["BaseCooldown"] ?? 14.0;

            _levels = (JArray)_cfg["Levels"];
            if (_levels == null || _levels.Count == 0)
                throw new InvalidOperationException("В конфиге Moonlight пустой Levels.");

            var talents = _cfg["Talents"] as JObject;
            _talentLevels = talents?["Levels"] as JArray ?? new JArray();
        }

        // ===== Твоё API (имена/стиль как в исходнике) =====
        [JsonIgnore] public string Description { get; set; } = "";

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

        [JsonIgnore] public double BaseTimeCooldown = 14;
        private double coefficientMagicalDD = 0.2;

        private int lvlTalant = 0;
        public int LvlTalant
        {
            get => lvlTalant;
            set { lvlTalant = value; NotifyPropertyChanged("LvlTalant"); }
        }

        public int Formula(int magicaldd)
        {
            // ленивый пересчёт — как у тебя
            Level = level;
            int result = (int)(magicaldd * coefficientMagicalDD);
            return result;
        }

        // ===== Внутреннее =====
        private void ApplyLevelFromConfig(int lvl)
        {
            // 1) базовый коэффициент по уровню умения
            var row = _levels.FirstOrDefault(x => (int)x["Level"] == lvl) as JObject
                   ?? _levels.FirstOrDefault(x => (int)x["Level"] == 1) as JObject;

            // Percents -> коэффициент (20 -> 0.20)
            coefficientMagicalDD = (((double?)row?["Percents"]) ?? 20.0) / 100.0;

            // 2) прибавка от таланта — тоже из Levels (п.п. => доли)
            if (LvlTalant > 0)
            {
                // если в конфиге нет такого уровня — берём максимум
                var trow = _talentLevels.FirstOrDefault(x => (int)x["Level"] == LvlTalant) as JObject
                       ?? _talentLevels
                            .OrderBy(x => (int?)((JObject)x)["Level"] ?? 0)
                            .LastOrDefault() as JObject;

                if (trow != null)
                {
                    var addPerc = (double?)trow["Percents"] ?? 0.0;
                    coefficientMagicalDD += addPerc / 100.0; // +0.03 / +0.06 / +0.09
                }
            }

            BaseTimeCooldown = BaseCooldownCfg;
        }

        // ===== INotifyPropertyChanged =====
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string prop = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
