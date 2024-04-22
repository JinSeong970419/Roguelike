using ExcelDataReader;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Roguelike.Core
{
    [CreateAssetMenu(fileName = "Excel Table Collection", menuName = "TheSalt/Excel/Excel Table Collection")]
    public class ExcelTableCollection : ScriptableObject
    {
        [SerializeField] private DataStorage storage;
        [SerializeField] private string excelFilePath;
        [SerializeField] private string key;
        [SerializeField] private List<ExcelTable> tables = new List<ExcelTable>();

        public DataStorage Storage { get { return storage; } set { storage = value; } }
        public string ExcelFilePath { get { return excelFilePath; } }
        public List<ExcelTable> Tables { get { return tables; } }
        public string Key { get { return key; } }

        private void OnValidate()
        {
            int count = tables.Count;
            for (int i = 0; i < count; i++)
            {
                var table = tables[i];
                table.Collection = this;
            }
        }

#if UNITY_EDITOR
        [DebugButton]
        public void Pull()
        {
            string path = Application.dataPath + "/.." + excelFilePath;
            using (var stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                DataSet dataSet;
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    dataSet = reader.AsDataSet();
                }

                int tableCount = tables.Count;
                for (int i = 0; i < tableCount; i++)
                {
                    ExcelTable table = tables[i];
                    table.Pull(dataSet);
                }
            }

            AssetDatabase.SaveAssets();
        }

        [DebugButton]
        public void Open()
        {
            string path = Application.dataPath + "/.." + excelFilePath;
            System.Diagnostics.Process.Start(path);
        }
#endif
    }
}
