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
    public class ChainLightning : INotifyPropertyChanged
    {
        private static JObject _cfg;
        private static JArray _levels;

        private static double BaseCooldownCfg = 19;
        private static double RelicBonusPercentCfg = 0.12; // 12% -> 0.12

        static ChainLightning()
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var path = Path.Combine(baseDir, "config", "skills", "chain_lightning.json");
            if (!File.Exists(path))
                throw new FileNotFoundException("Не найден конфиг chain_lightning.json", path);

            var json = File.ReadAllText(path);
            _cfg = JObject.Parse(json);

            BaseCooldownCfg = (double?)_cfg["BaseCooldown"] ?? 19.0;
            RelicBonusPercentCfg = ((double?)_cfg["RelicBonusPercent"] ?? 12.0) / 100.0;

            _levels = (JArray)_cfg["Levels"];
            if (_levels == null || _levels.Count == 0)
                throw new InvalidOperationException("В конфиге ChainLightning пустой Levels.");
        }

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

        [JsonIgnore] public double BaseTimeCooldown = 19;

        private double coefficientMageDD = 1;
        //public double CoefficientMageDD
        //{
        //    get => coefficientMageDD;
        //    set { coefficientMageDD = value; NotifyPropertyChanged("MageDD"); }
        //}

        private double coefficientPhysicalDD = 0.55;
        //public double CoefficientPhysicalDD
        //{
        //    get => coefficientPhysicalDD;
        //    set { coefficientPhysicalDD = value; NotifyPropertyChanged("PhysicalDD"); }
        //}

        private bool hasRelic;
        public bool HasRelic
        {
            get => hasRelic;
            set { hasRelic = value; NotifyPropertyChanged("HasRelic"); }
        }

        public int Formula(int mageDD, int physicalDD)
        {
            ApplyLevelFromConfig(level);

            double result = (mageDD >= physicalDD)
                ? coefficientMageDD * mageDD
                : coefficientPhysicalDD * physicalDD;

            if (HasRelic)
                result *= 1.0 + RelicBonusPercentCfg;

            return (int)result;
        }

        private void ApplyLevelFromConfig(int lvl)
        {
            var row = _levels.FirstOrDefault(x => (int)x["Level"] == lvl) as JObject
                   ?? _levels.FirstOrDefault(x => (int)x["Level"] == 1) as JObject;

            // читаем MagicalPercents вместо MagePercents
            coefficientMageDD = (((double?)row?["MagicalPercents"]) ?? 100.0) / 100.0;
            coefficientPhysicalDD = (((double?)row?["PhysicalPercents"]) ?? 55.0) / 100.0;

            BaseTimeCooldown = BaseCooldownCfg;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string prop = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
