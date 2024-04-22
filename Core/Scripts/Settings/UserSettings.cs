using UnityEngine;

namespace Roguelike.Core
{
    [CreateAssetMenu(fileName = "UserSettings", menuName = "TheSalt/Settings/UserSettings")]
    public class UserSettings : LocalStorage
    {
        [SerializeField] private FrameworkSettings frameworkSettings;
        [SerializeField] private bool sound = true;
        [SerializeField] private bool music = true;
        [SerializeField] private bool vibration = true;
        [SerializeField] private bool reducedVFX = false;
        [SerializeField] private bool showJoystick = true;
        [SerializeField] private SystemLanguage language = SystemLanguage.English;
        [SerializeField] private Variable<SystemLanguage> systemLanguage;

        public bool Sound { get { return sound; } set { sound = value; } }
        public bool Music { get { return music; } set { music = value; } }
        public bool Vibration { get { return vibration; } set { vibration = value; } }
        public bool ReducedVFX { get { return reducedVFX; } set { reducedVFX = value; } }
        public bool ShowJoystick { get { return showJoystick; } set { showJoystick = value; } }
        public SystemLanguage Language { get { return language; } set { language = value; } }
        public SystemLanguage SystemLanguage { get { return systemLanguage.Value; } }

        [DebugButton]
        public override void Save()
        {
            UserSettingsData settingsData = new UserSettingsData()
            {
                Sound = Sound,
                Music = Music,
                Vibration = Vibration,
                ReducedVFX = ReducedVFX,
                ShowJoystick = ShowJoystick,
                Language = Language.ToString(),
            };
            FirestoreManager.Instance.SetUserSettingsData(FirestoreManager.Instance.UserData.UserId, settingsData);
        }

        [DebugButton]
        public override void Load()
        {
            UserSettingsData settingsData = new UserSettingsData()
            {
                Sound = true,
                Music = true,
                Vibration = true,
                ReducedVFX = false,
                ShowJoystick = true,
                Language = SystemLanguage.English.ToString(),
            };
            FirestoreManager.Instance.GetUserSettingsDataAsync(FirestoreManager.Instance.UserData.UserId, o =>
            {
                if (o == null)
                {
                    Save();
                }
                else
                {
                    Sound = o.Sound;
                    Music = o.Music;
                    Vibration = o.Vibration;
                    ReducedVFX = o.ReducedVFX;
                    ShowJoystick = o.ShowJoystick;
                    Language = (UnityEngine.SystemLanguage)System.Enum.Parse(typeof(UnityEngine.SystemLanguage),o.Language);

                    Localization.SetLocale(Language);
                }

            });
        }


    }

}
