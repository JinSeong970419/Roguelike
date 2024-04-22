using UnityEngine;

namespace Roguelike.Core
{
    [CreateAssetMenu(fileName = "FrameworkSettings", menuName = "TheSalt/Settings/FrameworkSettings")]
    public class FrameworkSettings : ScriptableObject
    {
        [Header("LocalStorage")]
        [SerializeField] private string _persistentDataPath;
        [SerializeField] private string _userSettingFileName;

        [Space(20)]
        [Header("Game")]
        [SerializeField] private int _targetFrameRate = 60;
        [SerializeField] private GameWorldDimension _dimension;


        public string PersistentDataPath { get { return _persistentDataPath; } }
        public string UserSettingFileName { get { return _userSettingFileName; } }
        public int TargetFrameRate { get { return _targetFrameRate; } }
        public GameWorldDimension Dimension { get { return _dimension; } }
      


        [DebugButton]
        public void SetDefault()
        {
            _persistentDataPath = Application.persistentDataPath;
            _userSettingFileName = "UserSettings.dat";

            
        }
    }
}
