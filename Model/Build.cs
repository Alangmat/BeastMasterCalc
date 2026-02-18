using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Shared;

namespace Model
{
    [Serializable]
    public class Build //: INotifyPropertyChanged
    {
        #region Данные о сборке

        public string Name { get; set; } = $"New Data Set";
        public string Description { get; set; } = string.Empty;
        public Guid ID { get; set; } = Guid.NewGuid();
        public string LastDate { get; set; } = DateTime.Now.ToString();


        public int ResultDD { get; set; }


        #endregion

        #region skills
        public Attack Attack { get; set; }
        public MoonTouch MoonTouch { get; set; }
        public BeastAwakening BeastAwakening { get; set; }
        public OrderToAttack OrderToAttack { get; set; }
        public ChainLightning ChainLightning { get; set; }
        public AuraOfTheForest AuraOfTheForest { get; set; }
        public BestialRampage BestialRampage { get; set; }
        public Moonlight Moonlight { get; set; }
        public BlessingOfTheMoon BlessingOfTheMoon { get; set; } = new BlessingOfTheMoon();
        public DoubleConcentration DoubleConcentration { get; set; } = new DoubleConcentration();
        #endregion
        #region Stats
        public string MagicalDamage { get; set; }
        public double PercentMagicalDD { get; set; } = 0;
        public string PhysicalDamage { get; set; }
        public double PercentPhysicalDD { get; set; } = 0;
        public double SkillCooldown { get; set; } = 0;
        public double AttackSpeed { get; set; } = 0;
        public double CriticalHit { get; set; } = 0;
        public double CriticalDamage { get; set; } = 0;
        public double Penetration { get; set; } = 0;    
        public double Accuracy { get; set; } = 0;
        public double AttackStrength { get; set; } = 0;
        public double PiercingAttack { get; set; } = 0;
        public double Rage { get; set; } = 0;
        public double Facilitation { get; set; } = 0;
        public double DepthsFury { get; set; } = 0;
        public double Protection { get; set; } = 0;
        public double Dodge { get; set; } = 0;
        public double Resilience { get; set; } = 0;
        public double SkillPower { get; set; } = 0;
        #endregion
        #region StatsOfPot
        public double SkillCooldownPot { get; set; } = 0;
        public double AttackSpeedPot { get; set; } = 0;
        public double CriticalHitPot { get; set; } = 0;
        public double CriticalDamagePot { get; set; } = 0;
        public double PenetrationPot { get; set; } = 0;
        public double AccuracyPot { get; set; } = 0;
        public double AttackStrengthPot { get; set; } = 0;
        public double PiercingAttackPot { get; set; } = 0;
        public double RagePot { get; set; } = 0;
        public double FacilitationPot { get; set; } = 0;
        #endregion
        #region StatsOfScroll
        public double SkillCooldownScroll { get; set; } = 0;
        public double AttackSpeedScroll { get; set; } = 0;
        public double CriticalHitScroll { get; set; } = 0;
        public double CriticalDamageScroll { get; set; } = 0;
        public double PenetrationScroll { get; set; } = 0;
        public double AccuracyScroll { get; set; } = 0;
        public double AttackStrengthScroll { get; set; } = 0;
        public double PiercingAttackScroll { get; set; } = 0;
        public double RageScroll { get; set; } = 0;
        public double FacilitationScroll { get; set; } = 0;
        public double DepthsFuryScroll { get; set; } = 0;
        #endregion
        #region StatsOfPet
        public double SkillCooldownPet { get; set; } = 0;
        public double AttackSpeedPet { get; set; } = 0;
        public double CriticalDamagePet { get; set; } = 0;
        public double PenetrationPet { get; set; } = 0;
        public double AccuracyPet { get; set; } = 0;
        public double AttackStrengthPet { get; set; } = 0;
        public double RagePet { get; set; } = 0;
        public double FacilitationPet { get; set; } = 0;
        #endregion
        #region other
        #region тритоны

        public bool CrushingWill { get; set; } = false;
        public bool IrreversibleAnger { get; set; } = false;
        #endregion
        public bool Counterstand = false;
        #region исходные бафы

        public bool GuildDamageStartModifierActive = false;
        public bool CastleStartModifierActive = false;
        public bool TalentDamageStartModifierActive = false;
        public bool HarmoniousPowerStartModifierActive = false;
        public double AdditionalPercentMDDStart = 0;
        public double AdditionalPercentPDDStart = 0;
        public double AdditionalPercentMDDFinal = 0;
        public double AdditionalPercentPDDFinal = 0;

