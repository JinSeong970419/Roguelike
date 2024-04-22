using UnityEngine;

namespace Roguelike.Core
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T _instance;
        public static T Instance { get { return _instance; } }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                
                if(transform.parent == null)
                {
                    DontDestroyOnLoad(gameObject);
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
