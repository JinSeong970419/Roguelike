using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Roguelike.Core
{
    [CreateAssetMenu(fileName = "SkillSettings", menuName = "TheSalt/Settings/SkillSettings")]
    public class SkillSettings : ScriptableObject
    {
        [SerializeField] private List<SkillInfo> skillInfos = new List<SkillInfo>();

        public List<SkillInfo> SkillInfos { get { return skillInfos; } }

        [DebugButton]
        public void GenerateSkillKind()
        {
            const string fileName = "SkillKind.cs";
            string path = "Assets/Core/Scripts/Skill/";
            string fullPath = FileManager.Combine(path, fileName);
            var list = skillInfos.Select(o => o.Name);
            Extension.GenerateEnumWithEnd(fullPath, "SkillKind", list);

            int count = skillInfos.Count;
            for (int i = 0; i < count; i++)
            {
                skillInfos[i].Kind = (SkillKind)i;
            }
        }
    }
}
