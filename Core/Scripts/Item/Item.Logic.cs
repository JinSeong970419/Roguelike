using UnityEngine;

namespace Roguelike.Core
{
    public abstract partial class Item : ScriptableObject
    {
        public static void RecoverHp(Actor actor)
        {
            actor.RecoverHp();
        }

        public static void GainExperience(Actor actor)
        {

        }
    }
}
