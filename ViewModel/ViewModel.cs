using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using System.IO;
using System.Collections.ObjectModel;
using Newtonsoft.Json.Linq;
using Shared;

namespace ViewModel
{
    public class ViewModel : INotifyPropertyChanged
    {
        public ViewModel()
        {
            LoadBuilds();
            GenerateNewDataSet();
            Calculate();
        }

        /*private ObservableCollection<KeyValuePair<CastleSectors, string>> _castles = new ObservableCollection<KeyValuePair<CastleSectors, string>>()
        {
            new KeyValuePair<CastleSectors, string>(CastleSectors.Empty, "Без замка | 0%"),
            new KeyValuePair<CastleSectors, string>(CastleSectors.First, "1 сектор | 5%"),
            new KeyValuePair<CastleSectors, string>(CastleSectors.Second, "2 сектор | 7.5%"),
            new KeyValuePair<CastleSectors, string>(CastleSectors.Third, "3 сектор | 10%"),
            new KeyValuePair<CastleSectors, string>(CastleSectors.Fourth, "4 сектор | 12.5%"),
            new KeyValuePair<CastleSectors, string>(CastleSectors.Fifth, "5 сектор | 15%"),
        };*/
        /*public ObservableCollection<KeyValuePair<CastleSectors, string>> CastlesNew
        {
            get => _castles;
        }

        private CastleSectors selectedCastle = CastleSectors.Empty;
        public CastleSectors SelectedCastle
        {
            //get => selectedCastle;
            get => DataSet.SelectedCastle;
            set
            {
                DataSet.SelectedCastle = value;
                coefficientCastle = (2.5 * (int)value).ConvertToCoefficient();
                Calculate();
                NotifyPropertyChanged(nameof(SelectedCastle));
            }
        }*/








        #region работа с билдами
        private ObservableCollection<Build> builds = new ObservableCollection<Build>();
        public ObservableCollection<Build> Builds
        {
            get => builds;
            set
            {
                builds = value;
                NotifyPropertyChanged(nameof(Builds));
            }
        } 
        public void LoadBuilds()
        {
            string jsonFromFile = File.ReadAllText("saves.json");
            Builds = JsonConvert.DeserializeObject<ObservableCollection<Build>>(jsonFromFile);
        }
        public void SaveBuilds()
        {
            string json = JsonConvert.SerializeObject(Builds);
            File.WriteAllText("saves.json", json);
        }
        public void AddDataSet()
        {
            GenerateNewDataSet();
            builds.Add(DataSet);
            string json = JsonConvert.SerializeObject(DataSet);
            DataSet = JsonConvert.DeserializeObject<Build>(json);
            SelectedDataSet = builds[builds.Count() - 1];
        }
        public void AddCurrentDataSet()
        {
            DataSet.LastDate = DateTime.Now.ToString();
            DataSet.ID = Guid.NewGuid();
            builds.Add(DataSet);
            string json = JsonConvert.SerializeObject(DataSet);
            DataSet = JsonConvert.DeserializeObject<Build>(json);
            SelectedDataSet = builds[builds.Count() - 1];
        }
        private Build selectedDataSet;
        public Build SelectedDataSet
        {
            get => selectedDataSet;
            set
            {
                selectedDataSet = value;
                NotifyPropertyChanged(nameof(SelectedDataSet));
            }
        }
        
        public void ChoiceDataSet()
        {
            if (!(SelectedDataSet is null))
            {
                string json = JsonConvert.SerializeObject(SelectedDataSet);
                DataSet = JsonConvert.DeserializeObject<Build>(json);
                updateStateDataSet();
                //SelectedDataSet = null;
            }
        }
        public int EditDataSet()
        {
            var editList = Builds.Where(x => x.ID == DataSet.ID).ToList();
            if (editList.Count > 0)
            {
                var currentDataSet = editList[0];
                int curID = Builds.IndexOf(currentDataSet);
                Builds[curID] = DataSet;
                Builds[curID].LastDate = DateTime.Now.ToString();
                string json = JsonConvert.SerializeObject(DataSet);
                DataSet = JsonConvert.DeserializeObject<Build>(json);
                updateStateDataSet();
                SelectedDataSet = Builds[curID];
                return 0;
            }
            return -1;
        }
        public void DeleteSelectedDataSet()
        {
            Builds.Remove(SelectedDataSet);
        }
        private void GenerateNewDataSet()
        {
            DataSet = new Build();

            DataSet.Attack = new Attack();
            //attack = DataSet.Attack; //хз зщачем тут это
            DataSet.MaceSelected = true;
            MaceSelected = DataSet.MaceSelected;
            DataSet.BeastAwakening = new BeastAwakening();

            DataSet.BeastAwakening = new BeastAwakening();
            Beast_Awakening.Level = 1;

            DataSet.OrderToAttack = new OrderToAttack(DataSet.BeastAwakening);
            OrderToAttack.Level = 1;

            DataSet.MoonTouch = new MoonTouch();
            Moon_Touch.Level = 1;

            DataSet.ChainLightning = new ChainLightning();
            Chain_Lightning.Level = 1;

            DataSet.BestialRampage = new BestialRampage(DataSet.BeastAwakening);
            Bestial_Rampage.Level = 1;

            DataSet.AuraOfTheForest = new AuraOfTheForest();
            AuraOfTheForest.Level = 1;

            DataSet.Moonlight = new Moonlight();
            Moonlight.Level = 1;

            DataSet.BlessingOfTheMoon = new BlessingOfTheMoon();
            BlessingOfTheMoon.Level = 1;

            DataSet.DoubleConcentration = new DoubleConcentration();
            DoubleConcentration.Level = 1;

            DataSet.MagicalDamage = "0";
            DataSet.PhysicalDamage = "0";

            //DataSet.NumberCastle = Castles[0];

            updateStateDataSet();
        }

        private void updateStateDataSet()
        {

            NotifyPropertyChanged(nameof(Name));
            NotifyPropertyChanged(nameof(ID));
            NotifyPropertyChanged(nameof(Description));


            DataSet.BestialRampage.Luna = DataSet.BeastAwakening;
            DataSet.OrderToAttack.Luna = DataSet.BeastAwakening;


            #region Вызовы событий об обновлении статов
            NotifyPropertyChanged(nameof(MagicalDD));
            NotifyPropertyChanged(nameof(PhysicalDD));
            NotifyPropertyChanged(nameof(SkillCooldown));
            NotifyPropertyChanged(nameof(CriticalHit));
            NotifyPropertyChanged(nameof(CriticalDamage));
            NotifyPropertyChanged(nameof(AttackSpeed));
            NotifyPropertyChanged(nameof(Penetration));
            NotifyPropertyChanged(nameof(Accuracy));
            NotifyPropertyChanged(nameof(AttackStrength));
            NotifyPropertyChanged(nameof(PiercingAttack));
            NotifyPropertyChanged(nameof(Rage));
            NotifyPropertyChanged(nameof(Facilitation));
            //NotifyPropertyChanged(nameof(PercentMagicalDD));
            //NotifyPropertyChanged("PercentPhysicalDD");

            NotifyPropertyChanged(nameof(AdditionalPercentPDDStart));
            NotifyPropertyChanged(nameof(AdditionalPercentMDDStart));

            NotifyPropertyChanged(nameof(AdditionalPercentPDDFinal));
            NotifyPropertyChanged(nameof(AdditionalPercentMDDFinal));


            NotifyPropertyChanged(nameof(Protection));
            NotifyPropertyChanged(nameof(Dodge));
            NotifyPropertyChanged(nameof(Resilience));
            #endregion

            #region Обновление пух, рел
            NotifyPropertyChanged(nameof(AttackActive));
            NotifyPropertyChanged(nameof(HasRelicMoonTouch));
            NotifyPropertyChanged(nameof(HasRelicChainLightning));

            NotifyPropertyChanged(nameof(AxeSelected));
            NotifyPropertyChanged(nameof(MaceSelected));
            NotifyPropertyChanged(nameof(SpearSelected));
            NotifyPropertyChanged(nameof(StaffSelected));
            NotifyPropertyChanged(nameof(SwordSelected));

            NotifyPropertyChanged(nameof(ChechBPDungeon));
            NotifyPropertyChanged(nameof(SacredShieldHeroActive));
            NotifyPropertyChanged(nameof(SacredShieldLunaActive));
            NotifyPropertyChanged(nameof(Counterstand));

            NotifyPropertyChanged(nameof(GuildDamageStartModifierActive));
            NotifyPropertyChanged(nameof(GuildDamageModifierActive));
            NotifyPropertyChanged(nameof(TalentDamageStartModifierActive));
            NotifyPropertyChanged(nameof(TalentDamageModifierActive));
            NotifyPropertyChanged(nameof(CastleStartModifierActive));
            NotifyPropertyChanged(nameof(CastleSwordActive));


            NotifyPropertyChanged(nameof(GodsAid));

            #endregion

            #region Обновление талантов
            
            ForestInspirationActive = DataSet.ForestInspirationActive;
            DualRageActive = DataSet.DualRageActive;
            GuardianUnityActive = DataSet.GuardianUnityActive;

            NotifyPropertyChanged(nameof(HarmoniousPowerStartModifierActive));

            NotifyPropertyChanged(nameof(HasTalantMoonTouchPlus));
            NotifyPropertyChanged(nameof(HasTalantPowerOfNature));


            NotifyPropertyChanged(nameof(HasTalantBeastAwakeningMage));
            NotifyPropertyChanged(nameof(LvlTalantBeastAwakeningPhysical));
            NotifyPropertyChanged(nameof(HasTalantBestialRampage));
            NotifyPropertyChanged(nameof(HasTalantGrandeurOfTheLotus));
            NotifyPropertyChanged(nameof(LvlTalantMoonlightPlus));
            NotifyPropertyChanged(nameof(HasTalantSymbiosis));
            NotifyPropertyChanged(nameof(LvlTalantOrderToAttackPlusDualRage));
            NotifyPropertyChanged(nameof(LvlTalantOrderToAttackPlusGuardianUnity));


            LvlTalantBestialRage = DataSet.LvlTalantBestialRage;
            LvlTalantPredatoryDelirium = DataSet.LvlTalantPredatoryDelirium;
            LvlTalantAnimalRage = DataSet.LvlTalantAnimalRage;
            LvlTalantMomentOfPower = DataSet.LvlTalantMomentOfPower;
            LvlTalantLongDeath = DataSet.LvlTalantLongDeath;

            HasTalentHarmoniousPower = DataSet.HasTalentHarmoniousPower;

            LvlTalantContinuousFury = DataSet.LvlTalantContinuousFury;

            CriticalHit = DataSet.CriticalHit;
            Penetration = DataSet.Penetration;
            Accuracy = DataSet.Accuracy;

            NotifyPropertyChanged(nameof(SelectedAmulet));
            NotifyPropertyChanged(nameof(SelectedCloak));
            NotifyPropertyChanged(nameof(SelectedRingL));
            NotifyPropertyChanged(nameof(SelectedRingR));
            NotifyPropertyChanged(nameof(SelectedBraceletL));
            NotifyPropertyChanged(nameof(SelectedBraceletR));

            NotifyPropertyChanged(nameof(SelectedSet));

            NotifyPropertyChanged(nameof(SelectedHelmet));
            NotifyPropertyChanged(nameof(SelectedBody));
            NotifyPropertyChanged(nameof(SelectedHands));
            NotifyPropertyChanged(nameof(SelectedBelt));
            NotifyPropertyChanged(nameof(SelectedFoots));

            #endregion
            #region Свойства, зависимые от изменений
            // зависимые от изменений - в set присутствует какая-либо логика кроме калка и обновления

            //PercentMagicalDD = DataSet.PercentMagicalDD;
            //PercentPhysicalDD = DataSet.PercentPhysicalDD;

            SelectedCastle = DataSet.SelectedCastle;
            //NumberCastle = DataSet.NumberCastle;
            IsUsingBlessingOfTheMoonOnLuna = DataSet.IsUsingBlessingOfTheMoonOnLuna;
            CrushingWillActive = DataSet.CrushingWill;
            IrreversibleAngerActive = DataSet.IrreversibleAnger;
            #endregion

            Calculate();
        }

        /*private ICommand loadCommand;
        public ICommand LoadCommand
        {
            get => loadCommand == null ? new RelayCommand(Load) : loadCommand;
        }*/
        private const string FILE_SAVE = "save.json";
        private void Save()
        {
            string json = JsonConvert.SerializeObject(DataSet);
            File.WriteAllText(FILE_SAVE, json);
        }
        private ICommand saveCommand;
        public ICommand SaveCommand
        {
            get => saveCommand == null ? new RelayCommand(Save) : saveCommand;
        }

        private Build dataSet;
        public Build DataSet { 
            get => dataSet;
            set {
                dataSet = value;
                Calculate();
                NotifyPropertyChanged(nameof(DataSet));
            }
        }
        #endregion

        #region Калькуляторы
        public void Calculate()
        {
            int magicdd = 0;
            int physdd = 0;

            if (int.TryParse(MagicalDD, out magicdd))
            {
                if (int.TryParse(PhysicalDD, out physdd))
                {

                    /*CalcSkillCooldown();
                    CalcAttackSpeed();
                    CalcCriticalHit();
                    CalcCriticalDamage();
                    CalcPenetration();
                    CalcAccuracy();
                    CalcAttackStrength();
                    CalcPiercingAttack();
                    CalcRage();
                    CalcFacilitation();

                    calcHarmoniousPowerDD();

                    CalcPercentMagicalDDStart();
                    CalcPercentMagicalDD();
                    CalcPercentPhysicalDDStart();
                    CalcPercentPhysicalDD();*/

                    CalcStats();

                    CalcPercents();

                    double coefRage = FormulaCoefficientOfRage();

                    int pureMagicalDD = (int)(magicdd / percentMagicalDDStart.ConvertToCoefficient());
                    int purePhysicalDD = (int)(physdd / percentPhysicalDDStart.ConvertToCoefficient());

                    if (HarmoniousPowerStartModifierActive) 
                    {
                        pureMagicalDD = (int)(pureMagicalDD / harmoniousPowerMDD.ConvertToCoefficient());
                        purePhysicalDD = (int)(purePhysicalDD / harmoniousPowerPDD.ConvertToCoefficient());

                    }

                    magicdd = (int)(pureMagicalDD * (coefficientTriton  * MermanDuration() + coefRage)  + pureMagicalDD * percentMagicalDD.ConvertToCoefficient());
                    physdd = (int)((purePhysicalDD * coefRage + purePhysicalDD * percentPhysicalDD.ConvertToCoefficient()));
                    
                    if (HasTalentHarmoniousPower) 
                    {
                        magicdd = (int)(magicdd * harmoniousPowerMDD.ConvertToCoefficient());
                        physdd = (int)(physdd * harmoniousPowerPDD.ConvertToCoefficient());
                        
                    } 

                    int dpmAttack = CalcAttack(magicdd, physdd);
                    int dpmMoonTouch = CalcMoonTouch(magicdd);
                    int dpmBeastAwakening = CalcBeastAwakening(magicdd, physdd);
                    int dpmOrderToAttack = CalcOrderToAttack(magicdd, physdd);
                    int dpmChainLightning = CalcChainLightning(magicdd, physdd);
                    int dpmBestialRampage = CalcBestialRampage(magicdd, physdd);
                    var dpmAuraOfTheForest = CalcAuraOfTheForest(magicdd);
                    int dpmAuraOfTheForestLuna = dpmAuraOfTheForest[SourcesDamage.Luna];
                    int dpmAuraOfTheForestHero = dpmAuraOfTheForest[SourcesDamage.Hero];

                    int dpmMoonlight = CalcMoonlight(magicdd, pureMagicalDD);
                    var dpmSymbiosis = CalcSymbiosis(magicdd, physdd);
                    int dpmSymbiosisLuna = dpmSymbiosis[SourcesDamage.Luna];
                    int dpmSymbiosisHero = dpmSymbiosis[SourcesDamage.Hero];
                    //double realCooldawnBestialRampage = (Bestial_Rampage.BaseTimeCooldown / (1 + SkillCooldown / 100)) + TIME_CAST;
                    int resultDD = 0;
                    int resultDDLuna = 0;
                    int resultDDHero = 0;

                    // Перенести все проверки на активность внутрь калькуляторов
                    if (AttackActive)
                        resultDDHero += dpmAttack;
                    if (MoonTouchActive) 
                        resultDDHero += dpmMoonTouch;

                    if (BeastAwakeningActive)
                    {
                        if (BestialRampageActive)
                        {
                            resultDDLuna += (int)(dpmBeastAwakening * TimeWithoutBestialRampage() 
                                            + dpmBestialRampage * TimeBestialRampage());
                        }
                        else
                        {
                            resultDDLuna += dpmBeastAwakening;
                        }
                        if (OrderToAttackActive)
                        {
                            resultDDLuna += dpmOrderToAttack;
                        }
                        if (HasTalantSymbiosis )
                        {
                            resultDDHero += dpmSymbiosisHero;
                            resultDDLuna += dpmSymbiosisLuna;
                        }
                        
                    }
                    if (ChainLightningActive) resultDDHero += dpmChainLightning;
                    if (AuraOfTheForestActive) 
                    {
                        resultDDLuna += dpmAuraOfTheForestLuna;
                        resultDDHero += dpmAuraOfTheForestHero;
                    }
                    resultDDHero += dpmMoonlight;

                    resultDDHero = (int)(resultDDHero * sacredShieldHeroCoef());
                    resultDDLuna = (int)(resultDDLuna * sacredShieldLunaCoef());

                    resultDD = resultDDHero + resultDDLuna;
                    OutDD = resultDD.ToString();
                    OutDDHero = resultDDHero.ToString();
                    OutDDLuna = resultDDLuna.ToString();
                }
                else OutDD = "Ошибка данных";
            }
            else OutDD = "Ошибка данных";
        }
        private ICommand calculateCommand;
        public ICommand CalculateCommand
        {
            get => calculateCommand == null ? new RelayCommand(Calculate) : calculateCommand;
        }
        public void CalcStats()
        {
            CalcSkillCooldown();
            CalcAttackSpeed();
            CalcCriticalHit();
            CalcCriticalDamage();
            CalcPenetration();
            CalcAccuracy();
            CalcAttackStrength();
            CalcPiercingAttack();
            CalcRage();
            CalcFacilitation();
        }
        public void CalcPercents()
        {
            calcHarmoniousPowerDD();

            CalcPercentMagicalDDStart();
            CalcPercentMagicalDD();
            CalcPercentPhysicalDDStart();
            CalcPercentPhysicalDD();
        }

