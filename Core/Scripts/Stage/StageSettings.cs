using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.Compilation;
#endif

namespace Roguelike.Core
{
    [CreateAssetMenu(fileName = "StageSettings", menuName = "TheSalt/Settings/StageSettings")]
    public class StageSettings : ScriptableObject
    {
        private const string fileName = "StageKind.cs";
        [SerializeField] private string path = "Assets/Core/Scripts/Stage/";

        [SerializeField] private Variable<int> spawnedMonsterCount;
        [SerializeField] private Variable<float> startTime;
        [SerializeField] private Variable<float> playTime;

        [SerializeField] private StageInfo[] stageInfos = new StageInfo[1];

        private MethodInfo[] methodInfos;

        public Variable<int> SpawnedMonsterCount { get { return spawnedMonsterCount; } }
        public Variable<float> StartTime { get { return startTime; } }
        public Variable<float> PlayTime { get { return playTime; } }
        public StageInfo[] StageInfos { get { return stageInfos; } }

        private void OnEnable()
        {
            if(methodInfos == null)
            {
                Type classType = typeof(StageLogic);
                methodInfos = classType.GetMethods(BindingFlags.Public | BindingFlags.Static);
            }
        }

        [DebugButton]
        public void GenerateStageKind()
        {
#if UNITY_EDITOR
            string fullPath = FileManager.Combine(path, fileName);

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("namespace TheSalt.Core");
            stringBuilder.AppendLine("{");
            stringBuilder.AppendLine("\tpublic enum StageKind");
            stringBuilder.AppendLine("\t{");
            int count = stageInfos.Length;
            for (int i = 0; i < count; i++)
            {
                var param = stageInfos[i];
                stringBuilder.AppendLine($"\t\t{param.CodeName},");
            }
            stringBuilder.AppendLine($"\t\tEnd");
            stringBuilder.AppendLine("\t}");
            stringBuilder.AppendLine("}");

            string code = stringBuilder.ToString();
            File.WriteAllText(fullPath, code);
            Debug.Log($"Genetated {fullPath}");
            CompilationPipeline.RequestScriptCompilation();
#endif
        }

        [DebugButton]
        public void GenerateStageLogic()
        {
            string fullPath = FileManager.Combine(path, "StageLogicKind.cs");

            Type classType = typeof(StageLogic);
            MethodInfo[] methodInfos = classType.GetMethods(BindingFlags.Public | BindingFlags.Static);

            Extension.GenerateEnum(fullPath, "StageLogicKind", methodInfos.Select(o => o.Name).ToList());
        }
    }
}
