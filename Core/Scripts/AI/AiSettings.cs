using System;
using System.IO;
using System.Linq;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Roguelike.Core
{
    [CreateAssetMenu(fileName = "AI Settings", menuName = "TheSalt/Settings/AI Settings")]
    public class AiSettings : ScriptableObject
    {
        private const string fileName = "AiLogicType";
        private const string fileName2 = "AiVariableType";
        [SerializeField] private string path = "Assets/Core/Scripts/AI/";

        [DebugButton]
        public void GenerateAiLogicType()
        {
            string fullPath = Path.Combine(path, fileName + ".cs");
            Type classType = typeof(AI);
            MethodInfo[] methodInfos = classType.GetMethods(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            Extension.GenerateEnum(fullPath, fileName, methodInfos.Select(o => o.Name));

            string fullPath2 = Path.Combine(path, fileName2 + ".cs");
            FieldInfo[] fieldInfos = classType.GetFields(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            Extension.GenerateEnum(fullPath2, fileName2, fieldInfos.Select(o => o.Name));
        }

    }
}
