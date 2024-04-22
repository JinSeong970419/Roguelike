using System;
using System.Data;
using System.Linq;
using System.Reflection;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Roguelike.Core
{
    [System.Serializable]
    public abstract class ExcelTable : ScriptableObject
    {
        [SerializeField] private ExcelTableCollection collection;
        [SerializeField] private string sheetName;

        public string SheetName { get { return sheetName; } }
        public ExcelTableCollection Collection { get { return collection; } set { collection = value; } }
        public abstract ExcelRowData[] Rows { get; set; }
        public abstract ExcelRowData InstanceForTypeInference { get; }



        private void OnValidate()
        {
            int count = Rows.Length;
            for (int i = 0; i < count; i++)
            {
                var row = Rows[i];
                row.Table = this;
            }
        }

#if UNITY_EDITOR
        public void Pull(DataSet dataSet)
        {
            PullInternal(dataSet);
        }

        private void PullInternal(DataSet dataSet)
        {
            if (string.IsNullOrEmpty(sheetName))
            {
                Debug.LogError("Sheet Name is null or empty.");
                return;
            }

            int sheetIndex = dataSet.Tables.IndexOf(sheetName);
            if (sheetIndex == -1)
            {
                Debug.LogWarning($"Sheet Name [{sheetName}] does not exist.");
                return;
            }

            var sheet = dataSet.Tables[sheetIndex];
            var columnList = sheet.Rows[0].ItemArray.Select(x => x.ToString().ToLower()).ToList();
            int columnCount = columnList.Count;
            int rowCount = sheet.Rows.Count;

            // Type Inference
            Type type = InstanceForTypeInference.GetType();
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            Rows = new ExcelRowData[rowCount - 1];

            for (int row = 1; row < rowCount; row++)
            {
                string tid = sheet.Rows[row][0].ToString();
                if (string.IsNullOrEmpty(tid)) continue;

                // Parse Row
                var rowData = Activator.CreateInstance(type) as ExcelRowData;

                for (int i = 0; i < fields.Length; i++)
                {
                    var field = fields[i];
                    Type fieldType = field.FieldType;
                    string fieldName = field.Name;
                    if (fieldName[0] == '_')
                    {
                        fieldName = fieldName.Substring(1);
                    }

                    int columnIndex = columnList.IndexOf(fieldName.ToLower());
                    if (columnIndex == -1)
                    {
                        Debug.LogWarning($"Column [{fieldName}] does not exist in [{sheetName}]");
                        continue;
                    }

                    string cell = sheet.Rows[row][columnIndex].ToString();
                    if (string.IsNullOrEmpty(cell)) continue;
                    var data = cell.Parse(fieldType);
                    if (data == null)
                    {
                        if (fieldType.Name == "String")
                        {
                            data = string.Empty;
                        }
                        else if (fieldType == typeof(Level))
                        {
                            int temp = (int)cell.Parse(typeof(int));
                            Level level = (Level)(temp - 1);
                            data = level;
                        }
                        else if (fieldType.IsEnum)
                        {
                            data = Enum.Parse(fieldType, cell);
                        }
                        else
                        {
                            Debug.LogError($"Cell data [{cell}] can't be converted to [{fieldType.Name}]. ");
                            continue;
                        }

                    }

                    field.SetValue(rowData, data);
                }

                //rowData.Tid = tid;
                Rows[row - 1] = rowData;

                if (type == typeof(LocalizationData))
                {
                    Collection.Storage.LocalizationData.Add(rowData.Tid, rowData as LocalizationData);
                }
                else if (type == typeof(ActorStats))
                {
                    Collection.Storage.ActorStats.Add(rowData.Tid, rowData as ActorStats);
                }
                else if (type == typeof(AttackSkillStat))
                {
                    Collection.Storage.AttackSkillStats.Add(rowData.Tid, rowData as AttackSkillStat);
                }
                else if (type == typeof(BuffSkillStat))
                {
                    Collection.Storage.BuffSkillStats.Add(rowData.Tid, rowData as BuffSkillStat);
                }
                else if (type == typeof(SkillCombinationData))
                {
                    Collection.Storage.SkillCombinations.Add(rowData.Tid, rowData as SkillCombinationData);
                }

                Debug.Log($"Pull from {sheetName}/{tid}");
            }

            EditorUtility.SetDirty(this);
        }


#endif
    }
}
