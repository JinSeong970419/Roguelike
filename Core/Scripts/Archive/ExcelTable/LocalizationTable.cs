using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Roguelike.Core
{
    [CreateAssetMenu(fileName = "Localization Table", menuName = "TheSalt/Excel/Localization Table")]
    [System.Serializable]
    public class LocalizationTable : ExcelTable
    {
        [ArrayElementTitle("tid")]
        [SerializeField] private LocalizationData[] rows;


        public override ExcelRowData[] Rows { get => rows; set => rows = value.Select(o => o as LocalizationData).ToArray(); }
        public override ExcelRowData InstanceForTypeInference => new LocalizationData();

    }
}
