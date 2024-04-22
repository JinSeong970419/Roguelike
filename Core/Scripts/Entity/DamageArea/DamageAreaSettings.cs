using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Roguelike.Core
{
    [CreateAssetMenu(fileName = "DamageAreaSettings", menuName = "TheSalt/Settings/DamageAreaSettings")] 
    public class DamageAreaSettings : ScriptableObject
    {
        [SerializeField] private List<DamageAreaInfo> damageAreaInfos = new List<DamageAreaInfo>();

        public List<DamageAreaInfo> DamageAreaInfos { get { return damageAreaInfos; } }

        [DebugButton]
        public void GenerateDamageAreaKind()
        {
            const string fileName = "DamageAreaKind.cs";
            string path = "Assets/Core/Scripts/Entity/DamageArea/";
            string fullPath = FileManager.Combine(path, fileName);
            var list = damageAreaInfos.Select(o => o.Name);
            Extension.GenerateEnum(fullPath, "DamageAreaKind", list);
        }
    }
}
