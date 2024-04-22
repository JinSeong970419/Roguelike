namespace Roguelike.Core
{
    public abstract class AttackSkill : Skill
    {
        public AttackSkillStat Stat { get { return SkillStats as AttackSkillStat; } }
        protected AttackSkill(Actor owner) : base(owner)
        {
        }
    }
}