        #endregion

        #region Конечные бафы

        public bool GuildDamageModifierActive = false;
        public bool CastleSwordActive = false;
        public bool TalentDamageModifierActive = false;
        //public string NumberCastle { get; set; } = "Без замка";
        public CastleSectors SelectedCastle = CastleSectors.Empty;
        public bool BPDungeon { get; set; } = false;
        public bool SacredShieldHeroActive = false;
        public bool SacredShieldLunaActive = false;
        public bool GodsAid = false;
        public bool GodsAidLuna = false;

        public bool PairingTalentAlmahadActive = false;
        public bool RoarTalentAlmahadActive = false;
        public bool PredatoryBondTalentAlmahadActive = false;

        #endregion

        #region шмот

        public PercentsDamage SelectedAmuletNew = PercentsDamage.None;
        public PercentsDamage SelectedCloakNew = PercentsDamage.None;
        public PercentsDamage SelectedRingLNew = PercentsDamage.None;
        public PercentsDamage SelectedRingRNew = PercentsDamage.None;
        public PercentsDamage SelectedBraceletLNew = PercentsDamage.None;
        public PercentsDamage SelectedBraceletRNew = PercentsDamage.None;
        public PercentsDamage SelectedSetNew = PercentsDamage.None;

        public TypesEquipment SelectedHelmetNew = TypesEquipment.None;
        public TypesEquipment SelectedBodyNew = TypesEquipment.None;
        public TypesEquipment SelectedHandsNew = TypesEquipment.None;
        public TypesEquipment SelectedBeltNew = TypesEquipment.None;
        public TypesEquipment SelectedFootsNew = TypesEquipment.None;

        #endregion




        #endregion
        #region active

        #region ветки
        #region 3 Ветка
        public bool ForestInspirationActive { get; set; }
        public bool HasTalantGrandeurOfTheLotus { get; set; }

        #endregion

        #region 2 ветка
        public bool DualRageActive { get; set; }
        public bool HasTalantSymbiosis { get; set; }

        #endregion
        #region 1 ветка

        public bool GuardianUnityActive { get; set; } = false;
        public bool HasTalentHarmoniousPower { get; set; } = false;


        #endregion
        #endregion
        #region общие таланты

        public int LvlTalantBestialRage { get; set; }
        public int LvlTalantPredatoryDelirium { get; set; }
        public int LvlTalantAnimalRage { get; set; }
        public int LvlTalantMomentOfPower { get; set; }
        public int LvlTalantLongDeath { get; set; }

        #endregion
        #region ивенты
        public int LvlTalantContinuousFury { get; set; }
        #endregion

        #region скиллы

        public bool AttackActive { get; set; }
        public bool MoonTouchActive { get; set; }
        public bool BeastAwakeningActive { get; set; }
        public bool OrderToAttackActive { get; set; }
        public bool HealingActive { get; set; }
        public bool ChainLightningActive { get; set; }
        public bool BestialRampageActive { get; set; }
        public bool AuraOfTheForestActive { get; set; }
        public bool MoonlightPermanentActive { get; set; }
        public bool MoonlightNonPermanentActive { get; set; }
        public bool BlessingOfTheMoonActive { get; set; }
        public bool IsUsingBlessingOfTheMoonOnLuna { get; set; }
        public bool DoubleConcentrationActive { get; set; }

        #endregion
        #region weapon
        public bool StaffSelected { get; set; }
        public bool SpearSelected { get; set; }
        public bool MaceSelected { get; set; } = true;
        public bool SwordSelected { get; set; }
        public bool AxeSelected { get; set; }

        #endregion
        #endregion


        public bool OverLimit = false;

        //public event PropertyChangedEventHandler PropertyChanged;
        ///// <summary>
        ///// Метод для вызова события PropertyChanged
        ///// </summary>
        ///// <param name="prop">Имя свойства, которое изменилось</param>
        //public void NotifyPropertyChanged([CallerMemberName] string prop = "") =>
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        [OnDeserialized]
        internal void OnDeserialized(StreamingContext _)
        {
            if (OrderToAttack != null)
                OrderToAttack.Luna = this.BeastAwakening;
            if (BestialRampage != null)
                BestialRampage.Luna = BeastAwakening;
        }
    }
}
