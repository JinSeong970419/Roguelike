using UnityEngine;

namespace Roguelike.Core
{
    [System.Serializable]
    public class BuffSkillStat : SkillStat
    {
        [SerializeField] private float hpBuff;
        [SerializeField] private float regenBuff;
        [SerializeField] private float damageBuff;
        [SerializeField] private float movementSpeedBuff;
        [SerializeField] private float cooltimeReductionBuff;
        [SerializeField] private float durationBuff;
        [SerializeField] private float magnetBuff;
        [SerializeField] private float growthBuff;
        [SerializeField] private int luckBuff;
        [SerializeField] private float avoidBuff;
        [SerializeField] private float damageReductionBuff;

        public float HpBuff { get { return hpBuff; } set { hpBuff = value; } }
        public float RegenBuff { get { return regenBuff; } set { regenBuff = value; } }
        public float DamageBuff { get { return damageBuff; } set { damageBuff = value; } }
        public float MovementSpeedBuff { get { return movementSpeedBuff; } set { movementSpeedBuff = value; } }
        public float CooltimeReductionBuff { get { return cooltimeReductionBuff; } set { cooltimeReductionBuff = value; } }
        public float DurationBuff { get { return durationBuff; } set { durationBuff = value; } }
        public float MagnetBuff { get { return magnetBuff; } set { magnetBuff = value; } }
        public float GrowthBuff { get { return growthBuff; } set { growthBuff = value; } }
        public int LuckBuff { get { return luckBuff; } set { luckBuff = value; } }
        public float AvoidBuff { get { return avoidBuff; } set { avoidBuff = value; } }
        public float DamageReductionBuff { get { return damageReductionBuff; } set { damageReductionBuff = value; } }

        public BuffSkillStat() : base()
        {

        }
        public BuffSkillStat(SkillStat rhs) : base(rhs)
        {
            BuffSkillStat skillStat = rhs as BuffSkillStat;
            if (skillStat == null)
            {
                Debug.LogError("rhs is not BuffSkillStats!!");
                return;
            }

            hpBuff = skillStat.hpBuff;
            regenBuff = skillStat.regenBuff;
            damageBuff = skillStat.damageBuff;
            movementSpeedBuff = skillStat.movementSpeedBuff;
            cooltimeReductionBuff = skillStat.cooltimeReductionBuff;
            durationBuff = skillStat.durationBuff;
            magnetBuff = skillStat.magnetBuff;
            growthBuff = skillStat.growthBuff;
            luckBuff = skillStat.luckBuff;
            avoidBuff = skillStat.avoidBuff;
            damageReductionBuff = skillStat.damageReductionBuff;

        }

        public override void Copy(SkillStat rhs)
        {
            base.Copy(rhs);

            BuffSkillStat skillStat = rhs as BuffSkillStat;
            if (skillStat == null)
            {
                Debug.LogError("rhs is not BuffSkillStats!!");
                return;
            }

            hpBuff = skillStat.hpBuff;
            regenBuff = skillStat.regenBuff;
            damageBuff = skillStat.damageBuff;
            movementSpeedBuff = skillStat.movementSpeedBuff;
            cooltimeReductionBuff = skillStat.cooltimeReductionBuff;
            durationBuff = skillStat.durationBuff;
            magnetBuff = skillStat.magnetBuff;
            growthBuff = skillStat.growthBuff;
            luckBuff = skillStat.luckBuff;
            avoidBuff = skillStat.avoidBuff;
            damageReductionBuff = skillStat.damageReductionBuff;
        }
    }
}
