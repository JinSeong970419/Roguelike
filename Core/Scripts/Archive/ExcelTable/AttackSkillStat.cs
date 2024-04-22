using UnityEngine;

namespace Roguelike.Core
{
    [System.Serializable]
    public class AttackSkillStat : SkillStat
    {
        public WeaponType _weaponType;
        public int _count = 1;
        public float _damage = 1;
        public float _speed = 1;
        
        public float _duration = 1;
        public bool _guided;
        public int _piercingCount;
        public bool _knockback;
        public float _angle;
        public bool _oneShot;
        public float _scope = 1;
        public Aim _aim;
        public float _range = 1;
        public SkillKind _summonSkill;
        public ActorKind _summonActor;

        public WeaponType WeaponType { get { return _weaponType; } set { _weaponType = value; } }
        public int Count { get { return _count; } set { _count = value; } }
        public float Damage { get { return _damage; } set { _damage = value; } }
        public float Speed { get { return _speed; } set { _speed = value; } }
        public float Duration { get { return _duration; } set { _duration = value; } }
        public bool Guided { get { return _guided; } set { _guided = value; } }
        public int PiercingCount { get { return _piercingCount; } set { _piercingCount = value; } }
        public bool Knockback { get { return _knockback; } set { _knockback = value; } }
        public float Angle { get { return _angle; } set { _angle = value; } }
        public bool OneShot { get { return _oneShot; } set { _oneShot = value; } }
        public float Scope { get { return _scope; } set { _scope = value; } }
        public Aim Aim { get { return _aim; } set { _aim = value; } }
        public float Range { get { return _range; } set { _range = value; } }
        public SkillKind SummonSkill { get { return _summonSkill; } set { _summonSkill = value; } }
        public ActorKind SummonActor { get { return _summonActor; } set { _summonActor = value; } }

        public AttackSkillStat() : base()
        {

        }
        public AttackSkillStat(SkillStat rhs) : base(rhs)
        {
            AttackSkillStat skillStat = rhs as AttackSkillStat;
            if (skillStat == null)
            {
                Debug.LogError("rhs is not AttackSkillStatss!!");
                return;
            }

            _count = skillStat.Count;
            _damage = skillStat.Damage;
            _speed = skillStat.Speed;
            _duration = skillStat.Duration;
            _guided = skillStat.Guided;
            _piercingCount = skillStat.PiercingCount;
            _knockback = skillStat.Knockback;
            _angle = skillStat.Angle;
            _oneShot = skillStat.OneShot;
            _scope = skillStat.Scope;
            _aim = skillStat.Aim;
            _range = skillStat.Range;
            _summonSkill = skillStat.SummonSkill;
            _summonActor= skillStat.SummonActor;
        }

        public override void Copy(SkillStat rhs)
        {
            base.Copy(rhs);

            AttackSkillStat skillStat = rhs as AttackSkillStat;
            if (skillStat == null)
            {
                Debug.LogError("rhs is not AttackSkillStatss!!");
                return;
            }

            _count = skillStat.Count;
            _damage = skillStat.Damage;
            _speed = skillStat.Speed;
            _duration = skillStat.Duration;
            _guided = skillStat.Guided;
            _piercingCount = skillStat.PiercingCount;
            _knockback = skillStat.Knockback;
            _angle = skillStat.Angle;
            _oneShot = skillStat.OneShot;
            _scope = skillStat.Scope;
            _aim = skillStat.Aim;
            _range = skillStat.Range;
            _summonSkill = skillStat.SummonSkill;
            _summonActor = skillStat.SummonActor;
        }
    }

}
