using UnityEngine;

namespace Roguelike.Core
{
    [CreateAssetMenu(fileName = "Equipment", menuName = "TheSalt/Data/Equipment")]
    public abstract class Equipment : Item
    {
        [SerializeField] private ActorStats stat;

        public ActorStats Stat { get { return stat; } }
    }
}
