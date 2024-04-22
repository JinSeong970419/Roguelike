using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Roguelike.Core
{
    [CreateAssetMenu(fileName = "ActorSettings", menuName = "TheSalt/Settings/ActorSettings")]
    public class ActorSettings : ScriptableObject
    {
        private const string fileName = "ActorKind";
        [SerializeField] private string path = "Assets/Core/Scripts/Entity/Actor/";
        [SerializeField] private List<ActorInfo> actorInfos= new List<ActorInfo>();
        /// <summary>
        /// ActorKind ¼ø¼­
        /// </summary>
        public List<ActorInfo> ActorInfos { get { return actorInfos; } }

        [DebugButton]
        public void GenerateActorKind()
        {
            string fullPath = FileManager.Combine(path, $"{fileName}.cs");
            var nameList = actorInfos.Select(o => o.Name);
            Extension.GenerateEnumWithEnd(fullPath, fileName, nameList);
        }
    }
}
