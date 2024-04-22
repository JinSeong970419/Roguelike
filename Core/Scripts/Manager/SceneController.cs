using System.Drawing.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Roguelike.Core
{
    public class SceneController : MonoSingleton<SceneController>
    {
        [SerializeField] private SceneSettings sceneSettings;
        [SerializeField] private GameEvent<string> sceneLoadedEvent;
        [SerializeField] private GameEvent<string> requestLoadSceneEvent;
        [SerializeField] private GameEvent<string> sceneChangedEvent;

        private string previousSceneName = string.Empty;

        public SceneSettings SceneSettings { get { return sceneSettings; } }

        public bool IsGameScene
        {
            get
            {
                Scene currentScene = SceneManager.GetActiveScene();
                return SceneSettings.IsGameScene(currentScene.name);
            }
        }
        protected override void Awake()
        {
            base.Awake();
            
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            requestLoadSceneEvent.AddListener(LoadScene);
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            requestLoadSceneEvent.RemoveListener(LoadScene);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            sceneLoadedEvent.Invoke(scene.name);
            if(previousSceneName != scene.name)
            {
                previousSceneName = scene.name;
                sceneChangedEvent.Invoke(scene.name);
                Debug.Log($"OnSceneChanged: {scene.name}");
            }
        }

        public static void LoadScene(string sceneName)
        {
            //Debug.Log($"LoadScene: {sceneName}");
            SceneManager.LoadScene(sceneName);
        }
    }
}
