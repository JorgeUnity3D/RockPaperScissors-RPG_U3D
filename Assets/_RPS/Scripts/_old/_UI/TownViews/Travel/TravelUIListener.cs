using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.Signals;
using UnityEngine;

namespace Kapibara.RPS {
    [RequireComponent(typeof(TravelUIController))]
    public class TravelUIListener : UIListener<StreamId.TravelStream> { }
}