        private double AttackDelay()
        {
            double result = ((Attack.TimeDelay * (100 - AttackSpeedFinal) / 100) / LegendaryCoefficientAttackSpeed());
            return result;
        }
        public int CalcAttack(int magedd, int physdd)
        {
            int result = (int)(Attack.Formula(magedd, physdd) 
                * coefficientPredatoryDeliriumTalant 
                * FormulaCoefficientOfAttackStrength() 
                * FormulaCoefficientOfPiercingAttack());
            OutAttackDD = result.ToString();
            result = (int)(result / AttackDelay() * 60);
            OutAttackDPM = result.ToString();
            // тут не умножается на пробив потому что формула пронзы в себе содержит коэффициент пробива просто с учетом пронзы
            // так что не надо дополнительно еще на пробив умножать
            result = (int)(result 
                * FormulaCoefficientOfCriticalHitHeroForAutoattack() 
                * FormulaCoefficientOfAccuracy() 
                * coefficientBPDungeon());
            return result;
        }
        private double MoonTouchCooldown()
        {
            double result = ((Moon_Touch.BaseTimeCooldown / SkillCooldownFinal.ConvertToCoefficient()) + TIME_CAST);
            return result;
        }
        public int CalcMoonTouch(int magedd)
        {
            int result = (int)(Moon_Touch.Formula(magedd) 
                * coefficientCastle
                * coefficientBestialRageTalant 
                * coefficientPredatoryDeliriumTalant 
                * coefficientMomentOfPowerTalant 
                * FormulaCoefficientOfPenetration()
                );
            OutMoonTouchDD = result.ToString();
            result = (int)(result * 60 / MoonTouchCooldown());
            OutMoonTouchDPM = result.ToString();
            result = (int)(result 
                * FormulaCoefficientOfCriticalHitForSkill() 
                * FormulaCoefficientOfAccuracy() 
                * coefficientBPDungeon());
            return result;
        }
        public double CoefficientOfMoonTouchForLuna()
        {
            double result = 1;
            if (MoonTouchActive)
                result = Moon_Touch.DurationMoonTouch * FormulaCounterstand() / MoonTouchCooldown() * Moon_Touch.CoefficientDD + 1;
            return result;
        }

        private double ChainLightningCooldown()
        {
            double result = ((Chain_Lightning.BaseTimeCooldown / SkillCooldownFinal.ConvertToCoefficient()) + TIME_CAST);
            return result;
        }
        public int CalcChainLightning(int magedd, int physdd)
        {
            int result = (int)(Chain_Lightning.Formula(magedd, physdd) 
                * coefficientCastle
                * coefficientBestialRageTalant 
                * coefficientPredatoryDeliriumTalant 
                * coefficientMomentOfPowerTalant 
                * FormulaCoefficientOfPenetration());

            OutChainLightningDD = result.ToString();
            result = (int)(result * 60 / ChainLightningCooldown() * LegendaryCoefficientChainLightning());
            OutChainLightningDPM = result.ToString();
            result = (int)(result 
                * FormulaCoefficientOfCriticalHitForSkill() 
                * FormulaCoefficientOfAccuracy() 
                * coefficientBPDungeon());
            return result;
        }
        public int CalcBeastAwakening(int magedd, int physdd)
        {
            int result = (int)(Beast_Awakening.Formula(magedd, physdd) 
                * FormulaCoefficientOfAttackStrength() 
                * FormulaCoefficientOfPiercingAttackLuna());
            OutBeastAwakeningDD = result.ToString();
            result = (int)(result * 60 / Beast_Awakening.BaseDelay);
            OutBeastAwakeningDPM = result.ToString();
            result = (int)(result 
                * CoefficientOfMoonTouchForLuna() 
                * FormulaCoefficientOfCriticalHitLuna() 
                * FormulaCoefficientOfAccuracyLuna());
            return result;
        }
        public double BestialRampageCooldown()
        {
            double result = (Bestial_Rampage.BaseTimeCooldown / SkillCooldownFinal.ConvertToCoefficient()) + TIME_CAST;

            return result;
        }
        public double TimeBestialRampage()
        {
            double result = (Bestial_Rampage.TimeActive * (FacilitationFinal.ConvertToCoefficient()) / BestialRampageCooldown());
            if (result < 0)
            {
                return 0;
            }
            if (result > 1) return 1;

            return result;
        }
        public double TimeWithoutBestialRampage()
        {
            double result = (BestialRampageCooldown() - Bestial_Rampage.TimeActive) * FacilitationFinal.ConvertToCoefficient() / BestialRampageCooldown();
            if (result < 0)
            {
                return 0;
            }
            if (result > 1) return 1;
            return result;
        }
        public double AttackDelayLunaWithBestialRampage()
        {
            double result = (Bestial_Rampage.Luna.BaseDelay * ((100 - Bestial_Rampage.IncreaseAttackSpeed) / 100));
            return result;
        }
        public int CalcBestialRampage(int magedd, int physdd)
        {
            int result = (int)(Bestial_Rampage.Formula(magedd, physdd) 
                * FormulaCoefficientOfAttackStrength() 
                * FormulaCoefficientOfPiercingAttackLuna());

            OutBestialRampageDD = result.ToString();
            result = (int)(result * 60 / (Bestial_Rampage.Luna.BaseDelay * ((100 - Bestial_Rampage.IncreaseAttackSpeed) / 100)));
            OutBestialRampageDPM = result.ToString();
            result = (int)(result 
                * CoefficientOfMoonTouchForLuna() 
                * FormulaCoefficientOfCriticalHitLuna() 
                * FormulaCoefficientOfAccuracyLuna());
            return result;
        }
        public double AuraOfTheForestCooldown()
        {
            double result = AuraOfTheForest.BaseTimeCooldown / SkillCooldownFinal.ConvertToCoefficient() + TIME_CAST;

            return result;
        }
        public Dictionary<SourcesDamage, int> CalcAuraOfTheForest(int magedd)
        { 
            // Коэффициенты разделены для вывода в дебаг вкладку. В резалте происходит умножение на кэфы, которые влияют исключительно на дпм скилла.
            var result = new Dictionary<SourcesDamage, int>();
            result.Add(SourcesDamage.Hero, 0);
            result.Add(SourcesDamage.Luna, 0);  
            int countHit = (int)(AuraOfTheForest.TimeActive * FacilitationFinal.ConvertToCoefficient() / AuraOfTheForest.Delay);
            int LunaAura = (int)(AuraOfTheForest.Formula(magedd) 
                * FormulaCoefficientOfPenetrationLuna()); 
            int HeroesAura = (int)(AuraOfTheForest.Formula(magedd) 
                * coefficientCastle
                * coefficientBestialRageTalant 
                * coefficientPredatoryDeliriumTalant 
                * coefficientMomentOfPowerTalant 
                * FormulaCoefficientOfPenetration());
            double realCooldown = AuraOfTheForest.BaseTimeCooldown / SkillCooldownFinal.ConvertToCoefficient() + TIME_CAST;
            if (HasTalantGrandeurOfTheLotus)
            {
                if (BeastAwakeningActive)
                {
                    LunaAura = (int)(LunaAura * 0.8);
                    OutAuraOfTheForestLunaDD = LunaAura.ToString();
                    LunaAura = (int)(LunaAura * 60 / AuraOfTheForestCooldown() * countHit);
                    OutAuraOfTheForestLunaDPM = LunaAura.ToString();
                    // ИТОГОВЫЙ ДД АУРЫ ЛЕСА ЛУНЫ НА ВСЕ КЭФЫ
                    result[SourcesDamage.Luna] += (int)(LunaAura 
                        * CoefficientOfMoonTouchForLuna() 
                        * FormulaCoefficientOfCriticalHitLuna() 
                        * FormulaCoefficientOfAccuracyLuna());
                }
                else
                {
                    OutAuraOfTheForestLunaDD = "0";
                    OutAuraOfTheForestLunaDPM = "0";
                }
                HeroesAura = (int)(HeroesAura * 0.8);
                OutAuraOfTheForestHeroDD = HeroesAura.ToString();
                HeroesAura = (int)(HeroesAura * 60 / AuraOfTheForestCooldown() * countHit);
                OutAuraOfTheForestHeroDPM = HeroesAura.ToString();
                result[SourcesDamage.Hero] += (int)(HeroesAura 
                    * FormulaCoefficientOfCriticalHitForSkill() 
                    * FormulaCoefficientOfAccuracy() 
                    * coefficientBPDungeon());
                return result;
            }
            if (BeastAwakeningActive)
            {
                OutAuraOfTheForestLunaDD = LunaAura.ToString();
                LunaAura = (int)(LunaAura * 60 / AuraOfTheForestCooldown() * countHit);
                OutAuraOfTheForestLunaDPM = LunaAura.ToString();
                OutAuraOfTheForestHeroDPM = "0";
                OutAuraOfTheForestHeroDD = "0";
                // ИТОГОВЫЙ ДД АУРЫ ЛЕСА ЛУНЫ НА ВСЕ КЭФЫ
                result[SourcesDamage.Luna] += (int)(LunaAura 
                    * CoefficientOfMoonTouchForLuna() 
                    * FormulaCoefficientOfCriticalHitLuna() 
                    * FormulaCoefficientOfAccuracyLuna());
                return result;
            }
            OutAuraOfTheForestHeroDD = HeroesAura.ToString();
            HeroesAura = (int)(HeroesAura * 60 / AuraOfTheForestCooldown() * countHit);
            OutAuraOfTheForestHeroDPM = HeroesAura.ToString();
            OutAuraOfTheForestLunaDD = "0";
            OutAuraOfTheForestLunaDPM = "0";
            result[SourcesDamage.Hero] += (int)(HeroesAura 
                * FormulaCoefficientOfCriticalHitForSkill() 
                * FormulaCoefficientOfAccuracy() 
                * coefficientBPDungeon());
            return result;
        }

        public double MoonLightCooldown()
        {
            double result = Moonlight.BaseTimeCooldown / SkillCooldownFinal.ConvertToCoefficient() + TIME_CAST;

            return result;
        }
        public int CalcMoonlight(int magicaldd, int pureMagicalDD)
        {
            int result = 0;
            if (MoonlightPermanentActive)
            {
                int permanentDD = (int)(Moonlight.Formula((int)(pureMagicalDD * coefficientTriton + magicaldd))
                    * coefficientCastle
                    * coefficientBestialRageTalant
                    * coefficientPredatoryDeliriumTalant
                    * coefficientLongDeathTalant 
                    * FormulaCoefficientOfPenetration());

                OutMoonlightPermanentDD = permanentDD.ToString();
                int permanentDPM = permanentDD * 30;
                OutMoonlightPermanentDPM = permanentDPM.ToString();
                result += permanentDPM;
            }
            if (MoonlightNonPermanentActive)
            {
                double realCooldown = Moonlight.BaseTimeCooldown / SkillCooldownFinal.ConvertToCoefficient() + TIME_CAST;

                int nonPermanentDD = (int)(Moonlight.Formula(magicaldd) 
                    * coefficientCastle
                    * coefficientBestialRageTalant 
                    * coefficientPredatoryDeliriumTalant 
                    * coefficientLongDeathTalant 
                    * FormulaCoefficientOfPenetration());

                OutMoonlightNonPermanentDD = nonPermanentDD.ToString();
                int nonPermanentDPM = (int)((nonPermanentDD * 4) / MoonLightCooldown() * 60 * LegendaryCoefficientMoonLight());
                OutMoonlightNonPermanentDPM = nonPermanentDPM.ToString();
                result += (int)(nonPermanentDPM * FormulaCoefficientOfAccuracy());
            }

            result = (int)(result * FormulaCoefficientOfCriticalHitForSkill() * coefficientBPDungeon());

            return result;
        }
        private double OrderToAttackCooldown()
        {
            double result = ((OrderToAttack.BaseTimeCooldown / SkillCooldownFinal.ConvertToCoefficient()) + TIME_CAST);
            return result;
        }
        public int CalcOrderToAttack(int magedd, int physdd)
        {
            int result = 0;

            result = (int)(OrderToAttack.Formula(magedd, physdd)
                * FormulaCoefficientOfAttackStrength() 
                * FormulaCoefficientOfPiercingAttackLuna());

            OutOrderToAttackDD = result.ToString();

            result = (int)(result * 60 / OrderToAttackCooldown());
            if (BestialRampageActive)
                result = (int)(result * (1 + (Bestial_Rampage.IncreaseDD - 1) * TimeBestialRampage()));
            OutOrderToAttackDPM = result.ToString();

            result = (int)(result 
                * CoefficientOfMoonTouchForLuna() 
                * FormulaCoefficientOfCriticalHitLuna() 
                * FormulaCoefficientOfAccuracyLuna());

            return result;
        }

        /// <summary>
        /// Метод для расчета урона в минуту таланта симбиоз из 2 ветки
        /// </summary>
        /// <param name="magedd"></param>
        /// <param name="physdd"></param>
        /// <returns>Hero - урон симбиоза от персонажа, Luna - урон симбиозиса от луны</returns>
        public Dictionary<SourcesDamage, int> CalcSymbiosis(int magedd, int physdd)
        {
            var result = new Dictionary<SourcesDamage, int>();
            result.Add(SourcesDamage.Hero, 0);
            result.Add(SourcesDamage.Luna, 0);

            double Tp = AttackDelay();
            double Tl = Beast_Awakening.BaseDelay;
            double T = Math.Max(Tp, Tl);
            double DpmHero = 0.1 * 60 / T * (
                    Beast_Awakening.Formula(magedd, physdd)
                    * FormulaCoefficientOfCriticalHitLuna()
                    * FormulaCoefficientOfPiercingAttackLuna()
                    * FormulaCoefficientOfAccuracyLuna()
                    );
            double DpmLuna = 0.1 * 60 / T * (
                    Attack.Formula(magedd, physdd)
                    * FormulaCoefficientOfCriticalHitHeroForAutoattack()
                    * FormulaCoefficientOfPiercingAttack()
                    * FormulaCoefficientOfAccuracy()
                    );

            if (BestialRampageActive)
            {
                double Tbr = AttackDelayLunaWithBestialRampage();
                T = Math.Max(Tp, Tbr);
                double DpmBestialRampageHero = 0.1 * 60 / T * (
                    Bestial_Rampage.Formula(magedd, physdd)
                    * FormulaCoefficientOfCriticalHitLuna()
                    * FormulaCoefficientOfPiercingAttackLuna()
                    * FormulaCoefficientOfAccuracyLuna()
                    );
                double DpmBestialRampageLuna = 0.1 * 60 / T * (
                    Attack.Formula(magedd, physdd)
                    * FormulaCoefficientOfCriticalHitHeroForAutoattack()
                    * FormulaCoefficientOfPiercingAttack()
                    * FormulaCoefficientOfAccuracy()
                    );

                result[SourcesDamage.Hero] = (int)(
                    (DpmHero * TimeWithoutBestialRampage() + DpmBestialRampageHero * TimeBestialRampage())
                    * coefficientPredatoryDeliriumTalant 
                    * CoefficientOfMoonTouchForLuna() 
                    * FormulaCoefficientOfAttackStrength()
                    );
                result[SourcesDamage.Luna] = (int)(
                    (DpmLuna * TimeWithoutBestialRampage() + DpmBestialRampageLuna * TimeBestialRampage())
                    * coefficientPredatoryDeliriumTalant
                    * CoefficientOfMoonTouchForLuna()
                    * FormulaCoefficientOfAttackStrength()
                    );

                
                OutSymbiosisDPM = (result[SourcesDamage.Hero] + result[SourcesDamage.Luna]).ToString();

                return result;
            }
            result[SourcesDamage.Hero] = (int)(
                DpmHero 
                * coefficientPredatoryDeliriumTalant 
                * CoefficientOfMoonTouchForLuna()
                * FormulaCoefficientOfAttackStrength() 
                * coefficientBPDungeon()
                );

            result[SourcesDamage.Luna] = (int)(
                DpmHero
                * coefficientPredatoryDeliriumTalant
                * CoefficientOfMoonTouchForLuna()
                * FormulaCoefficientOfAttackStrength()
                * coefficientBPDungeon()
                );

            OutSymbiosisDPM = (result[SourcesDamage.Hero] + result[SourcesDamage.Luna]).ToString();

            return result;
        }

        #endregion

        #region Характеристики персонажа

