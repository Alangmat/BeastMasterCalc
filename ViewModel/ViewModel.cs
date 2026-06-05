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
using System.Configuration;

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
            string jsonFromFile = File.ReadAllText(FILE_SAVE);
            Builds = JsonConvert.DeserializeObject<ObservableCollection<Build>>(jsonFromFile);
        }
        private static readonly JsonSerializerSettings _saveSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore
        };

        public void SaveBuilds()
        {
            string json = JsonConvert.SerializeObject(Builds, _saveSettings);
            File.WriteAllText(FILE_SAVE, json);
        }
        public void AddDataSet()
        {
            GenerateNewDataSet();
            builds.Add(DataSet);
            string json = JsonConvert.SerializeObject(DataSet);
            DataSet = JsonConvert.DeserializeObject<Build>(json);
            updateStateDataSet();
            SelectedDataSet = builds[builds.Count() - 1];
        }
        public void AddCurrentDataSet()
        {
            DataSet.LastDate = DateTime.Now.ToString();
            DataSet.ID = Guid.NewGuid();
            if (builds is null)
            {
                builds = new ObservableCollection<Build>();
            }
            int result = 0;
            bool flag = int.TryParse(OutDD, out result);
            if (flag) DataSet.ResultDD = result;
            builds.Add(DataSet);
            string json = JsonConvert.SerializeObject(DataSet);
            DataSet = JsonConvert.DeserializeObject<Build>(json);
            updateStateDataSet();
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
                int dd = 0;
                int.TryParse(OutDD, out dd);
                if (dd > 0) DataSet.ResultDD = dd;
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

        public void SaveBuildToFile(string filePath)
        {
            string json = JsonConvert.SerializeObject(DataSet, Formatting.Indented, _saveSettings);
            File.WriteAllText(filePath, json);
        }

        public void LoadBuildFromFile(string filePath)
        {
            string json = File.ReadAllText(filePath);
            var loaded = JsonConvert.DeserializeObject<Build>(json);
            if (loaded != null)
            {
                DataSet = loaded;
                updateStateDataSet();
            }
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
            NotifyPropertyChanged(nameof(DepthsFury));
            NotifyPropertyChanged(nameof(SkillPower));
            //NotifyPropertyChanged(nameof(PercentMagicalDD));
            //NotifyPropertyChanged("PercentPhysicalDD");

            #region Pots

            NotifyPropertyChanged(nameof(CriticalHitPot));
            NotifyPropertyChanged(nameof(CriticalDamagePot));
            NotifyPropertyChanged(nameof(AccuracyPot));
            NotifyPropertyChanged(nameof(AttackSpeedPot));
            NotifyPropertyChanged(nameof(PenetrationPot));
            NotifyPropertyChanged(nameof(SkillCooldownPot));
            NotifyPropertyChanged(nameof(RagePot));
            NotifyPropertyChanged(nameof(AttackStrengthPot));
            NotifyPropertyChanged(nameof(PiercingAttackPot));
            NotifyPropertyChanged(nameof(SkillPowerPot));
            NotifyPropertyChanged(nameof(FacilitationPot));

            #endregion

            #region Scroll

            NotifyPropertyChanged(nameof(CriticalHitScroll));
            NotifyPropertyChanged(nameof(CriticalDamageScroll));
            NotifyPropertyChanged(nameof(AccuracyScroll));
            NotifyPropertyChanged(nameof(AttackSpeedScroll));
            NotifyPropertyChanged(nameof(PenetrationScroll));
            NotifyPropertyChanged(nameof(SkillCooldownScroll));
            NotifyPropertyChanged(nameof(RageScroll));
            NotifyPropertyChanged(nameof(AttackStrengthScroll));
            NotifyPropertyChanged(nameof(PiercingAttackScroll));
            NotifyPropertyChanged(nameof(FacilitationScroll));
            NotifyPropertyChanged(nameof(DepthsFuryScroll));

            #endregion
            #region Pet

            NotifyPropertyChanged(nameof(CriticalDamagePet));
            NotifyPropertyChanged(nameof(AccuracyPet));
            NotifyPropertyChanged(nameof(AttackSpeedPet));
            NotifyPropertyChanged(nameof(PenetrationPet));
            NotifyPropertyChanged(nameof(SkillCooldownPet));
            NotifyPropertyChanged(nameof(RagePet));
            NotifyPropertyChanged(nameof(AttackStrengthPet));
            NotifyPropertyChanged(nameof(FacilitationPet));

            #endregion

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

            //NotifyPropertyChanged(nameof(AxeSelected));
            //NotifyPropertyChanged(nameof(MaceSelected));
            //NotifyPropertyChanged(nameof(SpearSelected));
            //NotifyPropertyChanged(nameof(StaffSelected));
            //NotifyPropertyChanged(nameof(SwordSelected));
            AxeSelected = DataSet.AxeSelected;
            MaceSelected = DataSet.MaceSelected;
            SpearSelected = DataSet.SpearSelected;
            StaffSelected = DataSet.StaffSelected;
            SwordSelected = DataSet.SwordSelected;

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
            NotifyPropertyChanged(nameof(GodsAidLuna));

            NotifyPropertyChanged(nameof(PairingTalentAlmahadActive));
            NotifyPropertyChanged(nameof(RoarTalentAlmahadActive));
            NotifyPropertyChanged(nameof(PredatoryBondTalentAlmahadActive));


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


            MoonTouchActive = DataSet.MoonTouchActive;
            //MoonTouchOpacity = changeOpacity(MoonTouchActive);
            BeastAwakeningActive = DataSet.BeastAwakeningActive;
            OrderToAttackActive = DataSet.OrderToAttackActive;
            HealingActive = DataSet.HealingActive;
            ChainLightningActive = DataSet.ChainLightningActive;
            BestialRampageActive = DataSet.BestialRampageActive;
            AuraOfTheForestActive = DataSet.AuraOfTheForestActive;
            MoonlightPermanentActive = DataSet.MoonlightPermanentActive;
            MoonlightNonPermanentActive = DataSet.MoonlightNonPermanentActive;

            #endregion
            #region Свойства, зависимые от изменений
            // зависимые от изменений - в set присутствует какая-либо логика кроме калка и обновления

            //PercentMagicalDD = DataSet.PercentMagicalDD;
            //PercentPhysicalDD = DataSet.PercentPhysicalDD;

            SelectedCastle = DataSet.SelectedCastle;
            SelectedCastleStart = DataSet.SelectedCastleStart;
            //NumberCastle = DataSet.NumberCastle;
            IsUsingBlessingOfTheMoonOnLuna = DataSet.IsUsingBlessingOfTheMoonOnLuna;
            CrushingWillActive = DataSet.CrushingWill;
            IrreversibleAngerActive = DataSet.IrreversibleAnger;
            #endregion

            //NotifyPropertyChanged(nameof(OverLimitClosed));
            NotifyPropertyChanged(nameof(AuraTalentAbuse));

            Calculate();
        }
        private const string FILE_SAVE = "saves.json";


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
                    CalcStats();

                    CalcPercents();
                    #region Вычисление чистой и итоговой силы персонажа
                    double coefRage = FormulaCoefficientOfRage();

                    int pureMagicalDD = (int)(magicdd / percentMagicalDDStart.ConvertToCoefficient());
                    int purePhysicalDD = (int)(physdd / percentPhysicalDDStart.ConvertToCoefficient());

                    if (HarmoniousPowerStartModifierActive)
                    {
                        pureMagicalDD = (int)(pureMagicalDD / harmoniousPowerMDD.ConvertToCoefficient());
                        purePhysicalDD = (int)(purePhysicalDD / harmoniousPowerPDD.ConvertToCoefficient());

                    }

                    magicdd = (int)(pureMagicalDD * (coefficientTriton * MermanDuration() + coefRage) + pureMagicalDD * percentMagicalDD.ConvertToCoefficient());
                    physdd = (int)((purePhysicalDD * coefRage + purePhysicalDD * percentPhysicalDD.ConvertToCoefficient()));

                    if (HasTalentHarmoniousPower)
                    {
                        magicdd = (int)(magicdd * harmoniousPowerMDD.ConvertToCoefficient());
                        physdd = (int)(physdd * harmoniousPowerPDD.ConvertToCoefficient());

                    }
                    #endregion
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

                    int resultDD = 0;
                    int resultDDLuna = 0;
                    int resultDDHero = 0;

                    if (AttackActive)
                    {
                        resultDDHero += dpmAttack;
                        DpmAttack = dpmAttack;
                    }
                    else DpmAttack = 0;

                    if (MoonTouchActive)
                    {
                        resultDDHero += dpmMoonTouch;
                        DpmMoonTouch = dpmMoonTouch;
                    }
                    else DpmMoonTouch = 0;

                    if (BeastAwakeningActive)
                    {
                        if (BestialRampageActive)
                        {
                            resultDDLuna += (int)(dpmBeastAwakening * TimeWithoutBestialRampage()
                                            + dpmBestialRampage * TimeBestialRampage());
                            DpmBestialRampage = (int)(dpmBestialRampage * TimeBestialRampage());
                            DpmBeastAwakening = (int)(dpmBeastAwakening * TimeWithoutBestialRampage());
                        }
                        else
                        {
                            resultDDLuna += dpmBeastAwakening;
                            DpmBeastAwakening = dpmBeastAwakening;
                            DpmBestialRampage = 0;
                        }
                        if (OrderToAttackActive)
                        {
                            resultDDLuna += dpmOrderToAttack;
                            DpmOrderToAttack = dpmOrderToAttack;
                        }
                        else DpmOrderToAttack = 0;

                        if (HasTalantSymbiosis)
                        {
                            resultDDHero += dpmSymbiosisHero;
                            resultDDLuna += dpmSymbiosisLuna;
                            DpmSymbiosisLuna = dpmSymbiosisLuna;
                            DpmSymbiosisHero = dpmSymbiosisHero;
                        }
                        else
                        {
                            DpmSymbiosisLuna = 0;
                            DpmSymbiosisHero = 0;
                        }
                    }
                    else
                    {
                        DpmBeastAwakening = 0;
                        DpmBestialRampage = 0;
                        DpmOrderToAttack = 0;
                        DpmSymbiosisLuna = 0;
                        DpmSymbiosisHero = 0;
                    }

                    if (ChainLightningActive)
                    {
                        resultDDHero += dpmChainLightning;
                        DpmChainLightning = dpmChainLightning;
                    }
                    else DpmChainLightning = 0;

                    if (AuraOfTheForestActive)
                    {
                        resultDDLuna += dpmAuraOfTheForestLuna;
                        resultDDHero += dpmAuraOfTheForestHero;
                        DpmAuraOfTheForestHero = dpmAuraOfTheForestHero;
                        DpmAuraOfTheForestLuna = dpmAuraOfTheForestLuna;
                    }
                    else
                    {
                        DpmAuraOfTheForestHero = 0;
                        DpmAuraOfTheForestLuna = 0;
                    }

                    resultDDHero += dpmMoonlight;
                    DpmMoonLight = dpmMoonlight;

                    //resultDDHero = (int)(resultDDHero * sacredShieldHeroCoef());
                    //resultDDLuna = (int)(resultDDLuna * sacredShieldLunaCoef());

                    resultDD = resultDDHero + resultDDLuna;
                    //OutDD = resultDD.ToString();
                    DataSet.ResultDD = resultDD;
                    NotifyPropertyChanged(nameof(OutDD));
                    getRecommendCommand?.RaiseCanExecuteChanged();
                    OutDDHero = resultDDHero.ToString();
                    OutDDLuna = resultDDLuna.ToString();
                }
                //else OutDD = "Ошибка данных";
            }
            //else OutDD = "Ошибка данных";
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
            CalcSkillPower();
            CalcDepthsFury();
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
            double coeffsStart = coefficientPredatoryDeliriumTalant
                * FormulaCoefficientOfAttackStrength()
                * FormulaCoefficientOfPiercingAttack();

            double coeffsFinal = FormulaCoefficientOfCriticalHitHeroForAutoattack()
                * FormulaCoefficientOfAccuracy()
                * coefficientBPDungeon()
                * sacredShieldHeroCoef();

            int result = (int)(Attack.Formula(magedd, physdd)
                * coeffsStart);
            OutAttackDD = result.ToString();
            result = (int)(result / AttackDelay() * 60);
            OutAttackDPM = result.ToString();
            // тут не умножается на пробив потому что формула пронзы в себе содержит коэффициент пробива просто с учетом пронзы
            // так что не надо дополнительно еще на пробив умножать
            result = (int)(result
                * coeffsFinal);
            return result;
        }
        private double MoonTouchCooldown()
        {
            double result = ((Moon_Touch.BaseTimeCooldown / SkillCooldownFinal.ConvertToCoefficient()) + TIME_CAST);
            return result;
        }
        public int CalcMoonTouch(int magedd)
        {
            double coeffsStart = FormulaCoefficientSkillPower()
                * coefficientBestialRageTalant
                * coefficientPredatoryDeliriumTalant
                * coefficientMomentOfPowerTalant
                * FormulaCoefficientOfPenetration();

            double coeffsFinal = FormulaCoefficientOfCriticalHitForSkill()
                * FormulaCoefficientOfAccuracy()
                * coefficientBPDungeon()
                * sacredShieldHeroCoef();

            int result = (int)(Moon_Touch.Formula(magedd) * coeffsStart);
            OutMoonTouchDD = result.ToString();
          
            result = (int)(result * 60 / MoonTouchCooldown());  
            result = (int)(result * coeffsFinal);
            OutMoonTouchDPM = result.ToString();
            
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
            double coeffsStart = FormulaCoefficientSkillPower()
                * coefficientBestialRageTalant
                * coefficientPredatoryDeliriumTalant
                * coefficientMomentOfPowerTalant
                * FormulaCoefficientOfPenetration();

            double coeffsFinal = FormulaCoefficientOfCriticalHitForSkill()
                * FormulaCoefficientOfAccuracy()
                * coefficientBPDungeon()
                * sacredShieldHeroCoef();

            int result = (int)(Chain_Lightning.Formula(magedd, physdd)
                * coeffsStart);

            OutChainLightningDD = result.ToString();
            result = (int)(result * 60 / ChainLightningCooldown() * LegendaryCoefficientChainLightning());
            OutChainLightningDPM = result.ToString();
            result = (int)(result * coeffsFinal);
            return result;
        }
        public int CalcBeastAwakening(int magedd, int physdd)
        {
            double coeffsStart = FormulaCoefficientOfAttackStrengthLuna()
                * FormulaCoefficientOfPiercingAttackLuna();

            double coeffsFinal = CoefficientOfMoonTouchForLuna()
                * FormulaCoefficientOfCriticalHitLuna()
                * FormulaCoefficientOfAccuracyLuna()
                * sacredShieldLunaCoef();

            int result = (int)(Beast_Awakening.Formula(magedd, physdd)
                * coeffsStart);
            OutBeastAwakeningDD = result.ToString();
            result = (int)(result * 60 / (Beast_Awakening.BaseDelay * ((100 - (GodsAidLuna ? ModifiersDamage.GODS_AID_ATTACK_SPEED : 0)) / 100.0)));
            OutBeastAwakeningDPM = result.ToString();
            result = (int)(result * coeffsFinal);
            return result;
        }
        public double BestialRampageCooldown()
        {
            double result = (Bestial_Rampage.BaseTimeCooldown / SkillCooldownFinal.ConvertToCoefficient()) + TIME_CAST;

            return result;
        }
        public double TimeBestialRampage()
        {
            double result = (Bestial_Rampage.TimeActive * (FacilitationLuna.ConvertToCoefficient()) / BestialRampageCooldown());
            if (result < 0)
            {
                return 0;
            }
            if (result > 1) return 1;

            return result;
        }
        public double TimeWithoutBestialRampage()
        {
            double result = (BestialRampageCooldown() - Bestial_Rampage.TimeActive * FacilitationLuna.ConvertToCoefficient()) / BestialRampageCooldown();
            if (result < 0)
            {
                return 0;
            }
            if (result > 1) return 1;
            return result;
        }
        public double AttackDelayLunaWithBestialRampage()
        {
            double result = (Bestial_Rampage.Luna.BaseDelay * ((100 - Bestial_Rampage.IncreaseAttackSpeed - (GodsAidLuna ? ModifiersDamage.GODS_AID_ATTACK_SPEED : 0)) / 100));
            return result;
        }
        public int CalcBestialRampage(int magedd, int physdd)
        {
            double coeffsStart = FormulaCoefficientOfAttackStrengthLuna()
                * FormulaCoefficientOfPiercingAttackLuna();

            double coeffsFinal = CoefficientOfMoonTouchForLuna()
                * FormulaCoefficientOfCriticalHitLuna()
                * FormulaCoefficientOfAccuracyLuna()
                * sacredShieldLunaCoef();

            int result = (int)(Bestial_Rampage.Formula(magedd, physdd)
                * coeffsStart);

            OutBestialRampageDD = result.ToString();
            double increaseAttackSpeed = (100 - (Bestial_Rampage.IncreaseAttackSpeed + (GodsAidLuna ? ModifiersDamage.GODS_AID_ATTACK_SPEED : 0))) / 100;
            result = (int)(result * 60 / (Bestial_Rampage.Luna.BaseDelay * increaseAttackSpeed));
            OutBestialRampageDPM = result.ToString();
            result = (int)(result
                * coeffsFinal);
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
            double coefGrandeurOfTheLotus = 0.75;

            double coeffsLunaStart = FormulaCoefficientOfPenetrationLuna();
            double coeffsHeroStart = FormulaCoefficientSkillPower()
                * coefficientBestialRageTalant
                * coefficientPredatoryDeliriumTalant
                * coefficientMomentOfPowerTalant
                * FormulaCoefficientOfPenetration();

            double coeffsLunaFinal = CoefficientOfMoonTouchForLuna()
                        * FormulaCoefficientOfCriticalHitLuna()
                        * FormulaCoefficientOfAccuracyLuna()
                        * sacredShieldLunaCoef();
            double coeffsHeroFinal = FormulaCoefficientOfCriticalHitForSkill()
                * FormulaCoefficientOfAccuracy()
                * coefficientBPDungeon()
                * sacredShieldHeroCoef();

            int countHitByHero = (int)(AuraOfTheForest.TimeActive * FacilitationFinal.ConvertToCoefficient() / AuraOfTheForest.Delay);
            int countHitByLuna = (int)(AuraOfTheForest.TimeActive * FacilitationLuna.ConvertToCoefficient() / AuraOfTheForest.Delay);
            int LunaAura = (int)(AuraOfTheForest.Formula(magedd)
                * coeffsLunaStart);
            int HeroesAura = (int)(AuraOfTheForest.Formula(magedd)
                * coeffsHeroStart);
            double realCooldown = AuraOfTheForest.BaseTimeCooldown / SkillCooldownFinal.ConvertToCoefficient() + TIME_CAST;
            if (HasTalantGrandeurOfTheLotus)
            {
                if (BeastAwakeningActive)
                {
                    LunaAura = (int)(LunaAura * (AuraTalentAbuse ? 1 : coefGrandeurOfTheLotus));
                    OutAuraOfTheForestLunaDD = LunaAura.ToString();
                    LunaAura = (int)(LunaAura * 60 / AuraOfTheForestCooldown() * countHitByLuna);
                    OutAuraOfTheForestLunaDPM = LunaAura.ToString();
                    // ИТОГОВЫЙ ДД АУРЫ ЛЕСА ЛУНЫ НА ВСЕ КЭФЫ
                    result[SourcesDamage.Luna] += (int)(LunaAura
                        * coeffsLunaFinal);
                }
                else
                {
                    OutAuraOfTheForestLunaDD = "0";
                    OutAuraOfTheForestLunaDPM = "0";
                }
                HeroesAura = (int)(HeroesAura * (AuraTalentAbuse ? 1 : coefGrandeurOfTheLotus));
                OutAuraOfTheForestHeroDD = HeroesAura.ToString();
                HeroesAura = (int)(HeroesAura * 60 / AuraOfTheForestCooldown() * countHitByHero);
                OutAuraOfTheForestHeroDPM = HeroesAura.ToString();
                result[SourcesDamage.Hero] += (int)(HeroesAura
                    * coeffsHeroFinal);
                return result;
            }
            if (BeastAwakeningActive)
            {
                OutAuraOfTheForestLunaDD = LunaAura.ToString();
                LunaAura = (int)(LunaAura * 60 / AuraOfTheForestCooldown() * countHitByLuna);
                OutAuraOfTheForestLunaDPM = LunaAura.ToString();
                OutAuraOfTheForestHeroDPM = "0";
                OutAuraOfTheForestHeroDD = "0";
                // ИТОГОВЫЙ ДД АУРЫ ЛЕСА ЛУНЫ НА ВСЕ КЭФЫ
                result[SourcesDamage.Luna] += (int)(LunaAura
                    * coeffsLunaFinal);
                return result;
            }
            OutAuraOfTheForestHeroDD = HeroesAura.ToString();
            HeroesAura = (int)(HeroesAura * 60 / AuraOfTheForestCooldown() * countHitByHero);
            OutAuraOfTheForestHeroDPM = HeroesAura.ToString();
            OutAuraOfTheForestLunaDD = "0";
            OutAuraOfTheForestLunaDPM = "0";
            result[SourcesDamage.Hero] += (int)(HeroesAura
                * coeffsHeroFinal);
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

            double coeffsStart = FormulaCoefficientSkillPower()
                    * coefficientBestialRageTalant
                    * coefficientPredatoryDeliriumTalant
                    * coefficientLongDeathTalant
                    * FormulaCoefficientOfPenetration();

            double coeffsFinal = FormulaCoefficientOfCriticalHitForSkill() * coefficientBPDungeon() * sacredShieldHeroCoef();

            if (MoonlightPermanentActive)
            {
                int permanentDD = (int)(3 * Moonlight.Formula((int)(pureMagicalDD * coefficientTriton + magicaldd))
                    * coeffsStart);

                OutMoonlightPermanentDD = permanentDD.ToString();
                int permanentDPM = permanentDD * 30;
                OutMoonlightPermanentDPM = permanentDPM.ToString();
                result += permanentDPM;
            }
            if (MoonlightNonPermanentActive)
            {
                //double realCooldown = Moonlight.BaseTimeCooldown / SkillCooldownFinal.ConvertToCoefficient() + TIME_CAST;

                int nonPermanentDD = (int)(Moonlight.Formula(magicaldd)
                    * coeffsStart);

                OutMoonlightNonPermanentDD = nonPermanentDD.ToString();
                int nonPermanentDPM = (int)((nonPermanentDD * 4) / MoonLightCooldown() * 60 * LegendaryCoefficientMoonLight());
                OutMoonlightNonPermanentDPM = nonPermanentDPM.ToString();
                result += (int)(nonPermanentDPM * FormulaCoefficientOfAccuracy());
            }

            result = (int)(result * coeffsFinal);

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

            double coeffsStart = FormulaCoefficientOfAttackStrengthLuna()
                * FormulaCoefficientOfPiercingAttackLuna();

            double coeffsFinal = CoefficientOfMoonTouchForLuna()
                * FormulaCoefficientOfCriticalHitLuna()
                * FormulaCoefficientOfAccuracyLuna()
                * sacredShieldLunaCoef();

            result = (int)(OrderToAttack.Formula(magedd, physdd)
                * coeffsStart);

            OutOrderToAttackDD = result.ToString();

            result = (int)(result * 60 / OrderToAttackCooldown());
            if (BestialRampageActive)
                result = (int)(result * (1 + (Bestial_Rampage.IncreaseDD - 1) * TimeBestialRampage()));
            OutOrderToAttackDPM = result.ToString();

            result = (int)(result * coeffsFinal);

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

            double coeffsForLunaStart = FormulaCoefficientOfCriticalHitHeroForAutoattack()
                    * FormulaCoefficientOfPiercingAttack()
                    * FormulaCoefficientOfAccuracy()
                    * FormulaCoefficientOfAttackStrength()
                    * sacredShieldHeroCoef();
            double coeffsForHeroStart = FormulaCoefficientOfCriticalHitLuna()
                    * FormulaCoefficientOfPiercingAttackLuna()
                    * FormulaCoefficientOfAccuracyLuna()
                    * FormulaCoefficientOfAttackStrengthLuna()
                    * sacredShieldLunaCoef();


            double coeffsForLunaFinal = coefficientPredatoryDeliriumTalant
                * CoefficientOfMoonTouchForLuna()
                * coefficientBPDungeon()
                * FormulaCoefficientOfPiercingAttackLuna()
                * sacredShieldLunaCoef();
            double coeffsForHeroFinal = coefficientPredatoryDeliriumTalant
                * CoefficientOfMoonTouchForLuna()
                * coefficientBPDungeon()
                * FormulaCoefficientOfPiercingAttack()
                * sacredShieldHeroCoef();

            double Tp = AttackDelay();
            double Tl = Beast_Awakening.BaseDelay * (1 - (GodsAidLuna ? ModifiersDamage.GODS_AID_ATTACK_SPEED : 0) / 100);
            double T = Math.Max(Tp, Tl);
            double DpmHero = 0.15 * 60 / T * (
                    Beast_Awakening.Formula(magedd, physdd) * coeffsForHeroStart);
            double DpmLuna = 0.15 * 60 / T * (
                    Attack.Formula(magedd, physdd) * coeffsForLunaStart);

            if (BestialRampageActive)
            {
                double Tbr = AttackDelayLunaWithBestialRampage();
                T = Math.Max(Tp, Tbr);
                double DpmBestialRampageHero = 0.15 * 60 / T * (
                    Bestial_Rampage.Formula(magedd, physdd) * coeffsForHeroStart);
                double DpmBestialRampageLuna = 0.15 * 60 / T * (
                    Attack.Formula(magedd, physdd) * coeffsForLunaStart);

                result[SourcesDamage.Hero] = (int)(
                    (DpmHero * TimeWithoutBestialRampage() + DpmBestialRampageHero * TimeBestialRampage())
                    * coeffsForHeroFinal);
                result[SourcesDamage.Luna] = (int)(
                    (DpmLuna * TimeWithoutBestialRampage() + DpmBestialRampageLuna * TimeBestialRampage())
                    * coeffsForLunaFinal);


                OutSymbiosisDPM = (result[SourcesDamage.Hero] + result[SourcesDamage.Luna]).ToString();

                return result;
            }
            result[SourcesDamage.Hero] = (int)(DpmHero * coeffsForHeroFinal);

            result[SourcesDamage.Luna] = (int)(DpmLuna * coeffsForLunaFinal);

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
        /// Свойство связанное с полем на вьюхе расходники Эликсир "Перезарядка навыков"
        /// </summary>
        public double SkillCooldownPot
        {
            get => DataSet.SkillCooldownPot;
            set
            {
                DataSet.SkillCooldownPot = StatsLimit.CheckLimit(value, StatsLimit.MAX_SKILL_COOLDOWN);
                Calculate();
                NotifyPropertyChanged(nameof(SkillCooldownPot));
            }
        }
        /// <summary>
        /// Свойство связанное с полем на вьюхе расходники свиток "Перезарядка навыков"
        /// </summary>
        public double SkillCooldownScroll
        {
            get => DataSet.SkillCooldownScroll;
            set
            {
                DataSet.SkillCooldownScroll = StatsLimit.CheckLimit(value, StatsLimit.MAX_SKILL_COOLDOWN);
                Calculate();
                NotifyPropertyChanged(nameof(SkillCooldownScroll));
            }
        }
        /// <summary>
        /// Свойство связанное с полем на вьюхе расходники пет "Перезарядка навыков"
        /// </summary>
        public double SkillCooldownPet
        {
            get => DataSet.SkillCooldownPet;
            set
            {
                DataSet.SkillCooldownPet = StatsLimit.CheckLimit(value, StatsLimit.MAX_SKILL_COOLDOWN);
                Calculate();
                NotifyPropertyChanged(nameof(SkillCooldownPet));
            }
        }
        /// <summary>
        /// Метод для пересчета характеристики персонажа "Перезарядка навыков"
        /// </summary>
        private void CalcSkillCooldown()
        {
            SkillCooldownFinal = 0;
            SkillCooldownFinal += SkillCooldown;
            SkillCooldownFinal += SkillCooldownPot;
            SkillCooldownFinal += SkillCooldownScroll;
            SkillCooldownFinal += SkillCooldownPet;
            if (CastleSwordActive) SkillCooldownFinal += 5;
            if (DoubleConcentrationActive)
                SkillCooldownFinal += DoubleConcentration.AddSkillCooldown();
            if (CastleStartModifierActive) SkillCooldownFinal -= 5;

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
        /// Свойство связанное с полем на вьюхе расходники Эликсир "Скорость атаки"
        /// </summary>
        public double AttackSpeedPot
        {
            get => DataSet.AttackSpeedPot;
            set
            {
                DataSet.AttackSpeedPot = StatsLimit.CheckLimit(value, StatsLimit.MAX_ATTACK_SPEED); ;
                Calculate(); NotifyPropertyChanged(nameof(AttackSpeedPot));
            }
        }
        /// <summary>
        /// Свойство связанное с полем на вьюхе расходники свиток "Скорость атаки"
        /// </summary>
        public double AttackSpeedScroll
        {
            get => DataSet.AttackSpeedScroll;
            set
            {
                DataSet.AttackSpeedScroll = StatsLimit.CheckLimit(value, StatsLimit.MAX_ATTACK_SPEED); ;
                Calculate(); NotifyPropertyChanged(nameof(AttackSpeedScroll));
            }
        }
        /// <summary>
        /// Свойство связанное с полем на вьюхе расходники пет "Скорость атаки"
        /// </summary>
        public double AttackSpeedPet
        {
            get => DataSet.AttackSpeedPet;
            set
            {
                DataSet.AttackSpeedPet = StatsLimit.CheckLimit(value, StatsLimit.MAX_ATTACK_SPEED); ;
                Calculate(); NotifyPropertyChanged(nameof(AttackSpeedPet));
            }
        }
        /// <summary>
        /// Метод для пересчета характеристики персонажа "Скорость атаки"
        /// </summary>
        private void CalcAttackSpeed()
        {
            AttackSpeedFinal = 0;
            AttackSpeedFinal += AttackSpeed;
            AttackSpeedFinal += AttackSpeedPot;
            AttackSpeedFinal += AttackSpeedScroll;
            AttackSpeedFinal += AttackSpeedPet;
            if (CastleSwordActive) AttackSpeedFinal += 5;
            if (DoubleConcentrationActive)
                AttackSpeedFinal += DoubleConcentration.AddAttackSpeed();
            if (GodsAid) AttackSpeedFinal += 12;
            if (CastleStartModifierActive) AttackSpeedFinal -= 5;
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
                //criticalHitLuna = value;
                criticalHitLuna = StatsLimit.CheckLimit(value, StatsLimit.MAX_CRITICAL_HIT);
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
        /// Свойство связанное с полем на вьюхе расходников, а именно прибавка "Критического удара" от эликсира
        /// </summary>
        public double CriticalHitPot
        {
            get => DataSet.CriticalHitPot;
            set
            {
                DataSet.CriticalHitPot = StatsLimit.CheckLimit(value, StatsLimit.MAX_CRITICAL_HIT);
                Calculate(); NotifyPropertyChanged(nameof(CriticalHitPot));
            }
        }
        /// <summary>
        /// Свойство связанное с полем на вьюхе расходников, а именно прибавка "Критического удара" от свитка
        /// </summary>
        public double CriticalHitScroll
        {
            get => DataSet.CriticalHitScroll;
            set
            {
                DataSet.CriticalHitScroll = StatsLimit.CheckLimit(value, StatsLimit.MAX_CRITICAL_HIT);
                Calculate(); NotifyPropertyChanged(nameof(CriticalHitScroll));
            }
        }
        /// <summary>
        /// Метод для пересчета характеристики персонажа "Критический урон"
        /// </summary>
        private void CalcCriticalHit()
        {
            CriticalHitHeroFinal = 0;
            CriticalHitHeroFinal += CriticalHit;
            CriticalHitHeroFinal += CriticalHitPot;
            CriticalHitHeroFinal += CriticalHitScroll;
            if (CastleSwordActive) CriticalHitHeroFinal += 5;
            if (BlessingOfTheMoonActive) CriticalHitHeroFinal += BlessingOfTheMoon.AdditionCriticalHit;
            if (CrushingWillActive) CriticalHitHeroFinal += MermanModifiers.CRUSHING_WILL_ADDITIONAL_CRITICAL_HIT;
            if (GodsAid) CriticalHitHeroFinal += 10;
            if (CastleStartModifierActive) CriticalHitHeroFinal -= 5;
            CriticalHitHeroFinal = Math.Max(CriticalHitHeroFinal, 0);
            criticalHit = CriticalHitHeroFinal;
            //if (OverLimitClosed) 
            criticalHit = StatsLimit.CheckLimit(criticalHit, StatsLimit.MAX_CRITICAL_HIT_HERO);
            IsUsingBlessingOfTheMoonOnLuna = IsUsingBlessingOfTheMoonOnLuna;
            if (CriticalHitHeroFinal > maxCriticalHitHero) CriticalHitHeroFinal = maxCriticalHitHero;
            //if (OverLimitClosed)
            CriticalHitLuna = StatsLimit.CheckLimit(CriticalHitLuna, StatsLimit.MAX_CRITICAL_HIT_HERO);
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
        private double criticalDamageLuna = 0;
        /// <summary>
        /// Итоговое значение характеристики "Критический урон" Луны с учетом всех скиллов и бафов
        /// </summary>
        public double CriticalDamageLuna
        {
            get => criticalDamageLuna;
            set
            {
                //criticalHitLuna = value;
                criticalDamageLuna = value;
                NotifyPropertyChanged(nameof(CriticalDamageLuna));
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
        /// Свойство связанное с полем на вьюхе расходников, а именно прибавка "Критического урона" от эликсира
        /// </summary>
        public double CriticalDamagePot
        {
            get => DataSet.CriticalDamagePot;
            set
            {
                DataSet.CriticalDamagePot = StatsLimit.CheckLimit(value, StatsLimit.MAX_CRITICAL_DAMAGE);
                Calculate(); NotifyPropertyChanged(nameof(CriticalDamagePot));
            }
        }
        /// <summary>
        /// Свойство связанное с полем на вьюхе расходников, а именно прибавка "Критического урона" от свитка
        /// </summary>
        public double CriticalDamageScroll
        {
            get => DataSet.CriticalDamageScroll;
            set
            {
                DataSet.CriticalDamageScroll = StatsLimit.CheckLimit(value, StatsLimit.MAX_CRITICAL_DAMAGE);
                Calculate(); NotifyPropertyChanged(nameof(CriticalDamageScroll));
            }
        }
        /// <summary>
        /// Свойство связанное с полем на вьюхе расходников, а именно прибавка "Критического урона" от пета
        /// </summary>
        public double CriticalDamagePet
        {
            get => DataSet.CriticalDamagePet;
            set
            {
                DataSet.CriticalDamagePet = StatsLimit.CheckLimit(value, StatsLimit.MAX_CRITICAL_DAMAGE);
                Calculate(); NotifyPropertyChanged(nameof(CriticalDamagePet));
            }
        }
        /// <summary>
        /// Метод пересчета характеристики персонажа "критический урон"
        /// </summary>
        private void CalcCriticalDamage()
        {
            CriticalDamageFinal = 0;
            CriticalDamageFinal += CriticalDamage;
            CriticalDamageFinal += CriticalDamagePot;
            CriticalDamageFinal += CriticalDamageScroll;
            CriticalDamageFinal += CriticalDamagePet;
            if (DoubleConcentrationActive)
                CriticalDamageFinal += DoubleConcentration.AdditionCriticalDamage;
            // Крылья на ловчего
            if (GodsAid) CriticalDamageFinal += ModifiersDamage.GODS_AID_CRITICAL_DAMAGE;
            if (RoarTalentAlmahadActive) CriticalDamageFinal += ModifiersDamage.ROAR_TALENT_CRITICAL_DAMAGE;
            CriticalDamageLuna = CriticalDamageFinal;
            if (CrushingWillActive) CriticalDamageLuna += MermanModifiers.CRUSHING_WILL_ADDITIONAL_CRITICAL_DAMAGE;
            // Крылья на Луну
            if (GodsAidLuna) CriticalDamageLuna += ModifiersDamage.GODS_AID_CRITICAL_DAMAGE;

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
        /// Свойство связанное с полем на вьюхе расходники Эликсир "Пробивная способность"
        /// </summary>
        public double PenetrationPot
        {
            get => DataSet.PenetrationPot;
            set
            {
                DataSet.PenetrationPot = StatsLimit.CheckLimit(value, StatsLimit.MAX_PENETRATION);
                Calculate(); NotifyPropertyChanged(nameof(PenetrationPot));
            }
        }
        /// <summary>
        /// Свойство связанное с полем на вьюхе расходники Свиток "Пробивная способность"
        /// </summary>
        public double PenetrationScroll
        {
            get => DataSet.PenetrationScroll;
            set
            {
                DataSet.PenetrationScroll = StatsLimit.CheckLimit(value, StatsLimit.MAX_PENETRATION);
                Calculate(); NotifyPropertyChanged(nameof(PenetrationScroll));
            }
        }
        /// <summary>
        /// Свойство связанное с полем на вьюхе расходники пет "Пробивная способность"
        /// </summary>
        public double PenetrationPet
        {
            get => DataSet.PenetrationPet;
            set
            {
                DataSet.PenetrationPet = StatsLimit.CheckLimit(value, StatsLimit.MAX_PENETRATION);
                Calculate(); NotifyPropertyChanged(nameof(PenetrationPet));
            }
        }
        /// <summary>
        /// Метод для пересчета характеристики персонажа "Пробивная способность"
        /// </summary>
        private void CalcPenetration()
        {
            PenetrationHeroFinal = 0;
            PenetrationHeroFinal += Penetration;
            PenetrationHeroFinal += PenetrationPot;
            PenetrationHeroFinal += PenetrationScroll;
            PenetrationHeroFinal += PenetrationPet;
            if (CastleSwordActive) PenetrationHeroFinal += 5;
            if (BlessingOfTheMoonActive) PenetrationHeroFinal += BlessingOfTheMoon.AdditionPenetration;
            if (IrreversibleAngerActive) PenetrationHeroFinal += MermanModifiers.IRREVERSIBLE_ANGER_ADDITIONAL_PENETRATION;
            if (CastleStartModifierActive) PenetrationHeroFinal -= 5;
            PenetrationHeroFinal = Math.Max(PenetrationHeroFinal, 0);
            penetration = PenetrationHeroFinal;
            //if (OverLimitClosed)
            penetration = StatsLimit.CheckLimit(penetration, maxPenetrationHero);
            IsUsingBlessingOfTheMoonOnLuna = IsUsingBlessingOfTheMoonOnLuna;
            if (PenetrationHeroFinal > maxPenetrationHero) PenetrationHeroFinal = maxPenetrationHero;
            //if (OverLimitClosed) 
            PenetrationLuna = StatsLimit.CheckLimit(PenetrationLuna, maxPenetrationHero);
        }
        #endregion
        #region Точность
        //private double maxAccuracyHero = 50;
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
        /// Свойство связанное с полем на вьюхе расходники Эликсир "Точность"
        /// </summary>
        public double AccuracyPot
        {
            get => DataSet.AccuracyPot;
            set
            {
                DataSet.AccuracyPot = StatsLimit.CheckLimit(value, StatsLimit.MAX_ACCURACY);

                Calculate(); NotifyPropertyChanged(nameof(AccuracyPot));
            }
        }
        /// <summary>
        /// Свойство связанное с полем на вьюхе расходники свиток "Точность"
        /// </summary>
        public double AccuracyScroll
        {
            get => DataSet.AccuracyScroll;
            set
            {
                DataSet.AccuracyScroll = StatsLimit.CheckLimit(value, StatsLimit.MAX_ACCURACY);

                Calculate(); NotifyPropertyChanged(nameof(AccuracyScroll));
            }
        }
        /// <summary>
        /// Свойство связанное с полем на вьюхе расходники пет "Точность"
        /// </summary>
        public double AccuracyPet
        {
            get => DataSet.AccuracyPet;
            set
            {
                DataSet.AccuracyPet = StatsLimit.CheckLimit(value, StatsLimit.MAX_ACCURACY);

                Calculate(); NotifyPropertyChanged(nameof(AccuracyPet));
            }
        }
        /// <summary>
        /// Метод для пересчета характеристики персонажа "Точность"
        /// </summary>
        private void CalcAccuracy()
        {
            AccuracyHeroFinal = 0;
            AccuracyHeroFinal += Accuracy;
            AccuracyHeroFinal += AccuracyPot;
            AccuracyHeroFinal += AccuracyScroll;
            AccuracyHeroFinal += AccuracyPet;
            if (CastleSwordActive) AccuracyHeroFinal += 5;
            if (IrreversibleAngerActive) AccuracyHeroFinal += MermanModifiers.IRREVERSIBLE_ANGER_ADDITIONAL_ACCURACY;
            if (CastleStartModifierActive) AccuracyHeroFinal -= 5;
            AccuracyHeroFinal = Math.Max(AccuracyHeroFinal, 0);
            double finalAcc = StatsLimit.CheckLimit(AccuracyHeroFinal, StatsLimit.MAX_ACCURACY_HERO);
            //if (OverLimitClosed) AccuracyLuna = finalAcc; else AccuracyLuna = AccuracyHeroFinal;
            AccuracyLuna = finalAcc;
            AccuracyHeroFinal = finalAcc;
            //if (OverLimitClosed) AccuracyLuna = StatsLimit.CheckLimit(AccuracyLuna, StatsLimit.MAX_ACCURACY_HERO);
        }
        #endregion
        #region Сила атаки
        private double attackStrengthLuna = 0;
        /// <summary>
        /// Итоговое значение характеристики "Пробивная способность" Луны с учетом всех скиллов и бафов
        /// </summary>
        public double AttackStrengthLuna
        {
            get => attackStrengthLuna;
            set
            {
                attackStrengthLuna = value;
                NotifyPropertyChanged(nameof(AttackStrengthLuna));
            }
        }
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
        /// Свойство связанное с полем на вьюхе расходники Эликсир "Сила атаки"
        /// </summary>
        public double AttackStrengthPot
        {
            get => DataSet.AttackStrengthPot;
            set
            {
                DataSet.AttackStrengthPot = StatsLimit.CheckLimit(value, StatsLimit.MAX_ATTACK_STRENGTH);
                Calculate(); NotifyPropertyChanged(nameof(AttackStrengthPot));
            }
        }
        /// <summary>
        /// Свойство связанное с полем на вьюхе расходники Свиток "Сила атаки"
        /// </summary>
        public double AttackStrengthScroll
        {
            get => DataSet.AttackStrengthScroll;
            set
            {
                DataSet.AttackStrengthScroll = StatsLimit.CheckLimit(value, StatsLimit.MAX_ATTACK_STRENGTH);
                Calculate(); NotifyPropertyChanged(nameof(AttackStrengthScroll));
            }
        }
        /// <summary>
        /// Свойство связанное с полем на вьюхе расходники Пет "Сила атаки"
        /// </summary>
        public double AttackStrengthPet
        {
            get => DataSet.AttackStrengthPet;
            set
            {
                DataSet.AttackStrengthPet = StatsLimit.CheckLimit(value, StatsLimit.MAX_ATTACK_STRENGTH);
                Calculate(); NotifyPropertyChanged(nameof(AttackStrengthPet));
            }
        }
        /// <summary>
        /// Метод для пересчета характеристики персонажа "Сила атаки"
        /// </summary>
        private void CalcAttackStrength()
        {
            AttackStrengthFinal = 0;
            AttackStrengthFinal += AttackStrength;
            AttackStrengthFinal += AttackStrengthPot;
            AttackStrengthFinal += AttackStrengthScroll;
            AttackStrengthFinal += AttackStrengthPet;
            AttackStrengthLuna = AttackStrengthFinal;
            AttackStrengthFinal = StatsLimit.CheckLimit(AttackStrengthFinal, StatsLimit.MAX_ATTACK_STRENGTH);
            if (PredatoryBondTalentAlmahadActive) AttackStrengthLuna += AttackStrengthFinal * ModifiersDamage.PREDATORY_BOND_ATTACK_STRENGTH_COEFFICIENT;
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
        /// Свойство связанное с полем на вьюхе расходники Эликсир "Пронзающая атака"
        /// </summary>
        public double PiercingAttackPot
        {
            get => DataSet.PiercingAttackPot;
            set
            {
                DataSet.PiercingAttackPot = StatsLimit.CheckLimit(value, StatsLimit.MAX_PIERCING_ATTACK);
                Calculate(); NotifyPropertyChanged(nameof(PiercingAttackPot));
            }
        }
        /// <summary>
        /// Свойство связанное с полем на вьюхе расходники Свиток "Пронзающая атака"
        /// </summary>
        public double PiercingAttackScroll
        {
            get => DataSet.PiercingAttackScroll;
            set
            {
                DataSet.PiercingAttackScroll = StatsLimit.CheckLimit(value, StatsLimit.MAX_PIERCING_ATTACK);
                Calculate(); NotifyPropertyChanged(nameof(PiercingAttackScroll));
            }
        }
        /// <summary>
        /// Метод для пересчета характеристики персонажа "Пронзающая атака"
        /// </summary>
        private void CalcPiercingAttack()
        {
            PiercingAttackFinal = 0;
            PiercingAttackFinal += PiercingAttack;
            PiercingAttackFinal += PiercingAttackPot;
            PiercingAttackFinal += PiercingAttackScroll;
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
        /// Свойство связанное с полем на вьюхе расходники Эликсир "Ярость"
        /// </summary>
        public double RagePot
        {
            get => DataSet.RagePot;
            set
            {
                DataSet.RagePot = StatsLimit.CheckLimit(value, StatsLimit.MAX_RAGE);
                Calculate(); NotifyPropertyChanged(nameof(RagePot));
            }
        }
        /// <summary>
        /// Свойство связанное с полем на вьюхе расходники Свиток "Ярость"
        /// </summary>
        public double RageScroll
        {
            get => DataSet.RageScroll;
            set
            {
                DataSet.RageScroll = StatsLimit.CheckLimit(value, StatsLimit.MAX_RAGE);
                Calculate(); NotifyPropertyChanged(nameof(RageScroll));
            }
        }
        /// <summary>
        /// Свойство связанное с полем на вьюхе расходники Пет "Ярость"
        /// </summary>
        public double RagePet
        {
            get => DataSet.RagePet;
            set
            {
                DataSet.RagePet = StatsLimit.CheckLimit(value, StatsLimit.MAX_RAGE);
                Calculate(); NotifyPropertyChanged(nameof(RagePet));
            }
        }
        /// <summary>
        /// Метод для пересчета характеристики персонажа "Ярость"
        /// </summary>
        private void CalcRage()
        {
            RageFinal = 0;
            RageFinal += Rage;
            RageFinal += RagePot;
            RageFinal += RageScroll;
            RageFinal += RagePet;
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
        /// Свойство связанное с полем на вьюхе расходники Эликсир "Содействие"
        /// </summary>
        public double FacilitationPot
        {
            get => DataSet.FacilitationPot;
            set
            {
                DataSet.FacilitationPot = StatsLimit.CheckLimit(value, 300);
                Calculate(); NotifyPropertyChanged(nameof(FacilitationPot));
            }
        }
        /// <summary>
        /// Свойство связанное с полем на вьюхе расходники Свиток "Содействие"
        /// </summary>
        public double FacilitationScroll
        {
            get => DataSet.FacilitationScroll;
            set
            {
                DataSet.FacilitationScroll = StatsLimit.CheckLimit(value, 300);
                Calculate(); NotifyPropertyChanged(nameof(FacilitationScroll));
            }
        }
        /// <summary>
        /// Свойство связанное с полем на вьюхе расходники пет "Содействие"
        /// </summary>
        public double FacilitationPet
        {
            get => DataSet.FacilitationPet;
            set
            {
                DataSet.FacilitationPet = StatsLimit.CheckLimit(value, 300);
                Calculate(); NotifyPropertyChanged(nameof(FacilitationPet));
            }
        }
        private double facilitationLuna = 0;
        public double FacilitationLuna
        {
            get => facilitationLuna;
            set
            {
                facilitationLuna = value;
                NotifyPropertyChanged(nameof(FacilitationLuna));
            }
        }
        /// <summary>
        /// Метод для пересчета характеристики персонажа "Содействие"
        /// </summary>
        private void CalcFacilitation()
        {
            FacilitationFinal = 0;
            FacilitationFinal += Facilitation;
            FacilitationFinal += FacilitationPot;
            FacilitationFinal += FacilitationScroll;
            FacilitationFinal += FacilitationPet;
            FacilitationLuna = FacilitationFinal;
            FacilitationFinal = StatsLimit.CheckLimit(FacilitationFinal, StatsLimit.MAX_FACILITATION);
        }
        #endregion
        #region Сила навыков
        private double skillPower = 0;
        public double SkillPowerFinal
        {
            get => skillPower;
            set
            {
                skillPower = value;
                NotifyPropertyChanged(nameof(SkillPowerFinal));
            }
        }
        /// <summary>
        /// Свойство связанное с полем на вьюхе "Сила навыков"
        /// </summary>
        public double SkillPower
        {
            get => DataSet.SkillPower;
            set
            {
                DataSet.SkillPower = StatsLimit.CheckLimit(value, StatsLimit.MAX_SKILL_POWER);
                Calculate(); NotifyPropertyChanged(nameof(SkillPower));
            }
        }
        /// <summary>
        /// Свойство связанное с полем на вьюхе расходники Эликсир "Сила навыков"
        /// </summary>
        public double SkillPowerPot
        {
            get => DataSet.SkillPowerPot;
            set
            {
                DataSet.SkillPowerPot = StatsLimit.CheckLimit(value, StatsLimit.MAX_SKILL_POWER);
                Calculate(); NotifyPropertyChanged(nameof(SkillPowerPot));
            }
        }
        /// <summary>
        /// Метод для пересчета характеристики персонажа "Содействие"
        /// </summary>
        private void CalcSkillPower()
        {
            SkillPowerFinal = 0;
            SkillPowerFinal += SkillPower;
            if (coefficientCastleStart != 0) SkillPowerFinal -= Math.Round((coefficientCastleStart - 1) * 100, 1);
            SkillPowerFinal = Math.Max(SkillPowerFinal, 0);
            SkillPowerFinal += SkillPowerPot;
            if (coefficientCastle != 0) SkillPowerFinal += Math.Round((coefficientCastle - 1) * 100, 1);
            SkillPowerFinal = StatsLimit.CheckLimit(SkillPowerFinal, StatsLimit.MAX_SKILL_POWER);
            //SkillPowerFinal = Math.Round(SkillPowerFinal, 1);
        }
        #endregion
        #region Гнев Глубин
        private double depthsFury = 0;
        public double DepthsFuryFinal
        {
            get => depthsFury;
            set
            {
                depthsFury = value;
                NotifyPropertyChanged(nameof(DepthsFuryFinal));
            }
        }
        /// <summary>
        /// Свойство связанное с полем на вьюхе "Гнев Глубин"
        /// </summary>
        public double DepthsFury
        {
            get => DataSet.DepthsFury;
            set
            {
                DataSet.DepthsFury = StatsLimit.CheckLimit(value, StatsLimit.MAX_DEPTH_FURY);
                Calculate(); NotifyPropertyChanged(nameof(DepthsFury));
            }
        }
        /// <summary>
        /// Свойство связанное с полем на вьюхе расходники свиток "Гнев Глубин"
        /// </summary>
        public double DepthsFuryScroll
        {
            get => DataSet.DepthsFuryScroll;
            set
            {
                DataSet.DepthsFuryScroll = StatsLimit.CheckLimit(value, StatsLimit.MAX_DEPTH_FURY);
                Calculate(); NotifyPropertyChanged(nameof(DepthsFuryScroll));
            }
        }
        /// <summary>
        /// Метод для пересчета характеристики персонажа "Гнев глубин"
        /// </summary>
        private void CalcDepthsFury()
        {
            DepthsFuryFinal = 0;
            DepthsFuryFinal += DepthsFury;
            DepthsFuryFinal += DepthsFuryScroll;
            DepthsFuryFinal = StatsLimit.CheckLimit(DepthsFuryFinal, StatsLimit.MAX_DEPTH_FURY);
        }
        #endregion
        #region Проценты дд

        #region базовые модификаторы дд
        #region гильдия
        //private bool guildDamageStartModifierActive = true;
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
        //private bool guildDamageModifierActive = true;
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
        //private bool talentDamageStartModifierActive = true;
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

        //private bool talentDamageModifierActive = true;
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


        private double CalcModifiersDamageJewelrySet(TypesDamage type)
        {
            double result = SelectedCloak.ToPercentInDictionary(type);
            result += SelectedAmulet.ToPercentInDictionary(type);
            result += SelectedBraceletL.ToPercentInDictionary(type);
            result += SelectedBraceletR.ToPercentInDictionary(type);
            result += SelectedRingL.ToPercentInDictionary(type);
            result += SelectedRingR.ToPercentInDictionary(type);
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
            if (PairingTalentAlmahadActive) percentMagicalDD += ModifiersDamage.PAIRING_TALENT_MAGICAL_DD;
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
            if (PairingTalentAlmahadActive) percentPhysicalDD += ModifiersDamage.PAIRING_TALENT_PHYSICAL_DD;

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
            get => DataSet.ResultDD.ToString();
            //set { outDD = value; NotifyPropertyChanged(nameof(OutDD)); }
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

        #region Активность кнопок
        private const double ACTIVE_OPACITY = 1;
        private const double NON_ACTIVE_OPACITY = 0.3;

        private double changeOpacity(bool flag)
        {
            return flag ? ACTIVE_OPACITY : NON_ACTIVE_OPACITY;
        }
        #region Базовые скиллы

        private double moonTouchOpacity = NON_ACTIVE_OPACITY;
        public double MoonTouchOpacity
        {
            get => moonTouchOpacity;
            private set
            {
                moonTouchOpacity = value;
                NotifyPropertyChanged(nameof(MoonTouchOpacity));
            }
        }
        private double beastAwakeningOpacity = NON_ACTIVE_OPACITY;
        public double BeastAwakeningOpacity
        {
            get => beastAwakeningOpacity;
            private set
            {
                beastAwakeningOpacity = value;
                NotifyPropertyChanged(nameof(BeastAwakeningOpacity));
            }
        }
        private double orderToAttackOpacity = NON_ACTIVE_OPACITY;
        public double OrderToAttackOpacity
        {
            get => orderToAttackOpacity;
            private set
            {
                orderToAttackOpacity = value;
                NotifyPropertyChanged(nameof(OrderToAttackOpacity));
            }
        }
        private double healingOpacity = NON_ACTIVE_OPACITY;
        public double HealingOpacity
        {
            get => healingOpacity;
            private set
            {
                healingOpacity = value;
                NotifyPropertyChanged(nameof(HealingOpacity));
            }
        }
        private double chainLightningOpacity = NON_ACTIVE_OPACITY;
        public double ChainLightningOpacity
        {
            get => chainLightningOpacity;
            private set
            {
                chainLightningOpacity = value;
                NotifyPropertyChanged(nameof(ChainLightningOpacity));
            }
        }

        #endregion
        #region Экспертные навыки

        private double bestialRampageOpacity = NON_ACTIVE_OPACITY;
        public double BestialRampageOpacity
        {
            get => bestialRampageOpacity;
            private set
            {
                bestialRampageOpacity = value;
                NotifyPropertyChanged(nameof(BestialRampageOpacity));
            }
        }
        private double auraOfTheForestOpacity = NON_ACTIVE_OPACITY;
        public double AuraOfTheForestOpacity
        {
            get => auraOfTheForestOpacity;
            private set
            {
                auraOfTheForestOpacity = value;
                NotifyPropertyChanged(nameof(AuraOfTheForestOpacity));
            }
        }
        private double moonlightPermanentOpacity = NON_ACTIVE_OPACITY;
        public double MoonlightPermanentOpacity
        {
            get => moonlightPermanentOpacity;
            private set
            {
                moonlightPermanentOpacity = value;
                NotifyPropertyChanged(nameof(MoonlightPermanentOpacity));
            }
        }
        private double moonlightNonPermanentOpacity = NON_ACTIVE_OPACITY;
        public double MoonlightNonPermanentOpacity
        {
            get => moonlightNonPermanentOpacity;
            private set
            {
                moonlightNonPermanentOpacity = value;
                NotifyPropertyChanged(nameof(MoonlightNonPermanentOpacity));
            }
        }


        #endregion
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
        private readonly RecommendationSystem _rs = new RecommendationSystem();
        private volatile bool _suppressNotifications = false;
        private bool _isRecommendationTestRunning = false;

        private class RecommendationRunResult
        {
            public int Dpm { get; set; }
            public long TimeMs { get; set; }
            public long EvalCallCount { get; set; }
            public double[] Solution { get; set; }
        }

        private RecommendationAlgorithm CurrentRecommendationAlgorithm
            => SelectedRecommendationAlgorithm == "MCTS" ? RecommendationAlgorithm.MCTS : RecommendationAlgorithm.DE;

        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Метод для вызова события PropertyChanged
        /// </summary>
        /// <param name="prop">Имя свойства, которое изменилось</param>
        public void NotifyPropertyChanged([CallerMemberName] string prop = "")
        {
            if (_suppressNotifications) return;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private void SetStatusDirect(string s)
        {
            status = s;
            System.Windows.Application.Current?.Dispatcher.BeginInvoke(
                new Action(() => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Status)))));
        }

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

            double result = -0.3 * s + 1;

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
            set { 
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

        public int LvlTalantBestialRage
        {
            get => DataSet.LvlTalantBestialRage;
            set
            {
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


        public int LvlTalantPredatoryDelirium
        {
            get => DataSet.LvlTalantPredatoryDelirium;
            set
            {
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

        #region Таланты альмахада

        public bool PairingTalentAlmahadActive
        {
            get => DataSet.PairingTalentAlmahadActive;
            set
            {
                DataSet.PairingTalentAlmahadActive = value;
                Calculate(); NotifyPropertyChanged(nameof(PairingTalentAlmahadActive));
            }
        }
        public bool RoarTalentAlmahadActive
        {
            get => DataSet.RoarTalentAlmahadActive;
            set
            {
                DataSet.RoarTalentAlmahadActive = value;
                if (value) PredatoryBondTalentAlmahadActive = false;
                Calculate(); NotifyPropertyChanged(nameof(RoarTalentAlmahadActive));
            }
        }
        public bool PredatoryBondTalentAlmahadActive
        {
            get => DataSet.PredatoryBondTalentAlmahadActive;
            set
            {
                DataSet.PredatoryBondTalentAlmahadActive = value;
                if (value) RoarTalentAlmahadActive = false;
                Calculate(); NotifyPropertyChanged(nameof(PredatoryBondTalentAlmahadActive));
            }
        }

        #endregion

        #region Формулы статов

        //private bool isUsingBlessingOfTheMoonOnLuna = false;
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
                if (GodsAidLuna)
                {
                    CriticalHitLuna += ModifiersDamage.GODS_AID_CRITICAL_HIT;
                }
                if (PredatoryBondTalentAlmahadActive)
                {
                    PenetrationLuna += Math.Min(penetration, StatsLimit.MAX_PENETRATION_HERO) * ModifiersDamage.PREDATORY_BOND_PENETRATION_COEFFICIENT;
                }
                CriticalHitLuna = StatsLimit.CheckLimit(CriticalHitLuna, maxCriticalHitHero);
                PenetrationLuna = StatsLimit.CheckLimit(PenetrationLuna, maxPenetrationHero);

                NotifyPropertyChanged(nameof(IsUsingBlessingOfTheMoonOnLuna));
            }
        }

        private double FormulaCoefficientOfCriticalHitHero()
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
            // Домножение на гнев глубин
            double result = ((1 - Resilience / 100) * (1 - criticalHitWithResilience) + Math.Pow((1 - Resilience / 100), 2) * criticalHitWithResilience * (2 + CriticalDamageFinal / 100) * DepthsFuryFinal.ConvertToCoefficient()) * DepthsFuryFinal.ConvertToCoefficient();

            return result;
        }
        private double FormulaCoefficientOfCriticalHitLuna()
        {
            double criticalHitWithResilience = (CriticalHitLuna - Resilience) / 100;
            //double critDamage = CriticalDamageFinal;
            double critDamage = CriticalDamageLuna;
            if (criticalHitWithResilience < 0) criticalHitWithResilience = 0;
            if (criticalHitWithResilience > 1) criticalHitWithResilience = 1;
            // Домножение на гнев глубин
            double result = ((1 - Resilience / 100) * (1 - criticalHitWithResilience) + Math.Pow((1 - Resilience / 100), 2) * criticalHitWithResilience * (2 + critDamage / 100) * DepthsFuryFinal.ConvertToCoefficient()) * DepthsFuryFinal.ConvertToCoefficient();

            return result;
        }
        private double FormulaCoefficientOfCriticalHitForSkill()
        {
            double criticalHitWithResilience = (CriticalHitHeroFinal - Resilience) / 100;
            double critDamage = CriticalDamageFinal;
            if (CrushingWillActive) critDamage += MermanModifiers.CRUSHING_WILL_ADDITIONAL_CRITICAL_DAMAGE;
            if (criticalHitWithResilience < 0) criticalHitWithResilience = 0;
            if (criticalHitWithResilience > 1) criticalHitWithResilience = 1;
            double result = ((1 - Resilience / 100) * (1 - criticalHitWithResilience) + Math.Pow((1 - Resilience / 100), 2) * criticalHitWithResilience * (2 + (critDamage + additionAnimalRageTalant) / 100) * DepthsFuryFinal.ConvertToCoefficient()) * DepthsFuryFinal.ConvertToCoefficient();

            return result;
        }

        private double FormulaCoefficientOfAttackStrength()
        {
            double result = AttackStrengthFinal.ConvertToCoefficient();
            return result;
        }
        private double FormulaCoefficientOfAttackStrengthLuna()
        {
            double result = AttackStrengthLuna.ConvertToCoefficient();
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
            double result = 1 - (Math.Max(0, (Protection - PenetrationHeroFinal) * (1 - (PiercingAttack / 100)))) / 100;
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

        private double FormulaCoefficientSkillPower()
        {
            return SkillPowerFinal.ConvertToCoefficient();
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

        //private CastleSectors selectedCastle = CastleSectors.Empty;
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
        public CastleSectors SelectedCastleStart
        {
            //get => selectedCastle;
            get => DataSet.SelectedCastleStart;
            set
            {
                DataSet.SelectedCastleStart = value;
                coefficientCastleStart = (2.5 * (int)value).ConvertToCoefficient();
                Calculate();
                NotifyPropertyChanged(nameof(SelectedCastleStart));
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

        public double coefficientCastle = 1;
        public double coefficientCastleStart = 1;

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
        public bool GodsAidLuna
        {
            get => DataSet.GodsAidLuna;
            set
            {
                DataSet.GodsAidLuna = value;
                Calculate();
                NotifyPropertyChanged(nameof(GodsAidLuna));
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
                MoonTouchOpacity = changeOpacity(value);
                Calculate(); NotifyPropertyChanged(nameof(MoonTouchActive)); }
        }
        //private bool beastAwakeningActive = true;
        public bool BeastAwakeningActive
        {
            get => DataSet.BeastAwakeningActive;
            set {
                DataSet.BeastAwakeningActive = value;
                BeastAwakeningOpacity = changeOpacity(value);
                Calculate(); NotifyPropertyChanged(nameof(BeastAwakeningActive)); }
        }
        //private bool orderToAttackActive = true;
        public bool OrderToAttackActive
        {
            //get => orderToAttackActive;
            get => DataSet.OrderToAttackActive;
            set { //orderToAttackActive = value; 
                DataSet.OrderToAttackActive = value;
                OrderToAttackOpacity = changeOpacity(value);
                Calculate(); NotifyPropertyChanged(nameof(OrderToAttackActive)); }
        }
        //private bool healingActive = true;
        public bool HealingActive
        {
            //get => healingActive;
            get => DataSet.HealingActive;
            set { //healingActive = value;
                DataSet.HealingActive = value;
                HealingOpacity = changeOpacity(value);
                Calculate(); NotifyPropertyChanged(nameof(HealingActive)); }
        }
        //private bool chainLightningActive = true;
        public bool ChainLightningActive
        {
            //get => chainLightningActive;
            get => DataSet.ChainLightningActive;
            set { //chainLightningActive = value; 
                DataSet.ChainLightningActive = value;
                ChainLightningOpacity = changeOpacity(value);
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
                BestialRampageOpacity = changeOpacity(value);
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
                AuraOfTheForestOpacity = changeOpacity(value);
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
                MoonlightPermanentOpacity = changeOpacity(value);
                Calculate(); NotifyPropertyChanged(nameof(MoonlightPermanentActive)); }
        }
        //private bool moonlightNonPermanentActive = true;
        public bool MoonlightNonPermanentActive
        {
            //get => moonlightNonPermanentActive;
            get => DataSet.MoonlightNonPermanentActive;
            set {
                //moonlightNonPermanentActive = value; 
                DataSet.MoonlightNonPermanentActive = value;
                MoonlightNonPermanentOpacity = changeOpacity(value);
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

        #region фичи

        //public bool OverLimitClosed
        //{
        //    get => DataSet.OverLimit;
        //    set
        //    {
        //        DataSet.OverLimit = value;
        //        Calculate(); NotifyPropertyChanged(nameof(OverLimitClosed));
        //    }
        //}

        public bool AuraTalentAbuse
        {
            get => DataSet.AuraTalentAbuse;
            set
            {
                DataSet.AuraTalentAbuse = value;
                Calculate(); NotifyPropertyChanged(nameof(AuraTalentAbuse));
            }
        }

        public void OptimizeByGradientOLD()
        {
            // ============================================================
            // CONFIG
            // ============================================================
            const double step = 0.1;
            const double budgetTolerance = 0.2;
            const int SCALE = 10000;
            const int MAX_ITERATIONS = 1000;
            const double LEARNING_RATE = 0.3;
            const int LOCAL_SEARCH_RADIUS = 2;

            Status = "Градиентная оптимизация";
            var sw = System.Diagnostics.Stopwatch.StartNew();

            // ============================================================
            // ВЕСА
            // ============================================================
            const double wSC = 0.42;
            const double wASp = 1.07;
            const double wCH = 0.78;
            const double wCD = 1.46;
            const double wP = 1.24;
            const double wAc = 0.86;
            const double wASt = 1.23;
            const double wPA = 0.97;
            const double wR = 0.93;
            const double wF = 1.77;

            // ============================================================
            // HARD CAPS
            // ============================================================
            const double hardSC = 200;
            const double hardASp = 70;
            const double hardCH = 53;
            const double hardCD = 200;
            const double hardP = 50;
            const double hardAc = 50;
            const double hardASt = 100;
            const double hardPA = 50;
            const double hardR = 50;
            const double hardF = 50;

            // ============================================================
            // БАЗА
            // ============================================================
            const double innateCH = 5;
            const double guildASp = 15;
            const double guildCH = 6;
            const double guildCD = 20;
            const double guildSC = 15;
            const double guildP = 6;
            const double guildAc = 7;

            int branch = DataSet.DualRageActive ? 2 :
                         DataSet.ForestInspirationActive ? 3 : 1;

            double tASp = (branch == 2) ? 5.75 : 4.25;
            double tCH = 4.75;
            double tCD = (branch == 2) ? 3.0 : 1.5;
            double tSC = (branch == 2) ? 4.25 : 5.75;
            double tP = (branch == 3) ? 2.75 : 3.75;
            double tAc = (branch == 1) ? 4.75 : 3.5;

            // ============================================================
            // КНИГИ (опционально)
            // ============================================================
            bool includeBooks = false;
            const double bookASp = 7, bookCH = 4, bookCD = 10, bookSC = 8;
            const double bookP = 3, bookAc = 4, bookPA = 4, bookASt = 4.7;
            const double bookF = 7.5, bookR = 8;

            double bASp = includeBooks ? bookASp : 0;
            double bCH = includeBooks ? bookCH : 0;
            double bCD = includeBooks ? bookCD : 0;
            double bSC = includeBooks ? bookSC : 0;
            double bP = includeBooks ? bookP : 0;
            double bAc = includeBooks ? bookAc : 0;
            double bPA = includeBooks ? bookPA : 0;
            double bASt = includeBooks ? bookASt : 0;
            double bF = includeBooks ? bookF : 0;
            double bR = includeBooks ? bookR : 0;

            // ============================================================
            // СОФТ-КАПЫ
            // ============================================================
            const double softGearASp = 41.8;
            const double softGearCH = 48.4;
            const double softGearCD = 24.0;
            const double softGearSC = 90.5;
            const double softGearP = 37.5;
            const double softGearAc = 52.2;
            const double softGearPA = 34.5;
            //const double softGearPA = 34.5;
            const double softGearASt = 30.0;
            const double softGearR = 41.9;
            const double softGearF = 23.8;

            // ============================================================
            // MIN и MAX
            // ============================================================
            double minASp = guildASp + tASp + bASp;
            double minCH = innateCH + guildCH + tCH + bCH;
            double minCD = guildCD + tCD + bCD;
            double minSC = guildSC + tSC + bSC;
            double minP = guildP + tP + bP;
            double minAc = guildAc + tAc + bAc;
            double minPA = bPA;
            double minASt = bASt;
            double minR = bR;
            double minF = bF;

            double maxASp = Math.Min(hardASp, minASp + softGearASp);
            double maxCH = Math.Min(hardCH, minCH + softGearCH);
            double maxCD = Math.Min(hardCD, minCD + softGearCD);
            double maxSC = Math.Min(hardSC, minSC + softGearSC);
            double maxP = Math.Min(hardP, minP + softGearP);
            double maxAc = Math.Min(hardAc, minAc + softGearAc);
            double maxPA = Math.Min(hardPA, minPA + softGearPA);
            double maxASt = Math.Min(hardASt, minASt + softGearASt);
            double maxR = Math.Min(hardR, minR + softGearR);
            double maxF = Math.Min(hardF, minF + softGearF);

            // ============================================================
            // Сохраняем исходные значения
            // ============================================================
            double oSC = DataSet.SkillCooldown;
            double oASp = DataSet.AttackSpeed;
            double oCH = DataSet.CriticalHit;
            double oCD = DataSet.CriticalDamage;
            double oP = DataSet.Penetration;
            double oAc = DataSet.Accuracy;
            double oASt = DataSet.AttackStrength;
            double oPA = DataSet.PiercingAttack;
            double oR = DataSet.Rage;
            double oF = DataSet.Facilitation;

            // ============================================================
            // Массивы для удобства
            // ============================================================
            double[] weights = { wSC, wASp, wCH, wCD, wP, wAc, wASt, wPA, wR, wF };
            double[] mins = { minSC, minASp, minCH, minCD, minP, minAc, minASt, minPA, minR, minF };
            double[] maxs = { maxSC, maxASp, maxCH, maxCD, maxP, maxAc, maxASt, maxPA, maxR, maxF };

            // ============================================================
            // Вычисление целевого бюджета
            // ============================================================
            double[] current = {
        Math.Max(mins[0], Math.Min(maxs[0], oSC)),
        Math.Max(mins[1], Math.Min(maxs[1], oASp)),
        Math.Max(mins[2], Math.Min(maxs[2], oCH)),
        Math.Max(mins[3], Math.Min(maxs[3], oCD)),
        Math.Max(mins[4], Math.Min(maxs[4], oP)),
        Math.Max(mins[5], Math.Min(maxs[5], oAc)),
        Math.Max(mins[6], Math.Min(maxs[6], oASt)),
        Math.Max(mins[7], Math.Min(maxs[7], oPA)),
        Math.Max(mins[8], Math.Min(maxs[8], oR)),
        Math.Max(mins[9], Math.Min(maxs[9], oF))
    };

            double targetBudget = 0;
            for (int i = 0; i < 10; i++)
                targetBudget += weights[i] * current[i];

            // ============================================================
            // Вспомогательные функции
            // ============================================================
            double Clamp(double val, double min, double max)
            {
                return Math.Max(min, Math.Min(max, val));
            }

            double Round01(double val)
            {
                return Math.Round(val * 10) / 10.0;
            }

            void ApplyToDataSet(double[] x1)
            {
                DataSet.SkillCooldown = Round01(x1[0]);
                DataSet.AttackSpeed = Round01(x1[1]);
                DataSet.CriticalHit = Round01(x1[2]);
                DataSet.CriticalDamage = Round01(x1[3]);
                DataSet.Penetration = Round01(x1[4]);
                DataSet.Accuracy = Round01(x1[5]);
                DataSet.AttackStrength = Round01(x1[6]);
                DataSet.PiercingAttack = Round01(x1[7]);
                DataSet.Rage = Round01(x1[8]);
                DataSet.Facilitation = Round01(x1[9]);
            }

            double ComputeBudget(double[] x2)
            {
                double sum = 0;
                for (int i = 0; i < 10; i++)
                    sum += weights[i] * x2[i];
                return sum;
            }

            // ЖЁСТКАЯ ПРОЕКЦИЯ НА БЮДЖЕТ методом множителей Лагранжа
            void ProjectToBudget(double[] x3, double target)
            {
                // Бинарный поиск множителя lambda
                double lambdaMin = -100, lambdaMax = 100;

                for (int iter = 0; iter < 30; iter++)
                {
                    double lambda = (lambdaMin + lambdaMax) / 2.0;

                    double[] xProj = new double[10];
                    for (int i = 0; i < 10; i++)
                    {
                        // x_i - lambda * w_i (градиент Лагранжиана)
                        xProj[i] = Clamp(x3[i] - lambda * weights[i], mins[i], maxs[i]);
                    }

                    double budget = ComputeBudget(xProj);

                    if (Math.Abs(budget - target) < 0.01)
                    {
                        for (int i = 0; i < 10; i++)
                            x3[i] = xProj[i];
                        return;
                    }

                    if (budget > target)
                        lambdaMin = lambda;
                    else
                        lambdaMax = lambda;
                }

                // Финальная проекция
                double lambda2 = (lambdaMin + lambdaMax) / 2.0;
                for (int i = 0; i < 10; i++)
                    x3[i] = Clamp(x3[i] - lambda2 * weights[i], mins[i], maxs[i]);
            }

            // ============================================================
            // ГРАДИЕНТНЫЙ СПУСК С ПРОЕКЦИЕЙ
            // ============================================================
            double[] x = (double[])current.Clone();
            double[] gradient = new double[10];
            const double h = 0.1;

            int bestDD = int.MinValue;
            double[] bestSolution = (double[])x.Clone();

            // Начальная проекция на бюджет
            ProjectToBudget(x, targetBudget);

            for (int iter = 0; iter < MAX_ITERATIONS; iter++)
            {
                // Вычисление численного градиента
                ApplyToDataSet(x);
                Calculate();
                double fx = DataSet.ResultDD;

                for (int i = 0; i < 10; i++)
                {
                    double[] xh = (double[])x.Clone();
                    xh[i] += h;
                    xh[i] = Clamp(xh[i], mins[i], maxs[i]);

                    // Временно проецируем на бюджет
                    ProjectToBudget(xh, targetBudget);

                    ApplyToDataSet(xh);
                    Calculate();
                    double fxh = DataSet.ResultDD;

                    gradient[i] = (fxh - fx) / h;
                }

                // Градиентный шаг
                double[] xNew = new double[10];
                for (int i = 0; i < 10; i++)
                {
                    xNew[i] = x[i] + LEARNING_RATE * gradient[i];
                    xNew[i] = Clamp(xNew[i], mins[i], maxs[i]);
                }

                // ЖЁСТКАЯ ПРОЕКЦИЯ НА БЮДЖЕТ
                ProjectToBudget(xNew, targetBudget);

                // Проверка улучшения
                ApplyToDataSet(xNew);
                Calculate();

                if (DataSet.ResultDD > bestDD)
                {
                    bestDD = DataSet.ResultDD;
                    bestSolution = (double[])xNew.Clone();
                    Status = $"Градиент (iter={iter}, DD={bestDD}, ΔB={Math.Abs(ComputeBudget(xNew) - targetBudget):F2})";
                }

                x = xNew;

                // Ранний выход если сошлись
                if (iter > 100)
                {
                    double gradNorm = 0;
                    for (int i = 0; i < 10; i++)
                        gradNorm += gradient[i] * gradient[i];

                    if (Math.Sqrt(gradNorm) < 0.001)
                        break;
                }
            }

            // ============================================================
            // ОКРУГЛЕНИЕ И ЛОКАЛЬНЫЙ ПОИСК
            // ============================================================
            double[] rounded = new double[10];
            for (int i = 0; i < 10; i++)
                rounded[i] = Round01(bestSolution[i]);

            // Перепроецируем округленное решение
            ProjectToBudget(rounded, targetBudget);

            // Локальный поиск с соблюдением бюджета
            void LocalSearch(double[] solution, int radius)
            {
                ApplyToDataSet(solution);
                Calculate();
                int currentBest = DataSet.ResultDD;
                double[] currentSolution = (double[])solution.Clone();

                // Двухмерный поиск: уменьшаем одну характеристику, увеличиваем другую
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        if (i == j) continue;

                        for (int delta = 1; delta <= radius; delta++)
                        {
                            double[] neighbor = (double[])currentSolution.Clone();

                            // Уменьшаем i, увеличиваем j так, чтобы сохранить бюджет
                            double decrease = delta * step;
                            double increase = (weights[i] / weights[j]) * decrease;

                            neighbor[i] = Round01(neighbor[i] - decrease);
                            neighbor[j] = Round01(neighbor[j] + increase);

                            neighbor[i] = Clamp(neighbor[i], mins[i], maxs[i]);
                            neighbor[j] = Clamp(neighbor[j], mins[j], maxs[j]);

                            double budgetDiff = Math.Abs(ComputeBudget(neighbor) - targetBudget);
                            if (budgetDiff > budgetTolerance) continue;

                            ApplyToDataSet(neighbor);
                            Calculate();

                            if (DataSet.ResultDD > currentBest)
                            {
                                currentBest = DataSet.ResultDD;
                                currentSolution = (double[])neighbor.Clone();
                                bestDD = DataSet.ResultDD;
                            }
                        }
                    }
                }

                for (int i = 0; i < 10; i++)
                    solution[i] = currentSolution[i];
            }

            LocalSearch(rounded, LOCAL_SEARCH_RADIUS);

            // Финальная проверка бюджета
            double finalBudget = ComputeBudget(rounded);
            double finalDiff = Math.Abs(finalBudget - targetBudget);

            // ============================================================
            // СОХРАНЕНИЕ РЕЗУЛЬТАТОВ
            // ============================================================
            sw.Stop();
            TimeRec = sw.ElapsedMilliseconds;

            string resultsDir = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "results");
            System.IO.Directory.CreateDirectory(resultsDir);

            string fileName = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + "_gradient_opt.txt";
            string filePath = System.IO.Path.Combine(resultsDir, fileName);

            using (var writer = new System.IO.StreamWriter(filePath, false, System.Text.Encoding.UTF8))
            {
                writer.WriteLine("=== BeastMasterCalc | Gradient Optimization ===");
                writer.WriteLine("Execution time: " + TimeRec + " ms");
                writer.WriteLine();
                writer.WriteLine("Branch: " + branch);
                writer.WriteLine("IncludeBooks: " + includeBooks);
                writer.WriteLine("Target Budget: " + targetBudget.ToString("0.###"));
                writer.WriteLine("Budget Tolerance: ±" + budgetTolerance);
                writer.WriteLine();
                writer.WriteLine("=== BEST SOLUTION ===");
                writer.WriteLine("Best DD = " + bestDD);
                writer.WriteLine("Final Budget = " + finalBudget.ToString("0.###"));
                writer.WriteLine("Budget Diff = " + finalDiff.ToString("0.###"));
                writer.WriteLine();
                writer.WriteLine("SkillCooldown   = " + rounded[0].ToString("0.0"));
                writer.WriteLine("AttackSpeed     = " + rounded[1].ToString("0.0"));
                writer.WriteLine("CriticalHit     = " + rounded[2].ToString("0.0"));
                writer.WriteLine("CriticalDamage  = " + rounded[3].ToString("0.0"));
                writer.WriteLine("Penetration     = " + rounded[4].ToString("0.0"));
                writer.WriteLine("Accuracy        = " + rounded[5].ToString("0.0"));
                writer.WriteLine("AttackStrength  = " + rounded[6].ToString("0.0"));
                writer.WriteLine("PiercingAttack  = " + rounded[7].ToString("0.0"));
                writer.WriteLine("Rage            = " + rounded[8].ToString("0.0"));
                writer.WriteLine("Facilitation    = " + rounded[9].ToString("0.0"));
            }

            // ============================================================
            // ВОЗВРАТ ИСХОДНЫХ ДАННЫХ
            // ============================================================
            DataSet.SkillCooldown = oSC;
            DataSet.AttackSpeed = oASp;
            DataSet.CriticalHit = oCH;
            DataSet.CriticalDamage = oCD;
            DataSet.Penetration = oP;
            DataSet.Accuracy = oAc;
            DataSet.AttackStrength = oASt;
            DataSet.PiercingAttack = oPA;
            DataSet.Rage = oR;
            DataSet.Facilitation = oF;
            Calculate();

            Status = "Оптимизация завершена (ΔBudget=" + finalDiff.ToString("0.###") + ")";
        }

        #region Claude methods

        // ============================================================
        // ВАРИАНТ 1: ГРАДИЕНТНАЯ ОПТИМИЗАЦИЯ (исправленный бюджет)
        // ============================================================
        public void OptimizeByGradient()
        {
            const double step = 0.1;
            const double budgetTolerance = 0.2;
            const int MAX_ITERATIONS = 1000;
            const double LEARNING_RATE = 0.3;

            Status = "Градиентная оптимизация";
            var sw = System.Diagnostics.Stopwatch.StartNew();

            // ВЕСА
            double[] weights = { 0.42, 1.07, 0.78, 1.46, 1.24, 0.86, 1.23, 0.97, 0.93, 1.77 };

            // HARD CAPS
            double[] hardCaps = { 200, 70, 53, 200, 50, 50, 100, 50, 50, 50 };

            // БАЗА: гильдия + таланты
            int branch = DataSet.DualRageActive ? 2 : DataSet.ForestInspirationActive ? 3 : 1;

            double tASp = (branch == 2) ? 5.75 : 4.25;
            double tCH = 4.75;
            double tCD = (branch == 2) ? 3.0 : 1.5;
            double tSC = (branch == 2) ? 4.25 : 5.75;
            double tP = (branch == 3) ? 2.75 : 3.75;
            double tAc = (branch == 1) ? 4.75 : 3.5;

            // КНИГИ (индивидуальные флаги - добавь их в DataSet)
            // Пример: DataSet.HasBookASp, DataSet.HasBookCH и т.д.
            // Если у тебя нет этих полей, создай их как bool в DataSet
            bool hasBookSC = true;   // замени на DataSet.HasBookSC
            bool hasBookASp = false;  // замени на DataSet.HasBookASp
            bool hasBookCH = true;   // замени на DataSet.HasBookCH
            bool hasBookCD = false;   // замени на DataSet.HasBookCD
            bool hasBookP = true;    // замени на DataSet.HasBookP
            bool hasBookAc = true;   // замени на DataSet.HasBookAc
            bool hasBookASt = true;  // замени на DataSet.HasBookASt
            bool hasBookPA = true;   // замени на DataSet.HasBookPA
            bool hasBookR = false;    // замени на DataSet.HasBookR
            bool hasBookF = false;    // замени на DataSet.HasBookF

            double bSC = hasBookSC ? 8 : 0;
            double bASp = hasBookASp ? 7 : 0;
            double bCH = hasBookCH ? 4 : 0;
            double bCD = hasBookCD ? 10 : 0;
            double bP = hasBookP ? 3 : 0;
            double bAc = hasBookAc ? 4 : 0;
            double bASt = hasBookASt ? 4.7 : 0;
            double bPA = hasBookPA ? 4 : 0;
            double bR = hasBookR ? 8 : 0;
            double bF = hasBookF ? 7.5 : 0;

            double cFlat = CastleStartModifierActive ? 5 : 0;
            double[] baseStats = {
        15 + tSC  + bSC  - cFlat, // SC
        15 + tASp + bASp - cFlat, // ASp
        5 + 6 + tCH + bCH - cFlat,// CH
        20 + tCD + bCD,            // CD
        6 + tP  + bP  - cFlat,   // P
        7 + tAc + bAc - cFlat,   // Ac
        bASt,                      // ASt
        bPA,                       // PA
        bR,                        // R
        bF                         // F
    };

            // СОФТ-КАПЫ (шмот + кристаллы)
            double[] softGear = { 90.5, 41.8, 48.4, 24.0, 37.5, 52.2, 30.0, 34.5, 41.9, 23.8 };

            // MIN и MAX
            double[] mins = new double[10];
            double[] maxs = new double[10];
            for (int i = 0; i < 10; i++)
            {
                mins[i] = baseStats[i];
                maxs[i] = Math.Min(hardCaps[i], baseStats[i] + softGear[i]);
            }

            // Сохраняем исходные
            double[] original = {
        DataSet.SkillCooldown, DataSet.AttackSpeed, DataSet.CriticalHit, DataSet.CriticalDamage,
        DataSet.Penetration, DataSet.Accuracy, DataSet.AttackStrength, DataSet.PiercingAttack,
        DataSet.Rage, DataSet.Facilitation
    };

            // Текущие статы (ограничены min/max)
            double[] current = new double[10];
            for (int i = 0; i < 10; i++)
                current[i] = Math.Max(mins[i], Math.Min(maxs[i], original[i]));

            // ЦЕЛЕВОЙ БЮДЖЕТ (только переменная часть - шмот+кристаллы)
            double targetBudget = 0;
            for (int i = 0; i < 10; i++)
                targetBudget += weights[i] * (current[i] - baseStats[i]);

            // ВСПОМОГАТЕЛЬНЫЕ ФУНКЦИИ
            double Clamp(double val, double min, double max) => Math.Max(min, Math.Min(max, val));
            double Round01(double val) => Math.Round(val * 10) / 10.0;

            void ApplyToDataSet(double[] x1)
            {
                DataSet.SkillCooldown = Round01(x1[0]);
                DataSet.AttackSpeed = Round01(x1[1]);
                DataSet.CriticalHit = Round01(x1[2]);
                DataSet.CriticalDamage = Round01(x1[3]);
                DataSet.Penetration = Round01(x1[4]);
                DataSet.Accuracy = Round01(x1[5]);
                DataSet.AttackStrength = Round01(x1[6]);
                DataSet.PiercingAttack = Round01(x1[7]);
                DataSet.Rage = Round01(x1[8]);
                DataSet.Facilitation = Round01(x1[9]);
            }

            double ComputeGearBudget(double[] x2)
            {
                double sum = 0;
                for (int i = 0; i < 10; i++)
                    sum += weights[i] * (x2[i] - baseStats[i]);
                return sum;
            }

            // ПРОЕКЦИЯ НА БЮДЖЕТ (метод множителей Лагранжа)
            void ProjectToBudget(double[] x3, double target)
            {
                double lambdaMin = -100, lambdaMax = 100;

                for (int iter = 0; iter < 30; iter++)
                {
                    double lambda = (lambdaMin + lambdaMax) / 2.0;
                    double[] xProj = new double[10];

                    for (int i = 0; i < 10; i++)
                    {
                        double variablePart = x3[i] - baseStats[i];
                        variablePart = Clamp(variablePart - lambda * weights[i], 0, maxs[i] - baseStats[i]);
                        xProj[i] = baseStats[i] + variablePart;
                    }

                    double budget = ComputeGearBudget(xProj);

                    if (Math.Abs(budget - target) < 0.01)
                    {
                        for (int i = 0; i < 10; i++) x3[i] = xProj[i];
                        return;
                    }

                    if (budget > target) lambdaMin = lambda;
                    else lambdaMax = lambda;
                }

                double lambda2 = (lambdaMin + lambdaMax) / 2.0;
                for (int i = 0; i < 10; i++)
                {
                    double variablePart = x3[i] - baseStats[i];
                    variablePart = Clamp(variablePart - lambda2 * weights[i], 0, maxs[i] - baseStats[i]);
                    x3[i] = baseStats[i] + variablePart;
                }
            }

            // ГРАДИЕНТНЫЙ СПУСК
            double[] x = (double[])current.Clone();
            ProjectToBudget(x, targetBudget);

            int bestDD = int.MinValue;
            double[] bestSolution = (double[])x.Clone();

            for (int iter = 0; iter < MAX_ITERATIONS; iter++)
            {
                ApplyToDataSet(x);
                Calculate();
                double fx = DataSet.ResultDD;

                double[] gradient = new double[10];
                for (int i = 0; i < 10; i++)
                {
                    double[] xh = (double[])x.Clone();
                    xh[i] += 0.1;
                    xh[i] = Clamp(xh[i], mins[i], maxs[i]);
                    ProjectToBudget(xh, targetBudget);

                    ApplyToDataSet(xh);
                    Calculate();
                    gradient[i] = (DataSet.ResultDD - fx) / 0.1;
                }

                double[] xNew = new double[10];
                for (int i = 0; i < 10; i++)
                    xNew[i] = Clamp(x[i] + LEARNING_RATE * gradient[i], mins[i], maxs[i]);

                ProjectToBudget(xNew, targetBudget);
                ApplyToDataSet(xNew);
                Calculate();

                if (DataSet.ResultDD > bestDD)
                {
                    bestDD = DataSet.ResultDD;
                    bestSolution = (double[])xNew.Clone();
                    Status = $"Градиент (iter={iter}, DD={bestDD})";
                }

                x = xNew;

                if (iter > 100)
                {
                    double gradNorm = 0;
                    for (int i = 0; i < 10; i++) gradNorm += gradient[i] * gradient[i];
                    if (Math.Sqrt(gradNorm) < 0.001) break;
                }
            }

            double[] rounded = new double[10];
            for (int i = 0; i < 10; i++)
                rounded[i] = Round01(bestSolution[i]);
            ProjectToBudget(rounded, targetBudget);

            bool[] bookFlags = { hasBookSC, hasBookASp, hasBookCH, hasBookCD, hasBookP,
                        hasBookAc, hasBookASt, hasBookPA, hasBookR, hasBookF };
            SaveResults(rounded, bestDD, targetBudget, baseStats, original, sw, "gradient", branch, bookFlags);

            for (int i = 0; i < 10; i++)
                ApplyToDataSet(original);
            Calculate();
            Status = "Оптимизация завершена";
        }
        // ============================================================
        // ВАРИАНТ 2: OptimizeByMCTS МЕТОД
        // ============================================================

        public void OptimizeByMCTS()
        {
            const double budgetTolerance = 0.2;
            const int MAX_ITERATIONS = 5000;
            const double EXPLORATION_CONSTANT = 1.414;

            Status = "Monte Carlo Tree Search";
            var sw = System.Diagnostics.Stopwatch.StartNew();

            // ВЕСА И ПАРАМЕТРЫ
            double[] weights = { 1.88, 1.91, 3.68, 5.15, 4.46, 2.73, 3.55, 11.36, 7.94, 11.11 };
            double[] hardCaps = { 200, 70, 53, 200, 50, 50, 100, 50, 50, 50 };
            double[] softGear = { 90.5, 41.8, 48.4, 24.0, 37.5, 52.2, 30.0, 34.5, 41.9, 23.8 };

            int branch = DataSet.DualRageActive ? 2 : DataSet.ForestInspirationActive ? 3 : 1;

            double tASp = (branch == 2) ? 5.75 : 4.25;
            double tCH = 4.75;
            double tCD = (branch == 2) ? 3.0 : 1.5;
            double tSC = (branch == 2) ? 4.25 : 5.75;
            double tP = (branch == 3) ? 2.75 : 3.75;
            double tAc = (branch == 1) ? 4.75 : 3.5;

            bool hasBookSC = true;
            bool hasBookASp = false;
            bool hasBookCH = true;
            bool hasBookCD = false;
            bool hasBookP = true;
            bool hasBookAc = true;
            bool hasBookASt = true;
            bool hasBookPA = true;
            bool hasBookR = false;
            bool hasBookF = false;

            double bSC = hasBookSC ? 8 : 0;
            double bASp = hasBookASp ? 7 : 0;
            double bCH = hasBookCH ? 3 : 0;
            double bCD = hasBookCD ? 10 : 0;
            double bP = hasBookP ? 3 : 0;
            double bAc = hasBookAc ? 4 : 0;
            double bPA = hasBookPA ? 4 : 0;
            double bASt = hasBookASt ? 4.7 : 0;
            double bR = hasBookR ? 8 : 0;
            double bF = hasBookF ? 7.5 : 0;

            double cFlat = CastleStartModifierActive ? 5 : 0;
            double[] baseStats = {
        15 + tSC  + bSC  - cFlat, 15 + tASp + bASp - cFlat, 5 + 6 + tCH + bCH - cFlat, 20 + tCD + bCD,
        6 + tP + bP - cFlat, 7 + tAc + bAc - cFlat, bASt, bPA, bR, bF
    };

            double[] mins = new double[10];
            double[] maxs = new double[10];
            for (int i = 0; i < 10; i++)
            {
                mins[i] = baseStats[i];
                maxs[i] = Math.Min(hardCaps[i], baseStats[i] + softGear[i]);
            }

            double[] userMins = { 0, 0, 30, 0, 24, 0, 0, 0, 8.1, 0 };
            for (int i = 0; i < 10; i++)
            {
                mins[i] = Math.Max(mins[i], userMins[i]);
                if (mins[i] > maxs[i]) mins[i] = maxs[i];
            }

            double[] original = {
        DataSet.SkillCooldown, DataSet.AttackSpeed, DataSet.CriticalHit, DataSet.CriticalDamage,
        DataSet.Penetration, DataSet.Accuracy, DataSet.AttackStrength, DataSet.PiercingAttack,
        DataSet.Rage, DataSet.Facilitation
    };

            double[] current = new double[10];
            for (int i = 0; i < 10; i++)
                current[i] = Math.Max(mins[i], Math.Min(maxs[i], original[i]));

            double Clamp(double val, double min, double max) => Math.Max(min, Math.Min(max, val));
            double Round01(double val) => Math.Round(val * 10) / 10.0;

            void ApplyToDataSet(double[] x)
            {
                DataSet.SkillCooldown = Round01(x[0]);
                DataSet.AttackSpeed = Round01(x[1]);
                DataSet.CriticalHit = Round01(x[2]);
                DataSet.CriticalDamage = Round01(x[3]);
                DataSet.Penetration = Round01(x[4]);
                DataSet.Accuracy = Round01(x[5]);
                DataSet.AttackStrength = Round01(x[6]);
                DataSet.PiercingAttack = Round01(x[7]);
                DataSet.Rage = Round01(x[8]);
                DataSet.Facilitation = Round01(x[9]);
            }

            double ComputeGearBudget(double[] x)
            {
                double sum = 0;
                for (int i = 0; i < 10; i++)
                    sum += weights[i] * (x[i] - baseStats[i]);
                return sum;
            }

            double targetBudget = ComputeGearBudget(current);

            void ProjectToBudget(double[] x, double target)
            {
                double lambdaMin = -100, lambdaMax = 100;
                for (int iter = 0; iter < 30; iter++)
                {
                    double lambda = (lambdaMin + lambdaMax) / 2.0;
                    double[] xProj = new double[10];
                    for (int i = 0; i < 10; i++)
                    {
                        double minVar = mins[i] - baseStats[i];
                        double maxVar = maxs[i] - baseStats[i];
                        double variablePart = x[i] - baseStats[i];
                        variablePart = Clamp(variablePart - lambda * weights[i], minVar, maxVar);
                        xProj[i] = baseStats[i] + variablePart;
                    }
                    double budget = ComputeGearBudget(xProj);
                    if (Math.Abs(budget - target) < 0.01)
                    {
                        for (int i = 0; i < 10; i++) x[i] = xProj[i];
                        return;
                    }
                    if (budget > target) lambdaMin = lambda;
                    else lambdaMax = lambda;
                }
                double lambda2 = (lambdaMin + lambdaMax) / 2.0;
                for (int i = 0; i < 10; i++)
                {
                    double minVar = mins[i] - baseStats[i];
                    double maxVar = maxs[i] - baseStats[i];
                    double variablePart = x[i] - baseStats[i];
                    variablePart = Clamp(variablePart - lambda2 * weights[i], minVar, maxVar);
                    x[i] = baseStats[i] + variablePart;
                }
            }

            double Evaluate(double[] x)
            {
                ApplyToDataSet(x);
                Calculate();
                double budgetDiff = Math.Abs(ComputeGearBudget(x) - targetBudget);
                double penalty = budgetDiff > budgetTolerance ? budgetDiff * 100000 : 0;
                return DataSet.ResultDD - penalty;
            }

            // ПРОСТАЯ РЕАЛИЗАЦИЯ БЕЗ ДЕРЕВА (упрощенный вариант MCTS)
            var rand = new System.Random();

            int bestDD = int.MinValue;
            double[] bestSolution = (double[])current.Clone();

            // Храним лучшие найденные решения
            double[][] topSolutions = new double[20][];
            double[] topScores = new double[20];
            for (int i = 0; i < 20; i++)
            {
                topSolutions[i] = (double[])current.Clone();
                topScores[i] = double.MinValue;
            }

            // Действия: индекс стата + дельта
            int[] actionStats = new int[80];
            double[] actionDeltas = new double[80];
            int actionIdx = 0;
            double[] deltas = { -5.0, -2.0, -1.0, -0.5, 0.5, 1.0, 2.0, 5.0 };
            for (int i = 0; i < 10; i++)
            {
                for (int d = 0; d < 8; d++)
                {
                    actionStats[actionIdx] = i;
                    actionDeltas[actionIdx] = deltas[d];
                    actionIdx++;
                }
            }

            double[] ApplyAction(double[] state, int actionIndex)
            {
                double[] newState = (double[])state.Clone();
                int statIdx = actionStats[actionIndex];
                double delta = actionDeltas[actionIndex];

                newState[statIdx] += delta;
                newState[statIdx] = Clamp(newState[statIdx], mins[statIdx], maxs[statIdx]);
                newState[statIdx] = Round01(newState[statIdx]);

                ProjectToBudget(newState, targetBudget);
                return newState;
            }

            // Основной цикл MCTS (упрощенный)
            for (int iteration = 0; iteration < MAX_ITERATIONS; iteration++)
            {
                // Выбираем базовое состояние (эксплуатация лучших или исследование текущего)
                double[] baseState;
                if (iteration % 5 == 0 && iteration > 0)
                {
                    // Эксплуатация: берем одно из лучших решений
                    int topIdx = rand.Next(Math.Min(5, 20));
                    baseState = (double[])topSolutions[topIdx].Clone();
                }
                else
                {
                    // Исследование: случайное решение из топ-20
                    int topIdx = rand.Next(20);
                    baseState = (double[])topSolutions[topIdx].Clone();
                }

                // Симуляция: делаем 5-15 случайных действий
                double[] simState = (double[])baseState.Clone();
                int steps = rand.Next(5, 15);

                for (int step = 0; step < steps; step++)
                {
                    int action = rand.Next(80);
                    simState = ApplyAction(simState, action);
                }

                // Оцениваем результат
                double reward = Evaluate(simState);

                // Обновляем лучшее решение
                if (reward > bestDD)
                {
                    bestDD = (int)reward;
                    bestSolution = (double[])simState.Clone();
                    Status = $"MCTS: iter={iteration}, DD={bestDD}";
                }

                // Обновляем топ-20
                for (int i = 0; i < 20; i++)
                {
                    if (reward > topScores[i])
                    {
                        // Сдвигаем вниз
                        for (int j = 19; j > i; j--)
                        {
                            topSolutions[j] = topSolutions[j - 1];
                            topScores[j] = topScores[j - 1];
                        }
                        topSolutions[i] = (double[])simState.Clone();
                        topScores[i] = reward;
                        break;
                    }
                }

                // Каждые 100 итераций пробуем улучшить топ решения локальным поиском
                if (iteration % 100 == 0 && iteration > 0)
                {
                    for (int t = 0; t < 3; t++)
                    {
                        double[] state = (double[])topSolutions[t].Clone();

                        // Пробуем небольшие изменения
                        for (int attempt = 0; attempt < 10; attempt++)
                        {
                            int action = rand.Next(80);
                            double[] newState = ApplyAction(state, action);
                            double newReward = Evaluate(newState);

                            if (newReward > topScores[t])
                            {
                                state = newState;
                                topScores[t] = newReward;

                                if (newReward > bestDD)
                                {
                                    bestDD = (int)newReward;
                                    bestSolution = (double[])newState.Clone();
                                }
                            }
                        }
                    }
                }
            }

            // ФИНАЛИЗАЦИЯ
            double[] rounded = new double[10];
            for (int i = 0; i < 10; i++)
                rounded[i] = Round01(bestSolution[i]);

            ProjectToBudget(rounded, targetBudget);
            for (int i = 0; i < 10; i++)
                rounded[i] = Clamp(Round01(rounded[i]), mins[i], maxs[i]);

            ApplyToDataSet(rounded);
            Calculate();
            bestDD = DataSet.ResultDD;

            bool[] bookFlags = {
        hasBookSC, hasBookASp, hasBookCH, hasBookCD, hasBookP,
        hasBookAc, hasBookASt, hasBookPA, hasBookR, hasBookF
    };

            SaveResults(rounded, bestDD, targetBudget, baseStats, original, sw, "mcts", branch, bookFlags);

            ApplyToDataSet(original);
            Calculate();
            Status = "MCTS завершен";
        }


        // ============================================================
        // ВАРИАНТ 3: DIFFERENTIAL EVOLUTION
        // ============================================================
        public void OptimizeByDE() => _ = OptimizeByDEAsync();

        private async Task<RecommendationRunResult> OptimizeByDEAsync(bool saveSingleResult = true)
        {
            var algorithm = CurrentRecommendationAlgorithm;
            Status = RecommendationSystem.GetAlgorithmName(algorithm);
            var sw = System.Diagnostics.Stopwatch.StartNew();

            // Порядок статов: 0 SC, 1 ASp, 2 CH, 3 CD, 4 P, 5 Ac, 6 ASt, 7 PA, 8 R, 9 F, 10 SP

            // Веса
            const double wSC  = 0.0315; // SkillCooldown
            const double wASp = 0.0681; // AttackSpeed
            const double wCH  = 0.0588; // CriticalHit
            const double wCD  = 0.1186; // CriticalDamage
            const double wP   = 0.0759; // Penetration
            const double wAc  = 0.0545; // Accuracy
            const double wASt = 0.0949; // AttackStrength
            const double wPA  = 0.0825; // PiercingAttack
            const double wR   = 0.0679; // Rage
            const double wF   = 0.1196; // Facilitation
            const double wSP  = 0.2277; // SkillPower

            double[] weights = { wSC, wASp, wCH, wCD, wP, wAc, wASt, wPA, wR, wF, wSP };
            //double[] weights = { 0.42, 1.07, 0.78, 1.46, 1.24, 0.86, 1.23, 0.97, 0.93, 1.77 };
            double[] hardCaps = { 200, 70, 53, 200, 50, 50, 100, 50, 50, 50, 100 };
            double[] softGear = { 90.5, 41.8, 48.4, 24.0, 37.5, 52.2, 30.0, 34.5, 41.9, 23.8, 12.5 };

            // ============================================================
            // БАЗА (таланты + книги + гильдия; без расовых, без замка)
            // ============================================================
            double cFlat = CastleStartModifierActive ? 5 : 0;
            double[] baseStats = ComputeRecBaseStats();

            // ============================================================
            // MIN/MAX по перебору (min = база, max = min(hard, база + soft))
            // ============================================================
            double[] mins = new double[11];
            double[] maxs = new double[11];
            for (int i = 0; i < 11; i++)
            {
                mins[i] = baseStats[i];
                maxs[i] = System.Math.Min(hardCaps[i], baseStats[i] + softGear[i]);
            }

            // ============================================================
            // ДОП. МИНИМУМЫ (user-min): ниже этих значений итоговый стат быть не может
            // Если не задаёшь ограничение — ставь 0.
            // Пример: CH не ниже 38
            // ============================================================
            double[] userMins =
            {
        UserMinSC,   // SC
        UserMinASp,  // ASp
        UserMinCH,   // CH
        UserMinCD,   // CD
        UserMinP,    // P
        UserMinAc,   // Ac
        UserMinASt,  // ASt
        UserMinPA,   // PA
        UserMinR,    // R
        UserMinF,    // F
        UserMinSP    // SP
    };

            for (int i = 0; i < 11; i++)
            {
                mins[i] = System.Math.Max(mins[i], userMins[i]);
                if (mins[i] > maxs[i]) mins[i] = maxs[i]; // защита (можешь заменить на throw)
            }

            // ============================================================
            // Сохраняем исходные значения (до-замковые, как хранит DataSet)
            // ============================================================
            double[] original =
            {
        DataSet.SkillCooldown, DataSet.AttackSpeed, DataSet.CriticalHit, DataSet.CriticalDamage,
        DataSet.Penetration, DataSet.Accuracy, DataSet.AttackStrength, DataSet.PiercingAttack,
        DataSet.Rage, DataSet.Facilitation, DataSet.SkillPower
    };

            double[] current = new double[11];
            for (int i = 0; i < 11; i++)
                current[i] = System.Math.Max(mins[i], System.Math.Min(maxs[i], original[i]));

            double Round01(double val) => System.Math.Round(val * 10) / 10.0;

            void ApplyToDataSet(double[] x)
            {
                DataSet.SkillCooldown  = Round01(x[0]);
                DataSet.AttackSpeed    = Round01(x[1]);
                DataSet.CriticalHit    = Round01(x[2]);
                DataSet.CriticalDamage = Round01(x[3]);
                DataSet.Penetration    = Round01(x[4]);
                DataSet.Accuracy       = Round01(x[5]);
                DataSet.AttackStrength = Round01(x[6]);
                DataSet.PiercingAttack = Round01(x[7]);
                DataSet.Rage           = Round01(x[8]);
                DataSet.Facilitation   = Round01(x[9]);
                DataSet.SkillPower     = Round01(x[10]);
            }

            double originalBudget = 0;
            for (int i = 0; i < 11; i++)
                originalBudget += weights[i] * (original[i] - baseStats[i]);

            double targetBudget = 0;
            for (int i = 0; i < 11; i++)
                targetBudget += weights[i] * (current[i] - baseStats[i]);

            double maxAllowedBudget = originalBudget * 1.03;
            if (targetBudget > maxAllowedBudget)
                targetBudget = maxAllowedBudget;

            // ============================================================
            // ЗАПУСК DE АСИНХРОННО (UI не блокируется)
            // ============================================================
            _suppressNotifications = true;
            double[] rounded;
            var recInp = new RecommendationInput
            {
                Algorithm      = algorithm,
                DePopulationSize = RecommendationDePopulationSize,
                DeMaxGenerations = RecommendationDeMaxGenerations,
                DeMutationFactor = RecommendationDeMutationFactor,
                DeCrossoverRate  = RecommendationDeCrossoverRate,
                MctsMaxIterations = RecommendationMctsMaxIterations,
                MctsTop          = RecommendationMctsTop,
                MctsMinSteps     = RecommendationMctsMinSteps,
                MctsMaxSteps     = RecommendationMctsMaxSteps,
                BudgetTolerance  = RecommendationBudgetTolerance,
                Initial        = current,
                Weights        = weights,
                BaseStats      = baseStats,
                Mins           = mins,
                Maxs           = maxs,
                TargetBudget   = targetBudget,
                Evaluate       = x => { ApplyToDataSet(x); Calculate(); return DataSet.ResultDD; },
                ReportStatus   = s => SetStatusDirect(s),
            };
            try
            {
                rounded = await _rs.GetRecommendationAsync(recInp);
            }
            finally
            {
                _suppressNotifications = false;
            }
            RecCalcCount = recInp.EvalCallCount;

            _lastRounded = rounded;
            _applyRecResultCommand?.RaiseCanExecuteChanged();

            ApplyToDataSet(rounded);
            Calculate();
            int bestDD = DataSet.ResultDD;

            RecFinalSC  = SkillCooldownFinal.ToString("F1");
            RecFinalASp = AttackSpeedFinal.ToString("F1");
            RecFinalCH  = CriticalHitHeroFinal.ToString("F1");
            RecFinalCD  = CriticalDamageFinal.ToString("F1");
            RecFinalP   = PenetrationHeroFinal.ToString("F1");
            RecFinalAc  = AccuracyHeroFinal.ToString("F1");
            RecFinalASt = AttackStrengthFinal.ToString("F1");
            RecFinalPA  = PiercingAttackFinal.ToString("F1");
            RecFinalR   = RageFinal.ToString("F1");
            RecFinalF   = FacilitationFinal.ToString("F1");
            RecFinalSP  = SkillPowerFinal.ToString("F1");
            IsRecFinalVisible = true;

            RecValSC  = rounded[0].ToString("F1");  DeltaSC  = FormatDelta(rounded[0]  - original[0]);
            RecValASp = rounded[1].ToString("F1");  DeltaASp = FormatDelta(rounded[1]  - original[1]);
            RecValCH  = rounded[2].ToString("F1");  DeltaCH  = FormatDelta(rounded[2]  - original[2]);
            RecValCD  = rounded[3].ToString("F1");  DeltaCD  = FormatDelta(rounded[3]  - original[3]);
            RecValP   = rounded[4].ToString("F1");  DeltaP   = FormatDelta(rounded[4]  - original[4]);
            RecValAc  = rounded[5].ToString("F1");  DeltaAc  = FormatDelta(rounded[5]  - original[5]);
            RecValASt = rounded[6].ToString("F1");  DeltaASt = FormatDelta(rounded[6]  - original[6]);
            RecValPA  = rounded[7].ToString("F1");  DeltaPA  = FormatDelta(rounded[7]  - original[7]);
            RecValR   = rounded[8].ToString("F1");  DeltaR   = FormatDelta(rounded[8]  - original[8]);
            RecValF   = rounded[9].ToString("F1");  DeltaF   = FormatDelta(rounded[9]  - original[9]);
            RecValSP  = rounded[10].ToString("F1"); DeltaSP  = FormatDelta(rounded[10] - original[10]);
            RecDPMDisplay = bestDD.ToString();

            int branch = DataSet.DualRageActive ? 2 : DataSet.ForestInspirationActive ? 3 : 1;
            bool[] bookFlags =
            {
        RecHasBookSC, RecHasBookASp, RecHasBookCH, RecHasBookCD, RecHasBookP,
        RecHasBookAc, RecHasBookASt, RecHasBookPA, RecHasBookR, RecHasBookF
    };

            if (saveSingleResult)
                SaveResults(rounded, bestDD, targetBudget, baseStats, original, sw, RecommendationSystem.GetAlgorithmCode(algorithm), branch, bookFlags);
            else
            {
                sw.Stop();
                TimeRec = sw.ElapsedMilliseconds;
            }

            ApplyToDataSet(original);
            Calculate();

            Status = RecommendationSystem.GetAlgorithmName(algorithm) + " завершена";
            return new RecommendationRunResult
            {
                Dpm = bestDD,
                TimeMs = TimeRec,
                EvalCallCount = recInp.EvalCallCount,
                Solution = rounded
            };
        }

        // ============================================================
        // ОБЩАЯ ФУНКЦИЯ СОХРАНЕНИЯ РЕЗУЛЬТАТОВ
        // + разница статов (recommended - start)
        // bookFlags: 10 bool в порядке statNames: SC,ASp,CH,CD,P,Ac,ASt,PA,R,F
        // ============================================================
        private double[] GetCurrentRecommendationStats()
        {
            return new double[]
            {
                DataSet.SkillCooldown, DataSet.AttackSpeed, DataSet.CriticalHit, DataSet.CriticalDamage,
                DataSet.Penetration, DataSet.Accuracy, DataSet.AttackStrength, DataSet.PiercingAttack,
                DataSet.Rage, DataSet.Facilitation, DataSet.SkillPower
            };
        }

        private void ApplyRecommendationStats(double[] stats)
        {
            if (stats == null || stats.Length < 11) return;

            DataSet.SkillCooldown  = stats[0];
            DataSet.AttackSpeed    = stats[1];
            DataSet.CriticalHit    = stats[2];
            DataSet.CriticalDamage = stats[3];
            DataSet.Penetration    = stats[4];
            DataSet.Accuracy       = stats[5];
            DataSet.AttackStrength = stats[6];
            DataSet.PiercingAttack = stats[7];
            DataSet.Rage           = stats[8];
            DataSet.Facilitation   = stats[9];
            DataSet.SkillPower     = stats[10];
        }

        private async Task RunRecommendationTestAsync()
        {
            if (_isRecommendationTestRunning) return;

            int runs = Math.Max(1, RecommendationTestRuns);
            RecommendationTestRuns = runs;

            _isRecommendationTestRunning = true;
            _recommendationTestCommand?.RaiseCanExecuteChanged();

            var startStats = GetCurrentRecommendationStats();
            var results = new List<RecommendationRunResult>();

            try
            {
                for (int i = 0; i < runs; i++)
                {
                    ApplyRecommendationStats(startStats);
                    Calculate();
                    Status = $"Тест рекомендаций: {i + 1}/{runs}";

                    var result = await OptimizeByDEAsync(false);
                    results.Add(result);
                }
            }
            finally
            {
                ApplyRecommendationStats(startStats);
                Calculate();
                _isRecommendationTestRunning = false;
                _recommendationTestCommand?.RaiseCanExecuteChanged();
            }

            string filePath = SaveRecommendationTestResults(results);
            Status = $"Тест рекомендаций завершен: {System.IO.Path.GetFileName(filePath)}";
        }

        private string SaveRecommendationTestResults(List<RecommendationRunResult> results)
        {
            var algorithm = CurrentRecommendationAlgorithm;
            string resultsDir = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "results");
            System.IO.Directory.CreateDirectory(resultsDir);

            string fileName = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + "_recommendation_test.txt";
            string filePath = System.IO.Path.Combine(resultsDir, fileName);

            int maxDpm = results.Count > 0 ? results.Max(x => x.Dpm) : 0;
            int minDpm = results.Count > 0 ? results.Min(x => x.Dpm) : 0;
            double avgDpm = results.Count > 0 ? results.Average(x => x.Dpm) : 0;
            double avgTime = results.Count > 0 ? results.Average(x => x.TimeMs) : 0;

            using (var writer = new System.IO.StreamWriter(filePath, false, System.Text.Encoding.UTF8))
            {
                writer.WriteLine("=== BeastMasterCalc | Recommendation Algorithm Test ===");
                writer.WriteLine("Algorithm: " + RecommendationSystem.GetAlgorithmName(algorithm));
                writer.WriteLine("Algorithm code: " + RecommendationSystem.GetAlgorithmCode(algorithm));
                writer.WriteLine("Hyperparameters:");
                if (algorithm == RecommendationAlgorithm.DE)
                {
                    writer.WriteLine("  populationSize = " + RecommendationDePopulationSize);
                    writer.WriteLine("  maxGenerations = " + RecommendationDeMaxGenerations);
                    writer.WriteLine("  F = " + RecommendationDeMutationFactor.ToString("0.###"));
                    writer.WriteLine("  CR = " + RecommendationDeCrossoverRate.ToString("0.###"));
                }
                else
                {
                    writer.WriteLine("  maxIterations = " + RecommendationMctsMaxIterations);
                    writer.WriteLine("  top = " + RecommendationMctsTop);
                    writer.WriteLine("  minSteps = " + RecommendationMctsMinSteps);
                    writer.WriteLine("  maxSteps = " + RecommendationMctsMaxSteps);
                }
                writer.WriteLine("  budgetTolerance = " + RecommendationBudgetTolerance.ToString("0.###"));
                writer.WriteLine();
                writer.WriteLine("Runs: " + results.Count);
                writer.WriteLine("Max DPM: " + maxDpm);
                writer.WriteLine("Min DPM: " + minDpm);
                writer.WriteLine("Avg DPM: " + avgDpm.ToString("0.##"));
                writer.WriteLine("Avg time: " + avgTime.ToString("0.##") + " ms");
                writer.WriteLine();
                writer.WriteLine("=== ALL RUNS ===");
                writer.WriteLine("Run\tDPM\tTimeMs\tCalculateCalls\tSC\tASp\tCH\tCD\tP\tAc\tASt\tPA\tR\tF\tSP");

                for (int i = 0; i < results.Count; i++)
                {
                    var r = results[i];
                    string solution = r.Solution != null
                        ? string.Join("\t", r.Solution.Select(x => x.ToString("F1")))
                        : "";
                    writer.WriteLine($"{i + 1}\t{r.Dpm}\t{r.TimeMs}\t{r.EvalCallCount}\t{solution}");
                }
            }

            return filePath;
        }

        private void SaveResults(
            double[] solution,
            int dd,
            double targetBudget,
            double[] baseStats,
            double[] startStats,
            System.Diagnostics.Stopwatch sw,
            string method,
            int branch,
            bool[] bookFlags
        )
        {
            sw.Stop();
            TimeRec = sw.ElapsedMilliseconds;

            // Веса
            const double wSC  = 0.0315; // SkillCooldown
            const double wASp = 0.0681; // AttackSpeed
            const double wCH  = 0.0588; // CriticalHit
            const double wCD  = 0.1186; // CriticalDamage
            const double wP   = 0.0759; // Penetration
            const double wAc  = 0.0545; // Accuracy
            const double wASt = 0.0949; // AttackStrength
            const double wPA  = 0.0825; // PiercingAttack
            const double wR   = 0.0679; // Rage
            const double wF   = 0.1196; // Facilitation
            const double wSP  = 0.2277; // SkillPower

            double[] weights = { wSC, wASp, wCH, wCD, wP, wAc, wASt, wPA, wR, wF, wSP };
            //double[] weights = { 0.42, 1.07, 0.78, 1.46, 1.24, 0.86, 1.23, 0.97, 0.93, 1.77 };

            int n = solution.Length;

            double gearBudget = 0;
            for (int i = 0; i < n; i++)
                gearBudget += weights[i] * (solution[i] - baseStats[i]);

            // Стартовый бюджет (только шмот+кристаллы) для сравнения
            double startGearBudget = 0;
            if (startStats != null && startStats.Length == n)
            {
                for (int i = 0; i < n; i++)
                    startGearBudget += weights[i] * (startStats[i] - baseStats[i]);
            }

            string resultsDir = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "results");
            System.IO.Directory.CreateDirectory(resultsDir);

            string fileName = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + $"_{method}_opt.txt";
            string filePath = System.IO.Path.Combine(resultsDir, fileName);

            string[] statNames =
            {
        "SkillCooldown", "AttackSpeed", "CriticalHit", "CriticalDamage",
        "Penetration", "Accuracy", "AttackStrength", "PiercingAttack",
        "Rage", "Facilitation", "SkillPower"
    };

            string[] bookNames =
            {
        "BookSkillCooldown(+8)",
        "BookAttackSpeed(+7)",
        "BookCriticalHit(+4)",
        "BookCriticalDamage(+10)",
        "BookPenetration(+3)",
        "BookAccuracy(+4)",
        "BookAttackStrength(+4.7)",
        "BookPiercingAttack(+4)",
        "BookRage(+8)",
        "BookFacilitation(+7.5)"
    };

            using (var writer = new System.IO.StreamWriter(filePath, false, System.Text.Encoding.UTF8))
            {
                writer.WriteLine($"=== BeastMasterCalc | {method.ToUpper()} Optimization ===");
                writer.WriteLine("Execution time: " + TimeRec + " ms");
                writer.WriteLine();

                writer.WriteLine("Branch: " + branch + " (1=GuardianUnity, 2=DualRage, 3=ForestInspiration)");
                writer.WriteLine();

                // Книги: раздельно
                if (bookFlags != null && bookFlags.Length == 10)
                {
                    writer.WriteLine("Books:");
                    for (int i = 0; i < 10; i++)
                        writer.WriteLine($"  {bookNames[i],-26}: {(bookFlags[i] ? "ON" : "OFF")}");
                    writer.WriteLine();
                }

                writer.WriteLine("Target Gear Budget (W): " + targetBudget.ToString("0.###"));
                writer.WriteLine("Actual Gear Budget (W): " + gearBudget.ToString("0.###"));
                writer.WriteLine("Budget Diff: " + System.Math.Abs(gearBudget - targetBudget).ToString("0.###"));
                writer.WriteLine();

                if (startStats != null && startStats.Length == n)
                {
                    writer.WriteLine("Start Gear Budget (W):  " + startGearBudget.ToString("0.###"));
                    writer.WriteLine("ΔBudget (rec-start):    " + (gearBudget - startGearBudget).ToString("0.###"));
                    writer.WriteLine();
                }

                writer.WriteLine("=== BEST SOLUTION ===");
                writer.WriteLine("Best DD = " + dd);
                writer.WriteLine();

                // NEW: разница статов старт/рекомендовано
                if (startStats != null && startStats.Length == n)
                {
                    writer.WriteLine("РАЗНИЦА СТАТОВ (recommended - start):");
                    for (int i = 0; i < n; i++)
                    {
                        double start = startStats[i];
                        double rec = solution[i];
                        double delta = rec - start;

                        writer.WriteLine(
                            $"{statNames[i],-20}: start={start:F1}  rec={rec:F1}  Δ={delta:+0.0;-0.0;+0.0}"
                        );
                    }
                    writer.WriteLine();
                }

                writer.WriteLine("ИТОГОВЫЕ СТАТЫ (база + шмот+кристаллы):");
                for (int i = 0; i < n; i++)
                {
                    writer.WriteLine(
                        $"{statNames[i],-20} = {solution[i]:F1} " +
                        $"(база: {baseStats[i]:F1}, шмот: {(solution[i] - baseStats[i]):F1})"
                    );
                }

                writer.WriteLine();
                writer.WriteLine("КОНТРОЛЬНАЯ СУММА (только шмот+кристаллы):");
                for (int i = 0; i < n; i++)
                {
                    double gear = solution[i] - baseStats[i];
                    double contribution = weights[i] * gear;
                    writer.WriteLine($"{statNames[i],-20}: {gear:F1} × {weights[i]:F4} = {contribution:F3}");
                }
                writer.WriteLine($"{"ИТОГО:",-20}                    = {gearBudget:F3}");
            }
        }


        #endregion

        private RelayCommand getRecommendCommand;
        public ICommand GetRecommendCommand
        {
            get => getRecommendCommand ?? (getRecommendCommand = new RelayCommand(
                () => _ = OptimizeByDEAsync(),
                () => DataSet.SkillCooldown > 0 || DataSet.AttackSpeed > 0 || DataSet.CriticalHit > 0 ||
                      DataSet.CriticalDamage > 0 || DataSet.Penetration > 0 || DataSet.Accuracy > 0 ||
                      DataSet.AttackStrength > 0 || DataSet.PiercingAttack > 0 || DataSet.Rage > 0 ||
                      DataSet.Facilitation > 0 || DataSet.SkillPower > 0));
        }

        private string _selectedRecommendationAlgorithm = "DE";
        public string SelectedRecommendationAlgorithm
        {
            get => _selectedRecommendationAlgorithm;
            set
            {
                _selectedRecommendationAlgorithm = value == "MCTS" ? "MCTS" : "DE";
                NotifyPropertyChanged(nameof(SelectedRecommendationAlgorithm));
            }
        }

        private int _recommendationDePopulationSize = RecommendationSystem.DefaultDePopulationSize;
        public int RecommendationDePopulationSize
        {
            get => _recommendationDePopulationSize;
            set { _recommendationDePopulationSize = Math.Max(4, value); NotifyPropertyChanged(nameof(RecommendationDePopulationSize)); }
        }

        private int _recommendationDeMaxGenerations = RecommendationSystem.DefaultDeMaxGenerations;
        public int RecommendationDeMaxGenerations
        {
            get => _recommendationDeMaxGenerations;
            set { _recommendationDeMaxGenerations = Math.Max(1, value); NotifyPropertyChanged(nameof(RecommendationDeMaxGenerations)); }
        }

        private double _recommendationDeMutationFactor = RecommendationSystem.DefaultDeMutationFactor;
        public double RecommendationDeMutationFactor
        {
            get => _recommendationDeMutationFactor;
            set { _recommendationDeMutationFactor = Math.Max(0.001, value); NotifyPropertyChanged(nameof(RecommendationDeMutationFactor)); }
        }

        private double _recommendationDeCrossoverRate = RecommendationSystem.DefaultDeCrossoverRate;
        public double RecommendationDeCrossoverRate
        {
            get => _recommendationDeCrossoverRate;
            set { _recommendationDeCrossoverRate = Math.Max(0, Math.Min(1, value)); NotifyPropertyChanged(nameof(RecommendationDeCrossoverRate)); }
        }

        private int _recommendationMctsMaxIterations = RecommendationSystem.DefaultMctsMaxIterations;
        public int RecommendationMctsMaxIterations
        {
            get => _recommendationMctsMaxIterations;
            set { _recommendationMctsMaxIterations = Math.Max(1, value); NotifyPropertyChanged(nameof(RecommendationMctsMaxIterations)); }
        }

        private int _recommendationMctsTop = RecommendationSystem.DefaultMctsTop;
        public int RecommendationMctsTop
        {
            get => _recommendationMctsTop;
            set { _recommendationMctsTop = Math.Max(1, value); NotifyPropertyChanged(nameof(RecommendationMctsTop)); }
        }

        private int _recommendationMctsMinSteps = RecommendationSystem.DefaultMctsMinSteps;
        public int RecommendationMctsMinSteps
        {
            get => _recommendationMctsMinSteps;
            set { _recommendationMctsMinSteps = Math.Max(1, value); NotifyPropertyChanged(nameof(RecommendationMctsMinSteps)); }
        }

        private int _recommendationMctsMaxSteps = RecommendationSystem.DefaultMctsMaxSteps;
        public int RecommendationMctsMaxSteps
        {
            get => _recommendationMctsMaxSteps;
            set { _recommendationMctsMaxSteps = Math.Max(RecommendationMctsMinSteps + 1, value); NotifyPropertyChanged(nameof(RecommendationMctsMaxSteps)); }
        }

        private double _recommendationBudgetTolerance = 0.2;
        public double RecommendationBudgetTolerance
        {
            get => _recommendationBudgetTolerance;
            set { _recommendationBudgetTolerance = Math.Max(0, value); NotifyPropertyChanged(nameof(RecommendationBudgetTolerance)); }
        }

        private int _recommendationTestRuns = 10;
        public int RecommendationTestRuns
        {
            get => _recommendationTestRuns;
            set
            {
                _recommendationTestRuns = Math.Max(1, value);
                NotifyPropertyChanged(nameof(RecommendationTestRuns));
            }
        }

        private RelayCommand _recommendationTestCommand;
        public ICommand RecommendationTestCommand
        {
            get => _recommendationTestCommand ?? (_recommendationTestCommand = new RelayCommand(
                () => _ = RunRecommendationTestAsync(),
                () => !_isRecommendationTestRunning &&
                      (DataSet.SkillCooldown > 0 || DataSet.AttackSpeed > 0 || DataSet.CriticalHit > 0 ||
                       DataSet.CriticalDamage > 0 || DataSet.Penetration > 0 || DataSet.Accuracy > 0 ||
                       DataSet.AttackStrength > 0 || DataSet.PiercingAttack > 0 || DataSet.Rage > 0 ||
                       DataSet.Facilitation > 0 || DataSet.SkillPower > 0)));
        }

        private double[] _lastRounded = null;

        private RelayCommand _applyRecResultCommand;
        public ICommand ApplyRecResultCommand
        {
            get => _applyRecResultCommand ?? (_applyRecResultCommand = new RelayCommand(
                ApplyRecResult,
                () => _lastRounded != null));
        }

        private void ApplyRecResult()
        {
            if (_lastRounded == null) return;
            double Round01(double val) => System.Math.Round(val * 10) / 10.0;
            DataSet.SkillCooldown    = Round01(_lastRounded[0]);
            DataSet.AttackSpeed      = Round01(_lastRounded[1]);
            DataSet.CriticalHit      = Round01(_lastRounded[2]);
            DataSet.CriticalDamage   = Round01(_lastRounded[3]);
            DataSet.Penetration      = Round01(_lastRounded[4]);
            DataSet.Accuracy         = Round01(_lastRounded[5]);
            DataSet.AttackStrength   = Round01(_lastRounded[6]);
            DataSet.PiercingAttack   = Round01(_lastRounded[7]);
            DataSet.Rage             = Round01(_lastRounded[8]);
            DataSet.Facilitation     = Round01(_lastRounded[9]);
            DataSet.SkillPower       = Round01(_lastRounded[10]);
            NotifyPropertyChanged(nameof(SkillCooldown));
            NotifyPropertyChanged(nameof(AttackSpeed));
            NotifyPropertyChanged(nameof(CriticalHit));
            NotifyPropertyChanged(nameof(CriticalDamage));
            NotifyPropertyChanged(nameof(Penetration));
            NotifyPropertyChanged(nameof(Accuracy));
            NotifyPropertyChanged(nameof(AttackStrength));
            NotifyPropertyChanged(nameof(PiercingAttack));
            NotifyPropertyChanged(nameof(Rage));
            NotifyPropertyChanged(nameof(Facilitation));
            NotifyPropertyChanged(nameof(SkillPower));
            Calculate();
        }

        private long timeRec = 0;
        public long TimeRec
        {
            get => timeRec;
            set { timeRec = value; NotifyPropertyChanged(nameof(TimeRec)); }
        }

        private long _recCalcCount = 0;
        public long RecCalcCount
        {
            get => _recCalcCount;
            set { _recCalcCount = value; NotifyPropertyChanged(nameof(RecCalcCount)); }
        }
        private string status = "Отключен";
        public string Status
        {
            get => status;
            set
            {
                status = value;
                NotifyPropertyChanged(nameof(Status));
            }
        }

        #region СППР настройки

        private int _guildLevel = 0;
        public int GuildLevel
        {
            get => _guildLevel;
            set { _guildLevel = Math.Max(0, Math.Min(12, value)); NotifyPropertyChanged(nameof(GuildLevel)); RefreshRecBaseDisplay(); }
        }

        private bool _recHasBookSC;
        public bool RecHasBookSC { get => _recHasBookSC; set { _recHasBookSC = value; NotifyPropertyChanged(nameof(RecHasBookSC)); RefreshRecBaseDisplay(); } }
        private bool _recHasBookASp;
        public bool RecHasBookASp { get => _recHasBookASp; set { _recHasBookASp = value; NotifyPropertyChanged(nameof(RecHasBookASp)); RefreshRecBaseDisplay(); } }
        private bool _recHasBookCH;
        public bool RecHasBookCH { get => _recHasBookCH; set { _recHasBookCH = value; NotifyPropertyChanged(nameof(RecHasBookCH)); RefreshRecBaseDisplay(); } }
        private bool _recHasBookCD;
        public bool RecHasBookCD { get => _recHasBookCD; set { _recHasBookCD = value; NotifyPropertyChanged(nameof(RecHasBookCD)); RefreshRecBaseDisplay(); } }
        private bool _recHasBookP;
        public bool RecHasBookP { get => _recHasBookP; set { _recHasBookP = value; NotifyPropertyChanged(nameof(RecHasBookP)); RefreshRecBaseDisplay(); } }
        private bool _recHasBookAc;
        public bool RecHasBookAc { get => _recHasBookAc; set { _recHasBookAc = value; NotifyPropertyChanged(nameof(RecHasBookAc)); RefreshRecBaseDisplay(); } }
        private bool _recHasBookASt;
        public bool RecHasBookASt { get => _recHasBookASt; set { _recHasBookASt = value; NotifyPropertyChanged(nameof(RecHasBookASt)); RefreshRecBaseDisplay(); } }
        private bool _recHasBookPA;
        public bool RecHasBookPA { get => _recHasBookPA; set { _recHasBookPA = value; NotifyPropertyChanged(nameof(RecHasBookPA)); RefreshRecBaseDisplay(); } }
        private bool _recHasBookR;
        public bool RecHasBookR { get => _recHasBookR; set { _recHasBookR = value; NotifyPropertyChanged(nameof(RecHasBookR)); RefreshRecBaseDisplay(); } }
        private bool _recHasBookF;
        public bool RecHasBookF { get => _recHasBookF; set { _recHasBookF = value; NotifyPropertyChanged(nameof(RecHasBookF)); RefreshRecBaseDisplay(); } }

        // Бонусы гильдии нарастающим итогом по уровню
        // 0=SC, 1=ASp, 2=CH, 3=CD, 4=P, 5=Ac, 6=ASt, 7=PA, 8=R, 9=F, 10=SP
        private static double[] GetGuildStatBonuses(int level)
        {
            double sc = 0, asp = 0, ch = 0, cd = 0, p = 0, ac = 0, ast = 0, pa = 0, r = 0, f = 0, sp = 0;
            if (level <= 0)  return new double[] { sc, asp, ch, cd, p, ac, ast, pa, r, f, sp };
            if (level >= 8)  { ch += 6; p += 6; }
            if (level >= 9)  { cd += 20; }
            if (level >= 10) { sc += 15; asp += 15; }
            if (level >= 11) { ac += 7; }
            return new double[] { sc, asp, ch, cd, p, ac, ast, pa, r, f, sp };
        }

        private double[] ComputeRecBaseStats()
        {
            bool hasBranch = DataSet.DualRageActive || DataSet.ForestInspirationActive || DataSet.GuardianUnityActive;
            int branch = DataSet.DualRageActive ? 2 : DataSet.ForestInspirationActive ? 3 : 1;
            double tASp = hasBranch ? ((branch == 2) ? 5.75 : 4.25) : 0;
            double tCH  = hasBranch ? 4.75 : 0;
            double tCD  = hasBranch ? ((branch == 2) ? 3.0  : 1.5)  : 0;
            double tSC  = hasBranch ? ((branch == 2) ? 4.25 : 5.75) : 0;
            double tP   = hasBranch ? ((branch == 3) ? 2.75 : 3.75) : 0;
            double tAc  = hasBranch ? ((branch == 1) ? 4.75 : 3.5)  : 0;

            double bSC  = RecHasBookSC  ? 8   : 0;
            double bASp = RecHasBookASp ? 7   : 0;
            double bCH  = RecHasBookCH  ? 3   : 0;
            double bCD  = RecHasBookCD  ? 10  : 0;
            double bP   = RecHasBookP   ? 3   : 0;
            double bAc  = RecHasBookAc  ? 4   : 0;
            double bASt = RecHasBookASt ? 4.7 : 0;
            double bPA  = RecHasBookPA  ? 4   : 0;
            double bR   = RecHasBookR   ? 8   : 0;
            double bF   = RecHasBookF   ? 7.5 : 0;

            double[] g = GetGuildStatBonuses(GuildLevel);
            double cFlat = CastleStartModifierActive ? 5 : 0;
            double cSP   = coefficientCastleStart != 0 ? System.Math.Round((coefficientCastleStart - 1) * 100, 1) : 0;
            return new double[]
            {
                tSC  + bSC  + g[0],
                tASp + bASp + g[1],
                5 + tCH + bCH + g[2],
                tCD + bCD + g[3],
                tP  + bP  + g[4],
                tAc + bAc + g[5],
                bASt + g[6],
                bPA  + g[7],
                bR   + g[8],
                bF   + g[9],
                g[10] - cSP
            };
        }

        private void RefreshRecBaseDisplay()
        {
            var b = ComputeRecBaseStats();
            RecBaseSC  = b[0].ToString("F1");
            RecBaseASp = b[1].ToString("F1");
            RecBaseCH  = b[2].ToString("F1");
            RecBaseCD  = b[3].ToString("F1");
            RecBaseP   = b[4].ToString("F1");
            RecBaseAc  = b[5].ToString("F1");
            RecBaseASt = b[6].ToString("F1");
            RecBasePA  = b[7].ToString("F1");
            RecBaseR   = b[8].ToString("F1");
            RecBaseF   = b[9].ToString("F1");
            RecBaseSP  = b[10].ToString("F1");
        }
        #region свойства для полей в рекомендации
        private string _recBaseSC  = "0"; public string RecBaseSC  { get => _recBaseSC;  private set { _recBaseSC  = value; NotifyPropertyChanged(nameof(RecBaseSC));  } }
        private string _recBaseASp = "0"; public string RecBaseASp { get => _recBaseASp; private set { _recBaseASp = value; NotifyPropertyChanged(nameof(RecBaseASp)); } }
        private string _recBaseCH  = "0"; public string RecBaseCH  { get => _recBaseCH;  private set { _recBaseCH  = value; NotifyPropertyChanged(nameof(RecBaseCH));  } }
        private string _recBaseCD  = "0"; public string RecBaseCD  { get => _recBaseCD;  private set { _recBaseCD  = value; NotifyPropertyChanged(nameof(RecBaseCD));  } }
        private string _recBaseP   = "0"; public string RecBaseP   { get => _recBaseP;   private set { _recBaseP   = value; NotifyPropertyChanged(nameof(RecBaseP));   } }
        private string _recBaseAc  = "0"; public string RecBaseAc  { get => _recBaseAc;  private set { _recBaseAc  = value; NotifyPropertyChanged(nameof(RecBaseAc));  } }
        private string _recBaseASt = "0"; public string RecBaseASt { get => _recBaseASt; private set { _recBaseASt = value; NotifyPropertyChanged(nameof(RecBaseASt)); } }
        private string _recBasePA  = "0"; public string RecBasePA  { get => _recBasePA;  private set { _recBasePA  = value; NotifyPropertyChanged(nameof(RecBasePA));  } }
        private string _recBaseR   = "0"; public string RecBaseR   { get => _recBaseR;   private set { _recBaseR   = value; NotifyPropertyChanged(nameof(RecBaseR));   } }
        private string _recBaseF   = "0"; public string RecBaseF   { get => _recBaseF;   private set { _recBaseF   = value; NotifyPropertyChanged(nameof(RecBaseF));   } }
        private string _recBaseSP  = "0"; public string RecBaseSP  { get => _recBaseSP;  private set { _recBaseSP  = value; NotifyPropertyChanged(nameof(RecBaseSP));  } }

        private double _userMinSC = 0;
        public double UserMinSC { get => _userMinSC; set { _userMinSC = value; NotifyPropertyChanged(nameof(UserMinSC)); } }
        private double _userMinASp = 0;
        public double UserMinASp { get => _userMinASp; set { _userMinASp = value; NotifyPropertyChanged(nameof(UserMinASp)); } }
        private double _userMinCH = 30;
        public double UserMinCH { get => _userMinCH; set { _userMinCH = value; NotifyPropertyChanged(nameof(UserMinCH)); } }
        private double _userMinCD = 0;
        public double UserMinCD { get => _userMinCD; set { _userMinCD = value; NotifyPropertyChanged(nameof(UserMinCD)); } }
        private double _userMinP = 24;
        public double UserMinP { get => _userMinP; set { _userMinP = value; NotifyPropertyChanged(nameof(UserMinP)); } }
        private double _userMinAc = 0;
        public double UserMinAc { get => _userMinAc; set { _userMinAc = value; NotifyPropertyChanged(nameof(UserMinAc)); } }
        private double _userMinASt = 0;
        public double UserMinASt { get => _userMinASt; set { _userMinASt = value; NotifyPropertyChanged(nameof(UserMinASt)); } }
        private double _userMinPA = 0;
        public double UserMinPA { get => _userMinPA; set { _userMinPA = value; NotifyPropertyChanged(nameof(UserMinPA)); } }
        private double _userMinR = 8.1;
        public double UserMinR { get => _userMinR; set { _userMinR = value; NotifyPropertyChanged(nameof(UserMinR)); } }
        private double _userMinF = 0;
        public double UserMinF { get => _userMinF; set { _userMinF = value; NotifyPropertyChanged(nameof(UserMinF)); } }
        private double _userMinSP = 0;
        public double UserMinSP { get => _userMinSP; set { _userMinSP = value; NotifyPropertyChanged(nameof(UserMinSP)); } }

        private string _recValSC = "—";
        public string RecValSC { get => _recValSC; private set { _recValSC = value; NotifyPropertyChanged(nameof(RecValSC)); } }
        private string _recValASp = "—";
        public string RecValASp { get => _recValASp; private set { _recValASp = value; NotifyPropertyChanged(nameof(RecValASp)); } }
        private string _recValCH = "—";
        public string RecValCH { get => _recValCH; private set { _recValCH = value; NotifyPropertyChanged(nameof(RecValCH)); } }
        private string _recValCD = "—";
        public string RecValCD { get => _recValCD; private set { _recValCD = value; NotifyPropertyChanged(nameof(RecValCD)); } }
        private string _recValP = "—";
        public string RecValP { get => _recValP; private set { _recValP = value; NotifyPropertyChanged(nameof(RecValP)); } }
        private string _recValAc = "—";
        public string RecValAc { get => _recValAc; private set { _recValAc = value; NotifyPropertyChanged(nameof(RecValAc)); } }
        private string _recValASt = "—";
        public string RecValASt { get => _recValASt; private set { _recValASt = value; NotifyPropertyChanged(nameof(RecValASt)); } }
        private string _recValPA = "—";
        public string RecValPA { get => _recValPA; private set { _recValPA = value; NotifyPropertyChanged(nameof(RecValPA)); } }
        private string _recValR = "—";
        public string RecValR { get => _recValR; private set { _recValR = value; NotifyPropertyChanged(nameof(RecValR)); } }
        private string _recValF = "—";
        public string RecValF { get => _recValF; private set { _recValF = value; NotifyPropertyChanged(nameof(RecValF)); } }
        private string _recValSP = "—";
        public string RecValSP { get => _recValSP; private set { _recValSP = value; NotifyPropertyChanged(nameof(RecValSP)); } }

        private string _deltaSC = "";
        public string DeltaSC { get => _deltaSC; private set { _deltaSC = value; NotifyPropertyChanged(nameof(DeltaSC)); } }
        private string _deltaASp = "";
        public string DeltaASp { get => _deltaASp; private set { _deltaASp = value; NotifyPropertyChanged(nameof(DeltaASp)); } }
        private string _deltaCH = "";
        public string DeltaCH { get => _deltaCH; private set { _deltaCH = value; NotifyPropertyChanged(nameof(DeltaCH)); } }
        private string _deltaCD = "";
        public string DeltaCD { get => _deltaCD; private set { _deltaCD = value; NotifyPropertyChanged(nameof(DeltaCD)); } }
        private string _deltaP = "";
        public string DeltaP { get => _deltaP; private set { _deltaP = value; NotifyPropertyChanged(nameof(DeltaP)); } }
        private string _deltaAc = "";
        public string DeltaAc { get => _deltaAc; private set { _deltaAc = value; NotifyPropertyChanged(nameof(DeltaAc)); } }
        private string _deltaASt = "";
        public string DeltaASt { get => _deltaASt; private set { _deltaASt = value; NotifyPropertyChanged(nameof(DeltaASt)); } }
        private string _deltaPA = "";
        public string DeltaPA { get => _deltaPA; private set { _deltaPA = value; NotifyPropertyChanged(nameof(DeltaPA)); } }
        private string _deltaR = "";
        public string DeltaR { get => _deltaR; private set { _deltaR = value; NotifyPropertyChanged(nameof(DeltaR)); } }
        private string _deltaF = "";
        public string DeltaF { get => _deltaF; private set { _deltaF = value; NotifyPropertyChanged(nameof(DeltaF)); } }
        private string _deltaSP = "";
        public string DeltaSP { get => _deltaSP; private set { _deltaSP = value; NotifyPropertyChanged(nameof(DeltaSP)); } }

        private string _recDPMDisplay = "—";
        public string RecDPMDisplay { get => _recDPMDisplay; private set { _recDPMDisplay = value; NotifyPropertyChanged(nameof(RecDPMDisplay)); } }

        private bool _isRecFinalVisible = false;
        public bool IsRecFinalVisible { get => _isRecFinalVisible; private set { _isRecFinalVisible = value; NotifyPropertyChanged(nameof(IsRecFinalVisible)); } }

        private string _recFinalSC  = "0.0"; public string RecFinalSC  { get => _recFinalSC;  private set { _recFinalSC  = value; NotifyPropertyChanged(nameof(RecFinalSC));  } }
        private string _recFinalASp = "0.0"; public string RecFinalASp { get => _recFinalASp; private set { _recFinalASp = value; NotifyPropertyChanged(nameof(RecFinalASp)); } }
        private string _recFinalCH  = "0.0"; public string RecFinalCH  { get => _recFinalCH;  private set { _recFinalCH  = value; NotifyPropertyChanged(nameof(RecFinalCH));  } }
        private string _recFinalCD  = "0.0"; public string RecFinalCD  { get => _recFinalCD;  private set { _recFinalCD  = value; NotifyPropertyChanged(nameof(RecFinalCD));  } }
        private string _recFinalP   = "0.0"; public string RecFinalP   { get => _recFinalP;   private set { _recFinalP   = value; NotifyPropertyChanged(nameof(RecFinalP));   } }
        private string _recFinalAc  = "0.0"; public string RecFinalAc  { get => _recFinalAc;  private set { _recFinalAc  = value; NotifyPropertyChanged(nameof(RecFinalAc));  } }
        private string _recFinalASt = "0.0"; public string RecFinalASt { get => _recFinalASt; private set { _recFinalASt = value; NotifyPropertyChanged(nameof(RecFinalASt)); } }
        private string _recFinalPA  = "0.0"; public string RecFinalPA  { get => _recFinalPA;  private set { _recFinalPA  = value; NotifyPropertyChanged(nameof(RecFinalPA));  } }
        private string _recFinalR   = "0.0"; public string RecFinalR   { get => _recFinalR;   private set { _recFinalR   = value; NotifyPropertyChanged(nameof(RecFinalR));   } }
        private string _recFinalF   = "0.0"; public string RecFinalF   { get => _recFinalF;   private set { _recFinalF   = value; NotifyPropertyChanged(nameof(RecFinalF));   } }
        private string _recFinalSP  = "0.0"; public string RecFinalSP  { get => _recFinalSP;  private set { _recFinalSP  = value; NotifyPropertyChanged(nameof(RecFinalSP));  } }

        private static string FormatDelta(double d) => d >= 0 ? $"+{d:F1}" : $"{d:F1}";
        #endregion

        #region поля для статистики дпм
        private int dpmAttack = 0;
        public int DpmAttack
        {
            get => dpmAttack;
            set
            {
                dpmAttack = value;
                NotifyPropertyChanged(nameof(DpmAttack));
            }
        }
        private int dpmMoonTouch = 0;
        public int DpmMoonTouch
        {
            get => dpmMoonTouch;
            set
            {
                dpmMoonTouch = value;
                NotifyPropertyChanged(nameof(DpmMoonTouch));
            }
        }
        private int dpmChainLightning = 0;
        public int DpmChainLightning
        {
            get => dpmChainLightning;
            set
            {
                dpmChainLightning = value;
                NotifyPropertyChanged(nameof(DpmChainLightning));
            }
        }
        private int dpmMoonLight = 0;
        public int DpmMoonLight
        {
            get => dpmMoonLight;
            set
            {
                dpmMoonLight = value;
                NotifyPropertyChanged(nameof(DpmMoonLight));
            }
        }
        private int dpmBeastAwakening = 0;
        public int DpmBeastAwakening
        {
            get => dpmBeastAwakening;
            set
            {
                dpmBeastAwakening = value;
                NotifyPropertyChanged(nameof(DpmBeastAwakening));
            }
        }
        private int dpmBestialRampage = 0;
        public int DpmBestialRampage
        {
            get => dpmBestialRampage;
            set
            {
                dpmBestialRampage = value;
                NotifyPropertyChanged(nameof(DpmBestialRampage));
            }
        }
        private int dpmOrderToAttack = 0;
        public int DpmOrderToAttack
        {
            get => dpmOrderToAttack;
            set
            {
                dpmOrderToAttack = value;
                NotifyPropertyChanged(nameof(DpmOrderToAttack));
            }
        }
        private int dpmAuraOfTheForestHero = 0;
        public int DpmAuraOfTheForestHero
        {
            get => dpmAuraOfTheForestHero;
            set
            {
                dpmAuraOfTheForestHero = value;
                NotifyPropertyChanged(nameof(DpmAuraOfTheForestHero));
            }
        }
        private int dpmAuraOfTheForestLuna = 0;
        public int DpmAuraOfTheForestLuna
        {
            get => dpmAuraOfTheForestLuna;
            set
            {
                dpmAuraOfTheForestLuna = value;
                NotifyPropertyChanged(nameof(DpmAuraOfTheForestLuna));
            }
        }
        private int dpmSymbiosisLuna = 0;
        public int DpmSymbiosisLuna
        {
            get => dpmSymbiosisLuna;
            set
            {
                dpmSymbiosisLuna = value;
                NotifyPropertyChanged(nameof(DpmSymbiosisLuna));
            }
        }
        private int dpmSymbiosisHero = 0;
        public int DpmSymbiosisHero
        {
            get => dpmSymbiosisHero;
            set
            {
                dpmSymbiosisHero = value;
                NotifyPropertyChanged(nameof(DpmSymbiosisHero));
            }
        }

        #endregion
        #endregion

        #endregion





    }
}
