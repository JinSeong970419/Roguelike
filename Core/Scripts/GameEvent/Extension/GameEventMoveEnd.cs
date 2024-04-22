
using UnityEngine;

namespace Roguelike.Core
{
    [CreateAssetMenu(fileName = "GameEventMoveEnd", menuName = "TheSalt/Game Event/MoveEndEventArgs", order = 200)]
    public class GameEventMoveEnd : GameEvent<MoveEndEventArgs>
    {
    }
}

