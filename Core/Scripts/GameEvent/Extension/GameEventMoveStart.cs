
using UnityEngine;

namespace Roguelike.Core
{
    [CreateAssetMenu(fileName = "GameEventMoveStart", menuName = "TheSalt/Game Event/MoveStartEventArgs", order = 200)]
    public class GameEventMoveStart : GameEvent<MoveStartEventArgs>
    {
    }
}

