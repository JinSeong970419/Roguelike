using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using ExcelDataReader;
using System.Data;

namespace Roguelike.Core
{
    public class ExcelData
    {
        private DataSet dataSet;
    
        public static ExcelData Read(FileStream stream)
        {
            ExcelData excelData = new ExcelData();
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                var result = reader.AsDataSet();
                excelData.dataSet = result;
            }

            return excelData;
        }

        public string GetData(int sheet, int row, int column)
        {
            return dataSet.Tables[sheet].Rows[row][column].ToString();
        }
    
    }
}
