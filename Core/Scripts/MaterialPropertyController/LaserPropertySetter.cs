using UnityEngine;

namespace Roguelike.Core
{
    public class LaserPropertySetter : MaterialPropertySetter
    {
        [ColorUsage(true, true)]
        [SerializeField] private Color color;

        private SpriteRenderer spriteRenderer;

        protected override void Awake()
        {
            base.Awake();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        protected override void SetProperties()
        {
            propertyBlock.SetColor("_Color", color);
            spriteRenderer.SetPropertyBlock(propertyBlock);
        }
    }
}
