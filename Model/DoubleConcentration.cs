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
    public class DoubleConcentration : INotifyPropertyChanged
    {
        // ===== Конфиг (кэш на всё приложение) =====
        private static JObject _cfg;
        private static JArray _levels;

        private static int BaseCooldownCfg = 22;
        private static double TalentDexterityFactorCfg = 0.4; // итоговый коэффициент (после (100 - raw)/100)

        static DoubleConcentration()
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var path = Path.Combine(baseDir, "config", "skills", "double_concentration.json");
            if (!File.Exists(path))
                throw new FileNotFoundException("Не найден конфиг double_concentration.json", path);

            var json = File.ReadAllText(path);
            _cfg = JObject.Parse(json);

            BaseCooldownCfg = (int?)_cfg["BaseCooldown"] ?? 22;

            // В конфиге процент "среза": 60 => остаётся 40% => 0.4
            double rawPercent = (double?)_cfg["TalentDeadlyDexterityAttackSpeedFactor"] ?? 60.0;
            TalentDexterityFactorCfg = (100.0 - rawPercent) / 100.0;

            _levels = (JArray)_cfg["Levels"];
            if (_levels == null || _levels.Count == 0)
                throw new InvalidOperationException("В конфиге DoubleConcentration пустой Levels.");
        }

        public string Description { get; set; } = "";

        /// <summary>Прибавка крит урона (под 4 стаками)</summary>
        [JsonIgnore] public double AdditionCriticalDamage = 8;

        /// <summary>Прибавка перезарядки навыков (под 4 стаками)</summary>
        [JsonIgnore] public int AdditionSkillCooldown = 16;

        /// <summary>Талант: скорость атаки вместо КД (AS = AdditionSkillCooldown * 0.4 при значении 60 в конфиге)</summary>
        public bool HasTalentDeadlyDexterity = false;

        [JsonIgnore] public int BaseTimeCooldown = 22;

        /// <summary>Если талант активен — 0, иначе вернёт бонус к КД</summary>
        public double AddSkillCooldown()
        {
            return HasTalentDeadlyDexterity ? 0 : AdditionSkillCooldown;
        }

        /// <summary>Если талант активен — вернёт бонус к Скорости атаки (от КД * коэффициент), иначе 0</summary>
        public double AddAttackSpeed()
        {
            return HasTalentDeadlyDexterity ? (AdditionSkillCooldown * TalentDexterityFactorCfg) : 0;
        }

        private int level = 1;
        /// <summary>Обновление уровня: тянет базовые значения из конфига</summary>
        public int Level
        {
            get => level;
            set
            {
                if (value < 1 || value > 4) return;
                level = value;

                ApplyLevelFromConfig(level);

                NotifyPropertyChanged(nameof(Level));
            }
        }

        private void ApplyLevelFromConfig(int lvl)
        {
            var row = _levels.FirstOrDefault(x => (int)x["Level"] == lvl) as JObject
                   ?? _levels.FirstOrDefault(x => (int)x["Level"] == 1) as JObject;

            AdditionCriticalDamage = (double?)row?["AdditionCriticalDamage"] ?? AdditionCriticalDamage;
            AdditionSkillCooldown = (int?)row?["AdditionSkillCooldown"] ?? AdditionSkillCooldown;

            BaseTimeCooldown = BaseCooldownCfg;
        }

        // ===== INotifyPropertyChanged =====
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string prop = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}

