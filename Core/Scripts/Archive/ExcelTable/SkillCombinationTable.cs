using System.Linq;
using UnityEngine;

namespace Roguelike.Core
{
    [CreateAssetMenu(fileName = "Skill Combination Data Table", menuName = "TheSalt/Excel/Skill Combination Data Table")]
    public class SkillCombinationTable : ExcelTable
    {
        [ArrayElementTitle("tid")]
        [SerializeField] private SkillCombinationData[] rows;


        public override ExcelRowData[] Rows { get => rows; set => rows = value.Select(o => o as SkillCombinationData).ToArray(); }
        public override ExcelRowData InstanceForTypeInference => new SkillCombinationData();
    }
}
