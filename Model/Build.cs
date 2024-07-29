using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Serializable]
    public class Build : INotifyPropertyChanged
    {
        #region Данные о сборке

        public string Name { get; set; } = $"New Data Set {DateTime.Now.ToString()}";
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
        public double SkillCooldown { get; set; }
        public double AttackSpeed { get; set; }
        public double CriticalHit { get; set; }
        public double CriticalDamage { get; set; }
        public double Penetration { get; set; }
        public double Accuracy { get; set; }
        public double AttackStrength { get; set; }
        public double PiercingAttack { get; set; }
        public double Rage { get; set; }
        public double Facilitation { get; set; }
        public double Protection { get; set; }
        public double Dodge { get; set; }
        public double Resilience { get; set; }
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

        #endregion

        #region Конечные бафы

        public bool GuildDamageModifierActive = false;
        public bool CastleSwordActive = false;
        public bool TalentDamageModifierActive = false;
        public string NumberCastle { get; set; } = "Без замка";
        public bool BPDungeon { get; set; } = false;
        public bool SacredShieldHeroActive = false;
        public bool SacredShieldLunaActive = false;
        public bool GodsAid = false;

        #endregion

        #region шмот

        public string SelectedAmulet = "0%";
        public string SelectedCloak = "0%";
        public string SelectedRingL = "0%";
        public string SelectedRingR = "0%";
        public string SelectedBraceletL = "0%";
        public string SelectedBraceletR = "0%";
        public string SelectedSet = "0%";
        
        public string SelectedHelmet = "Empty";
        public string SelectedBody = "Empty";
        public string SelectedHands = "Empty";
        public string SelectedBelt = "Empty";
        public string SelectedFoots = "Empty";

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


        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Метод для вызова события PropertyChanged
        /// </summary>
        /// <param name="prop">Имя свойства, которое изменилось</param>
        public void NotifyPropertyChanged([CallerMemberName] string prop = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
