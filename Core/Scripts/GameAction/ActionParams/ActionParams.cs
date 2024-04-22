using UnityEngine;

namespace Roguelike.Core
{
    [System.Serializable]
    public class SpawnMonsterParam
    {
        [SerializeField] public ActorKind kind;
        [SerializeField] public int count;
        [SerializeField] public float distance;
    }

    [System.Serializable]
    public class SpawnItemAtRandomParam
    {
        [SerializeField] public ItemKind kind;
    }

    [System.Serializable]
    public class ZoomParam
    {
        [SerializeField] public float zoom;
        [SerializeField] public float time;
    }
}