        #region КД
        private double skillCooldown = 0;
        /// <summary>
        /// Итоговое значение характеристики "Перезарядка навыков" персонажа с учетом всех скиллов и бафов
        /// </summary>
        public double SkillCooldownFinal
        {
            get => skillCooldown;
            set
            {
                skillCooldown = value;
                NotifyPropertyChanged(nameof(SkillCooldownFinal));
            }
        }
        /// <summary>
        /// Свойство связанное с полем на вьюхе "Перезарядка навыков"
        /// </summary>
        public double SkillCooldown
        {
            get => DataSet.SkillCooldown;
            set {
                DataSet.SkillCooldown = StatsLimit.CheckLimit(value, StatsLimit.MAX_SKILL_COOLDOWN);
                Calculate();
                NotifyPropertyChanged(nameof(SkillCooldown)); }
        }
        /// <summary>
        /// Метод для пересчета характеристики персонажа "Перезарядка навыков"
        /// </summary>
        private void CalcSkillCooldown()
        {
            SkillCooldownFinal = 0;
            SkillCooldownFinal += SkillCooldown;
            if (CastleSwordActive) SkillCooldownFinal += 5;
            if (DoubleConcentrationActive)
                SkillCooldownFinal += DoubleConcentration.AddSkillCooldown();
            SkillCooldownFinal = StatsLimit.CheckLimit(SkillCooldownFinal, StatsLimit.MAX_SKILL_COOLDOWN);
        }
        #endregion
        #region Скорость атаки
        private double attackSpeed = 0;
        /// <summary>
        /// Итоговое значение характеристики "Скорость атаки" персонажа с учетом всех скиллов и бафов
        /// </summary>
        public double AttackSpeedFinal
        {
            get => attackSpeed;
            set
            {
                attackSpeed = value;
                NotifyPropertyChanged(nameof(AttackSpeedFinal));
            }
        }
        /// <summary>
        /// Свойство связанное с полем на вьюхе "Скорость атаки"
        /// </summary>
        public double AttackSpeed
        {
            get => DataSet.AttackSpeed;
            set {
                DataSet.AttackSpeed = StatsLimit.CheckLimit(value, StatsLimit.MAX_ATTACK_SPEED); ;
                Calculate(); NotifyPropertyChanged(nameof(AttackSpeed));
            }
        }
        /// <summary>
        /// Метод для пересчета характеристики персонажа "Скорость атаки"
        /// </summary>
        private void CalcAttackSpeed()
        {
            AttackSpeedFinal = 0;
            AttackSpeedFinal += AttackSpeed;
            if (CastleSwordActive) AttackSpeedFinal += 5;
            if (DoubleConcentrationActive)
                AttackSpeedFinal += DoubleConcentration.AddAttackSpeed();
            if (GodsAid) AttackSpeedFinal += 12;
            AttackSpeedFinal = StatsLimit.CheckLimit(AttackSpeedFinal, StatsLimit.MAX_ATTACK_SPEED);
            
        }
        #endregion
        #region Крит
        private double maxCriticalHitHero = 53;
        private double criticalHitHero = 0;
        /// <summary>
        /// Итоговое значение характеристики "Критический удар" персонажа с учетом всех скиллов и бафов
        /// </summary>
        public double CriticalHitHeroFinal
        {
            get => criticalHitHero;
            set
            {
                criticalHitHero = value;
                NotifyPropertyChanged(nameof(CriticalHitHeroFinal));
            }
        }
        private double additionCriticalHitHeroAttack = 0;
        private double criticalHit = 0;
        private double criticalHitLuna = 0;
        /// <summary>
        /// Итоговое значение характеристики "Критический удар" Луны с учетом всех скиллов и бафов
        /// </summary>
        public double CriticalHitLuna
        {
            get => criticalHitLuna;
            set
            {
                criticalHitLuna = value;
                NotifyPropertyChanged(nameof(CriticalHitLuna));
            }
        }
        /// <summary>
        /// Свойство связанное с полем на вьюхе "Критический удар"
        /// </summary>
        public double CriticalHit
        {
            get => DataSet.CriticalHit;
            set
            {
                DataSet.CriticalHit = StatsLimit.CheckLimit(value, StatsLimit.MAX_CRITICAL_HIT);
                Calculate(); NotifyPropertyChanged(nameof(CriticalHit));
            }
        }
        /// <summary>
        /// Метод для пересчета характеристики персонажа "Критический урон"
        /// </summary>
        private void CalcCriticalHit()
        {
            CriticalHitHeroFinal = 0;
            CriticalHitHeroFinal += CriticalHit;
            if (CastleSwordActive) CriticalHitHeroFinal += 5;
            if (BlessingOfTheMoonActive) CriticalHitHeroFinal += BlessingOfTheMoon.AdditionCriticalHit;
            if (CrushingWillActive) CriticalHitHeroFinal += MermanModifiers.CRUSHING_WILL_ADDITIONAL_CRITICAL_HIT;
            if (GodsAid) CriticalHitHeroFinal += 10;
            criticalHit = CriticalHitHeroFinal;
            IsUsingBlessingOfTheMoonOnLuna = IsUsingBlessingOfTheMoonOnLuna;
            if (CriticalHitHeroFinal > maxCriticalHitHero) CriticalHitHeroFinal = maxCriticalHitHero;
        }
        #endregion
        #region Крит урон
        private double criticalDamage = 0;
        /// <summary>
        /// Итоговое значение характеристики "Критический урон" персонажа с учетом всех скиллов и бафов
        /// </summary>
        public double CriticalDamageFinal
        {
            get => criticalDamage;
            set
            {
                criticalDamage = value; 
                NotifyPropertyChanged(nameof(CriticalDamageFinal));
            }
        }
        /// <summary>
        /// Свойство связанное с полем на вьюхе "Критический урон"
        /// </summary>
        public double CriticalDamage
        {
            get => DataSet.CriticalDamage;
            set
            {
                DataSet.CriticalDamage = StatsLimit.CheckLimit(value, StatsLimit.MAX_CRITICAL_DAMAGE);
                Calculate(); NotifyPropertyChanged(nameof(CriticalDamage));
            }
        }
        /// <summary>
        /// Метод пересчета характеристики персонажа "критический урон"
        /// </summary>
        private void CalcCriticalDamage()
        {
            CriticalDamageFinal = 0;
            CriticalDamageFinal += CriticalDamage;
            if (DoubleConcentrationActive)
                CriticalDamageFinal += DoubleConcentration.AdditionCriticalDamage;
            if (GodsAid) CriticalDamageFinal += 30;
            CriticalDamageFinal = StatsLimit.CheckLimit(CriticalDamageFinal, StatsLimit.MAX_CRITICAL_DAMAGE);
        }
        #endregion
        #region Пробив
        private double penetration = 0;
        private double maxPenetrationHero = 50;
        private double penetrationHero = 0;
        /// <summary>
        /// Итоговое значение характеристики "Пробивная способность" персонажа с учетом всех скиллов и бафов
        /// </summary>
        public double PenetrationHeroFinal
        {
            get => penetrationHero;
            set
            {
                penetrationHero = value;
                NotifyPropertyChanged(nameof(PenetrationHeroFinal));
            }
        }
        private double penetrationLuna = 0;
        /// <summary>
        /// Итоговое значение характеристики "Пробивная способность" Луны с учетом всех скиллов и бафов
        /// </summary>
        public double PenetrationLuna
        {
            get => penetrationLuna;
            set
            {
                penetrationLuna = value;
                NotifyPropertyChanged(nameof(PenetrationLuna));
            }
        }
        /// <summary>
        /// Свойство связанное с полем на вьюхе "Пробивная способность"
        /// </summary>
        public double Penetration
        {
            get => DataSet.Penetration;
            set
            {
                DataSet.Penetration = StatsLimit.CheckLimit(value, StatsLimit.MAX_PENETRATION);

                Calculate(); NotifyPropertyChanged(nameof(Penetration));
            }
        }
        /// <summary>
        /// Метод для пересчета характеристики персонажа "Пробивная способность"
        /// </summary>
        private void CalcPenetration()
        {
            PenetrationHeroFinal = 0;
            PenetrationHeroFinal += Penetration;
            if (CastleSwordActive) PenetrationHeroFinal += 5;
            if (BlessingOfTheMoonActive) PenetrationHeroFinal += BlessingOfTheMoon.AdditionPenetration;
            if (IrreversibleAngerActive) PenetrationHeroFinal += MermanModifiers.IRREVERSIBLE_ANGER_ADDITIONAL_PENETRATION;
            penetration = PenetrationHeroFinal;
            IsUsingBlessingOfTheMoonOnLuna = IsUsingBlessingOfTheMoonOnLuna;
            if (PenetrationHeroFinal > maxPenetrationHero) PenetrationHeroFinal = maxPenetrationHero;
        }
        #endregion
        #region Точность
        private double maxAccuracyHero = 50;
        private double accuracy = 0;
        /// <summary>
        /// Итоговое значение характеристики "Точность" Луны с учетом всех скиллов и бафов
        /// </summary>
        public double AccuracyLuna
        {
            get => accuracy;
            set
            {
                accuracy = value; NotifyPropertyChanged(nameof(AccuracyLuna));
            }
        }
        private double accuracyHero = 0;
        /// <summary>
        /// Итоговое значение характеристики "Точность персонажа с учетом всех скиллов и бафов
        /// </summary>
        public double AccuracyHeroFinal
        {
            get => accuracyHero;
            set
            {
                accuracyHero = value;
                NotifyPropertyChanged(nameof(AccuracyHeroFinal));
            }
        }
        /// <summary>
        /// Свойство связанное с полем на вьюхе "Точность"
        /// </summary>
        public double Accuracy
        {
            get => DataSet.Accuracy;
            set
            {
                DataSet.Accuracy = StatsLimit.CheckLimit(value, StatsLimit.MAX_ACCURACY);

                Calculate(); NotifyPropertyChanged(nameof(Accuracy));
            }
        }
        /// <summary>
        /// Метод для пересчета характеристики персонажа "Точность"
        /// </summary>
        private void CalcAccuracy()
        {
            AccuracyHeroFinal = 0;
            AccuracyHeroFinal += Accuracy;
            if (CastleSwordActive) AccuracyHeroFinal += 5;
            if (IrreversibleAngerActive) AccuracyHeroFinal += MermanModifiers.IRREVERSIBLE_ANGER_ADDITIONAL_ACCURACY;
            AccuracyLuna = AccuracyHeroFinal;
            if (AccuracyHeroFinal > maxAccuracyHero) AccuracyHeroFinal = maxAccuracyHero;

        }
        #endregion
        #region Сила атаки
        private double attackStrength = 0;
        /// <summary>
        /// Итоговое значение характеристики "Сила атаки" персонажа с учетом всех скиллов и бафов
        /// </summary>
        public double AttackStrengthFinal
        {
            get => attackStrength;
            set
            {
                attackStrength = value;
                NotifyPropertyChanged(nameof(AttackStrengthFinal));
            }
        }
        /// <summary>
        /// Свойство связанное с полем на вьюхе "Сила атаки"
        /// </summary>
        public double AttackStrength
        {
            get => DataSet.AttackStrength;
            set
            {
                DataSet.AttackStrength = StatsLimit.CheckLimit(value, StatsLimit.MAX_ATTACK_STRENGTH);
                Calculate(); NotifyPropertyChanged(nameof(AttackStrength));
            }
        }
        /// <summary>
        /// Метод для пересчета характеристики персонажа "Сила атаки"
        /// </summary>
        private void CalcAttackStrength()
        {
            AttackStrengthFinal = 0;
            AttackStrengthFinal += AttackStrength;
            AttackStrengthFinal = StatsLimit.CheckLimit(AttackStrengthFinal, StatsLimit.MAX_ATTACK_STRENGTH);
        }
        #endregion
        #region Пронза
        private double piercingAttack = 0;
        /// <summary>
        /// Итоговое значение характеристики "Пронзающая атака" персонажа с учетом всех скиллов и бафов
        /// </summary>
        public double PiercingAttackFinal
        {
            get => piercingAttack;
            set
            {
                piercingAttack = value;
                NotifyPropertyChanged(nameof(PiercingAttackFinal));
            }
        }
        /// <summary>
        /// Свойство связанное с полем на вьюхе "Пронзающая атака"
        /// </summary>
        public double PiercingAttack
        {
            get => DataSet.PiercingAttack;
            set
            {
                DataSet.PiercingAttack = StatsLimit.CheckLimit(value, StatsLimit.MAX_PIERCING_ATTACK);
                Calculate(); NotifyPropertyChanged(nameof(PiercingAttack));
            }
        }
        /// <summary>
        /// Метод для пересчета характеристики персонажа "Пронзающая атака"
        /// </summary>
        private void CalcPiercingAttack()
        {
            PiercingAttackFinal = 0;
            PiercingAttackFinal += PiercingAttack;
            PiercingAttackFinal = StatsLimit.CheckLimit(PiercingAttackFinal, StatsLimit.MAX_PIERCING_ATTACK);
        }
        #endregion
        #region Ярость
        private double rage = 0;
        /// <summary>
        /// Итоговое значение характеристики "Ярость" персонажа с учетом всех скиллов и бафов
        /// </summary>
        public double RageFinal
        {
            get => rage;
            set
            {
                rage = value;
                NotifyPropertyChanged(nameof(RageFinal));
            }
        }
        /// <summary>
        /// Свойство связанное с полем на вьюхе "Ярость"
        /// </summary>
        public double Rage
        {
            get => DataSet.Rage;
            set
            {
                DataSet.Rage = StatsLimit.CheckLimit(value, StatsLimit.MAX_RAGE);
                Calculate(); NotifyPropertyChanged("Rage");
            }
        }
        /// <summary>
        /// Метод для пересчета характеристики персонажа "Ярость"
        /// </summary>
        private void CalcRage()
        {
            RageFinal = 0;
            RageFinal += Rage;
            RageFinal = StatsLimit.CheckLimit(RageFinal, StatsLimit.MAX_RAGE);
        }
        #endregion
        #region Орк
        private double facilitation = 0;
        public double FacilitationFinal
        {
            get => facilitation;
            set
            {
                facilitation = value;
                NotifyPropertyChanged(nameof(FacilitationFinal));
            }
        }
        /// <summary>
        /// Свойство связанное с полем на вьюхе "Содействие"
        /// </summary>
        public double Facilitation
        {
            get => DataSet.Facilitation;
            set
            {
                DataSet.Facilitation = StatsLimit.CheckLimit(value, StatsLimit.MAX_FACILITATION);
                Calculate(); NotifyPropertyChanged(nameof(Facilitation));
            }
        }
        /// <summary>
        /// Метод для пересчета характеристики персонажа "Содействие"
        /// </summary>
        private void CalcFacilitation()
        {
            FacilitationFinal = 0;
            FacilitationFinal += Facilitation;
            FacilitationFinal = StatsLimit.CheckLimit(FacilitationFinal, StatsLimit.MAX_FACILITATION);
        }
        #endregion
        #region Проценты дд

        #region базовые модификаторы дд
        #region гильдия
        private bool guildDamageStartModifierActive = true;
        public bool GuildDamageStartModifierActive
        {
            //get => guildDamageStartModifierActive;
            get => DataSet.GuildDamageStartModifierActive;
            set
            {
                //guildDamageStartModifierActive = value;
                DataSet.GuildDamageStartModifierActive = value;
                Calculate(); NotifyPropertyChanged(nameof(GuildDamageStartModifierActive));
            }
        }
        private bool guildDamageModifierActive = true;
        public bool GuildDamageModifierActive
        {
            //get => guildDamageModifierActive;
            get => DataSet.GuildDamageModifierActive;
            set
            {
                //guildDamageModifierActive = value;
                DataSet.GuildDamageModifierActive = value;
                Calculate(); NotifyPropertyChanged(nameof(GuildDamageModifierActive));
            }
        }
        #endregion
        #region талант на дд
        private bool talentDamageStartModifierActive = true;
        public bool TalentDamageStartModifierActive
        {
            //get => talentDamageStartModifierActive;
            get => DataSet.TalentDamageStartModifierActive;
            set
            {
                //talentDamageStartModifierActive = value;
                DataSet.TalentDamageStartModifierActive = value;
                Calculate(); NotifyPropertyChanged(nameof(TalentDamageStartModifierActive));
            }
        }

        private bool talentDamageModifierActive = true;
        public bool TalentDamageModifierActive
        {
            //get => talentDamageModifierActive;
            get => DataSet.TalentDamageModifierActive;
            set
            {
                //talentDamageModifierActive = value;
                DataSet.TalentDamageModifierActive = value;
                Calculate(); NotifyPropertyChanged(nameof(TalentDamageModifierActive));
            }
        }
        #endregion
        #region дд от замка 
        //private bool castleStartModifierActive = false;
        public bool CastleStartModifierActive
        {
            //get => castleStartModifierActive;
            get => DataSet.CastleStartModifierActive;
            set
            {
                //castleStartModifierActive = value;
                DataSet.CastleStartModifierActive = value;
                Calculate(); NotifyPropertyChanged(nameof(CastleStartModifierActive));
            }
        }

        #endregion
        #region дд с первой ветки
        public bool HarmoniousPowerStartModifierActive
        {
            get => DataSet.HarmoniousPowerStartModifierActive;
            set
            {
                DataSet.HarmoniousPowerStartModifierActive = value;
                Calculate(); NotifyPropertyChanged(nameof(HarmoniousPowerStartModifierActive));
            }
        }
        #endregion
        #region доп %
        public double AdditionalPercentMDDStart
        {
            get => DataSet.AdditionalPercentMDDStart;
            set
            {
                double p = 0;
                if (double.TryParse(value.ToString(), out p))
                    DataSet.AdditionalPercentMDDStart = p;
                Calculate(); NotifyPropertyChanged(nameof(AdditionalPercentMDDStart));
            }
        }
        public double AdditionalPercentPDDStart
        {
            get => DataSet.AdditionalPercentPDDStart;
            set
            {
                double p = 0;
                if (double.TryParse(value.ToString(), out p))
                    DataSet.AdditionalPercentPDDStart = p;
                Calculate(); NotifyPropertyChanged(nameof(AdditionalPercentPDDStart));
            }
        }
        #endregion
        #endregion

        private double percentMagicalDDStart = 0;
        private double percentMagicalDD = 0;
        //private const int DD_CONCENTRATION = 4;
        /*public double PercentMagicalDD
        {
            get => DataSet.PercentMagicalDD;
            set
            {
                if (value >= 0)
                    DataSet.PercentMagicalDD = value;
                legendaryCoefficientMagicalDD = 1 + DataSet.PercentMagicalDD / 100;
                Calculate(); NotifyPropertyChanged("PercentMagicalDD");
            }
        }*/

