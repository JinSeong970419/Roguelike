using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Roguelike.Core
{
    public class Localization : MonoBehaviour
    {
        private static Localization instance;

        [SerializeField] private GameEvent<bool> isNewlyEvent;
        [SerializeField] private Variable<SystemLanguage> language;
        [SerializeField] private Variable<bool> newRegisteredUser;

        public SystemLanguage Language { get { return language.Value; } }
        public static SystemLanguage CurrentLanguage
        {
            get
            {
                int id = GetLocaleID();
                return LocaleIDToLanguage(id);
            }
        }


        private static bool waitForChangeLocale = false;


        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        private void OnEnable()
        {
            isNewlyEvent.AddListener(IsNewlyEventCallback);
            language.OnValueChanged.AddListener(OnLanguageChanged);
            UnityEngine.Localization.Settings.LocalizationSettings.SelectedLocaleChanged += OnSelectedLocaleChanged;

            if(newRegisteredUser.Value)
            {
                SetLocale(Application.systemLanguage);
            }
        }

        

        private void OnDisable()
        {
            isNewlyEvent.RemoveListener(IsNewlyEventCallback);
            language.OnValueChanged.RemoveListener(OnLanguageChanged);
        }

        public static int GetLocaleID()
        {
            int index = UnityEngine.Localization.Settings.LocalizationSettings.AvailableLocales.Locales.IndexOf(UnityEngine.Localization.Settings.LocalizationSettings.SelectedLocale);
            return index;
        }

        public static void SetLocale(SystemLanguage lang)
        {
            if (waitForChangeLocale) return;

            int localeID = LanguageToLocaleID(lang);
            int currentLocaleID = GetLocaleID();
            if (currentLocaleID != localeID)
            {
                instance.StartCoroutine(SetLocaleInternal(localeID));
            }

            instance.language.Value = lang;
        }

        private static IEnumerator SetLocaleInternal(int localeID)
        {
            waitForChangeLocale = true;
            yield return UnityEngine.Localization.Settings.LocalizationSettings.InitializationOperation;
            UnityEngine.Localization.Settings.LocalizationSettings.SelectedLocale = UnityEngine.Localization.Settings.LocalizationSettings.AvailableLocales.Locales[localeID];
            waitForChangeLocale = false;
        }

        public static int LanguageToLocaleID(SystemLanguage lang)
        {
            switch (lang)
            {
                case SystemLanguage.English: return 0;
                case SystemLanguage.Korean: return 1;
                case SystemLanguage.Japanese: return 2;
                case SystemLanguage.ChineseSimplified: return 3;
                case SystemLanguage.ChineseTraditional: return 4;
                default:
                    Debug.LogError("This language is not supported.");
                    break;
            }
            return 0;
        }

        public static SystemLanguage LocaleIDToLanguage(int localeID)
        {
            switch (localeID)
            {
                case 0: return SystemLanguage.English;
                case 1: return SystemLanguage.Korean;
                case 2: return SystemLanguage.Japanese;
                case 3: return SystemLanguage.ChineseSimplified;
                case 4: return SystemLanguage.ChineseTraditional;
                default:
                    Debug.LogError($"This language is not supported. [{localeID}]");
                    break;
            }
            return SystemLanguage.English;
        }


        private void IsNewlyEventCallback(bool isNewly)
        {
            if (isNewly == false) return;

            SetLocale(Application.systemLanguage);
        }

        private void OnLanguageChanged(SystemLanguage lang)
        {
            Debug.Log(lang);
        }

        private void OnSelectedLocaleChanged(UnityEngine.Localization.Locale obj) // 로케일 변경시
        {
            int index = UnityEngine.Localization.Settings.LocalizationSettings.AvailableLocales.Locales.IndexOf(obj);
            SystemLanguage lang = LocaleIDToLanguage(index);
            language.Value = lang;
        }
    }
}
