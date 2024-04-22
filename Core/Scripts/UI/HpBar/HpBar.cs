using UnityEngine;
using UnityEngine.UI;

namespace Roguelike.Core
{
    public class HpBar : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Image bar;
        [SerializeField] private Color maxColor;
        [SerializeField] private Color minColor;

        public float Value
        {
            get { return bar.fillAmount; }
            set
            {
                float fill = Mathf.Clamp(value, 0f, 1f);
                bar.fillAmount = fill;

                float empty = 1f - fill;

                bar.color = maxColor * fill + minColor * empty;
            }
        }

        public Transform Target { get { return target; } set { target = value; } }
    }
}
