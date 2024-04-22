using UnityEngine;

namespace Roguelike.Core
{
    [System.Serializable]
    public abstract class SkillStat : ExcelRowData
    {
        public Level _level;
        public string _description;
        public float _cooltime = 1;

        public Level Level { get { return _level; } set { _level = value; } }
        public string Description { get { return _description; } set { _description = value; } }
        public float Cooltime { get { return _cooltime; } set { _cooltime = value; } }

        public SkillStat()
        {

        }

        public SkillStat(SkillStat rhs)
        {

            _description = rhs._description;
            _cooltime = rhs.Cooltime;

        }

        public virtual void Copy(SkillStat rhs)
        {
            _description = rhs._description;
            _cooltime = rhs.Cooltime;
        }
    }
}
