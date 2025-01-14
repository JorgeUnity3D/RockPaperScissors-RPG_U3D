using System;
using System.Collections.Generic;
using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager;
using Doozy.Runtime.UIManager.Listeners;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Kapibara.RPS {
    public abstract class UIListener<T> : SerializedMonoBehaviour where T : Enum {
        private List<SignalListener> _signalListeners;
        [HideInInspector] public UnityAction<T> onSignalReceived;
        [HideInInspector] public UnityAction<T, int> onSignalReceivedIntValue;
        private bool debugSignal = false;

        protected virtual void Awake() {
            _signalListeners = new List<SignalListener>(GetComponentsInChildren<SignalListener>());
        }

        protected void Start() {
            for (int i = 0; i < _signalListeners.Count; i++) {
                SignalListener currentSignalListener = _signalListeners[i];
                currentSignalListener.Callback.Event.AddListener(() => Listen(currentSignalListener));
            }
        }

        private void Listen(SignalListener signalListener) {
            Debug.Log("[BaseListener] Listen() -> Listened to signal: " + signalListener.stream.category + " - " +
                      signalListener.stream.name);
            if (debugSignal)
                DebugSignalListener(signalListener);
            T currentSignal = (T) Enum.Parse(typeof(T), signalListener.stream.name);
            if (signalListener.stream.currentSignal.hasValue) {
                Debug.Log("[BaseListener] Listen() -> signal: " + signalListener.stream.category + " has value");
                if (typeof(int) == signalListener.stream.currentSignal.valueType) {
                    onSignalReceivedIntValue?.Invoke(currentSignal, (int) signalListener.stream.currentSignal.valueAsObject);
                }
            } else {
                onSignalReceived?.Invoke(currentSignal);
            }
        }

        // protected virtual void FilterAction(T streamID) {
        //     onButtonClicked?.Invoke(streamID);
        // }

        public void DebugSignalListener(SignalListener signalListener) {
            Debug.Log("[BaseListener] DebugSignalListener()");
            Debug.Log("signalListener.stream.category: " + signalListener.stream.category);
            Debug.Log("signalListener.stream.name: " + signalListener.stream.name);
            Debug.Log("signalListener.stream.currentSignal.hasValue: " + signalListener.stream.currentSignal.hasValue);
            if (signalListener.stream.currentSignal.hasValue) {
                //signalListener.stream.currentSignal.TryGetValueType()
                Type valueType = signalListener.stream.currentSignal.valueType;
                int value = (int) signalListener.stream.currentSignal.valueAsObject;
                Debug.Log("value: " + value);
                Debug.Log("valueType.Name: " + valueType.Name);
                Debug.Log("signalListener.stream.currentSignal.valueAsObject: " +
                          signalListener.stream.currentSignal.valueAsObject.ToString());
            }

            Debug.Log("signalListener.stream.infoMessage: " + signalListener.stream.infoMessage);
            Debug.Log("signalListener.stream.currentSignal.message: " +
                      signalListener.stream.currentSignal.message);
        }
    }
}