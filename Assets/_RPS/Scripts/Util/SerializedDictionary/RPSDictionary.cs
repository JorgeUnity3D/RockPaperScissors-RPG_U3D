using System;
using Doozy.Runtime.UIManager.Components;
using UnityEngine;


namespace Kapibara.RPS {
    
    [Serializable] public class LanguageDictionary : UnitySerializedDictionary<Actions, Sprite> { }
    [Serializable] public class ServiceDictionary : UnitySerializedDictionary<System.Type, Component> { }
    [Serializable] public class ManagerDictionary : UnitySerializedDictionary<System.Type, BaseManager> { }
    [Serializable] public class TownDictionary : UnitySerializedDictionary<TownMenu, UIButton> { }
}