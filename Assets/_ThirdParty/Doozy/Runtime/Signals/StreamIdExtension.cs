// Copyright (c) 2015 - 2021 Doozy Entertainment. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

//.........................
//.....Generated Class.....
//.........................
//.......Do not edit.......
//.........................

using UnityEngine;
// ReSharper disable All

namespace Doozy.Runtime.Signals
{
    public partial class Signal
    {
        public static bool Send(StreamId.CombatStream id, string message = "") => SignalsService.SendSignal(nameof(StreamId.CombatStream), id.ToString(), message);
        public static bool Send(StreamId.CombatStream id, GameObject signalSource, string message = "") => SignalsService.SendSignal(nameof(StreamId.CombatStream), id.ToString(), signalSource, message);
        public static bool Send(StreamId.CombatStream id, SignalProvider signalProvider, string message = "") => SignalsService.SendSignal(nameof(StreamId.CombatStream), id.ToString(), signalProvider, message);
        public static bool Send(StreamId.CombatStream id, Object signalSender, string message = "") => SignalsService.SendSignal(nameof(StreamId.CombatStream), id.ToString(), signalSender, message);
        public static bool Send<T>(StreamId.CombatStream id, T signalValue, string message = "") => SignalsService.SendSignal(nameof(StreamId.CombatStream), id.ToString(), signalValue, message);
        public static bool Send<T>(StreamId.CombatStream id, T signalValue, GameObject signalSource, string message = "") => SignalsService.SendSignal(nameof(StreamId.CombatStream), id.ToString(), signalValue, signalSource, message);
        public static bool Send<T>(StreamId.CombatStream id, T signalValue, SignalProvider signalProvider, string message = "") => SignalsService.SendSignal(nameof(StreamId.CombatStream), id.ToString(), signalValue, signalProvider, message);
        public static bool Send<T>(StreamId.CombatStream id, T signalValue, Object signalSender, string message = "") => SignalsService.SendSignal(nameof(StreamId.CombatStream), id.ToString(), signalValue, signalSender, message);

        public static bool Send(StreamId.GameStateStream id, string message = "") => SignalsService.SendSignal(nameof(StreamId.GameStateStream), id.ToString(), message);
        public static bool Send(StreamId.GameStateStream id, GameObject signalSource, string message = "") => SignalsService.SendSignal(nameof(StreamId.GameStateStream), id.ToString(), signalSource, message);
        public static bool Send(StreamId.GameStateStream id, SignalProvider signalProvider, string message = "") => SignalsService.SendSignal(nameof(StreamId.GameStateStream), id.ToString(), signalProvider, message);
        public static bool Send(StreamId.GameStateStream id, Object signalSender, string message = "") => SignalsService.SendSignal(nameof(StreamId.GameStateStream), id.ToString(), signalSender, message);
        public static bool Send<T>(StreamId.GameStateStream id, T signalValue, string message = "") => SignalsService.SendSignal(nameof(StreamId.GameStateStream), id.ToString(), signalValue, message);
        public static bool Send<T>(StreamId.GameStateStream id, T signalValue, GameObject signalSource, string message = "") => SignalsService.SendSignal(nameof(StreamId.GameStateStream), id.ToString(), signalValue, signalSource, message);
        public static bool Send<T>(StreamId.GameStateStream id, T signalValue, SignalProvider signalProvider, string message = "") => SignalsService.SendSignal(nameof(StreamId.GameStateStream), id.ToString(), signalValue, signalProvider, message);
        public static bool Send<T>(StreamId.GameStateStream id, T signalValue, Object signalSender, string message = "") => SignalsService.SendSignal(nameof(StreamId.GameStateStream), id.ToString(), signalValue, signalSender, message);

