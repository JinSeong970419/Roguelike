using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Roguelike.Core
{
    [CreateAssetMenu(fileName = "DataStorage", menuName = "TheSalt/Settings/DataStorage")]
    public class DataStorage : ScriptableObject
    {
        [SerializeField] private List<ExcelTableCollection> excelTableCollections = new List<ExcelTableCollection>();
        [SerializeField] private SerializableDictionary<string, ActorStats> actorStats = new SerializableDictionary<string, ActorStats>();
        [SerializeField] private SerializableDictionary<string, AttackSkillStat> attackSkillStats = new SerializableDictionary<string, AttackSkillStat>();
        [SerializeField] private SerializableDictionary<string, BuffSkillStat> buffSkillStats = new SerializableDictionary<string, BuffSkillStat>();
        [SerializeField] private SerializableDictionary<string, SkillCombinationData> skillCombinations = new SerializableDictionary<string, SkillCombinationData>();
        [SerializeField] private SerializableDictionary<string, LocalizationData> localizationData = new SerializableDictionary<string, LocalizationData>();

        public List<ExcelTableCollection> ExcelTableCollections { get { return excelTableCollections; } }
        public SerializableDictionary<string, ActorStats> ActorStats { get { return actorStats; } }
        public SerializableDictionary<string, AttackSkillStat> AttackSkillStats { get { return attackSkillStats; } }
        public SerializableDictionary<string, BuffSkillStat> BuffSkillStats { get { return buffSkillStats; } }
        public SerializableDictionary<string, SkillCombinationData> SkillCombinations { get { return skillCombinations; } }
        public SerializableDictionary<string, LocalizationData> LocalizationData { get { return localizationData; } }


        private void OnValidate()
        {
            int count = excelTableCollections.Count;
            for (int i = 0; i < count; i++)
            {
                var collection = excelTableCollections[i];
                collection.Storage = this;
            }
        }

#if UNITY_EDITOR
        [DebugButton]
        public void Pull()
        {
            actorStats.Clear();
            attackSkillStats.Clear();
            buffSkillStats.Clear();
            skillCombinations.Clear();
            localizationData.Clear();

            int count = excelTableCollections.Count;
            for (int i = 0; i < count; i++)
            {
                var collection = excelTableCollections[i];
                collection.Pull();
            }

            EditorUtility.SetDirty(this);
        }
#endif
    }
}
