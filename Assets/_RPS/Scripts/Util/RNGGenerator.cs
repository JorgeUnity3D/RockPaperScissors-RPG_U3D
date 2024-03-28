using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kapibara.RPS {
    public static class RNGGenerator {
        /// <summary>
        /// Rools 1 dice
        /// </summary>
        /// <param name="maxValue">Max Value of the dice</param>
        /// <returns></returns>
        public static int Roll1D(int maxValue) {
            UnityEngine.Random.InitState(int.Parse(GetTimestamp()));
            return UnityEngine.Random.Range(1, maxValue + 1);
        }

        /// <summary>
        /// Returns a random number between min and max.
        /// Always generates a new seed.
        /// </summary>
        /// <param name="min">min value [inclusive]</param>
        /// <param name="max">max value [inclusive]</param>
        /// <returns></returns>
        public static int RandomBetween(int min, int max) {
            //UnityEngine.Random.InitState(int.Parse(GetTimestamp()));
            return UnityEngine.Random.Range(min, max + 1);
        }

        /// <summary>
        /// Returns a random number between min and max.
        /// Always generates a new seed.
        /// </summary>
        /// <param name="min">min value [inclusive]</param>
        /// <param name="max">max value [inclusive]</param>
        /// <returns></returns>
        public static float RandomBetween(float min, float max) {
            UnityEngine.Random.InitState(int.Parse(GetTimestamp()));
            return UnityEngine.Random.Range(min, max + 1);
        }

        /// <summary>
        /// Returns a random enum value of type T.
        /// Always generates a new seed.
        /// </summary>
        /// <typeparam name="T">Type of enum</typeparam>
        /// <param name="includeNone">Should include the first value (NONE)? False by default</param>
        /// <returns></returns>
        public static T RandomEnumValue<T>(bool includeNone = false) where T : struct, IConvertible {
            UnityEngine.Random.InitState(int.Parse(GetTimestamp()));
            var values = Enum.GetValues(typeof(T));
            int randomIndex = RandomBetween(includeNone ? 0 : 1, values.Length);
            return (T) values.GetValue(randomIndex);
        }

        public static string GetTimestamp() {
            System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
            int currentTime = (int) (System.DateTime.UtcNow - epochStart).TotalSeconds;
            return currentTime.ToString();
        }
    }
}