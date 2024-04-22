using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Roguelike.Core
{
    public class PopupSetting : Popup
    {
        [SerializeField] private TMP_Text userIdText;
        [SerializeField] private Toggle soundToggle;
        [SerializeField] private Toggle musicToggle;
        [SerializeField] private Toggle vibrationToggle;
        [SerializeField] private Toggle reducedVFXToggle;
        [SerializeField] private Toggle showJoystickToggle;
        [SerializeField] private TMP_Dropdown dropdown;

        protected override void OnEnable()
        {
            base.OnEnable();

            userIdText.text = "ID: " + FirestoreManager.Instance.UserData.UserId;

            DataManager.Instance.UserSettings.Load();

            soundToggle.isOn = DataManager.Instance.UserSettings.Sound;
            musicToggle.isOn = DataManager.Instance.UserSettings.Music;
            vibrationToggle.isOn = DataManager.Instance.UserSettings.Vibration;
            reducedVFXToggle.isOn = DataManager.Instance.UserSettings.ReducedVFX;
            showJoystickToggle.isOn = DataManager.Instance.UserSettings.ShowJoystick;

            dropdown.value = Localization.GetLocaleID();
            UnityEngine.Localization.Settings.LocalizationSettings.SelectedLocaleChanged += OnSelectedLocaleChanged;

            soundToggle.onValueChanged.AddListener(OnSoundChanged);
            musicToggle.onValueChanged.AddListener(OnMusicChanged);
            vibrationToggle.onValueChanged.AddListener(OnVibrationChanged);
            reducedVFXToggle.onValueChanged.AddListener(OnReducedVFXChanged);
            showJoystickToggle.onValueChanged.AddListener(OnShowJoystickChanged);
        }


        protected override void OnDisable()
        {
            base.OnDisable();

            soundToggle.onValueChanged.RemoveListener(OnSoundChanged);
            musicToggle.onValueChanged.RemoveListener(OnMusicChanged);
            vibrationToggle.onValueChanged.RemoveListener(OnVibrationChanged);
            reducedVFXToggle.onValueChanged.RemoveListener(OnReducedVFXChanged);
            showJoystickToggle.onValueChanged.RemoveListener(OnShowJoystickChanged);
        }

        public void OnBackButtonClick()
        {
            Close();
        }

        public void OnSaveButtonClick()
        {
            DataManager.Instance.UserSettings.Save();
        }

        public void OnSelectedLanguageChanged(int langIndex) // 콤보박스에서 변경시
        {
            Localization.SetLocale(Localization.LocaleIDToLanguage(langIndex));
        }
        private void OnSelectedLocaleChanged(UnityEngine.Localization.Locale obj) // 로케일 변경시
        {
            dropdown.value = Localization.GetLocaleID();
            DataManager.Instance.UserSettings.Save();
        }

        private void OnSoundChanged(bool value)
        {
            DataManager.Instance.UserSettings.Sound = value;
            DataManager.Instance.UserSettings.Save();
        }

        private void OnMusicChanged(bool value)
        {
            DataManager.Instance.UserSettings.Music = value;
            DataManager.Instance.UserSettings.Save();
        }

        private void OnVibrationChanged(bool value)
        {
            DataManager.Instance.UserSettings.Vibration = value;
            DataManager.Instance.UserSettings.Save();
        }

        private void OnReducedVFXChanged(bool value)
        {
            DataManager.Instance.UserSettings.ReducedVFX = value;
            DataManager.Instance.UserSettings.Save();
        }

        private void OnShowJoystickChanged(bool value)
        {
            DataManager.Instance.UserSettings.ShowJoystick = value;
            DataManager.Instance.UserSettings.Save();
        }
    }
}