        private double CalcModifiersDamageJewelrySet(TypesDamage type)
        {
            //double result = ModifiersDamage.ConvertInModifiers(SelectedCloak)[type];
            double result = SelectedCloak.ToPercentInDictionary(type);
            //result += ModifiersDamage.ConvertInModifiers(SelectedAmulet)[type];
            result += SelectedAmulet.ToPercentInDictionary(type);
            //result += ModifiersDamage.ConvertInModifiers(SelectedBraceletL)[type];
            //result += ModifiersDamage.ConvertInModifiers(SelectedBraceletR)[type];
            result += SelectedBraceletL.ToPercentInDictionary(type);
            result += SelectedBraceletR.ToPercentInDictionary(type);
            //result += ModifiersDamage.ConvertInModifiers(SelectedRingL)[type];
            result += SelectedRingL.ToPercentInDictionary(type);
            result += SelectedRingR.ToPercentInDictionary(type);
            //result += ModifiersDamage.ConvertInModifiers(SelectedSet)[type];
            result += SelectedSet.ToPercentInDictionary(type);

            return result;
        }

        /// <summary>
        /// Метод для расчета исходного значения процентов Маг дд
        /// </summary>
        private void CalcPercentMagicalDDStart()
        {
            percentMagicalDDStart = 0;
            percentMagicalDDStart += ModifiersDamage.DD_PROCENT_PASSIVE;

            percentMagicalDDStart += CalcModifiersDamageJewelrySet(TypesDamage.Magical);

            if (GuildDamageStartModifierActive) percentMagicalDDStart += ModifiersDamage.DD_GUILD;
            if (TalentDamageStartModifierActive) percentMagicalDDStart += ModifiersDamage.DD_TALENTS;
            if (CastleStartModifierActive) percentMagicalDDStart += ModifiersDamage.DD_CASTLE;
            //if (HarmoniousPowerStartModifierActive) percentMagicalDDStart += harmoniousPowerMDD;
            percentMagicalDDStart += AdditionalPercentMDDStart;

        }
        /// <summary>
        /// Метод для расчета конечного значения процентов Маг дд
        /// </summary>
        private void CalcPercentMagicalDD()
        {
            percentMagicalDD = 0;
            percentMagicalDD += ModifiersDamage.DD_PROCENT_PASSIVE;

            percentMagicalDD += CalcModifiersDamageJewelrySet(TypesDamage.Magical);

            if (GuildDamageModifierActive) percentMagicalDD += ModifiersDamage.DD_GUILD;
            if (TalentDamageModifierActive) percentMagicalDD += ModifiersDamage.DD_TALENTS;
            if (CastleSwordActive) percentMagicalDD += ModifiersDamage.DD_CASTLE;
            //if (HasTalentHarmoniousPower) percentMagicalDD += harmoniousPowerMDD;
            percentMagicalDD += AdditionalPercentMDDFinal;
        }

        private double percentPhysicalDDStart = 0;
        private double percentPhysicalDD = 0;

        /*public double PercentPhysicalDD
        {
            get => DataSet.PercentPhysicalDD;
            set
            {
                if (value >= 0)
                    DataSet.PercentPhysicalDD = value;
                legendaryCoefficientPhysicalDD = 1 + DataSet.PercentPhysicalDD / 100;
                Calculate(); NotifyPropertyChanged("PercentPhysicalDD");
            }
        }*/
        /// <summary>
        /// Метод для расчета исходного значения процентов Физ дд
        /// </summary>
        private void CalcPercentPhysicalDDStart()
        {
            percentPhysicalDDStart = 0;
            percentPhysicalDDStart += ModifiersDamage.DD_PROCENT_PASSIVE;

            percentPhysicalDDStart += CalcModifiersDamageJewelrySet(TypesDamage.Physical);

            if (GuildDamageStartModifierActive) percentPhysicalDDStart += ModifiersDamage.DD_GUILD;
            if (TalentDamageStartModifierActive) percentPhysicalDDStart += ModifiersDamage.DD_TALENTS;
            if (CastleStartModifierActive) percentPhysicalDDStart += ModifiersDamage.DD_CASTLE;

            percentPhysicalDDStart += AdditionalPercentPDDStart;
        }
        /// <summary>
        /// Метод для расчета конечного значения процентов Физ дд
        /// </summary>
        private void CalcPercentPhysicalDD()
        {
            percentPhysicalDD = 0;
            percentPhysicalDD += ModifiersDamage.DD_PROCENT_PASSIVE;

            percentPhysicalDD += CalcModifiersDamageJewelrySet(TypesDamage.Physical);

            if (GuildDamageModifierActive) percentPhysicalDD += ModifiersDamage.DD_GUILD;
            if (TalentDamageModifierActive) percentPhysicalDD += ModifiersDamage.DD_TALENTS;
            if (CastleSwordActive) percentPhysicalDD += ModifiersDamage.DD_CASTLE;

            percentPhysicalDD += AdditionalPercentPDDFinal;
        }


        private int pureMagicalDD = 0;
        public int PureMagicalDD
        {
            get => pureMagicalDD;
            set
            {
                pureMagicalDD = value;
                Calculate(); NotifyPropertyChanged(nameof(PureMagicalDD));
            }
        }
        //private int MagicalDDFinal = 0;
        /*private void CalcMagicalDD()
        {
            MagicalDDFinal = 0; 
        }*/

        #endregion

        #endregion

        #region Характеристики цели

        //private double maxProtection = 80;
        //private double protection = 80;
        /// <summary>
        /// Свойство связанное с полем на вьюхе "Защита"
        /// </summary>
        public double Protection
        {
            get => DataSet.Protection;
            set
            {
                DataSet.Protection = StatsLimit.CheckLimit(value, StatsLimit.MAX_PROTECTION);
                Calculate(); NotifyPropertyChanged(nameof(Protection));
            }
        }

        //private double maxDodge = 60;
        //private double dodge = 50;
        /// <summary>
        /// Свойство связанное с полем на вьюхе "Уклонение"
        /// </summary>
        public double Dodge
        {
            get => DataSet.Dodge;
            set
            {
                DataSet.Dodge = StatsLimit.CheckLimit(value, StatsLimit.MAX_DODGE);
                Calculate(); NotifyPropertyChanged(nameof(Dodge));
            }
        }

        //private double maxResilience = 60;
        //private double resilience = 0;
        /// <summary>
        /// Свойство связанное с полем на вьюхе "Устойчивость"
        /// </summary>
        public double Resilience
        {
            get => DataSet.Resilience;
            set
            {
                DataSet.Resilience = StatsLimit.CheckLimit(value, StatsLimit.MAX_RESILIENCE);
                Calculate(); NotifyPropertyChanged(nameof(Resilience));
            }
        }

        #endregion

        #region Свойства для вывода на View

        #region показатели урона
        #region дд
        private string outDD;
        public string OutDD
        {
            get => outDD;
            set { outDD = value; NotifyPropertyChanged(nameof(OutDD)); }
        }

        private string outDDHero;
        public string OutDDHero
        {
            get => outDDHero;
            set { outDDHero = value; NotifyPropertyChanged(nameof(OutDDHero)); }
        }
        private string outDDLuna;
        public string OutDDLuna
        {
            get => outDDLuna;
            set { outDDLuna = value; NotifyPropertyChanged(nameof(OutDDLuna)); }
        }
        #endregion
        #region Attack

        private string outAttackDD;
        public string OutAttackDD
        {
            get => outAttackDD;
            set { outAttackDD = value; NotifyPropertyChanged(nameof(OutAttackDD)); }
        }
        private string outAttackDPM;
        public string OutAttackDPM
        {
            get => outAttackDPM;
            set { outAttackDPM = value; NotifyPropertyChanged(nameof(OutAttackDPM)); }
        }

        #endregion
        #region Moon Touch
        private string outMoonTouchDD;
        public string OutMoonTouchDD
        {
            get => outMoonTouchDD;
            set { outMoonTouchDD = value; NotifyPropertyChanged(nameof(OutMoonTouchDD)); }
        }
        private string outMoonTouchDPM;
        public string OutMoonTouchDPM
        {
            get => outMoonTouchDPM;
            set { outMoonTouchDPM = value; NotifyPropertyChanged(nameof(OutMoonTouchDPM)); }
        }
        #endregion
        #region Beast Awakening
        private string outBeastAwakeningDD;
        public string OutBeastAwakeningDD
        {
            get => outBeastAwakeningDD;
            set { outBeastAwakeningDD = value; NotifyPropertyChanged(nameof(OutBeastAwakeningDD)); }
        }
        private string outBeastAwakeningDPM;
        public string OutBeastAwakeningDPM
        {
            get => outBeastAwakeningDPM;
            set { outBeastAwakeningDPM = value; NotifyPropertyChanged(nameof(OutBeastAwakeningDPM)); }
        }
        #endregion
        #region BestialRampage
        private string outBestialRampageDD;
        public string OutBestialRampageDD
        {
            get => outBestialRampageDD;
            set { outBestialRampageDD = value; NotifyPropertyChanged(nameof(OutBestialRampageDD)); }
        }
        private string outBestialRampageDPM;
        public string OutBestialRampageDPM
        {
            get => outBestialRampageDPM;
            set { outBestialRampageDPM = value; NotifyPropertyChanged(nameof(OutBestialRampageDPM)); }
        }
        #endregion
        #region Chain Lightning
        private string outChainLightningDD;
        public string OutChainLightningDD
        {
            get => outChainLightningDD;
            set { outChainLightningDD = value; NotifyPropertyChanged(nameof(OutChainLightningDD)); }
        }
        private string outChainLightningDPM;
        public string OutChainLightningDPM
        {
            get => outChainLightningDPM;
            set { outChainLightningDPM = value; NotifyPropertyChanged(nameof(OutChainLightningDPM)); }
        }
        #endregion
        #region Aura of the Forest

        private string outAuraOfTheForestLunaDD;
        public string OutAuraOfTheForestLunaDD
        {
            get => outAuraOfTheForestLunaDD;
            set { outAuraOfTheForestLunaDD = value; NotifyPropertyChanged(nameof(OutAuraOfTheForestLunaDD)); }
        }
        private string outAuraOfTheForestLunaDPM;
        public string OutAuraOfTheForestLunaDPM
        {
            get => outAuraOfTheForestLunaDPM;
            set { outAuraOfTheForestLunaDPM = value; NotifyPropertyChanged(nameof(OutAuraOfTheForestLunaDPM)); }
        }
        private string outAuraOfTheForestHeroDD;
        public string OutAuraOfTheForestHeroDD
        {
            get => outAuraOfTheForestHeroDD;
            set { outAuraOfTheForestHeroDD = value; NotifyPropertyChanged(nameof(OutAuraOfTheForestHeroDD)); }
        }
        private string outAuraOfTheForestHeroDPM;
        public string OutAuraOfTheForestHeroDPM
        {
            get => outAuraOfTheForestHeroDPM;
            set { outAuraOfTheForestHeroDPM = value; NotifyPropertyChanged(nameof(OutAuraOfTheForestHeroDPM)); }
        }

        #endregion
        #region Moonlight

        private string outMoonlightPermanentDD;
        public string OutMoonlightPermanentDD
        {
            get => outMoonlightPermanentDD;
            set { outMoonlightPermanentDD = value; NotifyPropertyChanged(nameof(OutMoonlightPermanentDD)); }
        }
        private string outMoonlightPermanentDPM;
        public string OutMoonlightPermanentDPM
        {
            get => outMoonlightPermanentDPM;
            set { outMoonlightPermanentDPM = value; NotifyPropertyChanged(nameof(OutMoonlightPermanentDPM)); }
        }
        private string outMoonlightNonPermanentDD;
        public string OutMoonlightNonPermanentDD
        {
            get => outMoonlightNonPermanentDD;
            set { outMoonlightNonPermanentDD = value; NotifyPropertyChanged(nameof(OutMoonlightNonPermanentDD)); }
        }
        private string outMoonlightNonPermanentDPM;
        public string OutMoonlightNonPermanentDPM
        {
            get => outMoonlightNonPermanentDPM;
            set { outMoonlightNonPermanentDPM = value; NotifyPropertyChanged(nameof(OutMoonlightNonPermanentDPM)); }
        }

        #endregion
        #region OrderToAttack
        private string outOrderToAttackDD = "0";
        public string OutOrderToAttackDD
        {
            get => outOrderToAttackDD;
            set { outOrderToAttackDD = value; NotifyPropertyChanged(nameof(OutOrderToAttackDD)); }
        }
        private string outOrderToAttackDPM = "0";
        public string OutOrderToAttackDPM
        {
            get => outOrderToAttackDPM;
            set { outOrderToAttackDPM = value; NotifyPropertyChanged(nameof(OutOrderToAttackDPM)); }
        }
        #endregion

        #region Symbiosis

        private string outSymbiosisDPM = "0";
        public string OutSymbiosisDPM
        {
            get => outSymbiosisDPM;
            set { outSymbiosisDPM = value; NotifyPropertyChanged(nameof(OutSymbiosisDPM)); }
        }

        #endregion
        #endregion

        #region Вкладка с билдами

        public string Name
        {
            get => DataSet.Name;
            set
            {
                DataSet.Name = value;
                NotifyPropertyChanged(nameof(Name));
            }
        }

        public string Description
        {
            get => DataSet.Description;
            set
            {
                DataSet.Description = value;
                NotifyPropertyChanged(nameof(Description));
            }
        }

        public string ID
        {
            get => DataSet.ID.ToString();
            set
            {
                NotifyPropertyChanged(nameof(ID));
            }
        }



        #endregion

        #region Списки

        /*public List<string> Amulets
        {
            get => ModifiersDamage.Amulets;
        }*/

        /*private string selectedAmulet = "0%";
        public string SelectedAmulet
        {
            get => DataSet.SelectedAmulet;
            set
            {
                DataSet.SelectedAmulet = value;
                Calculate();
                NotifyPropertyChanged(nameof(SelectedAmulet));
            }
        }*/

        private List<PercentsDamage> _amulets = new List<PercentsDamage>()
        {
            PercentsDamage.None,
            PercentsDamage.Magic6Percent,
            PercentsDamage.Magic10Percent,
            PercentsDamage.Magic15Percent,
            PercentsDamage.Physical4Percent,
            PercentsDamage.Physical7Percent,

        };
        public List<PercentsDamage> Amulets
        {
            get => _amulets;
        }

        //private PercentsDamage _selectedAmulet = PercentsDamage.None;
        public PercentsDamage SelectedAmulet
        {
            //get => _selectedAmulet;
            get => DataSet.SelectedAmuletNew;
            set
            {
                //_selectedAmulet = value;
                DataSet.SelectedAmuletNew = value;
                Calculate();
                NotifyPropertyChanged(nameof(SelectedAmulet));
            }
        }


        /*public List<string> Cloaks
        {
            get => ModifiersDamage.Cloaks;

        }*/
        private List<PercentsDamage> _cloaks = new List<PercentsDamage>() 
        {
            PercentsDamage.None,
            PercentsDamage.Magic5Percent,
            PercentsDamage.Magic10Percent,
            PercentsDamage.Magic15Percent,
            PercentsDamage.Physical4Percent,
            PercentsDamage.Physical7Percent,
        };
        public List<PercentsDamage> Cloaks
        {
            get => _cloaks;
        }

        //private string selectedCloak = "0%";
        //private PercentsDamage selectedCloak = PercentsDamage.None;
        /*public string SelectedCloak
        {
            get => DataSet.SelectedCloak;
            set
            {
                DataSet.SelectedCloak = value;
                Calculate();
                NotifyPropertyChanged(nameof(SelectedCloak));
            }
        }*/
        public PercentsDamage SelectedCloak
        {
            //get => selectedCloak;
            get => DataSet.SelectedCloakNew;
            set
            {
                //selectedCloak = value;
                DataSet.SelectedCloakNew = value;
                Calculate();
                NotifyPropertyChanged(nameof(SelectedCloak));
            }
        }

        /*public List<string> Rings
        {
            get => ModifiersDamage.Rings;
        }*/
        private List<PercentsDamage> _rings = new List<PercentsDamage>()
        {
            PercentsDamage.None,
            PercentsDamage.Magic5Percent,
            PercentsDamage.Magic9Percent,
            PercentsDamage.Magic10Percent,
            PercentsDamage.Physical3Percent,
            PercentsDamage.Physical6Percent,

        };
        public List<PercentsDamage> Rings
        {
            get => _rings;
        }

        //private PercentsDamage selectedRingL = PercentsDamage.None;
        /*public string SelectedRingL
        {
            get => DataSet.SelectedRingL;
            set
            {
                DataSet.SelectedRingL = value;
                Calculate();
                NotifyPropertyChanged(nameof(SelectedRingL));
            }
        }*/
        public PercentsDamage SelectedRingL
        {
            //get => selectedRingL;
            get => DataSet.SelectedRingLNew;
            set
            {
                DataSet.SelectedRingLNew = value;
                //selectedRingL = value;
                Calculate();
                NotifyPropertyChanged(nameof(SelectedRingL));
            }
        }

        //private PercentsDamage selectedRingR = PercentsDamage.None;
        /*public string SelectedRingR
        {
            get => DataSet.SelectedRingR;
            set
            {
                DataSet.SelectedRingR = value;
                Calculate();
                NotifyPropertyChanged(nameof(SelectedRingR));
            }
        }*/
        public PercentsDamage SelectedRingR
        {
            get => DataSet.SelectedRingRNew;
            //get => selectedRingR;
            set
            {
                //selectedRingR = value;
                DataSet.SelectedRingRNew = value;
                Calculate();
                NotifyPropertyChanged(nameof(SelectedRingR));
            }
        }
        private List<PercentsDamage> _bracelets = new List<PercentsDamage>() 
        {
            PercentsDamage.None,
            PercentsDamage.Magic6Percent,
            PercentsDamage.Magic7_5Percent,
            PercentsDamage.Physical4Percent,
            PercentsDamage.Physical5Percent,
        };
        /*public List<string> Bracelets
        {
            get => ModifiersDamage.Bracelets;
        }*/
        public List<PercentsDamage> Bracelets
        {
            get => _bracelets;
        }

