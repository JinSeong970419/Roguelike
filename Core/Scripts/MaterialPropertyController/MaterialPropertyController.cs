using UnityEngine;

namespace Roguelike.Core
{
    public abstract class MaterialPropertyController : MonoBehaviour
    {
        protected MaterialPropertyBlock propertyBlock;
        
        protected virtual void Awake()
        {
            propertyBlock = new MaterialPropertyBlock();
        }

        public abstract void UpdateProperties();
    }
}
