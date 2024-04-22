using Spine.Unity;
using UnityEngine;

namespace Roguelike.Core
{
    public class ActorMaterialPropertyController : MaterialPropertyController
    {
        [Header("Outline")]
        [SerializeField] private float outlineTickness;
        [SerializeField] private Color outlineColor;
        [SerializeField] private float blinkRatio;
        [SerializeField] private Color blinkColor;

        private SpriteRenderer spriteRenderer;


        public float OutlineTickness { get { return outlineTickness; } set { outlineTickness = value; } }
        public Color OutlineColor { get { return outlineColor; } set { outlineColor = value; } }
        public float BlinkRatio { get { return blinkRatio; } set { blinkRatio = value; } }
        public Color BlinkColor { get { return blinkColor; } set { blinkColor = value; } }

        public override void UpdateProperties()
        {

            var spriteRenderer = GetComponentInChildren<MeshRenderer>();
            if (spriteRenderer == null)
            {
                Debug.LogError("SpriteRenderer does not exist.");
                return;
            }
            propertyBlock.SetFloat("_OutlineTickness", OutlineTickness);
            propertyBlock.SetColor("_OutlineColor", OutlineColor);
            propertyBlock.SetFloat("_BlinkRatio", BlinkRatio);
            propertyBlock.SetColor("_BlinkColor", BlinkColor);
            //propertyBlock.SetTexture("_MainTex", spriteRenderer.sprite.texture);
            propertyBlock.SetTexture("_MainTex", spriteRenderer.material.mainTexture);
            spriteRenderer.SetPropertyBlock(propertyBlock);
        }
    }
}
