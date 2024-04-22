using UnityEngine;

namespace Roguelike.Core
{
    [System.Serializable]
    public class LocalizationData : ExcelRowData
    {
        [SerializeField] private string description;
        [SerializeField] private string english;
        [SerializeField] private string korean;
        [SerializeField] private string japanese;
        [SerializeField] private string chineseSimplified;
        [SerializeField] private string chineseTraditional;

        public string Description { get { return description; } }
        public string English { get { return english; } }
        public string Korean { get { return korean; } }
        public string Japanese { get { return japanese; } }
        public string ChineseSimplified { get { return chineseSimplified; } }
        public string ChineseTraditional { get { return chineseTraditional; } }

        public string LocalizedString
        {
            get
            {
                var systemLang = Localization.CurrentLanguage;
                switch (systemLang)
                {
                    case SystemLanguage.English: return English;
                    case SystemLanguage.Korean: return Korean;
                    case SystemLanguage.Japanese: return Japanese;
                    case SystemLanguage.ChineseSimplified: return ChineseSimplified;
                    case SystemLanguage.ChineseTraditional: return ChineseTraditional;
                    default:
                        Debug.LogError("This language is not supported.");
                        break;
                }
                return English;
            }
        }
    }
}