        //private PercentsDamage selectedBraceletL = PercentsDamage.None;
        /*public string SelectedBraceletL
        {
            get => DataSet.SelectedBraceletL;
            set
            {
                DataSet.SelectedBraceletL = value;
                Calculate();
                NotifyPropertyChanged(nameof(SelectedBraceletL));
            }
        }*/
        public PercentsDamage SelectedBraceletL
        {
            get => DataSet.SelectedBraceletLNew;
            //get => selectedBraceletL;
            set
            {
                DataSet.SelectedBraceletLNew = value;
                //selectedBraceletL = value;
                Calculate();
                NotifyPropertyChanged(nameof(SelectedBraceletL));
            }
        }

        //private PercentsDamage selectedBraceletR = PercentsDamage.None;
        /*public string SelectedBraceletR
        {
            get => DataSet.SelectedBraceletR;
            set
            {
                DataSet.SelectedBraceletR = value;
                Calculate();
                NotifyPropertyChanged(nameof(SelectedBraceletR));
            }
        }*/
        public PercentsDamage SelectedBraceletR
        {
            get => DataSet.SelectedBraceletRNew;
            //get => selectedBraceletR;
            set
            {
                DataSet.SelectedBraceletRNew = value;
                //selectedBraceletR = value;
                Calculate();
                NotifyPropertyChanged(nameof(SelectedBraceletR));
            }
        }


        private List<TypesEquipment> _equipments = new List<TypesEquipment>() 
        { 
            TypesEquipment.None,
            TypesEquipment.Cloth,
            TypesEquipment.Leather,
        };
        public List<TypesEquipment> Equipments
        {
            get => _equipments;
        }
        /*public List<string> Equipments
        {
            get => ModifiersDamage.Equipments;
        }*/
        //private TypesEquipment selectedHelmet = TypesEquipment.None;
        public TypesEquipment SelectedHelmet
        {
            get => DataSet.SelectedHelmetNew;
            //get => selectedHelmet;
            set
            {
                DataSet.SelectedHelmetNew = value;
                //selectedHelmet = value;
                Calculate();
                NotifyPropertyChanged(nameof(SelectedHelmet));
            }
        }

        //private TypesEquipment selectedBody = TypesEquipment.None;
        public TypesEquipment SelectedBody
        {
            get => DataSet.SelectedBodyNew;
            //get => selectedBody;
            set
            {
                DataSet.SelectedBodyNew = value;
                //selectedBody = value;
                Calculate();
                NotifyPropertyChanged(nameof(SelectedBody));
            }
        }

        //private TypesEquipment selectedHands = TypesEquipment.None;
        public TypesEquipment SelectedHands
        {
            get => DataSet.SelectedHandsNew;
            //get => selectedHands;
            set
            {
                DataSet.SelectedHandsNew = value;
                //selectedHands = value;
                Calculate();
                NotifyPropertyChanged(nameof(SelectedHands));
            }
        }

        //private TypesEquipment selectedBelt = TypesEquipment.None;
        public TypesEquipment SelectedBelt
        {
            get => DataSet.SelectedBeltNew;
            //get => selectedBelt;
            set
            {
                DataSet.SelectedBeltNew = value;
                //selectedBelt = value;
                Calculate();
                NotifyPropertyChanged(nameof(SelectedBelt));
            }
        }

        //private TypesEquipment selectedFoots = TypesEquipment.None;
        public TypesEquipment SelectedFoots
        {
            get => DataSet.SelectedFootsNew;
            //get => selectedFoots;
            set
            {
                DataSet.SelectedFootsNew = value;
                //selectedFoots = value;
                Calculate();
                NotifyPropertyChanged(nameof(SelectedFoots));
            }
        }

        /*public List<string> Sets
        {
            get => ModifiersDamage.Sets;
        }*/
        private List<PercentsDamage> _sets = new List<PercentsDamage>()
        {
            PercentsDamage.None,
            PercentsDamage.Magic12Percent,
            PercentsDamage.Physical8Percent,
        };
        public List<PercentsDamage> Sets
        {
            get => _sets;
        }
        //private PercentsDamage selectedSet = PercentsDamage.None;
        public PercentsDamage SelectedSet
        {
            //get => selectedSet;
            get => DataSet.SelectedSetNew;
            set
            {
                //selectedSet = value;
                DataSet.SelectedSetNew = value;
                Calculate();
                NotifyPropertyChanged(nameof(SelectedSet));
            }
        }


        #endregion

        #endregion

        #region Источники урона

        //private Attack attack;
        public Attack Attack
        {
            //get => attack;
            get => DataSet.Attack;
            set { //attack = value; 
                DataSet.Attack = value;
                NotifyPropertyChanged(nameof(Attack)); }
        }
        public MoonTouch Moon_Touch
        { //get => moonTouch;
            get => DataSet.MoonTouch;
            set { //moonTouch = value;
                DataSet.MoonTouch = value;
                NotifyPropertyChanged(nameof(Moon_Touch)); }
        }
        //private MoonTouch moonTouch;

        //private OrderToAttack orderToAttack;
        public OrderToAttack OrderToAttack
        {
            //get => orderToAttack;
            get => DataSet.OrderToAttack;
            set
            {
                //orderToAttack = value;
                DataSet.OrderToAttack = value;
                NotifyPropertyChanged(nameof(OrderToAttack));
            }
        }

        //private ChainLightning chainLightning;
        public ChainLightning Chain_Lightning
        {
            //get => chainLightning;
            get => DataSet.ChainLightning;
            set { //chainLightning = value;
                DataSet.ChainLightning = value;
                NotifyPropertyChanged(nameof(Chain_Lightning)); }
        }
        //private BeastAwakening beastAwakening;
        public BeastAwakening Beast_Awakening
        {
            //get => beastAwakening;
            get => DataSet.BeastAwakening;
            set { //beastAwakening = value; 
                DataSet.BeastAwakening = value;
                NotifyPropertyChanged(nameof(Beast_Awakening)); }
        }
        //private BestialRampage bestialRampage;
        public BestialRampage Bestial_Rampage
        {
            //get => bestialRampage;
            get => DataSet.BestialRampage;
            set { //bestialRampage = value; 
                DataSet.BestialRampage = value;
                NotifyPropertyChanged(nameof(Bestial_Rampage)); }
        }
        //private AuraOfTheForest auraOfTheForest;
        public AuraOfTheForest AuraOfTheForest
        {
            //get => auraOfTheForest;
            get => DataSet.AuraOfTheForest;
            set { //auraOfTheForest = value; 
                DataSet.AuraOfTheForest = value;
                NotifyPropertyChanged(nameof(AuraOfTheForest)); }
        }
        //private Moonlight moonlight;
        public Moonlight Moonlight
        {
            //get => moonlight;
            get => DataSet.Moonlight;
            set { //moonlight = value;
                DataSet.Moonlight = value;
                NotifyPropertyChanged(nameof(Moonlight)); }
        }

        public BlessingOfTheMoon BlessingOfTheMoon
        {
            get => DataSet.BlessingOfTheMoon;
            set
            {
                DataSet.BlessingOfTheMoon = value;
                NotifyPropertyChanged(nameof(BlessingOfTheMoon));
            }
        }
        public DoubleConcentration DoubleConcentration
        {
            get => DataSet.DoubleConcentration;
            set
            {
                DataSet.DoubleConcentration = value;
                NotifyPropertyChanged(nameof(DoubleConcentration));
            }
        }
        #endregion

