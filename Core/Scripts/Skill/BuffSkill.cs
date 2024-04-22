namespace Roguelike.Core
{
    public abstract class BuffSkill : Skill
    {
        public BuffSkillStat Stat { get { return SkillStats as BuffSkillStat; } }
        public BuffSkill(Actor owner) : base(owner)
        {
        }
    }
}
