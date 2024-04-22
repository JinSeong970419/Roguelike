using UnityEngine;

namespace Roguelike.Core
{
    [CreateAssetMenu(fileName = "ConsumableItem", menuName = "TheSalt/Data/ConsumableItem")]
    public class ConsumableItem : Item
    {

        public int Quantity { get; set; }

    }
}