        public static bool Send(StreamId.PlayerStream id, string message = "") => SignalsService.SendSignal(nameof(StreamId.PlayerStream), id.ToString(), message);
        public static bool Send(StreamId.PlayerStream id, GameObject signalSource, string message = "") => SignalsService.SendSignal(nameof(StreamId.PlayerStream), id.ToString(), signalSource, message);
        public static bool Send(StreamId.PlayerStream id, SignalProvider signalProvider, string message = "") => SignalsService.SendSignal(nameof(StreamId.PlayerStream), id.ToString(), signalProvider, message);
        public static bool Send(StreamId.PlayerStream id, Object signalSender, string message = "") => SignalsService.SendSignal(nameof(StreamId.PlayerStream), id.ToString(), signalSender, message);
        public static bool Send<T>(StreamId.PlayerStream id, T signalValue, string message = "") => SignalsService.SendSignal(nameof(StreamId.PlayerStream), id.ToString(), signalValue, message);
        public static bool Send<T>(StreamId.PlayerStream id, T signalValue, GameObject signalSource, string message = "") => SignalsService.SendSignal(nameof(StreamId.PlayerStream), id.ToString(), signalValue, signalSource, message);
        public static bool Send<T>(StreamId.PlayerStream id, T signalValue, SignalProvider signalProvider, string message = "") => SignalsService.SendSignal(nameof(StreamId.PlayerStream), id.ToString(), signalValue, signalProvider, message);
        public static bool Send<T>(StreamId.PlayerStream id, T signalValue, Object signalSender, string message = "") => SignalsService.SendSignal(nameof(StreamId.PlayerStream), id.ToString(), signalValue, signalSender, message);

        public static bool Send(StreamId.RPSViewStream id, string message = "") => SignalsService.SendSignal(nameof(StreamId.RPSViewStream), id.ToString(), message);
        public static bool Send(StreamId.RPSViewStream id, GameObject signalSource, string message = "") => SignalsService.SendSignal(nameof(StreamId.RPSViewStream), id.ToString(), signalSource, message);
        public static bool Send(StreamId.RPSViewStream id, SignalProvider signalProvider, string message = "") => SignalsService.SendSignal(nameof(StreamId.RPSViewStream), id.ToString(), signalProvider, message);
        public static bool Send(StreamId.RPSViewStream id, Object signalSender, string message = "") => SignalsService.SendSignal(nameof(StreamId.RPSViewStream), id.ToString(), signalSender, message);
        public static bool Send<T>(StreamId.RPSViewStream id, T signalValue, string message = "") => SignalsService.SendSignal(nameof(StreamId.RPSViewStream), id.ToString(), signalValue, message);
        public static bool Send<T>(StreamId.RPSViewStream id, T signalValue, GameObject signalSource, string message = "") => SignalsService.SendSignal(nameof(StreamId.RPSViewStream), id.ToString(), signalValue, signalSource, message);
        public static bool Send<T>(StreamId.RPSViewStream id, T signalValue, SignalProvider signalProvider, string message = "") => SignalsService.SendSignal(nameof(StreamId.RPSViewStream), id.ToString(), signalValue, signalProvider, message);
        public static bool Send<T>(StreamId.RPSViewStream id, T signalValue, Object signalSender, string message = "") => SignalsService.SendSignal(nameof(StreamId.RPSViewStream), id.ToString(), signalValue, signalSender, message);