        #region неотобранные элементы 
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Метод для вызова события PropertyChanged
        /// </summary>
        /// <param name="prop">Имя свойства, которое изменилось</param>
        public void NotifyPropertyChanged([CallerMemberName] string prop = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public string MagicalDD
        {
            //get => magicalDD;
            get => DataSet.MagicalDamage;
            set { //magicalDD = value;
                DataSet.MagicalDamage = value;
                Calculate(); NotifyPropertyChanged(nameof(MagicalDD)); }
        }
        //private string magicalDD;
        //private string physicalDD;
        public string PhysicalDD
        {
            //get => physicalDD;
            get => DataSet.PhysicalDamage;
            set { //physicalDD = value;
                DataSet.PhysicalDamage = value;
                Calculate(); NotifyPropertyChanged(nameof(PhysicalDD)); }
        }


        private const double TIME_CAST = 0.65; // задержка нажатия скилла
        /*public double TimeCast 
        {
            get => TIME_CAST;
            set
            {
                TIME_CAST = value;
                Calculate(); NotifyPropertyChanged("TimeCast");
            }
        }*/ // задержка нажатия скилла

        //private double legendaryCoefficientBestialRampage = 0.5; // Будет меняться в зависимости от скорости атаки и сколько у тебя кд
        
        //private double legendaryCoefficientChainLightning = 1;
        public double LegendaryCoefficientChainLightning()
        {
            double result = (1 - SkillCooldownFinal / 250);
            return result;
        }

        public double LegendaryCoefficientMoonLight()
        {
            double result = 0.65 * (1 - SkillCooldownFinal / 130);
            return result;
        }

        #region 

        private double blessingOfTheMoonCooldown()
        {
            double cooldown = BlessingOfTheMoon.BaseTimeCooldown;
            double result = ((cooldown / SkillCooldownFinal.ConvertToCoefficient()) + TIME_CAST);

            return result;
        }
        private double doubleConcentrationCooldown()
        {
            double cooldown = DoubleConcentration.BaseTimeCooldown;
            double result = ((cooldown / SkillCooldownFinal.ConvertToCoefficient()) + TIME_CAST);

            return result;
        }
        private double healingCooldown()
        {
            double cooldown = 14;
            double result = ((cooldown / SkillCooldownFinal.ConvertToCoefficient()) + TIME_CAST);

            return result;
        }

        #endregion
        public double LegendaryCoefficientAttackSpeed()
        {
            double s = 0;

            if (MoonTouchActive) s += 1 / MoonTouchCooldown();
            if (OrderToAttackActive) s += 1 / OrderToAttackCooldown();
            if (HealingActive) s += 1 / healingCooldown();
            if (ChainLightningActive) s += 1 / ChainLightningCooldown();
            if (BestialRampageActive) s += 1 / BestialRampageCooldown();
            if (AuraOfTheForestActive) s += 1 / AuraOfTheForestCooldown();
            if (MoonlightNonPermanentActive) s += 1 / MoonLightCooldown();
            if (BlessingOfTheMoonActive) s += 1 / blessingOfTheMoonCooldown();
            if (DoubleConcentrationActive) s += 1 / doubleConcentrationCooldown();
            s *= 1.55;
            if (s < 0) s = 0;
            if (s > 1) s = 1;

            double result = - 0.3 * s + 1;

            return result;
        }


        //private double legendaryCoefficientAttackSpeed = 1.276; // Будет менять в зависимости от скорости атаки, кд, включенных скиллов.
        //private double legendaryCoefficientMagicalDD = 1; // Тут в него входит ги, плащ, рассовая, ну и рандомные кольца +-
        //private double legendaryCoefficientPhysicalDD = 1; // тут ги и талики на урон вне ветки
        #endregion

        #region Дополнительные надбавки


        #region Triton
        //private bool crushingWillActive = false;
        //private const int CRUSHING_WILL_ADDITIONAL_CRITICAL_HIT = 20;

        public bool CrushingWillActive
        {
            //get => crushingWillActive;
            get => DataSet.CrushingWill;
            set { /*crushingWillActive = value;
                if (crushingWillActive)
                {
                    coefficientTriton = 0.3;
                }
                else coefficientTriton = 0;*/

                DataSet.CrushingWill = value;
                if (DataSet.CrushingWill)
                {
                    coefficientTriton = 0.3;
                }
                else coefficientTriton = 0;
                Calculate(); NotifyPropertyChanged(nameof(CrushingWillActive)); 
            }
        }

        //private const double MERMAN_CD = 15;
        //private const double SINGLE_MERMAN_DURATION = 10;

        private double MermanDuration() 
        {
            double result = MermanModifiers.SINGLE_MERMAN_DURATION 
                * FacilitationFinal.ConvertToCoefficient() 
                / MermanModifiers.CLOTH_COOLDOWN 
                * 0.9;

            return result;
        }
        private double coefficientTriton = 0;

        //private bool irreversibleAngerActive = false;
        public bool IrreversibleAngerActive
        {
            //get => irreversibleAngerActive;
            get => DataSet.IrreversibleAnger;
            set
            {
                /*irreversibleAngerActive = value;
                if (irreversibleAngerActive)*/
                DataSet.IrreversibleAnger = value;
                Calculate(); NotifyPropertyChanged(nameof(IrreversibleAngerActive));
            }
        }
        //private int irreversibleAngerAdditionalPenetration = 15;
        //private int irreversibleAngerAdditionalAccuracy = 15;


        #endregion

        #region Ветки

        #region 3 ветка
        //private bool forestInspirationActive = false;
        public bool ForestInspirationActive
        {
            //get => forestInspirationActive;
            get => DataSet.ForestInspirationActive;
            set {
                /*forestInspirationActive = value;
                if (!forestInspirationActive)*/
                DataSet.ForestInspirationActive = value;
                if (!DataSet.ForestInspirationActive)
                {
                    HasTalantGrandeurOfTheLotus = false;
                    HasTalantBeastAwakeningMage = false;
                    LvlTalantMoonlightPlus = 0;
                }
            }
        }



        #endregion

        #region 2 ветка

        //private bool dualRageActive = false;
        public bool DualRageActive
        {
            get => DataSet.DualRageActive;
            set {
                DataSet.DualRageActive = value;
                if (!DataSet.DualRageActive)
                {
                    HasTalantBestialRampage = false;
                    LvlTalantBeastAwakeningPhysical = 0;
                    LvlTalantOrderToAttackPlusDualRage = 0;
                    HasTalantSymbiosis = false;
                    HasTalantBlessingOfTheMoonPlusCriticalHit = false;
                    HasTalentDeadlyDexterity = false;

                    maxPenetrationHero = 50;
                }
                else maxPenetrationHero = 51.5;
            }
        }

        public bool HasTalentDeadlyDexterity
        {
            //get => hasTalantSymbiosis;
            get => DoubleConcentration.HasTalentDeadlyDexterity;
            set
            {
                //hasTalantSymbiosis = value; 
                DoubleConcentration.HasTalentDeadlyDexterity = value;
                Calculate();
                NotifyPropertyChanged(nameof(HasTalentDeadlyDexterity));
            }
        }

        #endregion

        #region 1 ветка

        //private bool guardianUnityActive = false;
        
        public bool GuardianUnityActive
        {
            //get => guardianUnityActive;
            get => DataSet.GuardianUnityActive;
            set
            {
                DataSet.GuardianUnityActive = value;
                // TODO
                // прописать логику выключения других веток,
                // взаимодействие с сейвом
                if (!value)
                {
                    HasTalantBlessingOfTheMoonPlusPenetration = false;
                    LvlTalantOrderToAttackPlusGuardianUnity = 0;
                    HasTalentHarmoniousPower = false;
                }
                

            }
        }
        //private bool hasTalentHarmoniousPower = false;
        public bool HasTalentHarmoniousPower
        {
            //get => hasTalentHarmoniousPower;
            get => DataSet.HasTalentHarmoniousPower;
            set
            {
                DataSet.HasTalentHarmoniousPower = value;
                calcHarmoniousPowerDD();
                Calculate(); NotifyPropertyChanged(nameof(HasTalentHarmoniousPower));
            }
        }
        private void calcHarmoniousPowerDD()
        {
            harmoniousPowerMDD = 0;
            harmoniousPowerPDD = 0;

            harmoniousPowerMDD += SelectedHelmet.ToPercentInDictionary(TypesDamage.Magical);
            harmoniousPowerMDD += SelectedBody.ToPercentInDictionary(TypesDamage.Magical);
            harmoniousPowerMDD += SelectedHands.ToPercentInDictionary(TypesDamage.Magical);
            harmoniousPowerMDD += SelectedBelt.ToPercentInDictionary(TypesDamage.Magical);
            harmoniousPowerMDD += SelectedFoots.ToPercentInDictionary(TypesDamage.Magical);

            harmoniousPowerPDD += SelectedHelmet.ToPercentInDictionary(TypesDamage.Physical);
            harmoniousPowerPDD += SelectedBody.ToPercentInDictionary(TypesDamage.Physical);
            harmoniousPowerPDD += SelectedHands.ToPercentInDictionary(TypesDamage.Physical);
            harmoniousPowerPDD += SelectedBelt.ToPercentInDictionary(TypesDamage.Physical);
            harmoniousPowerPDD += SelectedFoots.ToPercentInDictionary(TypesDamage.Physical);
        }
        private double harmoniousPowerMDD = 0;
        private double harmoniousPowerPDD = 0;


        #endregion
        #endregion

        #region Общие таланты

        #region Звериная ярость

        private double coefficientBestialRageTalant = 1;

        //private int lvlTalantBestialRage = 0;

        public int LvlTalantBestialRage
        {
            //get => lvlTalantBestialRage;
            get => DataSet.LvlTalantBestialRage;
            set
            {
                //lvlTalantBestialRage = value;
                DataSet.LvlTalantBestialRage = value;
                switch (value)
                {
                    case 0:
                        coefficientBestialRageTalant = 1;
                        break;
                    case 1:
                        coefficientBestialRageTalant = 1.01;
                        break;
                    case 2:
                        coefficientBestialRageTalant = 1.02;
                        break;
                    case 3:
                        coefficientBestialRageTalant = 1.03;
                        break;
                    default:
                        coefficientBestialRageTalant = 1;
                        break;
                }
                Calculate();
                NotifyPropertyChanged(nameof(LvlTalantBestialRage));
            }
        }

        #endregion

        #region Исступление хищника

        private double coefficientPredatoryDeliriumTalant = 1;

        //private int lvlTalantPredatoryDelirium = 0;

        public int LvlTalantPredatoryDelirium
        {
            //get => lvlTalantPredatoryDelirium;
            get => DataSet.LvlTalantPredatoryDelirium;
            set
            {
                //lvlTalantPredatoryDelirium = value;
                DataSet.LvlTalantPredatoryDelirium = value;
                switch (value)
                {
                    case 0:
                        coefficientPredatoryDeliriumTalant = 1;
                        break;
                    case 1:
                        coefficientPredatoryDeliriumTalant = 1.01;
                        break;
                    case 2:
                        coefficientPredatoryDeliriumTalant = 1.015;
                        break;
                    case 3:
                        coefficientPredatoryDeliriumTalant = 1.02;
                        break;
                    default:
                        coefficientPredatoryDeliriumTalant = 1;
                        break;
                }
                Calculate();
                NotifyPropertyChanged(nameof(LvlTalantPredatoryDelirium));
            }
        }

        #endregion

        #region Животный гнев

        private double additionAnimalRageTalant = 0;

        //private int lvlTalantAnimalRage = 0;

        public int LvlTalantAnimalRage
        {
            //get => lvlTalantAnimalRage;
            get => DataSet.LvlTalantAnimalRage;
            set
            {
                //lvlTalantAnimalRage = value;
                DataSet.LvlTalantAnimalRage = value;
                switch (value)
                {
                    case 0:
                        additionAnimalRageTalant = 0;
                        break;
                    case 1:
                        additionAnimalRageTalant = 1;
                        break;
                    case 2:
                        additionAnimalRageTalant = 2;
                        break;
                    case 3:
                        additionAnimalRageTalant = 3;
                        break;
                    default:
                        additionAnimalRageTalant = 0;
                        break;
                }
                Calculate();
                NotifyPropertyChanged(nameof(LvlTalantAnimalRage));
            }
        }

        #endregion

        #region Момент силы

        private double coefficientMomentOfPowerTalant = 1;

        //private int lvlTalantMomentOfPower = 0;
        public int LvlTalantMomentOfPower
        {
            //get => lvlTalantMomentOfPower;
            get => DataSet.LvlTalantMomentOfPower;
            set
            {
                //lvlTalantMomentOfPower = value;
                DataSet.LvlTalantMomentOfPower = value;
                switch (value)
                {
                    case 0:
                        coefficientMomentOfPowerTalant = 1;
                        break;
                    case 1:
                        coefficientMomentOfPowerTalant = 1.005;
                        break;
                    case 2:
                        coefficientMomentOfPowerTalant = 1.01;
                        break;
                    case 3:
                        coefficientMomentOfPowerTalant = 1.015;
                        break;
                    case 4:
                        coefficientMomentOfPowerTalant = 1.02;
                        break;
                    default:
                        coefficientMomentOfPowerTalant = 1;
                        break;
                }
                Calculate();
                NotifyPropertyChanged(nameof(LvlTalantMomentOfPower));
            }
        }

        #endregion

        #region Долгая смерть

        private double coefficientLongDeathTalant = 1;
        //private int lvlTalantLongDeath = 0;
        public int LvlTalantLongDeath
        {
            //get => lvlTalantLongDeath;
            get => DataSet.LvlTalantLongDeath;
            set
            {
                //lvlTalantLongDeath = value;
                DataSet.LvlTalantLongDeath = value;
                switch (value)
                {
                    case 0:
                        coefficientLongDeathTalant = 1;
                        break;
                    case 1:
                        coefficientLongDeathTalant = 1.005;
                        break;
                    case 2:
                        coefficientLongDeathTalant = 1.01;
                        break;
                    case 3:
                        coefficientLongDeathTalant = 1.015;
                        break;
                    case 4:
                        coefficientLongDeathTalant = 1.02;
                        break;
                    default:
                        coefficientLongDeathTalant = 1;
                        break;
                }
                Calculate();
                NotifyPropertyChanged(nameof(LvlTalantLongDeath));
            }
        }

        #endregion

        #endregion

        #region Таланты ивентов

        #region Длительная неистовость
        private double additionalContinuousFuryTalant = 0;
        //private int lvlTalantContinuousFury = 0;
        public int LvlTalantContinuousFury
        {
            //get => lvlTalantContinuousFury;
            get => DataSet.LvlTalantContinuousFury;
            set
            {
                //lvlTalantContinuousFury = value;
                DataSet.LvlTalantContinuousFury = value;
                switch (value)
                {
                    case 0: additionalContinuousFuryTalant = 0; break;
                    case 1: additionalContinuousFuryTalant = 0.5; break;
                    case 2: additionalContinuousFuryTalant = 1; break;
                    case 3: additionalContinuousFuryTalant = 1.5; break;
                }
                Calculate();
                NotifyPropertyChanged(nameof(LvlTalantContinuousFury));
            }
        }
        #endregion

        #endregion

        #region Формулы статов

        private bool isUsingBlessingOfTheMoonOnLuna = false;
        public bool IsUsingBlessingOfTheMoonOnLuna
        {
            //get => isUsingBlessingOfTheMoonOnLuna;
            get => DataSet.IsUsingBlessingOfTheMoonOnLuna;
            set
            {
                //isUsingBlessingOfTheMoonOnLuna = value;

                if (BlessingOfTheMoonActive) 
                {
                    DataSet.IsUsingBlessingOfTheMoonOnLuna = value;
                    if (value)
                    {
                        CriticalHitLuna = criticalHit + BlessingOfTheMoon.AdditionCriticalHit;
                        PenetrationLuna = penetration + BlessingOfTheMoon.AdditionPenetration;
                    }
                    else
                    {
                        CriticalHitLuna = criticalHit;
                        PenetrationLuna = penetration;
                    }

                }
                else
                {
                    CriticalHitLuna = criticalHit;
                    PenetrationLuna = penetration;
                }
                CriticalHitLuna = StatsLimit.CheckLimit(CriticalHitLuna, StatsLimit.MAX_CRITICAL_HIT);
                PenetrationLuna = StatsLimit.CheckLimit(PenetrationLuna, StatsLimit.MAX_PENETRATION);

                NotifyPropertyChanged(nameof(IsUsingBlessingOfTheMoonOnLuna));
            }
        }

        private double FormulaCoefficientOfCriticalHitHero( )
        {
            double criticalHitWithResilience = (CriticalHitHeroFinal - Resilience) / 100;
            if (criticalHitWithResilience < 0) criticalHitWithResilience = 0;
            if (criticalHitWithResilience > 1) criticalHitWithResilience = 1;
            double result = (1 - Resilience / 100) * (1 - criticalHitWithResilience) + Math.Pow((1 - Resilience / 100), 2) * criticalHitWithResilience * (2 + CriticalDamageFinal / 100);

            return result;
        }
        private double FormulaCoefficientOfCriticalHitHeroForAutoattack()
        {
            if (IrreversibleAngerActive)
                additionCriticalHitHeroAttack = 20 * (1 - CriticalHitHeroFinal / 100);
            else additionCriticalHitHeroAttack = 0;


            double criticalHitWithResilience = ((CriticalHitHeroFinal + additionCriticalHitHeroAttack) - Resilience) / 100;
            if (criticalHitWithResilience < 0) criticalHitWithResilience = 0;
            if (criticalHitWithResilience > 1) criticalHitWithResilience = 1;
            double result = (1 - Resilience / 100) * (1 - criticalHitWithResilience) + Math.Pow((1 - Resilience / 100), 2) * criticalHitWithResilience * (2 + CriticalDamageFinal / 100);

            return result;
        }
        private double FormulaCoefficientOfCriticalHitLuna()
        {
            double criticalHitWithResilience = (CriticalHitLuna - Resilience) / 100;
            double critDamage = CriticalDamageFinal;
            if (CrushingWillActive) critDamage += MermanModifiers.CRUSHING_WILL_ADDITIONAL_CRITICAL_DAMAGE;
            if (criticalHitWithResilience < 0) criticalHitWithResilience = 0;
            if (criticalHitWithResilience > 1) criticalHitWithResilience = 1;
            double result = (1 - Resilience / 100) * (1 - criticalHitWithResilience) + Math.Pow((1 - Resilience / 100), 2) * criticalHitWithResilience * (2 + critDamage / 100);

            return result;
        }
        private double FormulaCoefficientOfCriticalHitForSkill()
        {
            double criticalHitWithResilience = (CriticalHitHeroFinal - Resilience) / 100;
            double critDamage = CriticalDamageFinal;
            if (CrushingWillActive) critDamage += MermanModifiers.CRUSHING_WILL_ADDITIONAL_CRITICAL_DAMAGE;
            if (criticalHitWithResilience < 0) criticalHitWithResilience = 0;
            if (criticalHitWithResilience > 1) criticalHitWithResilience = 1;
            double result = (1 - Resilience / 100) * (1 - criticalHitWithResilience) + Math.Pow((1 - Resilience / 100), 2) * criticalHitWithResilience * (2 + (critDamage + additionAnimalRageTalant) / 100);

            return result;
        }

        private double FormulaCoefficientOfAttackStrength()
        {
            double result = AttackStrengthFinal.ConvertToCoefficient();
            return result;
        }

        private double FormulaCoefficientOfPenetration()
        {
            double result = 1 - Math.Max(0, Protection - PenetrationHeroFinal) / 100;
            return result;
        }
        private double FormulaCoefficientOfPenetrationLuna()
        {
            double result = 1 - Math.Max(0, Protection - PenetrationLuna) / 100;
            return result;
        }
        private double FormulaCoefficientOfAccuracy()
        {
            double result = 1 - Math.Max(0, Dodge - AccuracyHeroFinal) / 100;
            return result;
        }
        private double FormulaCoefficientOfAccuracyLuna()
        {
            double result = 1 - Math.Max(0, Dodge - AccuracyLuna) / 100;
            return result;
        }

        private double FormulaCoefficientOfPiercingAttack()
        {
            double result = 1 - (Math.Max(0, (Protection - PenetrationHeroFinal) * (1 - (PiercingAttack / 100)))) / 100 ;
            return result;
        }
        private double FormulaCoefficientOfPiercingAttackLuna()
        {
            double result = 1 - (Math.Max(0, (Protection - PenetrationLuna) * (1 - (PiercingAttack / 100)))) / 100;
            return result;
        }
        private double FormulaCoefficientOfRage()
        {
            double result = 0;
            double t = (10 + additionalContinuousFuryTalant) * FacilitationFinal.ConvertToCoefficient();
            double s = 0;
            if (AttackActive)
            {
                s += 1 / AttackDelay();
            }
            if (MoonTouchActive)
            {
                s += 1 / MoonTouchCooldown();
            }
            if (ChainLightningActive)
            {
                s += 1 / ChainLightningCooldown();
            }
            if (s == 0 || RageFinal == 0)
                return 0;
            result = t * RageFinal / 100 * s;
            if (result > 1) result = 1;
            if (result < 0) result = 0;
            return result * 0.1;
        }

        #endregion

        #region Замок (урон от скиллов)

        private ObservableCollection<KeyValuePair<CastleSectors, string>> _castles = new ObservableCollection<KeyValuePair<CastleSectors, string>>()
        {
            new KeyValuePair<CastleSectors, string>(CastleSectors.Empty, "Без замка | 0%"),
            new KeyValuePair<CastleSectors, string>(CastleSectors.First, "1 сектор | 5%"),
            new KeyValuePair<CastleSectors, string>(CastleSectors.Second, "2 сектор | 7.5%"),
            new KeyValuePair<CastleSectors, string>(CastleSectors.Third, "3 сектор | 10%"),
            new KeyValuePair<CastleSectors, string>(CastleSectors.Fourth, "4 сектор | 12.5%"),
            new KeyValuePair<CastleSectors, string>(CastleSectors.Fifth, "5 сектор | 15%"),
        };
        public ObservableCollection<KeyValuePair<CastleSectors, string>> CastlesNew
        {
            get => _castles;
        }

        private CastleSectors selectedCastle = CastleSectors.Empty;
        public CastleSectors SelectedCastle
        {
            //get => selectedCastle;
            get => DataSet.SelectedCastle;
            set
            {
                DataSet.SelectedCastle = value;
                coefficientCastle = (2.5 * (int)value).ConvertToCoefficient();
                Calculate();
                NotifyPropertyChanged(nameof(SelectedCastle));
            }
        }

        #endregion


        #region РАЗОБРАТЬ ЧТО ТУТ
        public bool HasTalantMoonTouchPlus
        {
            get => Moon_Touch.HasTalantPlus;
            set { Moon_Touch.HasTalantPlus = value;
                Calculate();
                NotifyPropertyChanged(nameof(HasTalantMoonTouchPlus)); }
        }
        public bool HasRelicMoonTouch
        {
            get => Moon_Touch.HasRelic;
            set { Moon_Touch.HasRelic = value;
                Calculate();
                NotifyPropertyChanged(nameof(HasRelicMoonTouch)); }
        }

        public bool HasRelicChainLightning
        {
            get => Chain_Lightning.HasRelic;
            set { Chain_Lightning.HasRelic = value;
                Calculate();
                NotifyPropertyChanged(nameof(HasRelicChainLightning));
            }
        }

        /*private List<string> castles = new List<string>()
        {
            "Без замка",
            "1 сектор, 5%",
            "2 сектор, 7.5%",
            "3 сектор, 10%",
            "4 сектор, 12.5%",
            "5 сектор, 15%",
        };*/
        /*public List<string> Castles
        {
            get => ModifiersDamage.Castle;
            //set { castles = value; NotifyPropertyChanged("Castles"); }
        }*/

        public double coefficientCastle = 1;
        //private string numberCastle = "Без замка";
        /*public string NumberCastle
        {
            //get => numberCastle;
            get => DataSet.NumberCastle;
            set { //numberCastle = value;
                DataSet.NumberCastle = value;
                switch (value)
                {
                    case "1 сектор, 5%":
                        coefficientCastle = 1.05;
                        break;
                    case "2 сектор, 7.5%":
                        coefficientCastle = 1.075;
                        break;
                    case "3 сектор, 10%":
                        coefficientCastle = 1.1;
                        break;
                    case "4 сектор, 12.5%":
                        coefficientCastle = 1.125;
                        break;
                    case "5 сектор, 15%":
                        coefficientCastle = 1.15;
                        break;
                    default:
                        coefficientCastle = 1;
                        break;
                }
                Calculate();
                NotifyPropertyChanged(nameof(NumberCastle)); }
        }*/
        public bool HasTalantBeastAwakeningMage
        {
            get => Beast_Awakening.HasTalantMage;
            set
            {
                if (LvlTalantBeastAwakeningPhysical == 0)
                {
                    Beast_Awakening.HasTalantMage = value;
                    Calculate();
                    NotifyPropertyChanged(nameof(HasTalantBeastAwakeningMage));
                }
            }
        }
        // временно
        /*public List<string> threeLevels = new List<string>()
        {
            "0", "1", "2", "3",
        };
        public List<string> ThreeLevels
        {
            get => threeLevels;
            set { threeLevels = value; NotifyPropertyChanged("ThreeLevels"); }
        }*/
        public int LvlTalantBeastAwakeningPhysical
        {
            get => Beast_Awakening.LvlTalantPhys;
            set
            {
                if (!HasTalantBeastAwakeningMage)
                {
                    Beast_Awakening.LvlTalantPhys = value;
                    Calculate();
                    NotifyPropertyChanged(nameof(LvlTalantBeastAwakeningPhysical));
                }
            }
        }
        public bool HasTalantBestialRampage
        {
            get => Bestial_Rampage.HasTalant;
            set { Bestial_Rampage.HasTalant = value; 
                Calculate(); NotifyPropertyChanged(nameof(HasTalantBestialRampage)); }
        }
        public bool HasTalantPowerOfNature
        {
            get => AuraOfTheForest.HasTalantPowerOfNature;
            set { AuraOfTheForest.HasTalantPowerOfNature = value; 
                Calculate(); NotifyPropertyChanged(nameof(HasTalantPowerOfNature)); }
        }
        //private bool hasTalantGrandeurOfTheLotus = false;
        public bool HasTalantGrandeurOfTheLotus
        {
            //get => hasTalantGrandeurOfTheLotus;
            get => DataSet.HasTalantGrandeurOfTheLotus;
            set { 
                //hasTalantGrandeurOfTheLotus = value; 
                DataSet.HasTalantGrandeurOfTheLotus = value; 
                Calculate(); NotifyPropertyChanged(nameof(HasTalantGrandeurOfTheLotus)); }
        }
        public int LvlTalantMoonlightPlus
        {
            get => Moonlight.LvlTalant;
            set
            {
                Moonlight.LvlTalant = value;
                Calculate();
                NotifyPropertyChanged(nameof(LvlTalantMoonlightPlus));
            }
        }
        public int LvlTalantOrderToAttackPlusDualRage
        {
            //get => OrderToAttack.LvlTalant;
            get => OrderToAttack.LvlTalantDualRage;
            set
            {
                OrderToAttack.LvlTalantDualRage = value;
                Calculate();
                NotifyPropertyChanged(nameof(LvlTalantOrderToAttackPlusDualRage));
            }
        }
        public int LvlTalantOrderToAttackPlusGuardianUnity
        {
            //get => OrderToAttack.LvlTalant;
            get => OrderToAttack.LvlTalantGuardianUnity;
            set
            {
                OrderToAttack.LvlTalantGuardianUnity = value;
                Calculate();
                NotifyPropertyChanged(nameof(LvlTalantOrderToAttackPlusGuardianUnity));
            }
        }

        public bool HasTalantBlessingOfTheMoonPlusCriticalHit
        {
            get => BlessingOfTheMoon.HasTalantPlusCriticalHit;
            set { BlessingOfTheMoon.HasTalantPlusCriticalHit = value; 
                Calculate(); NotifyPropertyChanged(nameof(HasTalantBlessingOfTheMoonPlusCriticalHit)); }
        }

        //private bool hasTalantSymbiosis = false;
        public bool HasTalantSymbiosis
        {
            //get => hasTalantSymbiosis;
            get => DataSet.HasTalantSymbiosis;
            set
            {
                //hasTalantSymbiosis = value; 
                DataSet.HasTalantSymbiosis = value; 
                Calculate(); 
                NotifyPropertyChanged(nameof(HasTalantSymbiosis));
            }
        }


        public bool HasTalantBlessingOfTheMoonPlusPenetration
        {
            get => BlessingOfTheMoon.HasTalantPlusPenetration;
            set { BlessingOfTheMoon.HasTalantPlusPenetration = value; 
                Calculate(); NotifyPropertyChanged(nameof(HasTalantBlessingOfTheMoonPlusPenetration)); }
        }
        #endregion

        #region БП

        //private double coefficientBPDungeon = 1;
        private double coefficientBPDungeon()
        {
            double result = 1;
            if (DataSet.BPDungeon)
            {
                result = 1.1;
            }
            else result = 1;
            return result;
        }
        public bool ChechBPDungeon
        {
            get => DataSet.BPDungeon;
            set
            {
                DataSet.BPDungeon = value;
                Calculate();
                NotifyPropertyChanged(nameof(ChechBPDungeon));
            }
        }

        #endregion

        #region Бафы на доп урон

        public bool SacredShieldHeroActive
        {
            get => DataSet.SacredShieldHeroActive;
            set
            {
                DataSet.SacredShieldHeroActive = value;
                Calculate(); NotifyPropertyChanged(nameof(SacredShieldHeroActive));
            }
        }
        private double sacredShieldHeroCoef()
        {
            double result = 1;
            if (SacredShieldHeroActive)
                result += 0.15;
            return result;
        }

        public bool SacredShieldLunaActive
        {
            get => DataSet.SacredShieldLunaActive;
            set 
            {
                DataSet.SacredShieldLunaActive = value;
                Calculate(); NotifyPropertyChanged(nameof(SacredShieldLunaActive));
            }
        }
        private double sacredShieldLunaCoef()
        {
            double result = 1;
            if (SacredShieldLunaActive)
                result += 0.15;
            return result;
        }
        #endregion

        #region крылья жц

        public bool GodsAid
        {
            get => DataSet.GodsAid;
            set
            {
                DataSet.GodsAid = value;
                Calculate();
                NotifyPropertyChanged(nameof(GodsAid));
            }
        }

        #endregion

        #region замок

        //private bool castleSwordActive = false;

        public bool CastleSwordActive
        {
            //get => castleSwordActive;
            get => DataSet.CastleSwordActive;
            set
            {
                DataSet.CastleSwordActive = value;
                Calculate();
                NotifyPropertyChanged(nameof(CastleSwordActive));
            }
        }

        #endregion

        #region дебафы

        #region противодействие у босса

        public bool Counterstand
        {
            get => DataSet.Counterstand;
            set
            {
                DataSet.Counterstand = value;
                Calculate();
                NotifyPropertyChanged(nameof(Counterstand));
            }
        }
        // TODO
        // вынести в константу
        private double FormulaCounterstand()
        {
            if (Counterstand) return 0.67;
            return 1;
        }

        #endregion

        #endregion

        #region доп %
        public double AdditionalPercentMDDFinal
        {
            get => DataSet.AdditionalPercentMDDFinal;
            set
            {
                double p = 0;
                if (double.TryParse(value.ToString(), out p))
                    DataSet.AdditionalPercentMDDFinal = p;
                Calculate(); NotifyPropertyChanged(nameof(AdditionalPercentMDDFinal));
            }
        }
        public double AdditionalPercentPDDFinal
        {
            get => DataSet.AdditionalPercentPDDFinal;
            set
            {
                double p = 0;
                if (double.TryParse(value.ToString(), out p))
                    DataSet.AdditionalPercentPDDFinal = p;
                Calculate(); NotifyPropertyChanged(nameof(AdditionalPercentPDDFinal));
            }
        }
        #endregion

        #endregion

        #region Регулирование уровня

        #region Базовые

        #region Лунное касание
        public int LvlMoonTouch
        {
            get => Moon_Touch.Level;
            set
            {
                Moon_Touch.Level = value;
                NotifyPropertyChanged(nameof(LvlMoonTouch));
            }
        }
        public void IncreaseLvlMoonTouch()
        {
            if (LvlMoonTouch < 5)
            {
                LvlMoonTouch = LvlMoonTouch + 1;
            }
            Calculate();
        }
        private ICommand increaseLvlMoonTouchCommand;
        public ICommand IncreaseLvlMoonTouchCommand
        {
            get => increaseLvlMoonTouchCommand == null ? new RelayCommand(IncreaseLvlMoonTouch) : increaseLvlMoonTouchCommand;
        }
        public void DecreaseLvlMoonTouch()
        {
            if (LvlMoonTouch > 1)
            {
                LvlMoonTouch -= 1;
            }
            Calculate();
        }
        private ICommand decreaseLvlMoonTouchCommand;
        public ICommand DecreaseLvlMoonTouchCommand
        {
            get => decreaseLvlMoonTouchCommand == null ? new RelayCommand(DecreaseLvlMoonTouch) : decreaseLvlMoonTouchCommand;
        }
        #endregion

        #region Цепная молния

        public int LvlChainLightning
        {
            get => Chain_Lightning.Level;
            set
            {
                Chain_Lightning.Level = value;
                Calculate();
                NotifyPropertyChanged(nameof(LvlChainLightning));
            }
        }
        public void IncreaseLvlChainLightning()
        {
            if (LvlChainLightning < 5)
            {
                LvlChainLightning += 1;
            }
        }
        private ICommand increaseLvlChainLightningCommand;
        public ICommand IncreaseLvlChainLightningCommand
        {
            get => increaseLvlChainLightningCommand == null ? new RelayCommand(IncreaseLvlChainLightning) : increaseLvlChainLightningCommand;
        }
        public void DecreaseLvlChainLightning()
        {
            if (LvlChainLightning > 1)
            {
                LvlChainLightning -= 1;
            }
        }
        private ICommand decreaseLvlChainLightningCommand;
        public ICommand DecreaseLvlChainLightningCommand
        {
            get => decreaseLvlChainLightningCommand == null ? new RelayCommand(DecreaseLvlChainLightning) : decreaseLvlChainLightningCommand;
        }
        #endregion

        #region Приказ к атаке

        public int LvlOrderToAttack
        {
            get => OrderToAttack.Level;
            set
            {
                OrderToAttack.Level = value;
                Calculate();
                NotifyPropertyChanged(nameof(LvlOrderToAttack));
            }
        }
        public void IncreaseLvlOrderToAttack()
        {
            if (LvlOrderToAttack < 5)
            {
                LvlOrderToAttack += 1;
            }
        }
        private ICommand increaseLvlOrderToAttackCommand;
        public ICommand IncreaseLvlOrderToAttackCommand
        {
            get => increaseLvlOrderToAttackCommand == null ? new RelayCommand(IncreaseLvlOrderToAttack) : increaseLvlOrderToAttackCommand;
        }
        public void DecreaseLvlOrderToAttack()
        {
            if (LvlOrderToAttack > 1)
            {
                LvlOrderToAttack -= 1;
            }
        }
        private ICommand decreaseLvlLvlOrderToAttackCommand;
        public ICommand DecreaseLvlLvlOrderToAttackCommand
        {
            get => decreaseLvlLvlOrderToAttackCommand == null ? new RelayCommand(DecreaseLvlOrderToAttack) : decreaseLvlLvlOrderToAttackCommand;
        }

        #endregion

        #region Пробуждение зверя
        public int LvlBeastAwakening
        {
            get => Beast_Awakening.Level;
            set
            {
                Beast_Awakening.Level = value;
                NotifyPropertyChanged(nameof(LvlBeastAwakening));
            }
        }
        public void IncreaseBeastAwakening()
        {
            if (LvlBeastAwakening < 5)
            {
                LvlBeastAwakening = LvlBeastAwakening + 1;
            }
            Calculate();
        }
        private ICommand increaseLvlBeastAwakeningCommand;
        public ICommand IncreaseLvlBeastAwakeningCommand
        {
            get => increaseLvlBeastAwakeningCommand == null ? new RelayCommand(IncreaseBeastAwakening) : increaseLvlBeastAwakeningCommand;
        }
        public void DecreaseBeastAwakening()
        {
            if (LvlBeastAwakening > 1)
            {
                LvlBeastAwakening = LvlBeastAwakening - 1;
            }
            Calculate();
        }
        private ICommand decreaseLvlBeastAwakeningCommand;
        public ICommand DecreaseLvlBeastAwakeningCommand
        {
            get => decreaseLvlBeastAwakeningCommand == null ? new RelayCommand(DecreaseBeastAwakening) : decreaseLvlBeastAwakeningCommand;
        }

        #endregion

        #endregion
        #region Экспертные

        #region Звериное буйство
        public int LvlBestialRampage
        {
            get => Bestial_Rampage.Level;
            set
            {
                Bestial_Rampage.Level = value;
                NotifyPropertyChanged(nameof(LvlBestialRampage));
            }
        }

        public void IncreaseBestialRampage()
        {
            if (LvlBestialRampage < 4)
            {
                LvlBestialRampage = LvlBestialRampage + 1;
            }
            Calculate();
        }
        private ICommand increaseLvlBestialRampageCommand;
        public ICommand IncreaseLvlBestialRampageCommand
        {
            get => increaseLvlBestialRampageCommand == null ? new RelayCommand(IncreaseBestialRampage) : increaseLvlBestialRampageCommand;
        }
        public void DecreaseBestialRampage()
        {
            if (LvlBestialRampage > 1)
            {
                LvlBestialRampage = LvlBestialRampage - 1;
            }
            Calculate();
        }
        private ICommand decreaseLvlBestialRampageCommand;
        public ICommand DecreaseLvlBestialRampageCommand
        {
            get => decreaseLvlBestialRampageCommand == null ? new RelayCommand(DecreaseBestialRampage) : decreaseLvlBestialRampageCommand;
        }
        #endregion
        #region Аура леса
        public int LvlAuraOfTheForest
        {
            get => AuraOfTheForest.Level;
            set
            {
                AuraOfTheForest.Level = value;
                NotifyPropertyChanged(nameof(AuraOfTheForest));
            }
        }
        public void IncreaseAuraOfTheForest()
        {
            if (LvlAuraOfTheForest < 4)
            {
                LvlAuraOfTheForest = LvlAuraOfTheForest + 1;
            }
            Calculate();
        }
        private ICommand increaseLvlAuraOfTheForestCommand;
        public ICommand IncreaseLvlAuraOfTheForestCommand
        {
            get => increaseLvlAuraOfTheForestCommand == null ? new RelayCommand(IncreaseAuraOfTheForest) : increaseLvlAuraOfTheForestCommand;
        }
        public void DecreaseAuraOfTheForest()
        {
            if (LvlAuraOfTheForest > 1)
            {
                LvlAuraOfTheForest = LvlAuraOfTheForest - 1;
            }
            Calculate();
        }
        private ICommand decreaseLvlAuraOfTheForestCommand;
        public ICommand DecreaseLvlAuraOfTheForestCommand
        {
            get => decreaseLvlAuraOfTheForestCommand == null ? new RelayCommand(DecreaseAuraOfTheForest) : decreaseLvlAuraOfTheForestCommand;
        }
        #endregion
        #region Лунный свет
        public int LvlMoonlight
        {
            get => Moonlight.Level;
            set
            {
                Moonlight.Level = value;
                NotifyPropertyChanged(nameof(LvlMoonlight));
            }
        }
        public void IncreaseMoonlight()
        {
            if (LvlMoonlight < 4)
            {
                LvlMoonlight = LvlMoonlight + 1;
            }
            Calculate();
        }
        private ICommand increaseLvlMoonlightCommand;
        public ICommand IncreaseLvlMoonlightCommand
        {
            get => increaseLvlMoonlightCommand == null ? new RelayCommand(IncreaseMoonlight) : increaseLvlMoonlightCommand;
        }
        public void DecreaseMoonlight()
        {
            if (LvlMoonlight > 1)
            {
                LvlMoonlight = LvlMoonlight - 1;
            }
            Calculate();
        }
        private ICommand decreaseLvlMoonlightCommand;
        public ICommand DecreaseLvlMoonlightCommand
        {
            get => decreaseLvlMoonlightCommand == null ? new RelayCommand(DecreaseMoonlight) : decreaseLvlMoonlightCommand;
        }
        #endregion
        #region Благословение луны
        /// <summary>
        /// Свойство связывающее бизнес логику со свойством Level навыка Благословение луны
        /// </summary>
        public int LvlBlessingOfTheMoon
        {
            get => BlessingOfTheMoon.Level;
            set
            {
                BlessingOfTheMoon.Level = value;
                Calculate();
                NotifyPropertyChanged(nameof(LvlBlessingOfTheMoon));
            }
        }
        /// <summary>
        /// Увеличение уровня навыка Благословение луны
        /// </summary>
        public void IncreaseBlessingOfTheMoon()
        {
            if (LvlBlessingOfTheMoon < 4)
            {
                LvlBlessingOfTheMoon = LvlBlessingOfTheMoon + 1;
            }
        }
        private ICommand increaseLvlBlessingOfTheMoonCommand;
        public ICommand IncreaseLvlBlessingOfTheMoonCommand
        {
            get => increaseLvlBlessingOfTheMoonCommand == null ? new RelayCommand(IncreaseBlessingOfTheMoon) : increaseLvlBlessingOfTheMoonCommand;
        }
        /// <summary>
        /// Снижение уровня навыка Благословение луны
        /// </summary>
        public void DecreaseBlessingOfTheMoon()
        {
            if (LvlBlessingOfTheMoon > 1)
            {
                LvlBlessingOfTheMoon = LvlBlessingOfTheMoon - 1;
            }
        }
        private ICommand decreaseLvlBlessingOfTheMoonCommand;
        public ICommand DecreaseLvlBlessingOfTheMoonCommand
        {
            get => decreaseLvlBlessingOfTheMoonCommand == null ? new RelayCommand(DecreaseBlessingOfTheMoon) : decreaseLvlBlessingOfTheMoonCommand;
        }

        #endregion
        #region Двойная концентрация
        /// <summary>
        /// Свойство связывающее бизнес логику со свойством Level навыка Двойная концентрация
        /// </summary>
        public int LvlDoubleConcentration
        {
            get => DoubleConcentration.Level;
            set
            {
                DoubleConcentration.Level = value;
                Calculate();
                NotifyPropertyChanged(nameof(LvlDoubleConcentration));
            }
        }
        /// <summary>
        /// Увеличение уровня навыка Двойная концентрация
        /// </summary>
        public void IncreaseDoubleConcentration()
        {
            if (LvlDoubleConcentration < 4)
            {
                LvlDoubleConcentration = LvlDoubleConcentration + 1;
            }
        }
        private ICommand increaseLvlDoubleConcentrationCommand;
        public ICommand IncreaseLvlDoubleConcentrationCommand
        {
            get => increaseLvlDoubleConcentrationCommand == null ? new RelayCommand(IncreaseDoubleConcentration) : increaseLvlDoubleConcentrationCommand;
        }
        /// <summary>
        /// Снижение уровня навыка Двойная концентрация
        /// </summary>
        public void DecreaseDoubleConcentration()
        {
            if (LvlDoubleConcentration > 1)
            {
                LvlDoubleConcentration = LvlDoubleConcentration - 1;
            }
        }
        private ICommand decreaseLvlDoubleConcentrationCommand;
        public ICommand DecreaseLvlDoubleConcentrationCommand
        {
            get => decreaseLvlDoubleConcentrationCommand == null ? new RelayCommand(DecreaseDoubleConcentration) : decreaseLvlDoubleConcentrationCommand;
        }
        #endregion



        #endregion

        #region Таланты

        #region Лунный свет +
        public void IncreaseTalantMoonlightPlus()
        {
            if (ForestInspirationActive)
            {
                if (LvlTalantMoonlightPlus < 3)
                {
                    LvlTalantMoonlightPlus = LvlTalantMoonlightPlus + 1;
                }
                Calculate();
            }
        }
        private ICommand increaseLvlTalantMoonlightPlusCommand;
        public ICommand IncreaseLvlTalantMoonlightPlusCommand
        {
            get => increaseLvlTalantMoonlightPlusCommand == null ? new RelayCommand(IncreaseTalantMoonlightPlus) : increaseLvlTalantMoonlightPlusCommand;
        }
        public void DecreaseTalantMoonlightPlus()
        {
            if (ForestInspirationActive)
            {
                if (LvlTalantMoonlightPlus > 0)
                {
                    LvlTalantMoonlightPlus = LvlTalantMoonlightPlus - 1;
                }
                Calculate();
            }
        }
        private ICommand decreaseLvlTalantMoonlightPlusCommand;
        public ICommand DecreaseLvlTalantMoonlightPlusCommand
        {
            get => decreaseLvlTalantMoonlightPlusCommand == null ? new RelayCommand(DecreaseTalantMoonlightPlus) : decreaseLvlTalantMoonlightPlusCommand;
        }
        #endregion

        #region Пробуждение зверя + (физ)
        public void IncreaseTalantBeastAwakeningPlusPhysical()
        {
            if (DualRageActive)
            {
                if (LvlTalantBeastAwakeningPhysical < 3)
                {
                    LvlTalantBeastAwakeningPhysical = LvlTalantBeastAwakeningPhysical + 1;
                }
                Calculate();
            }
        }
        private ICommand increaseLvlTalantBeastAwakeningPlusPhysicalCommand;
        public ICommand IncreaseLvlTalantBeastAwakeningPlusPhysicalCommand
        {
            get => increaseLvlTalantBeastAwakeningPlusPhysicalCommand == null ? new RelayCommand(IncreaseTalantBeastAwakeningPlusPhysical) : increaseLvlTalantBeastAwakeningPlusPhysicalCommand;

        }
        public void DecreaseTalantBeastAwakeningPlusPhysical()
        {
            if (DualRageActive)
            {
                if (LvlTalantBeastAwakeningPhysical > 0)
                {
                    LvlTalantBeastAwakeningPhysical = LvlTalantBeastAwakeningPhysical - 1;
                }
                Calculate();
            }
        }
        private ICommand decreaseLvlTalantBeastAwakeningPlusPhysicalCommand;
        public ICommand DecreaseLvlTalantBeastAwakeningPlusPhysicalCommand
        {
            get => decreaseLvlTalantBeastAwakeningPlusPhysicalCommand == null ? new RelayCommand(DecreaseTalantBeastAwakeningPlusPhysical) : decreaseLvlTalantBeastAwakeningPlusPhysicalCommand;

        }

        #endregion

        #region Звериный гнев 

        public void IncreaseTalantBestialRage()
        {
            if (LvlTalantBestialRage < 3)
            {
                LvlTalantBestialRage = LvlTalantBestialRage + 1;
            }
            Calculate();
        }
        private ICommand increaseLvlTalantBestialRageCommand;
        public ICommand IncreaseLvlTalantBestialRageCommand
        {
            get => increaseLvlTalantBestialRageCommand == null ? new RelayCommand(IncreaseTalantBestialRage) : increaseLvlTalantBestialRageCommand;
        }
        public void DecreaseTalantBestialRage()
        {
            if (LvlTalantBestialRage > 0)
            {
                LvlTalantBestialRage = LvlTalantBestialRage - 1;
            }
            Calculate();
        }
        private ICommand decreaseLvlTalantBestialRageCommand;
        public ICommand DecreaseLvlTalantBestialRageCommand
        {
            get => decreaseLvlTalantBestialRageCommand == null ? new RelayCommand(DecreaseTalantBestialRage) : decreaseLvlTalantBestialRageCommand;
        }
        #endregion

        #region Исступление хищника

        public void IncreaseTalantPredatoryDelirium()
        {
            if (LvlTalantPredatoryDelirium < 3)
            {
                LvlTalantPredatoryDelirium = LvlTalantPredatoryDelirium + 1;
            }
            Calculate();
        }
        private ICommand increaseLvlTalantPredatoryDeliriumCommand;
        public ICommand IncreaseLvlTalantPredatoryDeliriumCommand
        {
            get => increaseLvlTalantPredatoryDeliriumCommand == null ? new RelayCommand(IncreaseTalantPredatoryDelirium) : increaseLvlTalantPredatoryDeliriumCommand;
        }
        public void DecreaseTalantPredatoryDelirium()
        {
            if (LvlTalantPredatoryDelirium > 0)
            {
                LvlTalantPredatoryDelirium = LvlTalantPredatoryDelirium - 1;
            }
            Calculate();
        }
        private ICommand decreaseLvlTalantPredatoryDeliriumCommand;
        public ICommand DecreaseLvlTalantPredatoryDeliriumCommand
        {
            get => decreaseLvlTalantPredatoryDeliriumCommand == null ? new RelayCommand(DecreaseTalantPredatoryDelirium) : decreaseLvlTalantPredatoryDeliriumCommand;
        }

        #endregion

        #region Момент силы

        public void IncreaseTalantMomentOfPower()
        {
            if (LvlTalantMomentOfPower < 4)
            {
                LvlTalantMomentOfPower = LvlTalantMomentOfPower + 1;
            }
            Calculate();
        }
        private ICommand increaseLvlTalantMomentOfPowerCommand;
        public ICommand IncreaseLvlTalantMomentOfPowerCommand
        {
            get => increaseLvlTalantMomentOfPowerCommand == null ? new RelayCommand(IncreaseTalantMomentOfPower) : increaseLvlTalantMomentOfPowerCommand;
        }
        public void DecreaseTalantMomentOfPower()
        {
            if (LvlTalantMomentOfPower > 0)
            {
                LvlTalantMomentOfPower = LvlTalantMomentOfPower - 1;
            }
            Calculate();
        }
        private ICommand decreaseLvlTalantMomentOfPowerCommand;
        public ICommand DecreaseLvlTalantMomentOfPowerCommand
        {
            get => decreaseLvlTalantMomentOfPowerCommand == null ? new RelayCommand(DecreaseTalantMomentOfPower) : decreaseLvlTalantMomentOfPowerCommand;
        }
        #endregion

        #region Долгая смерть

        public void IncreaseTalantLongDeath()
        {
            if (LvlTalantLongDeath < 4)
            {
                LvlTalantLongDeath = LvlTalantLongDeath + 1;
            }
            Calculate();
        }
        private ICommand increaseLvlTalantLongDeathCommand;
        public ICommand IncreaseLvlTalantLongDeathCommand
        {
            get => increaseLvlTalantLongDeathCommand == null ? new RelayCommand(IncreaseTalantLongDeath) : increaseLvlTalantLongDeathCommand;
        }

        public void DecreaseTalantLongDeath()
        {
            if (LvlTalantLongDeath > 0)
            {
                LvlTalantLongDeath = LvlTalantLongDeath - 1;
            }
            Calculate();
        }
        private ICommand decreaseLvlTalantLongDeathCommand;
        public ICommand DecreaseLvlTalantLongDeathCommand
        {
            get => decreaseLvlTalantLongDeathCommand == null ? new RelayCommand(DecreaseTalantLongDeath) : decreaseLvlTalantLongDeathCommand;
        }

        #endregion

        #region Животный гнев

        public void IncreaseTalantAnimalRage()
        {
            if (LvlTalantAnimalRage < 3)
            {
                LvlTalantAnimalRage = LvlTalantAnimalRage + 1;
            }
            Calculate();
        }
        private ICommand increaseLvlTalantAnimalRageCommand;
        public ICommand IncreaseLvlTalantAnimalRageCommand
        {
            get => increaseLvlTalantAnimalRageCommand == null ? new RelayCommand(IncreaseTalantAnimalRage) : increaseLvlTalantAnimalRageCommand;
        }
        public void DecreaseTalantAnimalRage()
        {
            if (LvlTalantAnimalRage > 0)
            {
                LvlTalantAnimalRage = LvlTalantAnimalRage - 1;
            }
            Calculate();
        }
        private ICommand decreaseLvlTalantAnimalRageCommand;
        public ICommand DecreaseLvlTalantAnimalRageCommand
        {
            get => decreaseLvlTalantAnimalRageCommand == null ? new RelayCommand(DecreaseTalantAnimalRage) : decreaseLvlTalantAnimalRageCommand;
        }

        #endregion

        #region Приказ к атаке + 2 ветка

        public void IncreaseTalantOrderToAttackPlusDualRage()
        {
            if (DualRageActive)
            {
                if (LvlTalantOrderToAttackPlusDualRage < 3)
                {
                    LvlTalantOrderToAttackPlusDualRage = LvlTalantOrderToAttackPlusDualRage + 1;
                }
                Calculate();
            }
        }
        private ICommand increaseLvlTalantOrderToAttackPlusDualRageCommand;
        public ICommand IncreaseLvlTalantOrderToAttackPlusDualRageCommand
        {
            get => increaseLvlTalantOrderToAttackPlusDualRageCommand == null ? new RelayCommand(IncreaseTalantOrderToAttackPlusDualRage) : increaseLvlTalantOrderToAttackPlusDualRageCommand;

        }
        public void DecreaseTalantOrderToAttackPlusDualRage()
        {
            if (DualRageActive)
            {
                if (LvlTalantOrderToAttackPlusDualRage > 0)
                {
                    LvlTalantOrderToAttackPlusDualRage = LvlTalantOrderToAttackPlusDualRage - 1;
                }
                Calculate();
            }
        }
        private ICommand decreaseLvlTalantOrderToAttackPlusDualRageCommand;
        public ICommand DecreaseLvlTalantOrderToAttackPlusDualRageCommand
        {
            get => decreaseLvlTalantOrderToAttackPlusDualRageCommand == null ? new RelayCommand(DecreaseTalantOrderToAttackPlusDualRage) : decreaseLvlTalantOrderToAttackPlusDualRageCommand;

        }

        #endregion

        #region Приказ к атаке + 1 ветка

        public void IncreaseTalantOrderToAttackPlusGuardianUnity()
        {
            if (GuardianUnityActive)
            {
                if (LvlTalantOrderToAttackPlusGuardianUnity < 3)
                {
                    LvlTalantOrderToAttackPlusGuardianUnity = LvlTalantOrderToAttackPlusGuardianUnity + 1;
                }
                Calculate();
            }
        }
        private ICommand increaseLvlTalantOrderToAttackPlusGuardianUnityCommand;
        public ICommand IncreaseLvlTalantOrderToAttackPlusGuardianUnityCommand
        {
            get => increaseLvlTalantOrderToAttackPlusGuardianUnityCommand == null ? new RelayCommand(IncreaseTalantOrderToAttackPlusGuardianUnity) : increaseLvlTalantOrderToAttackPlusGuardianUnityCommand;

        }
        public void DecreaseTalantOrderToAttackPlusGuardianUnity()
        {
            if (GuardianUnityActive)
            {
                if (LvlTalantOrderToAttackPlusGuardianUnity > 0)
                {
                    LvlTalantOrderToAttackPlusGuardianUnity = LvlTalantOrderToAttackPlusGuardianUnity - 1;
                }
                Calculate();
            }
        }
        private ICommand decreaseLvlTalantOrderToAttackPlusGuardianUnityCommand;
        public ICommand DecreaseLvlTalantOrderToAttackPlusGuardianUnityCommand
        {
            get => decreaseLvlTalantOrderToAttackPlusGuardianUnityCommand == null ? new RelayCommand(DecreaseTalantOrderToAttackPlusGuardianUnity) : decreaseLvlTalantOrderToAttackPlusGuardianUnityCommand;

        }

        #endregion

        #region ивенты

        #region Длительная неистовость

        public void IncreaseTalantContinuousFury()
        {
            if (LvlTalantContinuousFury < 3)
            {
                LvlTalantContinuousFury = LvlTalantContinuousFury + 1;
            }
            Calculate();
        }
        private ICommand increaseLvlTalantContinuousFuryCommand;
        public ICommand IncreaseLvlTalantContinuousFuryCommand
        {
            get => increaseLvlTalantContinuousFuryCommand == null ? new RelayCommand(IncreaseTalantContinuousFury) : increaseLvlTalantContinuousFuryCommand;
        }
        public void DecreaseTalantContinuousFury()
        {
            if (LvlTalantContinuousFury > 0)
            {
                LvlTalantContinuousFury = LvlTalantContinuousFury - 1;
            }
            Calculate();
        }
        private ICommand decreaseLvlTalantContinuousFuryCommand;
        public ICommand DecreaseLvlTalantContinuousFuryCommand
        {
            get => decreaseLvlTalantContinuousFuryCommand == null ? new RelayCommand(DecreaseTalantContinuousFury) : decreaseLvlTalantContinuousFuryCommand;
        }

        #endregion

        #endregion

        #endregion

        #endregion

        #region Показатели включения источника урона

        #region Выбор оружия

        //private bool staffSelected = false;
        public bool StaffSelected
        {
            get => DataSet.StaffSelected;
            set
            {

                if (value)
                {
                    DataSet.StaffSelected = value;
                    DataSet.SwordSelected = false;
                    DataSet.SpearSelected = false;
                    DataSet.AxeSelected = false;
                    DataSet.MaceSelected = false;
                    Attack.IsStaff = true;
                    Attack.TimeDelay = 3.1;
                }
                Calculate();
                NotifyPropertyChanged(nameof(StaffSelected));
            }
        }
        //private bool spearSelected = false;
        public bool SpearSelected
        {
            get => DataSet.SpearSelected;
            set
            {
                if (value)
                {
                    DataSet.SpearSelected = value;
                    DataSet.SwordSelected = false;
                    DataSet.AxeSelected = false;
                    DataSet.StaffSelected = false;
                    DataSet.MaceSelected = false;
                    Attack.IsStaff = false;
                    Attack.TimeDelay = 3.4;
                }
                Calculate();
                NotifyPropertyChanged(nameof(SpearSelected));
            }
        }

        //private bool maceSelected = false;
        public bool MaceSelected
        {
            //get => maceSelected;
            get => DataSet.MaceSelected;
            set
            {
                if (value)
                {
                    DataSet.MaceSelected = value;
                    DataSet.StaffSelected = false;
                    DataSet.SpearSelected = false;
                    DataSet.SwordSelected = false;
                    DataSet.AxeSelected = false;
                    Attack.IsStaff = false;
                    Attack.TimeDelay = 3.2;
                }
                Calculate();
                NotifyPropertyChanged(nameof(MaceSelected));
            }
        }

        //private bool swordSelected = false;
        public bool SwordSelected
        {
            //get => swordSelected;
            get => DataSet.SwordSelected;
            set
            {
                if (value)
                {
                    DataSet.SwordSelected = value;
                    DataSet.SpearSelected = false;
                    DataSet.AxeSelected = false;
                    DataSet.StaffSelected = false;
                    DataSet.MaceSelected = false;
                    Attack.IsStaff = false;
                    Attack.TimeDelay = 3.2;
                }
                Calculate();
                NotifyPropertyChanged(nameof(SwordSelected));
            }
        }

        //private bool axeSelected = false;
        public bool AxeSelected
        {
            get => DataSet.AxeSelected;
            set
            {
                if (value)
                {
                    DataSet.AxeSelected = value;
                    DataSet.SwordSelected = false;
                    DataSet.SpearSelected = false;
                    DataSet.StaffSelected = false;
                    DataSet.MaceSelected = false;
                    Attack.IsStaff = false;
                    Attack.TimeDelay = 3.2;
                }
                Calculate();
                NotifyPropertyChanged(nameof(AxeSelected));
            }
        }

        #endregion

        //private bool attackActive = true;
        public bool AttackActive
        {
            get => DataSet.AttackActive;
            set {  
                DataSet.AttackActive = value; 
                Calculate(); NotifyPropertyChanged(nameof(AttackActive)); }
        }

        //private bool moonTouchActive = true;
        public bool MoonTouchActive
        {
            get => DataSet.MoonTouchActive;
            set { 
                DataSet.MoonTouchActive = value; 
                Calculate(); NotifyPropertyChanged(nameof(MoonTouchActive)); }
        }
        //private bool beastAwakeningActive = true;
        public bool BeastAwakeningActive
        {
            get => DataSet.BeastAwakeningActive;
            set { 
                DataSet.BeastAwakeningActive = value; 
                Calculate(); NotifyPropertyChanged(nameof(BeastAwakeningActive)); }
        }
        //private bool orderToAttackActive = true;
        public bool OrderToAttackActive
        {
            //get => orderToAttackActive;
            get => DataSet.OrderToAttackActive;
            set { //orderToAttackActive = value; 
                DataSet.OrderToAttackActive = value;
                Calculate(); NotifyPropertyChanged(nameof(OrderToAttackActive)); }
        }
        //private bool healingActive = true;
        public bool HealingActive
        {
            //get => healingActive;
            get => DataSet.HealingActive;
            set { //healingActive = value;
                DataSet.HealingActive = value;
                Calculate(); NotifyPropertyChanged(nameof(HealingActive)); }
        }
        //private bool chainLightningActive = true;
        public bool ChainLightningActive
        {
            //get => chainLightningActive;
            get => DataSet.ChainLightningActive;
            set { //chainLightningActive = value; 
                DataSet.ChainLightningActive = value;
                Calculate(); NotifyPropertyChanged(nameof(ChainLightningActive)); }
        }
        //private bool bestialRampageActive = true;
        public bool BestialRampageActive
        {
            //get => bestialRampageActive;
            get => DataSet.BestialRampageActive;
            set { 
                //bestialRampageActive = value; 
                DataSet.BestialRampageActive = value; 
                Calculate(); NotifyPropertyChanged(nameof(BestialRampageActive)); }
        }
        //private bool auraOfTheForestActive = true;
        public bool AuraOfTheForestActive
        {
            //get => auraOfTheForestActive;
            get => DataSet.AuraOfTheForestActive;
            set {
                //auraOfTheForestActive = value;
                DataSet.AuraOfTheForestActive = value;
                Calculate(); NotifyPropertyChanged(nameof(AuraOfTheForestActive)); }
        }
        //private bool moonlightPermanentActive = true;
        public bool MoonlightPermanentActive
        {
            //get => moonlightPermanentActive;
            get => DataSet.MoonlightPermanentActive;
            set { 
                //moonlightPermanentActive = value; 
                DataSet.MoonlightPermanentActive = value; 
                Calculate();NotifyPropertyChanged(nameof(MoonlightPermanentActive)); }
        }
        //private bool moonlightNonPermanentActive = true;
        public bool MoonlightNonPermanentActive
        {
            //get => moonlightNonPermanentActive;
            get => DataSet.MoonlightNonPermanentActive;
            set { 
                //moonlightNonPermanentActive = value; 
                DataSet.MoonlightNonPermanentActive = value; 
                Calculate(); NotifyPropertyChanged(nameof(MoonlightNonPermanentActive)); }
        }

        //private bool blessingOfTheMoonActive = true;
        public bool BlessingOfTheMoonActive
        {
            //get => blessingOfTheMoonActive;
            get => DataSet.BlessingOfTheMoonActive;
            set
            {
                //blessingOfTheMoonActive = value; 
                if (!value)
                {
                    IsUsingBlessingOfTheMoonOnLuna = false;
                }
                DataSet.BlessingOfTheMoonActive = value; 
                Calculate(); NotifyPropertyChanged(nameof(BlessingOfTheMoonActive));
            }
        }

        //private bool doubleConcentrationActive = true;
        public bool DoubleConcentrationActive
        {
            //get => doubleConcentrationActive;
            get => DataSet.DoubleConcentrationActive;
            set
            {
                //doubleConcentrationActive = value; 
                DataSet.DoubleConcentrationActive = value; 
                Calculate(); NotifyPropertyChanged(nameof(DoubleConcentrationActive));
            }
        }

        #endregion
    }
}