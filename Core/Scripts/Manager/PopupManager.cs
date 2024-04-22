using System.Collections.Generic;
using UnityEngine;

namespace Roguelike.Core
{
    public class PopupManager : MonoSingleton<PopupManager>
    {
        [SerializeField] private GameEvent<int> _popupOpenEvent;
        [SerializeField] private GameEvent<int> _popupCloseEvent;
        [SerializeField] private GameEvent<string> _sceneChangedEvent;

        private List<Popup> popupList;
        public IReadOnlyList<Popup> Popups { get { return popupList.AsReadOnly(); } }

        protected override void Awake()
        {
            base.Awake();
            popupList = new List<Popup>();

        }

        private void OnEnable()
        {
            _popupOpenEvent.AddListener(OpenPopup);
            _popupCloseEvent.AddListener(ClosePopup);
            _sceneChangedEvent.AddListener(OnSceneChanged);
        }

        private void OnDisable()
        {
            _popupOpenEvent.RemoveListener(OpenPopup);
            _popupCloseEvent.RemoveListener(ClosePopup);
            _sceneChangedEvent.RemoveListener(OnSceneChanged);
        }

        public void OpenPopup(int index)
        {
            var popup = popupList[index];
            if (popup.gameObject.activeSelf)
                return;
            popup.gameObject.SetActive(true);
            popup.OnOpen();
        }

        public void OpenPopup(PopupKind kind)
        {
            OpenPopup((int)kind);
        }

        public void ClosePopup(int index)
        {
            var popup = popupList[index];
            if (popup.gameObject.activeSelf == false)
                return;
            popup.gameObject.SetActive(false);
            popup.OnClosed();
        }

        public void ClosePopup(PopupKind kind)
        {
            ClosePopup((int)kind);
        }

        public bool IsOpen(int index)
        {
            var popup = popupList[index];
            return popup.gameObject.activeSelf;
        }

        public bool IsOpen(PopupKind kind)
        {
            return IsOpen((int)kind);
        }

        private void OnSceneChanged(string sceneName)
        {
            var container = FindObjectOfType<PopupContainer>();
            popupList = container.PopupList;
            if (popupList != null)
            {
                int count = popupList.Count;
                for (int i = 0; i < count; i++)
                {
                    var popup = popupList[i];
                    popup.Index = i;
                }
            }
        }
    }
}
