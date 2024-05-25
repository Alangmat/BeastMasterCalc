using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ViewModel;

namespace View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.ResizeMode = ResizeMode.CanMinimize;
            Logic = new ViewModel.ViewModel();
            DataContext = Logic;
            LoadHints();


            Load();
        }
        private void LoadHints()
        {
            string basicSkillPath = "hints/basicSkillHint.txt";
            if (File.Exists(basicSkillPath))
            {
                string text = File.ReadAllText(basicSkillPath);
                basicSkillHintText.Text = text;
            }
            else
            {
                basicSkillHintText.Text = "Файл не найден.";
            }
            string moonlightPath = "hints/moonlightHint.txt";
            if (File.Exists(moonlightPath))
            {
                string text = File.ReadAllText(moonlightPath);
                moonlightHintText.Text = text;
            }
            else
            {
                moonlightHintText.Text = "Файл не найден.";
            }
            string temporaryPath = "hints/temporaryHint.txt";
            if (File.Exists(temporaryPath))
            {
                string text = File.ReadAllText(temporaryPath);
                temporaryHintText.Text = text;
            }
            else
            {
                temporaryHintText.Text = "Файл не найден.";
            }
            string ddPath = "hints/ddHint.txt";
            if (File.Exists(ddPath))
            {
                string text = File.ReadAllText(ddPath);
                ddHintText.Text = text;
            }
            else
            {
                ddHintText.Text = "Файл не найден.";
            }
            string mermenPath = "hints/mermenHint.txt";
            if (File.Exists(mermenPath))
            {
                string text = File.ReadAllText(mermenPath);
                mermenHintText.Text = text;
            }
            else
            {
                mermenHintText.Text = "Файл не найден.";
            }
            string blessingPath = "hints/blessingHint.txt";
            if (File.Exists(blessingPath))
            {
                string text = File.ReadAllText(blessingPath);
                blessingHintText.Text = text;
            }
            else
            {
                blessingHintText.Text = "Файл не найден.";
            }
            string procentPath = "hints/procentHint.txt";
            if (File.Exists(procentPath))
            {
                string text = File.ReadAllText(procentPath);
                procentHintText.Text = text;
            }
            else
            {
                procentHintText.Text = "Файл не найден.";
            }
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            Logic.SaveBuilds();
        }

        private void addAndGenerateDataSetButton_Click(object sender, RoutedEventArgs e)
        {
            Logic.AddDataSet();
            Load();
            Logic.SaveBuilds();
        }
        private void editDataSetButton_Click(object sender, RoutedEventArgs e)
        {
            if (Logic.EditDataSet() == 0)
            {
                Load();
                Logic.SaveBuilds();
            }
            else
            {
                MessageBox.Show("Для начала создайте запись данного билда");
            }
        }
        private void addDataSetButton_Click(object sender, RoutedEventArgs e)
        {
            Logic.AddCurrentDataSet();
            Load();
            Logic.SaveBuilds();
        }
        private void Load()
        {
            #region Выключение всех самописных кнопок в виде радиобатанов при запуске

            updateStateButton(Logic.CrushingWillActive, crushingWillButton);
            updateStateButton(Logic.IrreversibleAngerActive, irreversibleAngerButton);
            //crushingWillButton.Opacity = nonActiveOpacity;

            #region Показатель активности скиллов

            updateStateButton(Logic.MoonTouchActive, moonTouchButton);
            updateStateButton(Logic.BeastAwakeningActive, beastAwakeningButton);
            updateStateButton(Logic.OrderToAttackActive, orderToAttackButton);
            updateStateButton(Logic.HealingActive, healingButton);
            updateStateButton(Logic.ChainLightningActive, chainLightningButton);

            updateStateButton(Logic.BestialRampageActive, bestialRampageButton);
            updateStateButton(Logic.AuraOfTheForestActive, auraOfTheForestButton);
            updateStateButton(Logic.MoonlightPermanentActive, moonlightPermanentButton);
            updateStateButton(Logic.MoonlightNonPermanentActive, moonlightNonPermanentButton);
            updateStateButton(Logic.BlessingOfTheMoonActive, blessingOfTheMoonButton);
            updateStateButton(Logic.DoubleConcentrationActive, doubleConcentrationButton);

            #endregion

            #region вывод уровня талантов

            updateStateButtonTalant(Logic.LvlTalantContinuousFury, continuousFuryTalantButton);
            updateLvlThree(Logic.LvlTalantContinuousFury, continuousFuryLvlIcon);

            updateStateButtonTalant(Logic.LvlTalantAnimalRage, animalRageTalantButton);
            updateLvlThree(Logic.LvlTalantAnimalRage, animalRageLvlIcon);
            updateStateButtonTalant(Logic.LvlTalantBestialRage, bestialRageTalantButton);
            updateLvlThree(Logic.LvlTalantBestialRage, bestialRageLvlIcon);
            updateStateButtonTalant(Logic.LvlTalantPredatoryDelirium, predatoryDeliriumTalantButton);
            updateLvlThree(Logic.LvlTalantPredatoryDelirium, predatoryDeliriumLvlIcon);

            updateStateButtonTalant(Logic.LvlTalantMoonlightPlus, moonlightPlusTalantButton);
            updateLvlThree(Logic.LvlTalantMoonlightPlus, moonlightPlusLvlIcon);

            

            updateStateButtonTalant(Logic.LvlTalantOrderToAttackPlusDualRage, orderToAttackTalantButton);
            updateLvlThree(Logic.LvlTalantOrderToAttackPlusDualRage, orderToAttackPlusLvlIcon);

            updateStateButtonTalant(Logic.LvlTalantBeastAwakeningPhysical, beastAwakeningPlusPhysicalTalantButton);
            updateLvlThree(Logic.LvlTalantBeastAwakeningPhysical, beastAwakeningPlusPhysicalLvlIcon);


            updateStateButtonTalant(Logic.LvlTalantMomentOfPower, momentOfPowerTalantButton);
            updateLvlFour(Logic.LvlTalantMomentOfPower, momentOfPowerLvlIcon);
            updateStateButtonTalant(Logic.LvlTalantLongDeath, longDeathTalantButton);
            updateLvlFour(Logic.LvlTalantLongDeath, longDeathLvlIcon);


            #region 1 ветка

            updateStateButtonTalant(Logic.LvlTalantOrderToAttackPlusGuardianUnity, orderToAttackGuardianUnityTalantButton);
            updateLvlThree(Logic.LvlTalantOrderToAttackPlusGuardianUnity, orderToAttackPlusGuardianUnityLvlIcon);

            #endregion
            #endregion

            updateStateButton(Logic.HasTalantGrandeurOfTheLotus, grandeurOfTheLotusTalantButton);
            updateStateButton(Logic.HasTalantSymbiosis, symbiosisTalantButton);
            //grandeurOfTheLotusTalantButton.Opacity = nonActiveOpacity;


            imageAuraOfTheForestButton.Source = auraOfTheForestIcon;


            updateStateButton(Logic.HasTalantMoonTouchPlus, moonTouchPlusTalantButton);
            //moonTouchPlusTalantButton.Opacity = nonActiveOpacity;
            updateStateButton(Logic.HasTalantPowerOfNature, powerOfNatureTalantButton);
            //powerOfNatureTalantButton.Opacity = nonActiveOpacity;
            updateStateButton(Logic.HasTalantBeastAwakeningMage, beastAwakeningPlusMagicalTalantButton);
            //beastAwakeningPlusMagicalTalantButton.Opacity = nonActiveOpacity;
            updateStateButton(Logic.ForestInspirationActive, forestInspirationActiveButton);
            //forestInspirationActiveButton.Opacity = nonActiveOpacity;
            updateStateButton(Logic.HasTalantBestialRampage, bestialRampageTalantButton);
            updateStateButton(Logic.DualRageActive, dualRageActiveButton);

            //moonlightPlusTalantButton.Opacity = nonActiveOpacity;




            updateStateButtonFiveLvl(Logic.LvlMoonTouch, moonTouchLvlIcon);
            //moonTouchLvlIcon.Source = lvlOneOfTheFiveIcon;
            updateStateButtonFiveLvl(Logic.LvlChainLightning, chainLightningLvlIcon);
            //chainLightningLvlIcon.Source = lvlOneOfTheFiveIcon;
            updateStateButtonFiveLvl(Logic.LvlBeastAwakening, beastAwakeningLvlIcon);
            //beastAwakeningLvlIcon.Source = lvlOneOfTheFiveIcon;

            updateStateButtonFiveLvl(Logic.LvlOrderToAttack, orderToAttackLvlIcon);

            updateStateButtonFourLvl(Logic.LvlBestialRampage, bestialRampageLvlIcon);
            //bestialRampageLvlIcon.Source = lvlOneOfTheFourIcon;
            updateStateButtonFourLvl(Logic.LvlAuraOfTheForest, auraOfTheForestLvlIcon);
            //auraOfTheForestLvlIcon.Source = lvlOneOfTheFourIcon;
            updateStateButtonFourLvl(Logic.LvlMoonlight, moonlightLvlIcon);
            //moonlightLvlIcon.Source = lvlOneOfTheFourIcon;
            moonlightLvlIcon_Copy.Source = moonlightLvlIcon.Source;

            updateStateButtonFourLvl(Logic.LvlBlessingOfTheMoon, blessingOfTheMoonLvlIcon);
            updateStateButtonFourLvl(Logic.LvlDoubleConcentration, doubleConcentrationLvlIcon);


            updateIconAuraOfTheForest();
            #endregion
        }

        private void choiceDataSet_Click(object sender, RoutedEventArgs e)
        {
            Logic.ChoiceDataSet();
            Load();
            Logic.SaveBuilds();
        }

        private void deleteDataSetButton_Click(object sender, RoutedEventArgs e)
        {
            Logic.DeleteSelectedDataSet();
            Logic.SaveBuilds();
        }

        #region Методы изменения состояния кнопок

        private void updateStateButton(bool flag, Button button)
        {
            if (flag)
            {
                button.Opacity = activeOpacity;
            }
            else button.Opacity = nonActiveOpacity;
        }
        private void updateStateButtonTalant(int lvl, Button button)
        {
            if (lvl < 1) 
            {
                button.Opacity = nonActiveOpacity;
            }
            else
            {
                button.Opacity = activeOpacity;
            }
        }
        private void updateStateButtonFiveLvl(int lvl, Image image)
        {
            switch (lvl)
            {
                case 1:
                    image.Source = lvlOneOfTheFiveIcon;
                    break;
                case 2:
                    image.Source =  lvlTwoOfTheFiveIcon; break;
                case 3: 
                    image.Source = lvlThreeOfTheFiveIcon; break;
                case 4:
                    image.Source = lvlFourOfTheFiveIcon; break;
                case 5:
                    image.Source = lvlFiveOfTheFiveIcon;
                    break;
            }
        }
        private void updateStateButtonFourLvl(int lvl, Image image)
        {
            switch (lvl)
            {
                case 1:
                    image.Source = lvlOneOfTheFourIcon; break;
                case 2:
                    image.Source= lvlTwoOfTheFourIcon; break;
                case 3:
                    image.Source = lvlThreeOfTheFourIcon; break;
                case 4:
                    image.Source = lvlFourOfTheFourIcon;
                    break;
            }
        }

        private void decreaseLvlThree(int lvl, Button icon, Image iconLvl)
        {
            updateStateButtonTalant(lvl - 1, icon);
            switch(lvl)
            {
                case 1:
                    iconLvl.Source = null; break;
                case 2:
                    iconLvl.Source = lvlOneOfTheThreeIcon; break;
                case 3:
                    iconLvl.Source = lvlTwoOfTheThreeIcon; break;
            }
        }
        private void increaseLvlThree(int lvl, Button icon, Image iconLvl)
        {
            icon.Opacity = activeOpacity;
            switch (lvl)
            {
                case 0:
                    iconLvl.Source = lvlOneOfTheThreeIcon; break;
                case 1:
                    iconLvl.Source = lvlTwoOfTheThreeIcon; break;
                case 2:
                    iconLvl.Source = lvlThreeOfTheThreeIcon; break;
            }
        }
        private void updateLvlThree(int lvl, Image iconLvl)
        {
            switch(lvl)
            {
                case 0:
                    iconLvl.Source = null; break;
                case 1:
                    iconLvl.Source = lvlOneOfTheThreeIcon; break;
                case 2:
                    iconLvl.Source = lvlTwoOfTheThreeIcon; break;
                case 3:
                    iconLvl.Source = lvlThreeOfTheThreeIcon; break;
            }
        }
        private void decreaseLvlFour(int lvl, Button icon, Image iconLvl)
        {
            updateStateButtonTalant(lvl - 1, icon);
            switch (lvl)
            {
                case 1:
                    iconLvl.Source = null; break;
                case 2:
                    iconLvl.Source = lvlOneOfTheFourIcon; break;
                case 3:
                    iconLvl.Source = lvlTwoOfTheFourIcon; break;
                case 4:
                    iconLvl.Source = lvlThreeOfTheFourIcon; break;
            }
        }
        private void increaseLvlFour(int lvl, Button icon, Image iconLvl)
        {
            icon.Opacity = activeOpacity;
            switch (lvl)
            {
                case 0:
                    iconLvl.Source = lvlOneOfTheFourIcon; break;
                case 1:
                    iconLvl.Source = lvlTwoOfTheFourIcon; break;
                case 2:
                    iconLvl.Source = lvlThreeOfTheFourIcon; break;
                case 3:
                    iconLvl.Source = lvlFourOfTheFourIcon; break;
            }
        }
        private void updateLvlFour(int lvl, Image iconLvl)
        {
            switch (lvl)
            {
                case 0:
                    iconLvl.Source = null; break;
                case 1:
                    iconLvl.Source = lvlOneOfTheFourIcon; break;
                case 2:
                    iconLvl.Source = lvlTwoOfTheFourIcon; break;
                case 3:
                    iconLvl.Source = lvlThreeOfTheFourIcon; break;
                case 4:
                    iconLvl.Source = lvlFourOfTheFourIcon; break;
            }
        }

        private void updateIconAuraOfTheForest()
        {
            if (Logic.HasTalantGrandeurOfTheLotus)
            {
                imageAuraOfTheForestButton.Source = grandeurOfTheLotusIcon;
            }
            else if (Logic.HasTalantPowerOfNature)
            {
                imageAuraOfTheForestButton.Source = powerOfNatureIcon;
            }
            else
                imageAuraOfTheForestButton.Source = auraOfTheForestIcon;
        }
        #endregion

        private ViewModel.ViewModel Logic;

        #region пути к иконкам
        private BitmapImage auraOfTheForestIcon = new BitmapImage(new Uri("/icons/expert/AuraOfTheForestFramed.png", UriKind.Relative));
        private BitmapImage grandeurOfTheLotusIcon = new BitmapImage(new Uri("/icons/expert/AuraOfTheForestUpgradeFramed.png", UriKind.Relative));
        private BitmapImage powerOfNatureIcon = new BitmapImage(new Uri("/icons/expert/AuraOfTheForestChoiceFramed.png", UriKind.Relative));
        
        //private BitmapImage testLvlIcon = new BitmapImage(new Uri("/icons/numbers/1.4.png",UriKind.Relative));

        #endregion

        #region Видимость элементов
        private double activeOpacity = 1;
        private double nonActiveOpacity = 0.3;
        #endregion

        #region Уровни

        private BitmapImage lvlOneOfTheThreeIcon = new BitmapImage(new Uri("/icons/numbers/1.3.png", UriKind.Relative));
        private BitmapImage lvlTwoOfTheThreeIcon = new BitmapImage(new Uri("/icons/numbers/2.3.png", UriKind.Relative));
        private BitmapImage lvlThreeOfTheThreeIcon = new BitmapImage(new Uri("/icons/numbers/3.3.png", UriKind.Relative));

        private BitmapImage lvlOneOfTheFourIcon = new BitmapImage(new Uri("/icons/numbers/1.4.png", UriKind.Relative));
        private BitmapImage lvlTwoOfTheFourIcon = new BitmapImage(new Uri("/icons/numbers/2.4.png", UriKind.Relative));
        private BitmapImage lvlThreeOfTheFourIcon = new BitmapImage(new Uri("/icons/numbers/3.4.png", UriKind.Relative));
        private BitmapImage lvlFourOfTheFourIcon = new BitmapImage(new Uri("/icons/numbers/4.4.png", UriKind.Relative));

        private BitmapImage lvlOneOfTheFiveIcon = new BitmapImage(new Uri("/icons/numbers/1.5.png", UriKind.Relative));
        private BitmapImage lvlTwoOfTheFiveIcon = new BitmapImage(new Uri("/icons/numbers/2.5.png", UriKind.Relative));
        private BitmapImage lvlThreeOfTheFiveIcon = new BitmapImage(new Uri("/icons/numbers/3.5.png", UriKind.Relative));
        private BitmapImage lvlFourOfTheFiveIcon = new BitmapImage(new Uri("/icons/numbers/4.5.png", UriKind.Relative));
        private BitmapImage lvlFiveOfTheFiveIcon = new BitmapImage(new Uri("/icons/numbers/5.5.png", UriKind.Relative));


        #endregion

        #region обработчики нажатий кнопок базовых скиллов
        private void moonTouchButton_Clicked(object sender, RoutedEventArgs e)
        {
            Logic.MoonTouchActive = !Logic.MoonTouchActive;

            updateStateButton(Logic.MoonTouchActive, moonTouchButton);
        }
        private void beastAwakeningButton_Click(object sender, RoutedEventArgs e)
        {
            Logic.BeastAwakeningActive = !Logic.BeastAwakeningActive;

            updateStateButton(Logic.BeastAwakeningActive, beastAwakeningButton);

            if (!Logic.BeastAwakeningActive) 
            {
                moonlightPermanentButton_Click(this, e);
                if (Logic.BestialRampageActive)
                {
                    bestialRampageButton_Click(this, e);
                }
                if (Logic.OrderToAttackActive)
                {
                    orderToAttackButton_Click(this, e);
                }
            }
        }

        private void chainLightningButton_Click(object sender, RoutedEventArgs e)
        {
            Logic.ChainLightningActive = !Logic.ChainLightningActive;

            updateStateButton(Logic.ChainLightningActive, chainLightningButton);
        }

        private void orderToAttackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Logic.BeastAwakeningActive)
            {
                Logic.OrderToAttackActive = !Logic.OrderToAttackActive;

                updateStateButton(Logic.OrderToAttackActive, orderToAttackButton);

            }
            else
            {
                Logic.OrderToAttackActive = false;

                updateStateButton(Logic.OrderToAttackActive, orderToAttackButton);
            }
        }

        private void healingButton_Click(object sender, RoutedEventArgs e)
        {
            Logic.HealingActive = !Logic.HealingActive;

            updateStateButton(Logic.HealingActive, healingButton);
        }

        private void bestialRampageButton_Click(object sender, RoutedEventArgs e)
        {
            if (Logic.BeastAwakeningActive)
            {
                Logic.BestialRampageActive = !Logic.BestialRampageActive;
            }
            else
            {
                Logic.BestialRampageActive = false;

            }
            updateStateButton(Logic.BestialRampageActive, bestialRampageButton);
        }
        #endregion

        #region обработчики экспертных скиллов
        private void auraOfTheForestButton_Click(object sender, RoutedEventArgs e)
        {
            Logic.AuraOfTheForestActive = !Logic.AuraOfTheForestActive;

            updateStateButton(Logic.AuraOfTheForestActive, auraOfTheForestButton);
        }

        private void moonlightPermanentButton_Click(object sender, RoutedEventArgs e)
        {
            if (!Logic.BeastAwakeningActive)
            {
                Logic.MoonlightPermanentActive = false;
                moonlightPermanentButton.Opacity = nonActiveOpacity;
            }
            else
            {
                Logic.MoonlightPermanentActive = !Logic.MoonlightPermanentActive;
                updateStateButton(Logic.MoonlightPermanentActive, moonlightPermanentButton);
                
            }

        }

        private void moonlightNonPermanentButton_Click(object sender, RoutedEventArgs e)
        {
            Logic.MoonlightNonPermanentActive = !Logic.MoonlightNonPermanentActive;
            updateStateButton(Logic.MoonlightNonPermanentActive, moonlightNonPermanentButton);
        }

        private void blessingOfTheMoonButton_Click(object sender, RoutedEventArgs e)
        {
            Logic.BlessingOfTheMoonActive = !Logic.BlessingOfTheMoonActive;
            updateStateButton(Logic.BlessingOfTheMoonActive, blessingOfTheMoonButton);
        }
        private void doubleConcentrationButton_Click(object sender, RoutedEventArgs e)
        {
            Logic.DoubleConcentrationActive = !Logic.DoubleConcentrationActive;
            updateStateButton(Logic.DoubleConcentrationActive, doubleConcentrationButton);
        }
        #endregion

        #region обработчик тритонов
        private void crushingWillButton_Click(object sender, RoutedEventArgs e)
        {
            Logic.CrushingWillActive = !Logic.CrushingWillActive;
            updateStateButton(Logic.CrushingWillActive, crushingWillButton);
        }
        private void irreversibleAngerButton_Click(object sender, RoutedEventArgs e)
        {
            Logic.IrreversibleAngerActive = !Logic.IrreversibleAngerActive;
            updateStateButton(Logic.IrreversibleAngerActive, irreversibleAngerButton);
        }
        #endregion


        private void moonTouchPlusTalantButton_Click(object sender, RoutedEventArgs e)
        {
            Logic.HasTalantMoonTouchPlus = !Logic.HasTalantMoonTouchPlus;

            updateStateButton(Logic.HasTalantMoonTouchPlus, moonTouchPlusTalantButton);
        }

        private void powerOfNatureTalantButton_Click(object sender, RoutedEventArgs e)
        {
            Logic.HasTalantPowerOfNature = !Logic.HasTalantPowerOfNature;

            if (Logic.HasTalantPowerOfNature)
            {
                powerOfNatureTalantButton.Opacity = activeOpacity;
                if (!Logic.HasTalantGrandeurOfTheLotus)
                {

                    imageAuraOfTheForestButton.Source = powerOfNatureIcon;
                    //imageAuraOfTheForestButton.Source = testLvlIcon;
                }
            }
            else 
            {
                if (!Logic.HasTalantGrandeurOfTheLotus) imageAuraOfTheForestButton.Source = auraOfTheForestIcon;
                powerOfNatureTalantButton.Opacity = nonActiveOpacity;
            }

        }

        #region 3 ветка
        private void forestInspirationActiveButton_Click(object sender, RoutedEventArgs e)
        {
            Logic.ForestInspirationActive = !Logic.ForestInspirationActive;
            if (!Logic.ForestInspirationActive) moonlightPlusLvlIcon.Source = null;
            updateStateButton(Logic.ForestInspirationActive, forestInspirationActiveButton);
            updateStateButton(Logic.HasTalantGrandeurOfTheLotus, grandeurOfTheLotusTalantButton);
            updateStateButton(Logic.HasTalantBeastAwakeningMage, beastAwakeningPlusMagicalTalantButton);
            updateStateButtonTalant(Logic.LvlTalantMoonlightPlus - 1, moonlightPlusTalantButton);
            if (Logic.HasTalantPowerOfNature)
            {
                imageAuraOfTheForestButton.Source = powerOfNatureIcon;
            }
            else imageAuraOfTheForestButton.Source = auraOfTheForestIcon;
            if (Logic.DualRageActive && Logic.ForestInspirationActive)
            {
                dualRageActiveButton_Click(sender, e);
            }
        }
        private void grandeurOfTheLotusTalantButton_Click(object sender, RoutedEventArgs e)
        {
            if (Logic.ForestInspirationActive)
            {
                Logic.HasTalantGrandeurOfTheLotus = !Logic.HasTalantGrandeurOfTheLotus;

                if (Logic.HasTalantGrandeurOfTheLotus)
                {
                    grandeurOfTheLotusTalantButton.Opacity = activeOpacity;
                    imageAuraOfTheForestButton.Source = grandeurOfTheLotusIcon;
                }
                else
                {
                    grandeurOfTheLotusTalantButton.Opacity = nonActiveOpacity;
                    if (Logic.HasTalantPowerOfNature)
                    {
                        imageAuraOfTheForestButton.Source = powerOfNatureIcon;
                    }
                    else imageAuraOfTheForestButton.Source = auraOfTheForestIcon;
                }
            }
        }
        private void decreaseLvlMoonlightPlusTalantButton_Click(object sender, RoutedEventArgs e)
        {
            if (Logic.ForestInspirationActive)
            {
                updateStateButtonTalant(Logic.LvlTalantMoonlightPlus - 1, moonlightPlusTalantButton);
                switch (Logic.LvlTalantMoonlightPlus)
                {
                    case 1:
                        moonlightPlusLvlIcon.Source = null; break;
                    case 2:
                        moonlightPlusLvlIcon.Source = lvlOneOfTheThreeIcon; break;
                    case 3:
                        moonlightPlusLvlIcon.Source = lvlTwoOfTheThreeIcon; break;
                }
            }
        }

        private void increaseLvlMoonlightPlusTalantButton_Click(object sender, RoutedEventArgs e)
        {
            if (Logic.ForestInspirationActive)
            {
                moonlightPlusTalantButton.Opacity = activeOpacity;
                switch(Logic.LvlTalantMoonlightPlus)
                {
                    case 0:
                        moonlightPlusLvlIcon.Source = lvlOneOfTheThreeIcon; break;
                    case 1:
                        moonlightPlusLvlIcon.Source = lvlTwoOfTheThreeIcon; break;
                    case 2:
                        moonlightPlusLvlIcon.Source = lvlThreeOfTheThreeIcon; break;
                }
            }
        }

        private void beastAwakeningPlusMagicalTalantButton_Click(object sender, RoutedEventArgs e)
        {
            if (Logic.ForestInspirationActive)
            {
                Logic.HasTalantBeastAwakeningMage = !Logic.HasTalantBeastAwakeningMage;
                updateStateButton(Logic.HasTalantBeastAwakeningMage, beastAwakeningPlusMagicalTalantButton);
            }
        }
        #endregion

        #region 2 ветка
        private void bestialRampageTalantButton_Click(object sender, RoutedEventArgs e)
        {
            if (Logic.DualRageActive)
            {
                Logic.HasTalantBestialRampage = !Logic.HasTalantBestialRampage;

                updateStateButton(Logic.HasTalantBestialRampage, bestialRampageTalantButton);
            }
        }
        private void decreaseLvlBeastAwakeningPlusPhysicalTalantButton_Click(object sender, RoutedEventArgs e)
        {
            if (Logic.DualRageActive)
            {

                decreaseLvlThree(Logic.LvlTalantBeastAwakeningPhysical, beastAwakeningPlusPhysicalTalantButton, beastAwakeningPlusPhysicalLvlIcon);
                /*updateStateButtonTalant(Logic.LvlTalantBeastAwakeningPhysical, beastAwakeningPlusPhysicalTalantButton);
                switch (Logic.LvlTalantBeastAwakeningPhysical)
                {
                    case 1:
                        beastAwakeningPlusPhysicalLvlIcon.Source = null; break;
                    case 2:
                        beastAwakeningPlusPhysicalLvlIcon.Source = lvlOneOfTheThreeIcon; break;
                    case 3:
                        beastAwakeningPlusPhysicalLvlIcon.Source = lvlTwoOfTheThreeIcon; break;
                }*/
            }

        }
        private void increaseLvlBeastAwakeningPlusPhysicalTalantButton_Click(object sender, RoutedEventArgs e)
        {
            if (Logic.DualRageActive)
            {
                increaseLvlThree(Logic.LvlTalantBeastAwakeningPhysical, beastAwakeningPlusPhysicalTalantButton, beastAwakeningPlusPhysicalLvlIcon);

                /*beastAwakeningPlusPhysicalTalantButton.Opacity = activeOpacity;
                switch (Logic.LvlTalantBeastAwakeningPhysical)
                {
                    case 0:
                        beastAwakeningPlusPhysicalLvlIcon.Source = lvlOneOfTheThreeIcon; break;
                    case 1:
                        beastAwakeningPlusPhysicalLvlIcon.Source = lvlTwoOfTheThreeIcon; break;
                    case 2:
                        beastAwakeningPlusPhysicalLvlIcon.Source = lvlThreeOfTheThreeIcon; break;
                }*/
            }
        }

        private void dualRageActiveButton_Click(object sender, RoutedEventArgs e)
        {
            Logic.DualRageActive = !Logic.DualRageActive;
            if (!Logic.DualRageActive)
            {
                beastAwakeningPlusPhysicalLvlIcon.Source = null;
                orderToAttackPlusLvlIcon.Source = null;
            }
            updateStateButton(Logic.DualRageActive, dualRageActiveButton);
            updateStateButton(Logic.HasTalantBestialRampage, bestialRampageTalantButton);
            updateStateButtonTalant(Logic.LvlTalantBeastAwakeningPhysical - 1, beastAwakeningPlusPhysicalTalantButton);
            updateStateButtonTalant(Logic.LvlTalantOrderToAttackPlusDualRage - 1, orderToAttackTalantButton);
            updateStateButton(Logic.HasTalantSymbiosis, symbiosisTalantButton);
            if (Logic.DualRageActive && Logic.ForestInspirationActive)
            {
                forestInspirationActiveButton_Click(sender, e);
            }
        }


        private void increaseLvlOrderToAttackPlusTalantButton_Click(object sender, RoutedEventArgs e)
        {
            if (Logic.DualRageActive)
                increaseLvlThree(Logic.LvlTalantOrderToAttackPlusDualRage, orderToAttackTalantButton, orderToAttackPlusLvlIcon);
        }
        private void decreaseLvlOrderToAttackPlusTalantButton_Click(object sender, RoutedEventArgs e)
        {
            if (Logic.DualRageActive)
                decreaseLvlThree(Logic.LvlTalantOrderToAttackPlusDualRage, orderToAttackTalantButton, orderToAttackPlusLvlIcon);
        }

        private void symbiosisTalantButton_Click(object sender, RoutedEventArgs e)
        {
            if (Logic.DualRageActive)
            {
                Logic.HasTalantSymbiosis = !Logic.HasTalantSymbiosis;

                updateStateButton(Logic.HasTalantSymbiosis, symbiosisTalantButton);
            }
        }

        #endregion

        private void decreaseLvlBestialRageTalantButton_Click(object sender, RoutedEventArgs e)
        {
            decreaseLvlThree(Logic.LvlTalantBestialRage, bestialRageTalantButton, bestialRageLvlIcon);
        }

        private void increaseLvlBestialRageTalantButton_Click(object sender, RoutedEventArgs e)
        {
            increaseLvlThree(Logic.LvlTalantBestialRage, bestialRageTalantButton, bestialRageLvlIcon);
        }

        private void decreaseLvlPredatoryDeliriumTalantButton_Click(object sender, RoutedEventArgs e)
        {
            decreaseLvlThree(Logic.LvlTalantPredatoryDelirium, predatoryDeliriumTalantButton, predatoryDeliriumLvlIcon);
        }

        private void increaseLvlPredatoryDeliriumTalantButton_Click(object sender, RoutedEventArgs e)
        {
            increaseLvlThree(Logic.LvlTalantPredatoryDelirium, predatoryDeliriumTalantButton, predatoryDeliriumLvlIcon);
        }

        private void decreaseLvlMomentOfPowerTalantButton_Click(object sender, RoutedEventArgs e)
        {
            decreaseLvlFour(Logic.LvlTalantMomentOfPower, momentOfPowerTalantButton, momentOfPowerLvlIcon);
        }

        private void increaseLvlMomentOfPowerTalantButton_Click(object sender, RoutedEventArgs e)
        {
            increaseLvlFour(Logic.LvlTalantMomentOfPower, momentOfPowerTalantButton, momentOfPowerLvlIcon);
        }

        private void decreaseLvlLongDeathTalantButton_Click(object sender, RoutedEventArgs e)
        {
            decreaseLvlFour(Logic.LvlTalantLongDeath, longDeathTalantButton, longDeathLvlIcon);
        }

        private void increaseLvlLongDeathTalantButton_Click(object sender, RoutedEventArgs e)
        {
            increaseLvlFour(Logic.LvlTalantLongDeath, longDeathTalantButton, longDeathLvlIcon);
        }



        private void increaseLvlMoonTouchButton_Click(object sender, RoutedEventArgs e)
        {
            switch(Logic.LvlMoonTouch)
            {
                case 0:
                    moonTouchLvlIcon.Source = lvlOneOfTheFiveIcon; break;
                case 1:
                    moonTouchLvlIcon.Source = lvlTwoOfTheFiveIcon; break;
                case 2:
                    moonTouchLvlIcon.Source = lvlThreeOfTheFiveIcon; break;
                case 3:
                    moonTouchLvlIcon.Source = lvlFourOfTheFiveIcon; break;
                case 4:
                    moonTouchLvlIcon.Source = lvlFiveOfTheFiveIcon; break;
            }
        }

        private void decreaseLvlMoonTouchButton_Click(object sender, RoutedEventArgs e)
        {
            switch (Logic.LvlMoonTouch)
            {
                case 2:
                    moonTouchLvlIcon.Source = lvlOneOfTheFiveIcon; break;
                case 3:
                    moonTouchLvlIcon.Source = lvlTwoOfTheFiveIcon; break;
                case 4:
                    moonTouchLvlIcon.Source = lvlThreeOfTheFiveIcon; break;
                case 5:
                    moonTouchLvlIcon.Source = lvlFourOfTheFiveIcon; break;
            }
        }

        private void decreaseLvlChainLightningButton_Click(object sender, RoutedEventArgs e)
        {
            switch (Logic.LvlChainLightning)
            {
                case 2:
                    chainLightningLvlIcon.Source = lvlOneOfTheFiveIcon; break;
                case 3:
                    chainLightningLvlIcon.Source = lvlTwoOfTheFiveIcon; break;
                case 4:
                    chainLightningLvlIcon.Source = lvlThreeOfTheFiveIcon; break;
                case 5:
                    chainLightningLvlIcon.Source = lvlFourOfTheFiveIcon; break;
            }
        }

        private void increaseLvlChainLightningButton_Click(object sender, RoutedEventArgs e)
        {
            switch (Logic.LvlChainLightning)
            {
                case 0:
                    chainLightningLvlIcon.Source = lvlOneOfTheFiveIcon; break;
                case 1:
                    chainLightningLvlIcon.Source = lvlTwoOfTheFiveIcon; break;
                case 2:
                    chainLightningLvlIcon.Source = lvlThreeOfTheFiveIcon; break;
                case 3:
                    chainLightningLvlIcon.Source = lvlFourOfTheFiveIcon; break;
                case 4:
                    chainLightningLvlIcon.Source = lvlFiveOfTheFiveIcon; break;
            }
        }

        private void decreaseLvlBeastAwakeningButton_Click(object sender, RoutedEventArgs e)
        {
            switch (Logic.LvlBeastAwakening)
            {
                case 2:
                    beastAwakeningLvlIcon.Source = lvlOneOfTheFiveIcon; break;
                case 3:
                    beastAwakeningLvlIcon.Source = lvlTwoOfTheFiveIcon; break;
                case 4:
                    beastAwakeningLvlIcon.Source = lvlThreeOfTheFiveIcon; break;
                case 5:
                    beastAwakeningLvlIcon.Source = lvlFourOfTheFiveIcon; break;
            }
        }

        private void increaseLvlBeastAwakeningButton_Click(object sender, RoutedEventArgs e)
        {
            switch (Logic.LvlBeastAwakening)
            {
                case 0:
                    beastAwakeningLvlIcon.Source = lvlOneOfTheFiveIcon; break;
                case 1:
                    beastAwakeningLvlIcon.Source = lvlTwoOfTheFiveIcon; break;
                case 2:
                    beastAwakeningLvlIcon.Source = lvlThreeOfTheFiveIcon; break;
                case 3:
                    beastAwakeningLvlIcon.Source = lvlFourOfTheFiveIcon; break;
                case 4:
                    beastAwakeningLvlIcon.Source = lvlFiveOfTheFiveIcon; break;
            }
        }

        private void decreaseLvlBestialRampageButton_Click(object sender, RoutedEventArgs e)
        {
            switch (Logic.LvlBestialRampage)
            {
                case 2:
                    bestialRampageLvlIcon.Source = lvlOneOfTheFourIcon; break;
                case 3:
                    bestialRampageLvlIcon.Source = lvlTwoOfTheFourIcon; break;
                case 4:
                    bestialRampageLvlIcon.Source = lvlThreeOfTheFourIcon; break;
            }
        }

        private void increaseLvlBestialRampageButton_Click(object sender, RoutedEventArgs e)
        {
            switch (Logic.LvlBestialRampage)
            {
                case 0:
                    bestialRampageLvlIcon.Source = lvlOneOfTheFourIcon; break;
                case 1:
                    bestialRampageLvlIcon.Source = lvlTwoOfTheFourIcon; break;
                case 2:
                    bestialRampageLvlIcon.Source = lvlThreeOfTheFourIcon; break;
                case 3:
                    bestialRampageLvlIcon.Source = lvlFourOfTheFourIcon; break;
            }
        }

        private void decreaseLvlAuraOfTheForestButton_Click(object sender, RoutedEventArgs e)
        {
            switch (Logic.LvlAuraOfTheForest)
            {
                case 2:
                    auraOfTheForestLvlIcon.Source = lvlOneOfTheFourIcon; break;
                case 3:
                    auraOfTheForestLvlIcon.Source = lvlTwoOfTheFourIcon; break;
                case 4:
                    auraOfTheForestLvlIcon.Source = lvlThreeOfTheFourIcon; break;
            }
        }

        private void increaseLvlAuraOfTheForestButton_Click(object sender, RoutedEventArgs e)
        {
            switch (Logic.LvlAuraOfTheForest)
            {
                case 0:
                    auraOfTheForestLvlIcon.Source = lvlOneOfTheFourIcon; break;
                case 1:
                    auraOfTheForestLvlIcon.Source = lvlTwoOfTheFourIcon; break;
                case 2:
                    auraOfTheForestLvlIcon.Source = lvlThreeOfTheFourIcon; break;
                case 3:
                    auraOfTheForestLvlIcon.Source = lvlFourOfTheFourIcon; break;
            }
        }

        private void increaseLvlMoonlightButton_Click(object sender, RoutedEventArgs e)
        {
            switch (Logic.LvlMoonlight)
            {
                case 0:
                    moonlightLvlIcon.Source = lvlOneOfTheFourIcon;
                    moonlightLvlIcon_Copy.Source = moonlightLvlIcon.Source;
                    break;
                case 1:
                    moonlightLvlIcon.Source = lvlTwoOfTheFourIcon;
                    moonlightLvlIcon_Copy.Source = moonlightLvlIcon.Source;
                    break;
                case 2:
                    moonlightLvlIcon.Source = lvlThreeOfTheFourIcon;
                    moonlightLvlIcon_Copy.Source = moonlightLvlIcon.Source;
                    break;
                case 3:
                    moonlightLvlIcon.Source = lvlFourOfTheFourIcon;
                    moonlightLvlIcon_Copy.Source = moonlightLvlIcon.Source;
                    break;
            }
        }

        private void decreaseLvlMoonlightButton_Click(object sender, RoutedEventArgs e)
        {
            switch (Logic.LvlMoonlight)
            {
                case 2:
                    moonlightLvlIcon.Source = lvlOneOfTheFourIcon;
                    moonlightLvlIcon_Copy.Source = moonlightLvlIcon.Source;
                    break;
                case 3:
                    moonlightLvlIcon.Source = lvlTwoOfTheFourIcon;
                    moonlightLvlIcon_Copy.Source = moonlightLvlIcon.Source;
                    break;
                case 4:
                    moonlightLvlIcon.Source = lvlThreeOfTheFourIcon;
                    moonlightLvlIcon_Copy.Source = moonlightLvlIcon.Source;
                    break;
            }
        }
        private void increaseLvlBlessingOfTheMoonButton_Click(object sender, RoutedEventArgs e)
        {
            switch (Logic.LvlBlessingOfTheMoon)
            {
                case 0:
                    blessingOfTheMoonLvlIcon.Source = lvlOneOfTheFourIcon;
                    break;
                case 1:
                    blessingOfTheMoonLvlIcon.Source = lvlTwoOfTheFourIcon;
                    break;
                case 2:
                    blessingOfTheMoonLvlIcon.Source = lvlThreeOfTheFourIcon;
                    break;
                case 3:
                    blessingOfTheMoonLvlIcon.Source = lvlFourOfTheFourIcon;
                    break;
            }
        }
        private void decreaseLvlBlessingOfTheMoonButton_Click(object sender, RoutedEventArgs e)
        {
            switch (Logic.LvlBlessingOfTheMoon)
            {
                case 2:
                    blessingOfTheMoonLvlIcon.Source = lvlOneOfTheFourIcon;
                    break;
                case 3:
                    blessingOfTheMoonLvlIcon.Source = lvlTwoOfTheFourIcon;
                    break;
                case 4:
                    blessingOfTheMoonLvlIcon.Source = lvlThreeOfTheFourIcon;
                    break;
            }
        }
        private void increaseLvlDoubleConcentrationButton_Click(object sender, RoutedEventArgs e)
        {
            switch (Logic.LvlDoubleConcentration)
            {
                case 0:
                    doubleConcentrationLvlIcon.Source = lvlOneOfTheFourIcon;
                    break;
                case 1:
                    doubleConcentrationLvlIcon.Source = lvlTwoOfTheFourIcon;
                    break;
                case 2:
                    doubleConcentrationLvlIcon.Source = lvlThreeOfTheFourIcon;
                    break;
                case 3:
                    doubleConcentrationLvlIcon.Source = lvlFourOfTheFourIcon;
                    break;
            }
        }

        private void decreaseLvlDoubleConcentrationButton_Click(object sender, RoutedEventArgs e)
        {
            switch (Logic.LvlDoubleConcentration)
            {
                case 2:
                    doubleConcentrationLvlIcon.Source = lvlOneOfTheFourIcon;
                    break;
                case 3:
                    doubleConcentrationLvlIcon.Source = lvlTwoOfTheFourIcon;
                    break;
                case 4:
                    doubleConcentrationLvlIcon.Source = lvlThreeOfTheFourIcon;
                    break;
            }
        }


        private void increaseLvlOrderToAttackButton_Click(object sender, RoutedEventArgs e)
        {
            switch (Logic.LvlOrderToAttack)
            {
                case 1:
                    orderToAttackLvlIcon.Source = lvlTwoOfTheFiveIcon; break;
                case 2:
                    orderToAttackLvlIcon.Source = lvlThreeOfTheFiveIcon; break;
                case 3:
                    orderToAttackLvlIcon.Source = lvlFourOfTheFiveIcon; break;
                case 4:
                    orderToAttackLvlIcon.Source = lvlFiveOfTheFiveIcon; break;
            }
        }
        private void decreaseLvlOrderToAttackButton_Click(object sender, RoutedEventArgs e)
        {
            switch (Logic.LvlOrderToAttack)
            {
                case 2:
                    orderToAttackLvlIcon.Source = lvlOneOfTheFiveIcon; break;
                case 3:
                    orderToAttackLvlIcon.Source = lvlTwoOfTheFiveIcon; break;
                case 4:
                    orderToAttackLvlIcon.Source = lvlThreeOfTheFiveIcon; break;
                case 5:
                    orderToAttackLvlIcon.Source = lvlFourOfTheFiveIcon; break;
            }
        }







        private void decreaseLvlAnimalRageTalantButton_Click(object sender, RoutedEventArgs e)
        {
            decreaseLvlThree(Logic.LvlTalantAnimalRage, animalRageTalantButton, animalRageLvlIcon);
        }

        private void increaseLvlAnimalRageTalantButton_Click(object sender, RoutedEventArgs e)
        {
            increaseLvlThree(Logic.LvlTalantAnimalRage, animalRageTalantButton, animalRageLvlIcon);
        }

        private void decreaseLvlContinuousFuryTalantButton_Click(object sender, RoutedEventArgs e)
        {
            decreaseLvlThree(Logic.LvlTalantContinuousFury, continuousFuryTalantButton, continuousFuryLvlIcon);
        }

        private void increaseLvlContinuousFuryTalantButton_Click(object sender, RoutedEventArgs e)
        {
            increaseLvlThree(Logic.LvlTalantContinuousFury, continuousFuryTalantButton, continuousFuryLvlIcon);
        }

        #region Подсказки

        private void moonlightlHintButton_Click(object sender, RoutedEventArgs e)
        {
            if (moonlightPopup.IsOpen)
                moonlightPopup.IsOpen = false;
            else
                moonlightPopup.IsOpen = true;
        }

        private void basicSkillHintButton_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            basicSkillPopup.IsOpen = !basicSkillPopup.IsOpen;
        }
        private void temporaryHintButton_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            temporaryPopup.IsOpen = !temporaryPopup.IsOpen;
        }
        private void ddHintButton_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            ddPopup.IsOpen = !ddPopup.IsOpen;
        }
        private void mermenHintButton_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            mermenPopup.IsOpen = !mermenPopup.IsOpen;
        }
        private void blessingHintButton_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            blessingPopup.IsOpen = !blessingPopup.IsOpen;
        }
        private void procentHintButton_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            procentPopup.IsOpen = !procentPopup.IsOpen;
        }


        #endregion

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Keyboard.ClearFocus();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Logic.Calculate();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            
        }

        private void TextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.SelectAll();
            }
        }

    }



}
