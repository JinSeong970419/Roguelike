using UnityEngine;

namespace Roguelike.Core
{
    public class Global : MonoSingleton<Global>
    {
        [SerializeField] private GameEventVoid applicationQuitEvent;
        [SerializeField] private GameEvent<bool> isNewlyEvent;
        [SerializeField] private Variable<bool> newRegisteredUser;
        protected override void Awake()
        {
            base.Awake();
        }

        private void OnEnable()
        {
            applicationQuitEvent.AddListener(Quit);
        }

        private void OnDisable()
        {
            applicationQuitEvent.RemoveListener(Quit);
        }

        private void Start()
        {
            //PlayerPrefs.SetInt("Newly", 0);
            //return;
            int isNewly = PlayerPrefs.GetInt("Newly", 0);
            if (isNewly == 0)
            {
                PlayerPrefs.SetInt("Newly", 1);
            }
            isNewlyEvent.Invoke(isNewly == 0);
            newRegisteredUser.Value = isNewly == 0 ? true : false;
            Debug.Log($"isNewly: {isNewly}");
        }

        private void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
        }
    }
}
