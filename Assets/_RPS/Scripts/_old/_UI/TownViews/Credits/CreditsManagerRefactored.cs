using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

namespace Kapibara.RPS {
    public class CreditsManagerRefactored : BaseManagerRefactored {

        public int maxCredits = 5;
        public int creditsLeft;

        public bool CreditsAtMax {
            get {
                return creditsLeft == maxCredits;
            }
            
        }
        public float hoursForACredit;
        public float timeLeftInSeconds;
        public bool timerIsRunning;
        
        
        
        [HideInInspector] public UnityAction<int> OnCreditsUpdated;
        [HideInInspector] public UnityAction<float> OnTimeUpdated;

        public override void InitializeData() {

        }
        
        private void Start() {
            OnCreditsUpdated?.Invoke(creditsLeft);
        }

        private void Update() {
            if (timerIsRunning) {
                if (timeLeftInSeconds > 0) {
                    timeLeftInSeconds -= Time.deltaTime;
                    OnTimeUpdated?.Invoke(timeLeftInSeconds);
                } else {
                    timerIsRunning = false;
                    creditsLeft++;
                    OnCreditsUpdated?.Invoke(creditsLeft);
                }
            }
        }

        public void StartTimeCounter() {
            if (!timerIsRunning) {
                timeLeftInSeconds = hoursForACredit * 60 * 60;    
            }
            timerIsRunning = true;
        }

        public void UseCredit() {
            if (creditsLeft > 0) {
                creditsLeft--;
            }
            OnCreditsUpdated?.Invoke(creditsLeft);
        }

        public void EarnCredit() {
            if (creditsLeft < maxCredits) {
                creditsLeft++;
            }
            OnCreditsUpdated?.Invoke(creditsLeft);
        }
    }
}