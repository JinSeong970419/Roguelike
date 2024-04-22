using UnityEngine;

namespace Roguelike.Core
{
    public class Skill_RadialProjectile : AttackSkill
    {
        private float shotTick = 0f;
        private float shotDelay = 0.1f;
        private int shotCount = 0;

        public Skill_RadialProjectile(Actor owner) : base(owner)
        {
        }

        public override void OnActivate()
        {
           shotCount= 0;
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
                    Shot(i);
                }
            }
            else
            {
                shotTick += Time.fixedDeltaTime;
                if (shotTick > shotDelay)
                {
                    shotTick = 0f;
                    Shot(shotCount);
                    shotCount++;
                }
            }
        }

        private void Shot(int index)
        {
            AttackSkillStat skillStat = SkillInfo.Stats[(int)Level] as AttackSkillStat;
            Vector3 fixedPosition = Owner.transform.position + Owner.ProjectileSpawnOffset;
            float totalAngle = Stat.Angle * Mathf.Deg2Rad;
            float anglePerUnit = (1f / skillStat.Count) * totalAngle;
            float ownerAngle = Vector3.Angle(Vector3.right, Owner.Direction);
            if (Owner.Direction.y < 0f)
            {
                ownerAngle = 360f - ownerAngle;
            }
            ownerAngle -= Stat.Angle * 0.5f;
            ownerAngle *= Mathf.Deg2Rad;

            float angle = index * anglePerUnit + ownerAngle;
            Vector3 to = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle));

            var mesObj = ObjectPool.Instance.Allocate(EntityType.Projectile);
            var projectile = mesObj.GetComponent<Projectile>();
            projectile.SetSkillKind(SkillKind);
            projectile.Initialize();
            projectile.Owner = this.Owner;
            projectile.Stat = new AttackSkillStat(SkillInfo.Stats[(int)Level]);
            projectile.Direction = to.normalized;
            projectile.Team = Owner.Team;
            projectile.IsGuided = Stat.Guided;
            float rotationAngle = Vector3.Angle(new Vector3(-1.0f, 0.0f, 0.0f), projectile.Direction);
            if (projectile.Direction.y > 0.0f)
                rotationAngle = -rotationAngle;
            var rotation = Quaternion.Euler(0f, 0f, rotationAngle);
            projectile.transform.SetPositionAndRotation(fixedPosition, rotation);
            projectile.transform.localScale = new Vector3(projectile.Stat.Range, projectile.Stat.Range, 1f);
        }
    }
}
