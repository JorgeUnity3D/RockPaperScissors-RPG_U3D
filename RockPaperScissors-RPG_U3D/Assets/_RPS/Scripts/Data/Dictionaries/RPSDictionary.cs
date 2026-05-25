using Kapibara.Util.SerializedDictionary;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Kapibara.RPS 
{
    [Serializable] public class LanguageDictionary : UnitySerializedDictionary<Actions, Sprite> { }
    [Serializable] public class ServiceDictionary : UnitySerializedDictionary<System.Type, Component> { }
    [Serializable] public class ManagerDictionary : UnitySerializedDictionary<System.Type, BaseManager> { }
    [Serializable] public class TownButtonEntry { public Button Button; public GameObject NpcObject; }
    [Serializable] public class TownDictionary : UnitySerializedDictionary<TownMenu, TownButtonEntry> { }
    [Serializable] public class IconsDictionary : UnitySerializedDictionary<Stats, Sprite> { }
    [Serializable] public class TrainingDictionary : UnitySerializedDictionary<Stats, TrainingButton> { }
    [Serializable] public class ScissorBonfireDictionary : UnitySerializedDictionary<Stats, ScissorBonfireVariation> { }
    [Serializable] public class LevelButtonsDictionary : UnitySerializedDictionary<int, Button> { }
}