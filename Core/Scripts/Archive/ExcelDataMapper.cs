using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Reflection;
using UnityEngine;
using Roguelike.Core;
using ExcelDataReader;

namespace Roguelike.InGame
{
    public class ExcelDataMapper
    {
        public static TableType Deserialize<TableType>(byte[] data) where TableType : new()
        {
            return new TableType();
        }

        public static TableType Deserialize<TableType>(FileStream stream) where TableType : class, new()
        {
            DataSet dataSet;
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                dataSet = reader.AsDataSet();
            }

            TableType instnace = new TableType();
            Type tableType = typeof(TableType);
            var properties = tableType.GetProperties();

            foreach (var property in properties)
            {
                // �ش� ���̺��� ������Ƽ�� �ƴ� ���
                if (property.ReflectedType != tableType) continue;

                var attribute = property.GetCustomAttribute(typeof(Tab), false) as Tab;
                // �� �Ӽ��� �ƴ� ���
                if (attribute == null) continue;

                string tableName = attribute.Name;
                int tableIndex;

                // ���̺� �̸��� ���� �� �Ǿ� �ִ� ���
                if (string.IsNullOrEmpty(tableName))
                {
                    tableName = property.Name;
                }

                tableIndex = dataSet.Tables.IndexOf(tableName);
                if (tableIndex == -1)
                {
                    // TODO : xlsx ���� �̸��� �˾ƾ� ��...
                    Debug.LogError($"Table [{tableName}] doesn't exist.");
                    continue;
                }

                // Data Parse
                Type propertyType = property.PropertyType;
                Type dataType = propertyType.GenericTypeArguments[0];
                var tableData = dataSet.Tables[tableIndex];
                int rowCount = tableData.Rows.Count;
                IList dataList = (IList)property.GetValue(instnace, null);

                // �÷�
                var columnList = (IList)tableData.Rows[0].ItemArray;
                int colCount = columnList.Count;

                // ������ ������Ƽ
                PropertyInfo[] columnProperties = new PropertyInfo[colCount];
                var dataProperties = dataType.GetProperties();
                int dataPropertyCount = dataProperties.Length;

                for (int dataPropertyIndex = 0; dataPropertyIndex < dataPropertyCount; dataPropertyIndex++)
                {
                    PropertyInfo dataProperty = dataProperties[dataPropertyIndex];
                    Type dataPropertyType = dataProperty.PropertyType;
                    // �⺻ �ڷ����� �ƴϸ� 
                    if (dataPropertyType.IsBuiltInType() == false)
                    {
                        continue;
                    }
                    var keyColumnAttribute = dataProperty.GetCustomAttribute(typeof(KeyColumn), false) as KeyColumn;
                    var columnAttribute = dataProperty.GetCustomAttribute(typeof(Column), false) as Column;
                    if (keyColumnAttribute != null)
                    {
                        string keyColumnName = keyColumnAttribute.Name;
                        int keyColumnIndex;
                        if (string.IsNullOrEmpty(keyColumnName))
                        {
                            keyColumnName = dataProperty.Name;
                        }
                        keyColumnIndex = columnList.IndexOf(keyColumnName);
                        if (keyColumnIndex == -1)
                        {
                            // TODO : xlsx ���� �̸��� �˾ƾ� ��...
                            Debug.LogError($"Key Column [{keyColumnName}] doesn't exist in Table [{tableName}]. ");
                            continue;
                        }

                        columnProperties[keyColumnIndex] = dataProperty;
                    }

                    if (columnAttribute != null)
                    {
                        string columnName = columnAttribute.Name;
                        int columnIndex;
                        if (string.IsNullOrEmpty(columnName))
                        {
                            columnName = dataProperty.Name;
                        }
                        columnIndex = columnList.IndexOf(columnName);
                        if (columnIndex == -1)
                        {
                            // TODO : xlsx ���� �̸��� �˾ƾ� ��...
                            Debug.LogError($"Key Column [{columnName}] doesn't exist in Table [{tableName}]. ");
                            continue;
                        }

                        columnProperties[columnIndex] = dataProperty;
                    }
                }

                for (int row = 1; row < rowCount; row++)
                {
                    var dataInstance = Activator.CreateInstance(dataType);
                    for (int col = 0; col < colCount; col++)
                    {
                        PropertyInfo info = columnProperties[col];
                        if (info == null) continue;

                        Type dataPropertyType = info.PropertyType;
                        string dataText = tableData.Rows[row][col].ToString();
                        var data = dataText.Parse(dataPropertyType);
                        if (data == null)
                        {
                            Debug.LogError($"Cell data  [{dataText}] can't be converted to [{dataPropertyType.Name}]. ");
                        }
                        info.SetValue(dataInstance, data);
                    }
                    dataList.Add(dataInstance);
                }
                property.SetValue(instnace, dataList);
            }

