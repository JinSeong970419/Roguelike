using System.Collections.Generic;
using UnityEngine;

namespace Roguelike.Core
{
    [CreateAssetMenu(fileName = "SceneSettings", menuName = "TheSalt/Settings/SceneSettings")]
    public class SceneSettings : ScriptableObject
    {
        [SerializeField] private List<string> gameScenes;

        public IReadOnlyList<string> GameScenes { get { return gameScenes.AsReadOnly(); } }
        public bool IsGameScene(string scene)
        {
            return gameScenes.Contains(scene);
        }
    }
}
