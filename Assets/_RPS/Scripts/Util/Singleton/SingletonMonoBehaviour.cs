using UnityEngine;

namespace Kapibara.Util.Singleton
{
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        //Singleton
        private static T _instance;

        [HideInInspector]
        public static T Instance
        {
            get { return _instance; }
        }

        #region UNITY_LIFECYCLE
  
        protected virtual void Awake()
        {
            if (_instance != null && _instance != this as T)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this as T;
            }
        }
        
        #endregion
    }
}