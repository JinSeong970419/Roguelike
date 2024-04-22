using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Roguelike.Core
{
    public class DamageFont : Entity
    {
        [SerializeField] private Transform offset;
        [SerializeField] private SpriteRenderer[] renderers = new SpriteRenderer[10];
        
        private int value;
        private DamageFontInfo fontInfo;
        private float lifeTick = 0f;

        protected override void OnEnable()
        {
            base.OnEnable();
            lifeTick = 0f;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            for(int i=0;i<renderers.Length;i++)
            {
                Renderer ren = renderers[i];
                if(ren != null)
                {
                    ren.gameObject.SetActive(false);
                }
            }
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            ProcessLifeTime();
            ProcessAlpha();
            ProcessFloating();
        }

        private void ProcessLifeTime()
        {
            lifeTick += Time.fixedDeltaTime;
            if (lifeTick > fontInfo.Duration)
            {
                Release();
            }
        }

        private void ProcessAlpha()
        {
            float alphaRatio = Mathf.Clamp(1f - (lifeTick / fontInfo.Duration), 0f, 1f);

            for (int i = 0; i < renderers.Length; i++)
            {
                SpriteRenderer ren = renderers[i];
                if (ren != null)
                {
                    ren.color = new Color(1f, 1f, 1f, alphaRatio);
                }
            }
        }

        private void ProcessFloating()
        {
            Vector3 originPos = transform.position;
            originPos.y += Time.fixedDeltaTime;
            transform.position = originPos;
        }

        public static DamageFont Create(DamageFontType type, int damage)
        {
            GameObject obj = ObjectPool.Instance.Allocate(EntityType.DamageFont);
            DamageFont font = obj.GetComponent<DamageFont>();
            font.SetFont(type, damage);
            return font;
        }

        public void SetFont(DamageFontType type, int value)
        {
            this.value = value;

            int count = 0;
            string valueString = value.ToString();
            int length = valueString.Length;
            fontInfo = DataManager.Instance.DamageFontSettings.Infos[(int)type];
            for(int i=0;i<10; i++)
            {
                if(i >= length)
                {
                    // ÃÊ°ú
                    renderers[i].gameObject.SetActive(false);
                    continue;
                }
                char c = valueString[i];
                if (c.IsDigit() == false) continue;

                int num = c.ToInt();
                Sprite sprite = fontInfo.Fonts[num];
                renderers[i].sprite = sprite;
                renderers[i].gameObject.SetActive(true);
                count++;
            }

            float xPos = (count - 1) * -0.1f;
            offset.localPosition = new Vector3(xPos, 0, 0);
        }

        public override void OnMoveEnd(MoveEndEventArgs args)
        {
        }

        public override void OnMoveStart(MoveStartEventArgs args)
        {
        }
    }
}
