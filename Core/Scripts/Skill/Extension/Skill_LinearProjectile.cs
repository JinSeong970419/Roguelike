using UnityEngine;

namespace Roguelike.Core
{
    public class Skill_LinearProjectile : AttackSkill
    {
        private float shotTick = 0f;
        private float shotDelay = 0.1f;
        private int shotCount = 0;

        public Skill_LinearProjectile(Actor owner) : base(owner)
        {
        }

        public override void OnAdd()
        {

        }

        public override void OnRemove()
        {

        }

        public override void OnActivate()
        {
            shotCount = 0;
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
            Vector3 direction;
            Vector3 fixedPosition = Owner.transform.position + Owner.ProjectileSpawnOffset;

            switch (Stat.Aim)
            {
                case Aim.Normal:
                    direction = Owner.Direction;
                    break;
                case Aim.Auto:
                    {
                        Actor target = null;
                        
                        Vector3 forward = Owner.Direction.normalized;
                        float angleHalf = Stat.Angle * 0.5f;

                        var monsters = GameManager.Instance.Monsters;
                        int monsterCount = monsters.Count;
                        if (monsterCount == 0) return;
                        for(int i=0; i<monsterCount; i++)
                        {
                            var monster = monsters[i];
                            Vector3 to = (monster.transform.position-Owner.transform.position).normalized;
                            var angleMonster = Mathf.Acos(Vector3.Dot(forward, to)) * Mathf.Rad2Deg;
                            if (angleMonster > angleHalf) continue;

                            target = monster;
                            break;
                        }

                        if (target == null) return;
                        direction = target.transform.position - fixedPosition;
                    }
                    break;
                case Aim.Random:
                    {
                        float ownerAngle = Vector3.Angle(Vector3.right, Owner.Direction);
                        if(Owner.Direction.y < 0f)
                        {
                            ownerAngle = 360f-ownerAngle;
                        }
                        ownerAngle -= Stat.Angle * 0.5f;
                        ownerAngle *= Mathf.Deg2Rad;

                        float radian = Random.Range(0f, Stat.Angle) * Mathf.Deg2Rad;
                        radian += ownerAngle;
                        direction = new Vector3(Mathf.Cos(radian), Mathf.Sin(radian));
                    }
                    break;
                default:
                    Debug.LogError("Invalid Aim.");
                    return;
            }


            var projectileObj = ObjectPool.Instance.Allocate(EntityType.Projectile);
            var projectile = projectileObj.GetComponent<Projectile>();
            projectile.SetSkillKind(SkillKind);
            projectile.Initialize();
            projectile.Owner = this.Owner;
            projectile.Stat = new AttackSkillStat(SkillInfo.Stats[(int)Level]);
            projectile.Direction = direction.normalized;
            projectile.Team = Owner.Team;
            projectile.IsGuided = Stat.Guided;
            float angle = Vector3.Angle(new Vector3(-1.0f, 0.0f, 0.0f), projectile.Direction);
            if (projectile.Direction.y > 0.0f)
                angle = -angle;
            var rotation = Quaternion.Euler(0f, 0f, angle);
            projectile.transform.SetPositionAndRotation(fixedPosition, rotation);
            projectile.transform.localScale = new Vector3(projectile.Stat.Range, projectile.Stat.Range, 1f);

            shotCount++;
        }

        public override void OnLevelUp()
        {
            
        }

        public override void OnUpdate()
        {
            
        }
    }
}
