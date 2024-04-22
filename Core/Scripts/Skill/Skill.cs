using UnityEngine;

namespace Roguelike.Core
{
    public abstract class Skill
    {
        private bool isCooldown = true;
        private float cooltimeTick = 0f;
        private Actor _owner;
        public Actor Owner => _owner;
        private SkillKind _skillKind;
        public SkillKind SkillKind { get { return _skillKind; } protected set { _skillKind = value; } }
        public Level Level { get; set; }
        public SkillInfo SkillInfo { get { return DataManager.Instance.SkillSettings.SkillInfos[(int)_skillKind]; } }
        public bool IsMaxLevel { get { return SkillInfo.MaxLevel == Level; } }
        public SkillType SkillType { get { return SkillInfo.Type; } }
        public SkillStat SkillStats { get { return SkillInfo.Stats[(int)Level]; } }
        public bool IsCooldown { get { return isCooldown; } }
        public float CooldownTick { get { return cooltimeTick; } }
        public float Cooldown { get { return SkillStats.Cooltime; } }
        public SkillBase SkillBase { get { return SkillInfo.Base; } }

        public Skill(Actor owner)
        {
            _owner = owner;
        }

        public static Skill Create(SkillKind kind, Actor owner)
        {
            Skill skill = null;
            SkillInfo info = DataManager.Instance.SkillSettings.SkillInfos[(int)kind];
            SkillBase _base = info.Base;

            switch (_base)
            {
                case SkillBase.Buff:
                    skill = new Skill_Buff(owner);
                    break;
                case SkillBase.LinearProjectile:
                    skill = new Skill_LinearProjectile(owner);
                    break;
                case SkillBase.RadialProjectile:
                    skill = new Skill_RadialProjectile(owner);
                    break;
                case SkillBase.Satellite:
                    skill = new Skill_Satellite(owner);
                    break;
                case SkillBase.Laser:
                    skill = new Skill_Laser(owner);
                    break;
                case SkillBase.Drone:
                    skill = new Skill_Drone(owner);
                    break;
                case SkillBase.DamageArea:
                    skill = new Skill_DamageArea(owner);
                    break;
                case SkillBase.Summon:
                    break;
                default:
                    Debug.LogError("SkillBase not set!!");
                    break;
            }

            if(skill != null)
            {
                skill.SkillKind= kind;
            }

            return skill;
        }

        public void LevelUp()
        {
            int maxLevel = (int)SkillInfo.MaxLevel;
            int level = (int)Level;
            if (level < maxLevel)
            {
                Level += 1;
                OnLevelUp();
            }
        }

        /// <summary>
        /// 스킬이 추가되었을 때
        /// </summary>
        public abstract void OnAdd();
        /// <summary>
        /// 스킬이 삭제되었을 때
        /// </summary>
        public abstract void OnRemove();
        /// <summary>
        /// 스킬 레벨이 올랐을 때
        /// </summary>
        public abstract void OnLevelUp();
        /// <summary>
        /// 매 프레임마다 수행할 스킬 로직
        /// </summary>
        public abstract void OnUpdate();
        /// <summary>
        /// 매 프레임마다 수행할 스킬 로직
        /// </summary>
        public virtual void Update()
        {
            ProcessCooldown();
            OnUpdate();
        }
        /// <summary>
        /// 스킬을 사용합니다.
        /// </summary>
        public void Activate()
        {
            if (isCooldown) return;
            isCooldown = true;
            cooltimeTick = 0f;
            OnActivate();
        }
        /// <summary>
        /// 스킬을 사용 시 호출됨. 스킬 로직을 작성하세요.
        /// </summary>
        public abstract void OnActivate();

        private void ProcessCooldown()
        {
            if (isCooldown == false) return;

            cooltimeTick += Time.fixedDeltaTime;
            if (cooltimeTick > SkillStats.Cooltime)
            {
                cooltimeTick = 0;
                isCooldown = false;
                if(SkillType == SkillType.Passive)
                {
                    Activate();
                }
            }
        }
    }
}
