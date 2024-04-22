using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Roguelike.Core
{
    [CreateAssetMenu(fileName = "DamageFontSettings", menuName = "TheSalt/Settings/DamageFontSettings")]
    public class DamageFontSettings : ScriptableObject
    {
        private const string fileName = "DamageFontType";
        [SerializeField] private string path = "Assets/Core/Scripts/Entity/DamageFont/";
        [SerializeField] private List<DamageFontInfo> _infos;

        public List<DamageFontInfo> Infos { get { return _infos; } }

        [DebugButton]
        public void Generate()
        {
            string fullPath = FileManager.Combine(path, fileName + ".cs");
            var list = _infos.Select(o => o.Name);
            Extension.GenerateEnum(fullPath, fileName, list);
        }
    }
}
