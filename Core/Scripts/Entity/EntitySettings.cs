using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.Compilation;
#endif

namespace Roguelike.Core
{
    [CreateAssetMenu(fileName = "EntitySettings", menuName = "TheSalt/Settings/EntitySettings")]
    public class EntitySettings : ScriptableObject
    {
        private const string fileName = "EntityType";
        [SerializeField] private string path = "Assets/Core/Scripts/Entity/";
        [SerializeField] private List<GameObject> _prefabs;
        public IReadOnlyList<GameObject> Prefabs { get { return _prefabs.AsReadOnly(); } }

        [DebugButton]
        public void GenerateEntityType()
        {
#if UNITY_EDITOR
            string fullPath = FileManager.Combine(path, $"{fileName}.cs");

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("namespace TheSalt.Core");
            stringBuilder.AppendLine("{");
            stringBuilder.AppendLine($"\tpublic enum {fileName}");
            stringBuilder.AppendLine("\t{");
            int count = _prefabs.Count;
            for (int i = 0; i < count; i++)
            {
                var param = _prefabs[i];
                stringBuilder.AppendLine($"\t\t{param.name},");
            }
            stringBuilder.AppendLine("\t}");
            stringBuilder.AppendLine("}");

            string code = stringBuilder.ToString();
            File.WriteAllText(fullPath, code);
            Debug.Log($"Genetated {fullPath}");
            CompilationPipeline.RequestScriptCompilation();
#endif
        }

    }
}