        public static bool Send(StreamId.StablesStream id, string message = "") => SignalsService.SendSignal(nameof(StreamId.StablesStream), id.ToString(), message);
        public static bool Send(StreamId.StablesStream id, GameObject signalSource, string message = "") => SignalsService.SendSignal(nameof(StreamId.StablesStream), id.ToString(), signalSource, message);
        public static bool Send(StreamId.StablesStream id, SignalProvider signalProvider, string message = "") => SignalsService.SendSignal(nameof(StreamId.StablesStream), id.ToString(), signalProvider, message);
        public static bool Send(StreamId.StablesStream id, Object signalSender, string message = "") => SignalsService.SendSignal(nameof(StreamId.StablesStream), id.ToString(), signalSender, message);
        public static bool Send<T>(StreamId.StablesStream id, T signalValue, string message = "") => SignalsService.SendSignal(nameof(StreamId.StablesStream), id.ToString(), signalValue, message);
        public static bool Send<T>(StreamId.StablesStream id, T signalValue, GameObject signalSource, string message = "") => SignalsService.SendSignal(nameof(StreamId.StablesStream), id.ToString(), signalValue, signalSource, message);
        public static bool Send<T>(StreamId.StablesStream id, T signalValue, SignalProvider signalProvider, string message = "") => SignalsService.SendSignal(nameof(StreamId.StablesStream), id.ToString(), signalValue, signalProvider, message);
        public static bool Send<T>(StreamId.StablesStream id, T signalValue, Object signalSender, string message = "") => SignalsService.SendSignal(nameof(StreamId.StablesStream), id.ToString(), signalValue, signalSender, message);

        public static bool Send(StreamId.StoneSmithyStream id, string message = "") => SignalsService.SendSignal(nameof(StreamId.StoneSmithyStream), id.ToString(), message);
        public static bool Send(StreamId.StoneSmithyStream id, GameObject signalSource, string message = "") => SignalsService.SendSignal(nameof(StreamId.StoneSmithyStream), id.ToString(), signalSource, message);
        public static bool Send(StreamId.StoneSmithyStream id, SignalProvider signalProvider, string message = "") => SignalsService.SendSignal(nameof(StreamId.StoneSmithyStream), id.ToString(), signalProvider, message);
        public static bool Send(StreamId.StoneSmithyStream id, Object signalSender, string message = "") => SignalsService.SendSignal(nameof(StreamId.StoneSmithyStream), id.ToString(), signalSender, message);
        public static bool Send<T>(StreamId.StoneSmithyStream id, T signalValue, string message = "") => SignalsService.SendSignal(nameof(StreamId.StoneSmithyStream), id.ToString(), signalValue, message);
        public static bool Send<T>(StreamId.StoneSmithyStream id, T signalValue, GameObject signalSource, string message = "") => SignalsService.SendSignal(nameof(StreamId.StoneSmithyStream), id.ToString(), signalValue, signalSource, message);
        public static bool Send<T>(StreamId.StoneSmithyStream id, T signalValue, SignalProvider signalProvider, string message = "") => SignalsService.SendSignal(nameof(StreamId.StoneSmithyStream), id.ToString(), signalValue, signalProvider, message);
        public static bool Send<T>(StreamId.StoneSmithyStream id, T signalValue, Object signalSender, string message = "") => SignalsService.SendSignal(nameof(StreamId.StoneSmithyStream), id.ToString(), signalValue, signalSender, message);

        public static bool Send(StreamId.TownStream id, string message = "") => SignalsService.SendSignal(nameof(StreamId.TownStream), id.ToString(), message);
        public static bool Send(StreamId.TownStream id, GameObject signalSource, string message = "") => SignalsService.SendSignal(nameof(StreamId.TownStream), id.ToString(), signalSource, message);
        public static bool Send(StreamId.TownStream id, SignalProvider signalProvider, string message = "") => SignalsService.SendSignal(nameof(StreamId.TownStream), id.ToString(), signalProvider, message);
        public static bool Send(StreamId.TownStream id, Object signalSender, string message = "") => SignalsService.SendSignal(nameof(StreamId.TownStream), id.ToString(), signalSender, message);
        public static bool Send<T>(StreamId.TownStream id, T signalValue, string message = "") => SignalsService.SendSignal(nameof(StreamId.TownStream), id.ToString(), signalValue, message);
        public static bool Send<T>(StreamId.TownStream id, T signalValue, GameObject signalSource, string message = "") => SignalsService.SendSignal(nameof(StreamId.TownStream), id.ToString(), signalValue, signalSource, message);
        public static bool Send<T>(StreamId.TownStream id, T signalValue, SignalProvider signalProvider, string message = "") => SignalsService.SendSignal(nameof(StreamId.TownStream), id.ToString(), signalValue, signalProvider, message);
        public static bool Send<T>(StreamId.TownStream id, T signalValue, Object signalSender, string message = "") => SignalsService.SendSignal(nameof(StreamId.TownStream), id.ToString(), signalValue, signalSender, message);

