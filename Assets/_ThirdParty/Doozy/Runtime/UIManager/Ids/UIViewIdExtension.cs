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
namespace Doozy.Runtime.UIManager.Containers
{
    public partial class UIView
    {
        public static IEnumerable<UIView> GetViews(UIViewId.Combat id) => GetViews(nameof(UIViewId.Combat), id.ToString());
        public static void Show(UIViewId.Combat id, bool instant = false) => Show(nameof(UIViewId.Combat), id.ToString(), instant);
        public static void Hide(UIViewId.Combat id, bool instant = false) => Hide(nameof(UIViewId.Combat), id.ToString(), instant);

        public static IEnumerable<UIView> GetViews(UIViewId.MainMenu id) => GetViews(nameof(UIViewId.MainMenu), id.ToString());
        public static void Show(UIViewId.MainMenu id, bool instant = false) => Show(nameof(UIViewId.MainMenu), id.ToString(), instant);
        public static void Hide(UIViewId.MainMenu id, bool instant = false) => Hide(nameof(UIViewId.MainMenu), id.ToString(), instant);

        public static IEnumerable<UIView> GetViews(UIViewId.RPS id) => GetViews(nameof(UIViewId.RPS), id.ToString());
        public static void Show(UIViewId.RPS id, bool instant = false) => Show(nameof(UIViewId.RPS), id.ToString(), instant);
        public static void Hide(UIViewId.RPS id, bool instant = false) => Hide(nameof(UIViewId.RPS), id.ToString(), instant);

        public static IEnumerable<UIView> GetViews(UIViewId.Town id) => GetViews(nameof(UIViewId.Town), id.ToString());
        public static void Show(UIViewId.Town id, bool instant = false) => Show(nameof(UIViewId.Town), id.ToString(), instant);
        public static void Hide(UIViewId.Town id, bool instant = false) => Hide(nameof(UIViewId.Town), id.ToString(), instant);
        public static IEnumerable<UIView> GetViews(UIViewId.TrainingHouse id) => GetViews(nameof(UIViewId.TrainingHouse), id.ToString());
        public static void Show(UIViewId.TrainingHouse id, bool instant = false) => Show(nameof(UIViewId.TrainingHouse), id.ToString(), instant);
        public static void Hide(UIViewId.TrainingHouse id, bool instant = false) => Hide(nameof(UIViewId.TrainingHouse), id.ToString(), instant);
    }
}

namespace Doozy.Runtime.UIManager
{
    public partial class UIViewId
    {
        public enum Combat
        {
            Actions,
            Combat,
            Combatants,
            Pause,
            PauseButton,
            Start
        }

        public enum MainMenu
        {
            MainMenu,
            Options
        }

        public enum RPS
        {
            CombatMain,
            IntroMain,
            LoadGameMain,
            TownMain
        }

        public enum Town
        {
            CloseMenu,
            General,
            House,
            Library,
            MenuInfo,
            Menus,
            PaperTree,
            ScissorsBonfire,
            Stables,
            StoneSmithy,
            Theater,
            TimeCounter,
            TrainingHouse,
            Travel,
            Unlock
        }
        public enum TrainingHouse
        {
            StatInfo,
            UnlockStat
        }    
    }
}