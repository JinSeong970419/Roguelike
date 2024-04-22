using UnityEngine;

namespace Roguelike.Core
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "TheSalt/Settings/GameSettings")]
    public class GameSettings : ScriptableObject
    {
        [SerializeField] private float monsterSpawnDistance;
        [SerializeField] private int maxFieldItemCount;

        public float MonsterSpawnDistance { get { return monsterSpawnDistance; } set { monsterSpawnDistance = value; } }
        public int MaxFieldItemCount { get { return maxFieldItemCount; } set { maxFieldItemCount = value; } }

    }
}