        public static bool Send(StreamId.TrainingHouseStream id, string message = "") => SignalsService.SendSignal(nameof(StreamId.TrainingHouseStream), id.ToString(), message);
        public static bool Send(StreamId.TrainingHouseStream id, GameObject signalSource, string message = "") => SignalsService.SendSignal(nameof(StreamId.TrainingHouseStream), id.ToString(), signalSource, message);
        public static bool Send(StreamId.TrainingHouseStream id, SignalProvider signalProvider, string message = "") => SignalsService.SendSignal(nameof(StreamId.TrainingHouseStream), id.ToString(), signalProvider, message);
        public static bool Send(StreamId.TrainingHouseStream id, Object signalSender, string message = "") => SignalsService.SendSignal(nameof(StreamId.TrainingHouseStream), id.ToString(), signalSender, message);
        public static bool Send<T>(StreamId.TrainingHouseStream id, T signalValue, string message = "") => SignalsService.SendSignal(nameof(StreamId.TrainingHouseStream), id.ToString(), signalValue, message);
        public static bool Send<T>(StreamId.TrainingHouseStream id, T signalValue, GameObject signalSource, string message = "") => SignalsService.SendSignal(nameof(StreamId.TrainingHouseStream), id.ToString(), signalValue, signalSource, message);
        public static bool Send<T>(StreamId.TrainingHouseStream id, T signalValue, SignalProvider signalProvider, string message = "") => SignalsService.SendSignal(nameof(StreamId.TrainingHouseStream), id.ToString(), signalValue, signalProvider, message);
        public static bool Send<T>(StreamId.TrainingHouseStream id, T signalValue, Object signalSender, string message = "") => SignalsService.SendSignal(nameof(StreamId.TrainingHouseStream), id.ToString(), signalValue, signalSender, message);

        public static bool Send(StreamId.TrainingHouseViewsStream id, string message = "") => SignalsService.SendSignal(nameof(StreamId.TrainingHouseViewsStream), id.ToString(), message);
        public static bool Send(StreamId.TrainingHouseViewsStream id, GameObject signalSource, string message = "") => SignalsService.SendSignal(nameof(StreamId.TrainingHouseViewsStream), id.ToString(), signalSource, message);
        public static bool Send(StreamId.TrainingHouseViewsStream id, SignalProvider signalProvider, string message = "") => SignalsService.SendSignal(nameof(StreamId.TrainingHouseViewsStream), id.ToString(), signalProvider, message);
        public static bool Send(StreamId.TrainingHouseViewsStream id, Object signalSender, string message = "") => SignalsService.SendSignal(nameof(StreamId.TrainingHouseViewsStream), id.ToString(), signalSender, message);
        public static bool Send<T>(StreamId.TrainingHouseViewsStream id, T signalValue, string message = "") => SignalsService.SendSignal(nameof(StreamId.TrainingHouseViewsStream), id.ToString(), signalValue, message);
        public static bool Send<T>(StreamId.TrainingHouseViewsStream id, T signalValue, GameObject signalSource, string message = "") => SignalsService.SendSignal(nameof(StreamId.TrainingHouseViewsStream), id.ToString(), signalValue, signalSource, message);
        public static bool Send<T>(StreamId.TrainingHouseViewsStream id, T signalValue, SignalProvider signalProvider, string message = "") => SignalsService.SendSignal(nameof(StreamId.TrainingHouseViewsStream), id.ToString(), signalValue, signalProvider, message);
        public static bool Send<T>(StreamId.TrainingHouseViewsStream id, T signalValue, Object signalSender, string message = "") => SignalsService.SendSignal(nameof(StreamId.TrainingHouseViewsStream), id.ToString(), signalValue, signalSender, message);

