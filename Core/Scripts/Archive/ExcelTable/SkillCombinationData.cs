using UnityEngine;

namespace Roguelike.Core
{
    [System.Serializable]
    public class SkillCombinationData : ExcelRowData
    {
        [SerializeField] private SkillKind _skillA;
        [SerializeField] private int _levelA;
        [SerializeField] private SkillKind _skillB;
        [SerializeField] private int _levelB;
        [SerializeField] private SkillKind _resultSkill;

        public SkillKind SkillA { get { return _skillA; } }
        public Level LevelA { get { return (Level)(_levelA - 1); } }
        public SkillKind SkillB { get { return _skillB; } }
        public Level LevelB { get { return (Level)(_levelB - 1); } }
        public SkillKind ResultSkill { get { return _resultSkill; } }


    }
}
