using UnityEngine;

namespace Roguelike.Core
{
    [CreateAssetMenu(fileName = "LocalizationSettings", menuName = "TheSalt/Settings/LocalizationSettings")]
    public class LocalizationSettings : ScriptableObject
    {
        [SerializeField] private Variable<SystemLanguage> language;

        public SystemLanguage Language { get { return language.Value; } }

    }
}
