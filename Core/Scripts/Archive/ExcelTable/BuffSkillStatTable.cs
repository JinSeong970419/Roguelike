using System.Linq;
using UnityEngine;

namespace Roguelike.Core
{
    [CreateAssetMenu(fileName = "Buff Skill Stat Table", menuName = "TheSalt/Excel/Buff Skill Stat Table")]
    [System.Serializable]
    public class BuffSkillStatTable : ExcelTable
    {
        [ArrayElementTitle("tid")]
        [SerializeField] private BuffSkillStat[] rows;


        public override ExcelRowData[] Rows { get => rows; set => rows = value.Select(o => o as BuffSkillStat).ToArray(); }
        public override ExcelRowData InstanceForTypeInference => new BuffSkillStat();
    }
}
