using System.Collections.Generic;
using UnityEngine;

namespace Roguelike.Core
{
    public class Skill_Satellite : AttackSkill
    {
        private class Satellite
        {
            public float Radius;
            public float Distance;
            public Projectile Projectile;
        }

        private float additiveAngle = 0f;
        private List<Satellite> satellites = new List<Satellite>();

        private float shotTick = 0f;
        private float shotDelay = 0.1f;
        private int shotCount = 0;

        public Skill_Satellite(Actor owner) : base(owner)
        {
        }

        public override void OnAdd()
        {
        }

        public override void OnActivate()
        {
            shotCount = 0;
        }

        public override void OnRemove()
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

            AttackSkillStat skillStat = SkillInfo.Stats[(int)Level] as AttackSkillStat;

            if (skillStat.OneShot)
            {
                float anglePerUnit = (1f / skillStat.Count) * Mathf.PI * 2f;
                for (int i = 0; i < skillStat.Count; i++)
                {
                    float angle = i * anglePerUnit + additiveAngle;
                    CreateSatellite(angle, skillStat.Scope);
                    shotCount++;
                }
            }
            else
            {
                shotTick += Time.fixedDeltaTime;
                if (shotTick > shotDelay)
                {
                    shotTick = 0f;
                    shotCount++;
                    Shot();
                }
            }
        }

        private void Shot()
        {
            AttackSkillStat skillStat = SkillInfo.Stats[(int)Level] as AttackSkillStat;
            CreateSatellite(0f, skillStat.Scope);
        }

        private void CreateSatellite(float radius, float dist)
        {
            Vector3 fixedPosition = Owner.transform.position + Owner.ProjectileSpawnOffset;

            var projectileObj = ObjectPool.Instance.Allocate(EntityType.Projectile);
            var projectile = projectileObj.GetComponent<Projectile>();
            projectile.SetSkillKind(SkillKind);
            projectile.Initialize();
            projectile.Owner = this.Owner;
            projectile.Stat = new AttackSkillStat(Stat);
            projectile.Team = Owner.Team;

            projectile.IsSatellite = true;
            projectile.SatelliteAngle = radius;
            projectile.SatelliteDistance = dist;

            Vector3 pos = fixedPosition + new Vector3(Mathf.Cos(radius), Mathf.Sin(radius)) * dist;
            projectile.transform.position = pos;
            projectile.transform.localScale = new Vector3(projectile.Stat.Range, projectile.Stat.Range, 1f);

            satellites.Add(new Satellite() { Radius = radius, Distance = dist, Projectile = projectile });
        }

        public override void OnLevelUp()
        {
            
        }

        public override void OnUpdate()
        {
            
        }
    }
}
