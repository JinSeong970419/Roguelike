using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Roguelike.Core
{
    public class PopupLoading : Popup
    {
        [SerializeField] private Image loadingRing;

        private void Update()
        {
            loadingRing.fillAmount = Mathf.Clamp01(Time.unscaledDeltaTime);
        }
    }
}
