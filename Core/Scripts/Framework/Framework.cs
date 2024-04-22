using UnityEngine;

namespace Roguelike.Core
{
    public class Framework : MonoSingleton<Framework>
    {
        [Header("Settings")]
        [SerializeField] private FrameworkSettings _frameworkSettings;
        [SerializeField] private EntitySettings _entitySettings;
        [SerializeField] private AutomataSettings _automataSettings;
        [SerializeField] private SceneSettings _sceneSettings;

        [Header("Tools")]
        [SerializeField] private AssetManagementTool _assetManagementTool;
        [SerializeField] private LogManagementTool _logManagementTool;

        [Header("Variables")]
        [SerializeField] private VariableBase _deltaTime;

        public FrameworkSettings Settings { get { return _frameworkSettings; } }
        public AutomataSettings AutomataSettings { get { return _automataSettings; } }
        public SceneSettings SceneSettings { get { return _sceneSettings; } }

        private static int _mainThreadID = 0;
        public static bool IsMainThread
        {
            get
            {
                return (System.Threading.Thread.CurrentThread.ManagedThreadId == _mainThreadID);
            }
        }


        protected override void Awake()
        {
            base.Awake();
            _mainThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
            Application.targetFrameRate = Settings.TargetFrameRate;
        }

        private void Update()
        {
            _deltaTime.BoxedValue = Time.deltaTime;
        }
    }
}