        public static bool Send(StreamId.TravelStream id, string message = "") => SignalsService.SendSignal(nameof(StreamId.TravelStream), id.ToString(), message);
        public static bool Send(StreamId.TravelStream id, GameObject signalSource, string message = "") => SignalsService.SendSignal(nameof(StreamId.TravelStream), id.ToString(), signalSource, message);
        public static bool Send(StreamId.TravelStream id, SignalProvider signalProvider, string message = "") => SignalsService.SendSignal(nameof(StreamId.TravelStream), id.ToString(), signalProvider, message);
        public static bool Send(StreamId.TravelStream id, Object signalSender, string message = "") => SignalsService.SendSignal(nameof(StreamId.TravelStream), id.ToString(), signalSender, message);
        public static bool Send<T>(StreamId.TravelStream id, T signalValue, string message = "") => SignalsService.SendSignal(nameof(StreamId.TravelStream), id.ToString(), signalValue, message);
        public static bool Send<T>(StreamId.TravelStream id, T signalValue, GameObject signalSource, string message = "") => SignalsService.SendSignal(nameof(StreamId.TravelStream), id.ToString(), signalValue, signalSource, message);
        public static bool Send<T>(StreamId.TravelStream id, T signalValue, SignalProvider signalProvider, string message = "") => SignalsService.SendSignal(nameof(StreamId.TravelStream), id.ToString(), signalValue, signalProvider, message);
        public static bool Send<T>(StreamId.TravelStream id, T signalValue, Object signalSender, string message = "") => SignalsService.SendSignal(nameof(StreamId.TravelStream), id.ToString(), signalValue, signalSender, message);   
    }

    public partial class StreamId
    {
        public enum CombatStream
        {
            DefenseAction,
            EnergyAction,
            Leave,
            NextStepSignal,
            PaperAction,
            Pause,
            RockAction,
            ScissorsAction,
            Start,
            StartWithSteps
        }

        public enum GameStateStream
        {
            LoadGame,
            SaveGame,
            SelectGame
        }

        public enum PlayerStream
        {
            GoldUpdated
        }

        public enum RPSViewStream
        {
            ShowCombatUI,
            ShowLibraryUI,
            ShowPaperTreeUI,
            ShowScissorsBonfireUI,
            ShowStablesUI,
            ShowStoneSmithyUI,
            ShowTheaterUI,
            ShowTrainingHouseUI,
            ShowTravelUI,
            ShowUnlockUI
        }

        public enum StablesStream
        {
            BuyGame,
            WatchAd
        }

        public enum StoneSmithyStream
        {
            BuyPotion,
            BuyShuriken,
            BuyTorch
        }

        public enum TownStream
        {
            GoToLibrary,
            GoToPaperTree,
            GoToScissorsBonfire,
            GoToStables,
            GoToStoneSmithy,
            GoToTheater,
            GoToTown,
            GoToTrainingHouse,
            GoToTravel,
            GoToUnlock
        }

        public enum TrainingHouseStream
        {
            Crit,
            Defense,
            EnergyBase,
            EnergyRecovery,
            Health,
            Mentality,
            Paper,
            Rock,
            Scissor,
            SuperPower,
            Thorns,
            Unlock
        }

        public enum TrainingHouseViewsStream
        {
            UnlockStatUI
        }
        public enum TravelStream
        {
            OpenLevel,
            SelectLevel01,
            SelectLevel02,
            SelectLevel03,
            SelectLevel04,
            SelectLevel05,
            SelectLevel06,
            SelectLevel07
        }         
    }
}
