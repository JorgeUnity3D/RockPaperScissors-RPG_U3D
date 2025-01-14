using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.Signals;
using UnityEngine;

namespace Kapibara.RPS {
    [RequireComponent(typeof(PlayerUIController))]
    public class PlayerListener : UIListener<StreamId.PlayerStream> { }
}