// Copyright (c) 2015 - 2022 Doozy Entertainment. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

//.........................
//.....Generated Class.....
//.........................
//.......Do not edit.......
//.........................

using System.Collections.Generic;
// ReSharper disable All
namespace Doozy.Runtime.UIManager.Components
{
    public partial class UIButton
    {
        public static IEnumerable<UIButton> GetButtons(UIButtonId.CombatActionButtons id) => GetButtons(nameof(UIButtonId.CombatActionButtons), id.ToString());
        public static bool SelectButton(UIButtonId.CombatActionButtons id) => SelectButton(nameof(UIButtonId.CombatActionButtons), id.ToString());

        public static IEnumerable<UIButton> GetButtons(UIButtonId.CombatButtons id) => GetButtons(nameof(UIButtonId.CombatButtons), id.ToString());
        public static bool SelectButton(UIButtonId.CombatButtons id) => SelectButton(nameof(UIButtonId.CombatButtons), id.ToString());

        public static IEnumerable<UIButton> GetButtons(UIButtonId.GameStateButtons id) => GetButtons(nameof(UIButtonId.GameStateButtons), id.ToString());
        public static bool SelectButton(UIButtonId.GameStateButtons id) => SelectButton(nameof(UIButtonId.GameStateButtons), id.ToString());

        public static IEnumerable<UIButton> GetButtons(UIButtonId.MainMenu id) => GetButtons(nameof(UIButtonId.MainMenu), id.ToString());
        public static bool SelectButton(UIButtonId.MainMenu id) => SelectButton(nameof(UIButtonId.MainMenu), id.ToString());

        public static IEnumerable<UIButton> GetButtons(UIButtonId.StablesButtons id) => GetButtons(nameof(UIButtonId.StablesButtons), id.ToString());
        public static bool SelectButton(UIButtonId.StablesButtons id) => SelectButton(nameof(UIButtonId.StablesButtons), id.ToString());

        public static IEnumerable<UIButton> GetButtons(UIButtonId.StoneSmithyButtons id) => GetButtons(nameof(UIButtonId.StoneSmithyButtons), id.ToString());
        public static bool SelectButton(UIButtonId.StoneSmithyButtons id) => SelectButton(nameof(UIButtonId.StoneSmithyButtons), id.ToString());

        public static IEnumerable<UIButton> GetButtons(UIButtonId.TownGeneralButtons id) => GetButtons(nameof(UIButtonId.TownGeneralButtons), id.ToString());
        public static bool SelectButton(UIButtonId.TownGeneralButtons id) => SelectButton(nameof(UIButtonId.TownGeneralButtons), id.ToString());

        public static IEnumerable<UIButton> GetButtons(UIButtonId.TownMapButtons id) => GetButtons(nameof(UIButtonId.TownMapButtons), id.ToString());
        public static bool SelectButton(UIButtonId.TownMapButtons id) => SelectButton(nameof(UIButtonId.TownMapButtons), id.ToString());

        public static IEnumerable<UIButton> GetButtons(UIButtonId.TrainingHouseButtons id) => GetButtons(nameof(UIButtonId.TrainingHouseButtons), id.ToString());
        public static bool SelectButton(UIButtonId.TrainingHouseButtons id) => SelectButton(nameof(UIButtonId.TrainingHouseButtons), id.ToString());
        public static IEnumerable<UIButton> GetButtons(UIButtonId.TravelButtons id) => GetButtons(nameof(UIButtonId.TravelButtons), id.ToString());
        public static bool SelectButton(UIButtonId.TravelButtons id) => SelectButton(nameof(UIButtonId.TravelButtons), id.ToString());
    }
}

namespace Doozy.Runtime.UIManager
{
    public partial class UIButtonId
    {
        public enum CombatActionButtons
        {
            Defense,
            Energy,
            Paper,
            Rock,
            Scissor
        }

        public enum CombatButtons
        {
            Back,
            Leave,
            NextStep,
            Pause,
            Resume,
            Start,
            StartWithSteps
        }

        public enum GameStateButtons
        {
            Load01,
            Load02,
            Load03,
            Save01,
            Save02,
            Save03
        }

        public enum MainMenu
        {
            Credits,
            Exit,
            ExitOptions,
            Options,
            PlayMusic,
            StartGame,
            StopMusic
        }

        public enum StablesButtons
        {
            BuyGame,
            WatchAd
        }

        public enum StoneSmithyButtons
        {
            BuyPotion,
            BuyShuriken,
            BuyTorch
        }

        public enum TownGeneralButtons
        {
            CloseMenu,
            Settings,
            Unlock
        }

        public enum TownMapButtons
        {
            House,
            Library,
            PaperTree,
            ScissorsBonfire,
            Stables,
            StoneSmithy,
            Theater,
            TrainingHouse,
            Travel
        }

        public enum TrainingHouseButtons
        {
            Crit,
            Defense,
            EnergyBase,
            EnergyRecovery,
            Health,
            Level,
            Mentality,
            Paper,
            Rock,
            Scissor,
            SelectTraining,
            SuperPower,
            Thorns,
            Unlock
        }
        public enum TravelButtons
        {
            GoLevel,
            Level01,
            Level02,
            Level03,
            Level04,
            Level05,
            Level06,
            Level07
        }    
    }
}