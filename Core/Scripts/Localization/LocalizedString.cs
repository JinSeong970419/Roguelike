using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

namespace Roguelike.Core
{
    [RequireComponent(typeof(TMP_Text))]
    public class LocalizedString : MonoBehaviour
    {
        [SerializeField] TMP_Text text;
        [TextArea]
        [SerializeField] private string textFormat;

        private void OnEnable()
        {
            UnityEngine.Localization.Settings.LocalizationSettings.SelectedLocaleChanged += OnSelectedLocaleChanged;
        }

        private void OnDisable()
        {
            UnityEngine.Localization.Settings.LocalizationSettings.SelectedLocaleChanged -= OnSelectedLocaleChanged;
        }

        private void OnSelectedLocaleChanged(Locale obj)
        {
            Parse();
        }

        private void OnValidate()
        {
            if (text == null) text = GetComponent<TMP_Text>();
        }

        [DebugButton]
        public void Parse()
        {
            if (string.IsNullOrEmpty(textFormat)) return;

            List<ParsingElement> elements = new List<ParsingElement>();
            string parsed = textFormat;
            int len = parsed.Length;
            int startIndex = 0;
            for (int i = 0; i < len; i++)
            {
                char c = parsed[i];
                if (c == '{')
                {
                    startIndex = i;
                    continue;
                }
                else if (c == '}')
                {
                    string key;
                    string part = parsed.Substring(startIndex, (i - startIndex) + 1);
                    key = part.Trim('{');
                    key = key.Trim('}');
                    key = key.Trim(' ');

                    if (DataManager.Instance.Storage.LocalizationData.TryGetValue(key, out var value))
                    {
                        ParsingElement element = new ParsingElement();
                        element.OldValue = part;
                        element.NewValue = value.LocalizedString;
                        elements.Add(element);
                    }

                }

            }

            int count = elements.Count;
            for (int i = 0; i < count; i++)
            {
                ParsingElement element = elements[i];
                parsed = parsed.Replace(element.OldValue, element.NewValue);
            }

            text.text = parsed;

        }
    }

    public struct ParsingElement
    {
        public string OldValue;
        public string NewValue;
    }
}