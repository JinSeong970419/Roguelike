using UnityEngine;

namespace Roguelike.Core
{
    public class MoveStartEventArgs
    {
        public int EntityID;
        public Vector3 Direction;
    }

    public class MoveEndEventArgs
    {
        public int EntityID;
    }

    public struct TriggerEnterEventArgs
    {
        public Entity Self;
        public Entity Target;
    }

    public struct TriggerStayEventArgs
    {
        public Entity Self;
        public Entity Target;
    }

    public struct TriggerLeaveEventArgs
    {
        public Entity Self;
        public Entity Target;
    }

    public struct SpawnMonsterEventArgs
    {
        public ActorKind Kind;
        public int Count;
    }

}
