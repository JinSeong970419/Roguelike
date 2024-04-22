
using UnityEngine;

namespace Roguelike.Core
{
    [CreateAssetMenu(fileName = "GameEventSpawnMonster", menuName = "TheSalt/Game Event/SpawnMonsterEventArgs", order = 200)]
    public class GameEventSpawnMonster : GameEvent<SpawnMonsterEventArgs>
    {
    }
}

