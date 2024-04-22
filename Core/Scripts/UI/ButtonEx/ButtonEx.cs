using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Roguelike.Core
{
    public class ButtonEx : Button
    {
        [SerializeField] public UnityEvent ww;
        [SerializeField] public UnityAction dd;

        private void OnMouseOver()
        {
            ww.Invoke();
        }

        private void OnMouseEnter()
        {
            
        }
    }

}
