using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Roguelike.Core
{
    public abstract class MaterialPropertySetter : MonoBehaviour
    {
        protected MaterialPropertyBlock propertyBlock;

        protected virtual void Awake()
        {
            propertyBlock = new MaterialPropertyBlock();
        }

        private void OnEnable()
        {
            SetProperties();
        }

        protected abstract void SetProperties();
    }
}
