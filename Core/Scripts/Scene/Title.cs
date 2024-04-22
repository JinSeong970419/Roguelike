using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Roguelike.Core
{
    public class Title : MonoBehaviour
    {

        private void Start()
        {
            PopupManager.Instance.OpenPopup((int)PopupKind.PopupSignin);
        }
    }
}