            return instnace;
        }


        public static TableType Backup<TableType>(FileStream stream) where TableType : class, new()
        {
            DataSet dataSet;
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                dataSet = reader.AsDataSet();
            }

            TableType instnace = new TableType();
            Type tableType = typeof(TableType);
            var properties = tableType.GetProperties();

            foreach (var property in properties)
            {
                // �ش� ���̺��� ������Ƽ�� �ƴ� ���
                if (property.ReflectedType != tableType) continue;

                var attribute = property.GetCustomAttribute(typeof(Tab), false) as Tab;
                // �� �Ӽ��� �ƴ� ���
                if (attribute == null) continue;

                string tableName = attribute.Name;
                int tableIndex;

                // ���̺� �̸��� ���� �� �Ǿ� �ִ� ���
                if (string.IsNullOrEmpty(tableName))
                {
                    tableName = property.Name;
                }

                tableIndex = dataSet.Tables.IndexOf(tableName);
                if (tableIndex == -1)
                {
                    // TODO : xlsx ���� �̸��� �˾ƾ� ��...
                    Debug.LogError($"Table [{tableName}] doesn't exist.");
                    continue;
                }

                // Data Parse
                Type propertyType = property.PropertyType;
                Type dataType = propertyType.GenericTypeArguments[0];
                var tableData = dataSet.Tables[tableIndex];
                int rowCount = tableData.Rows.Count;
                IList dataList = (IList)property.GetValue(instnace, null);

                // �÷�
                var columnList = (IList)tableData.Rows[0].ItemArray;

                for (int row = 1; row < rowCount; row++)
                {
                    var dataInstance = Activator.CreateInstance(dataType);
                    var dataProperties = dataType.GetProperties();

                    foreach (var dataProperty in dataProperties)
                    {
                        Type dataPropertyType = dataProperty.PropertyType;
                        // �⺻ �ڷ����� �ƴϸ� 
                        if (dataPropertyType.IsBuiltInType() == false)
                        {
                            continue;
                        }
                        var keyColumnAttribute = dataProperty.GetCustomAttribute(typeof(KeyColumn), false) as KeyColumn;
                        var columnAttribute = dataProperty.GetCustomAttribute(typeof(Column), false) as Column;
                        if (keyColumnAttribute != null)
                        {
                            string keyColumnName = keyColumnAttribute.Name;
                            int keyColumnIndex;
                            if (string.IsNullOrEmpty(keyColumnName))
                            {
                                keyColumnName = dataProperty.Name;
                            }
                            keyColumnIndex = columnList.IndexOf(keyColumnName);
                            if (keyColumnIndex == -1)
                            {
                                // TODO : xlsx ���� �̸��� �˾ƾ� ��...
                                Debug.LogError($"Key Column [{keyColumnName}] doesn't exist in Table [{tableName}]. ");
                                continue;
                            }

                            // TODO : �ϴ��� string ���� �ִµ� int �� float ���� �͵�
                            string dataText = tableData.Rows[row][keyColumnIndex].ToString();
                            var data = dataText.Parse(dataPropertyType);
                            if (data == null)
                            {
                                Debug.LogError($"Cell data  [{dataText}] can't be converted to [{dataPropertyType.Name}]. ");
                            }
                            dataProperty.SetValue(dataInstance, data);

                        }
                        else if (columnAttribute != null)
                        {
                            string columnName = columnAttribute.Name;
                            int columnIndex;
                            if (string.IsNullOrEmpty(columnName))
                            {
                                columnName = dataProperty.Name;
                            }
                            columnIndex = columnList.IndexOf(columnName);
                            if (columnIndex == -1)
                            {
                                // TODO : xlsx ���� �̸��� �˾ƾ� ��...
                                Debug.LogError($"Column [{columnName}] doesn't exist in Table [{tableName}]. ");
                                continue;
                            }

                            // TODO : �ϴ��� string ���� �ִµ� int �� float ���� �͵�
                            string dataText = tableData.Rows[row][columnIndex].ToString();
                            var data = dataText.Parse(dataPropertyType);
                            if (data == null)
                            {
                                Debug.LogError($"Cell data  [{dataText}] can't be converted to [{dataPropertyType.Name}]. ");
                            }
                            dataProperty.SetValue(dataInstance, data);
                        }
                    }

                    dataList.Add(dataInstance);
                }
                property.SetValue(instnace, dataList);
            }

            return instnace;
        }
    }
}
