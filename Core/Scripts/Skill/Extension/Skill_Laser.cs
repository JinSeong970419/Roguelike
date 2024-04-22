using System.Collections.Generic;
using UnityEngine;

namespace Roguelike.Core
{
    public class Skill_Laser : AttackSkill
    {
        private List<EffectObject> lasers = new List<EffectObject>();



        public Skill_Laser(Actor owner) : base(owner)
        {
        }

        public override void OnAdd()
        {
            GameObject effectObj = ObjectPool.Instance.Allocate(EntityType.EffectObject);
            var effect = effectObj.GetComponent<EffectObject>();
            effect.SetEffectKind(EffectKind.Laser);
            effectObj.transform.SetParent(Owner.transform);
            effectObj.transform.localPosition = Vector3.zero;
            lasers.Add(effect);
        }

        public override void OnActivate()
        {
            Shot();
        }

        public override void OnRemove()
        {

        }

        public override void OnLevelUp()
        {

        }

        public override void OnUpdate()
        {
            float ownerAngle = Vector3.Angle(Vector3.right, Owner.Direction);
            if (Owner.Direction.y < 0f)
            {
                ownerAngle = 360f - ownerAngle;
            }

            var rotation = Quaternion.Euler(0f, 0f, ownerAngle);

            if(lasers.Count > 0)
            {
                lasers[0].transform.localRotation = rotation;
            }
        }

        private void Shot()
        {
            Vector3 fixedPosition = Owner.transform.position + Owner.ProjectileSpawnOffset;
            float launchAngle;

            switch (Stat.Aim)
            {
                case Aim.Normal:
                    launchAngle = Vector3.Angle(Vector3.right, Owner.Direction);
                    if (Owner.Direction.y < 0f)
                    {
                        launchAngle = 360f - launchAngle;
                    }
                    break;
                case Aim.Auto:
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

                    if (target == null) return;

                    Vector3 toTarget = target.transform.position - Owner.transform.position;
                    launchAngle = Vector3.Angle(Vector3.right, toTarget.normalized);
                    if (Owner.Direction.y < 0f)
                    {
                        launchAngle = 360f - launchAngle;
                    }
                    break;
                case Aim.Random:
                    float ownerAngle = Vector3.Angle(Vector3.right, Owner.Direction);
                    if (Owner.Direction.y < 0f)
                    {
                        ownerAngle = 360f - ownerAngle;
                    }
                    ownerAngle -= Stat.Angle * 0.5f;

                    float randomAngle = Random.Range(0f, Stat.Angle);
                    randomAngle += ownerAngle;
                    launchAngle = randomAngle;
                    break;
                default:
                    Debug.LogError("Invalid Aim.");
                    return;
            }


            var damageAreaObj = ObjectPool.Instance.Allocate(EntityType.DamageArea);
            var damageArea = damageAreaObj.GetComponent<DamageArea>();
            damageArea.SetDamageArea(DamageAreaKind.Laser);
            damageArea.Owner = Owner;
            damageArea.Team= Owner.Team;
            damageArea.Stat = new AttackSkillStat(SkillInfo.Stats[(int)Level]);
            damageArea.Duration = damageArea.Stat.Duration;
            damageArea.IsFadeout = true;

            var rotation = Quaternion.Euler(0f, 0f, launchAngle);
            //damageArea.transform.SetLocalPositionAndRotation(fixedPosition, rotation);
            damageArea.transform.localPosition = fixedPosition;
            damageArea.transform.localRotation = rotation;
            damageArea.transform.localScale = new Vector3(Stat.Range, Stat.Range, 1f);

        }
    }
}
