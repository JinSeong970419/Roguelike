using System;
using System.Data;
using System.Linq;
using System.Reflection;
using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Roguelike.Core
{
    [System.Serializable]
    public abstract class ExcelRowData
    {
        [SerializeField] private ExcelTable table;
        [SerializeField] protected string tid;

        public ExcelTable Table { get { return table; } set { table = value; } }
        public string Tid { get { return tid; } set { tid = value; } }

        public ExcelRowData()
        {
            
        }

        public void Pull(DataSet dataSet, string sheetName)
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

            int rowIndex = -1;
            for (int row = 0; row < rowCount; row++)
            {
                if (sheet.Rows[row][0].ToString() == tid)
                {
                    rowIndex = row;
                    break;
                }
            }

            if (rowIndex == -1)
            {
                Debug.LogWarning($"Tid [{tid}] does not exist in [{sheetName}]");
                return;
            }

            Type type = GetType();
            var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            
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

                string cell = sheet.Rows[rowIndex][columnIndex].ToString();
                if (string.IsNullOrEmpty(cell)) continue;
                var data = cell.Parse(fieldType);
                if (data == null)
                {
                    if(fieldType.Name == "String")
                    {
                        data = string.Empty;
                    }
                    else if(fieldType == typeof(Level))
                    {
                        int temp = (int)cell.Parse(typeof(int));
                        Level level = (Level)(temp - 1);
                        data = level;
                    }
                    else if(fieldType.IsEnum)
                    {
                        data = Enum.Parse(fieldType, cell);
                    }
                    else
                    {
                        Debug.LogError($"Cell data [{cell}] can't be converted to [{fieldType.Name}]. ");
                        continue;
                    }
                    
                }
                field.SetValue(this, data);


            }

            Debug.Log($"Pull from {sheetName}/{tid}");
        }
    }
}
