using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.Compilation;
#endif

namespace Roguelike.Core
{
    [CreateAssetMenu(fileName = "AutomataSettings", menuName = "TheSalt/Settings/AutomataSettings")]
    public class AutomataSettings : ScriptableObject
    {
        private const string fileName = "Gen_Automata_Data.cs";
        [SerializeField] private string path = "Assets/Core/Scripts/Automata/";

        [ArrayElementTitle("_name")]
        [SerializeField] private List<Automata.Parameter> parameters = new List<Automata.Parameter>();

        public IReadOnlyList<Automata.Parameter> Parameters { get { return parameters; } }


        [DebugButton]
        public void SetPathToDefault()
        {
            path = "Assets/Core/Scripts/Automata/";
        }
        [DebugButton]
        public void GenerateCodeOfParameterList()
        {
#if UNITY_EDITOR
            string fullPath = FileManager.Combine(path, fileName);

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("namespace TheSalt.Core");
            stringBuilder.AppendLine("{");
            stringBuilder.AppendLine("\tpublic enum ParameterKind");
            stringBuilder.AppendLine("\t{");
            int count = parameters.Count;
            for (int i = 0; i < count; i++)
            {
                var param = parameters[i];
                stringBuilder.AppendLine($"\t\t{param.Name},");
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
