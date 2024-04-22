using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.Compilation;
#endif

namespace Roguelike.Core
{
    [CreateAssetMenu(fileName = "ItemSettings", menuName = "TheSalt/Settings/ItemSettings")]
    public class ItemSettings : ScriptableObject
    {
        private const string fileName = "ItemKind";
        [SerializeField] private string path = "Assets/Core/Scripts/Item/";

        [SerializeField] List<Item> items= new List<Item>();

        public IReadOnlyList<Item> Items { get { return items.AsReadOnly();} }

        [DebugButton]
        public void GenerateItemKind()
        {
#if UNITY_EDITOR
            string fullPath = FileManager.Combine(path, $"{fileName}.cs");

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("namespace TheSalt.Core");
            stringBuilder.AppendLine("{");
            stringBuilder.AppendLine($"\tpublic enum {fileName}");
            stringBuilder.AppendLine("\t{");
            int count = items.Count;
            for (int i = 0; i < count; i++)
            {
                var param = items[i];
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
