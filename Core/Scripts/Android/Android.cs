using UnityEngine;

namespace Roguelike.Core
{
    public class Android : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if(PopupManager.Instance.IsOpen(PopupKind.PopupExit))
                {
                    PopupManager.Instance.ClosePopup(PopupKind.PopupExit);
                }
                else
                {
                    PopupManager.Instance.OpenPopup(PopupKind.PopupExit);
                }

                
            }
        }
    }
}