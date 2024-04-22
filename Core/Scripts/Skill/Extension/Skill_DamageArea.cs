using UnityEngine;

namespace Roguelike.Core
{
    public class Skill_DamageArea : AttackSkill
    {
        private float shotTick = 0f;
        private float shotDelay = 0.1f;
        private int shotCount = 0;

        public Skill_DamageArea(Actor owner) : base(owner)
        {
        }

        public override void OnActivate()
        {
            shotCount = 0;
        }

        public override void OnAdd()
        {

        }

        public override void OnLevelUp()
        {

        }

        public override void OnRemove()
        {

        }

        public override void OnUpdate()
        {
            
        }

        public override void Update()
        {
            base.Update();
            ProcessShot();
        }

        private void ProcessShot()
        {
            if (shotCount >= Stat.Count) return;

            if (Stat.OneShot)
            {
                for (int i = 0; i < Stat.Count; i++)
                {
                    Shot();
                }
            }
            else
            {
                shotTick += Time.fixedDeltaTime;
                if (shotTick > shotDelay)
                {
                    shotTick = 0f;
                    Shot();
                }
            }

        }

        private void Shot()
        {
            Vector3 fixedPosition;

            switch (Stat.Aim)
            {
                case Aim.Normal:
                case Aim.Auto:
                    {
                        Actor target = null;

                        Vector3 forward = Owner.Direction.normalized;
                        float angleHalf = Stat.Angle * 0.5f;

                        var monsters = GameManager.Instance.Monsters;
                        int monsterCount = monsters.Count;
                        if (monsterCount == 0) return;
                        for (int i = 0; i < monsterCount; i++)
                        {
                            var monster = monsters[i];
                            Vector3 to = (monster.transform.position - Owner.transform.position).normalized;
                            var angleMonster = Mathf.Acos(Vector3.Dot(forward, to)) * Mathf.Rad2Deg;
                            if (angleMonster > angleHalf) continue;

                            target = monster;
                            break;
                        }

                        if (target == null)
                        {
                            Debug.LogError("Target is null");
                            return;
                        }
                        fixedPosition = target.transform.position;
                    }
                    break;
                case Aim.Random:
                    {
                        float ownerAngle = Vector3.Angle(Vector3.right, Owner.Direction);
                        if (Owner.Direction.y < 0f)
                        {
                            ownerAngle = 360f - ownerAngle;
                        }
                        ownerAngle -= Stat.Angle * 0.5f;
                        ownerAngle *= Mathf.Deg2Rad;

                        float radian = Random.Range(0f, Stat.Angle) * Mathf.Deg2Rad;
                        radian += ownerAngle;

                        float randomScope = Random.Range(0f, Stat.Scope);
                        fixedPosition = new Vector3(Mathf.Cos(radian), Mathf.Sin(radian)) * randomScope + Owner.transform.position;
                    }
                    break;
                default:
                    Debug.LogError("Invalid Aim.");
                    return;
            }

            var damageAreaObj = ObjectPool.Instance.Allocate(EntityType.DamageArea);
            var damageArea = damageAreaObj.GetComponent<DamageArea>();
            damageArea.SetDamageArea(DamageAreaKind.Circle);
            damageArea.Owner = Owner;
            damageArea.Team = Owner.Team;
            damageArea.Stat = new AttackSkillStat(SkillInfo.Stats[(int)Level]);
            damageArea.Duration = damageArea.Stat.Duration;
            damageArea.IsFadeout = true;

            damageArea.transform.position = fixedPosition;
            damageArea.transform.localScale = new Vector3(damageArea.Stat.Range, damageArea.Stat.Range, 1f);

            //effect
            GameObject effectObj = ObjectPool.Instance.Allocate(EntityType.EffectObject);
            var effect = effectObj.GetComponent<EffectObject>();
            effect.SetEffectKind(EffectKind.Lightning);
            effect.transform.position = fixedPosition;
            effect.transform.localScale = new Vector3(damageArea.Stat.Range, damageArea.Stat.Range, 1f);

            shotCount++;
        }
    }
}
