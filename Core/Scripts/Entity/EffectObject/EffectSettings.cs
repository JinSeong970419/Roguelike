using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Roguelike.Core
{
    [CreateAssetMenu(fileName = "EffectSettings", menuName = "TheSalt/Settings/EffectSettings")]
    public class EffectSettings : ScriptableObject
    {
        [SerializeField] private List<EffectInfo> effectInfos = new List<EffectInfo>();

        public List<EffectInfo> EffectInfos { get { return effectInfos; } }

        [DebugButton]
        public void GenerateEffectKind()
        {
            const string fileName = "EffectKind.cs";
            string path = "Assets/Core/Scripts/Entity/EffectObject/";
            string fullPath = FileManager.Combine(path, fileName);
            var list = effectInfos.Select(o => o.Name);
            Extension.GenerateEnum(fullPath, "EffectKind", list);
        }
    }
}
