using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Roguelike.Core
{
    public abstract class LocalStorage : ScriptableObject
    {
        [SerializeField] protected Crypter crypter; 
        public abstract void Load();
        public abstract void Save();

        public virtual void LoadAsync()
        {

        }

        public virtual void SaveAsync()
        {

        }
    }
}
