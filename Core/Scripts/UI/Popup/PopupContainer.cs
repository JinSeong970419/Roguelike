using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Roguelike.Core
{
    public class PopupContainer : MonoBehaviour
    {
        [SerializeField] private List<Popup> popupList;

        public List<Popup> PopupList { get { return popupList; } }

        [DebugButton]
        public void Generate()
        {
            var items = popupList.Select(o => o.name).ToList();
            Extension.GenerateEnum("Assets/Core/Scripts/UI/Popup/PopupKind.cs", "PopupKind", items);
        }
    }


}
