using UnityEngine;

namespace Roguelike.Core
{
    [System.Serializable]
    public class ActorStats : ExcelRowData
    {
        [SerializeField] private bool isBoss = false;
        [SerializeField] private bool isInvincibility = false;
        [SerializeField] private float maxHp = 1f;
        [SerializeField] private float hp = 1f;
        [SerializeField] private float recovery = 0f;
        [SerializeField] private float regen = 0f;
        [SerializeField] private float damage = 1f;
        [SerializeField] private float closeDamage = 1f;
        [SerializeField] private float closeRange = 1f;
        [SerializeField] private float projectileDamage = 1f;
        [SerializeField] private float projectileRange = 1f;
        [SerializeField] private float projectileSpeed = 1f;
        [SerializeField] private float bombDamage = 1f;
        [SerializeField] private float bombRange = 1f;
        [SerializeField] private float normalDamage = 1f;
        [SerializeField] private float normalRange = 1f;
        [SerializeField] private float cooltimeReduction = 0f;
        [SerializeField] private float duration = 1f;
        [SerializeField] private float movementSpeed = 0f;
        [Range(0f, 1f)]
        [SerializeField] private float criticalChance = 0f;
        [Range(0f, 3f)]
        [SerializeField] private float criticalDamage = 1.2f;
        [Range(0f, 0.99f)]
        [SerializeField] private float damageReduction = 0f;
        [SerializeField] private float luck = 0f;
        [Range(0f, 0.5f)]
        [SerializeField] private float avoid = 0f;
        [SerializeField] private float magnet;
        [SerializeField] private float growth;
        [SerializeField] private bool knockback;

        public bool IsBoss { get { return isBoss; } set { isBoss = value; } }
        public bool IsInvincibility { get { return isInvincibility; } set { isInvincibility = value; } }
        public float MaxHp { get { return maxHp; } set { maxHp = value; } }
        public float Hp { get { return hp; } set { hp = value; } }
        public float Recovery { get { return recovery; } set { recovery = value; } }
        public float Regen { get { return regen; } set { regen = value; } }
        public float Damage { get { return damage; } set { damage = value; } }
        public float CloseDamage { get { return closeDamage; } set { closeDamage = value; } }
        public float CloseRange { get { return closeRange; } set { closeRange = value; } }
        public float ProjectileDamage { get { return projectileDamage; } set { projectileDamage = value; } }
        public float BulletRange { get { return projectileRange; } set { projectileRange = value; } }
        public float BulletSpeed { get { return projectileSpeed; } set { projectileSpeed = value; } }
        public float BombDamage { get { return bombDamage; } set { bombDamage = value; } }
        public float BombRange { get { return bombRange; } set { bombRange = value; } }
        public float NormalDamage { get { return normalDamage; } set { normalDamage = value; } }
        public float NormalRange { get { return normalRange; } set { normalRange = value; } }
        public float CooltimeReduction { get { return cooltimeReduction; } set { cooltimeReduction = value; } }
        public float Duration { get { return duration; } set { duration = value; } }
        public float MovementSpeed { get { return movementSpeed; } set { movementSpeed = value; } }
        public float CriticalChance { get { return criticalChance; } set { criticalChance = value; } }
        public float CriticalDamage { get { return criticalDamage; } set { criticalDamage = value; } }
        public float DamageReduction { get { return damageReduction; } set { damageReduction = value; } }
        public float Luck { get { return luck; } set { luck = value; } }
        public float Avoid { get { return avoid; } set { avoid = value; } }
        public float Magnet { get { return magnet; } set { magnet = value; } }
        public float Growth { get { return growth; } set { growth = value; } }
        public bool Knockback { get { return knockback; } set { knockback = value; } }

        public ActorStats()
        {

        }
        public ActorStats(ActorStats rhs)
        {
            isBoss = rhs.isBoss;
            isInvincibility = rhs.isInvincibility;
            maxHp = rhs.maxHp;
            hp = rhs.hp;
            recovery = rhs.recovery;
            regen = rhs.regen;
            damage = rhs.damage;
            closeDamage = rhs.closeDamage;
            closeRange = rhs.closeRange;
            projectileDamage = rhs.projectileDamage;
            projectileRange = rhs.projectileRange;
            projectileSpeed = rhs.projectileSpeed;
            bombDamage = rhs.bombDamage;
            bombRange = rhs.bombRange;
            normalDamage = rhs.normalDamage;
            normalRange = rhs.normalRange;
            cooltimeReduction = rhs.cooltimeReduction;
            duration = rhs.duration;
            movementSpeed = rhs.movementSpeed;
            criticalChance = rhs.criticalChance;
            criticalDamage = rhs.criticalDamage;
            damageReduction = rhs.damageReduction;
            luck = rhs.luck;
            avoid = rhs.avoid;
            magnet = rhs.magnet;
            growth = rhs.growth;
            knockback = rhs.knockback;
        }

        public void Copy(ActorStats rhs)
        {
            isBoss = rhs.isBoss;
            isInvincibility = rhs.isInvincibility;
            maxHp = rhs.maxHp;
            hp = rhs.hp;
            recovery = rhs.recovery;
            regen = rhs.regen;
            damage = rhs.damage;
            closeDamage = rhs.closeDamage;
            closeRange = rhs.closeRange;
            projectileDamage = rhs.projectileDamage;
            projectileRange = rhs.projectileRange;
            projectileSpeed = rhs.projectileSpeed;
            bombDamage = rhs.bombDamage;
            bombRange = rhs.bombRange;
            normalDamage = rhs.normalDamage;
            normalRange = rhs.normalRange;
            cooltimeReduction = rhs.cooltimeReduction;
            duration = rhs.duration;
            movementSpeed = rhs.movementSpeed;
            criticalChance = rhs.criticalChance;
            criticalDamage = rhs.criticalDamage;
            damageReduction = rhs.damageReduction;
            luck = rhs.luck;
            avoid = rhs.avoid;
            magnet = rhs.magnet;
            growth = rhs.growth;
            knockback = rhs.knockback;
        }

        public float GetElementalDamage(WeaponType type)
        {
            switch (type)
            {
                case WeaponType.Normal: return NormalDamage;
                case WeaponType.Projectile: return ProjectileDamage;
                case WeaponType.Bomb: return BombDamage;
                case WeaponType.Close: return CloseDamage;
                default:
                    return 0f;
            }
        }

        public float GetTotalCriticalChance()
        {
            return criticalChance + (luck * 0.05f);
        }
    }
}
