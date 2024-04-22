using System.Linq;
using UnityEngine;

namespace Roguelike.Core
{
    [CreateAssetMenu(fileName = "Attack Skill Stat Table", menuName = "TheSalt/Excel/Attack Skill Stat Table")]
    public class AttackSkillStatTable : ExcelTable
    {
        [ArrayElementTitle("tid")]
        [SerializeField] private AttackSkillStat[] rows;


        public override ExcelRowData[] Rows { get => rows; set => rows = value.Select(o => o as AttackSkillStat).ToArray(); }
        public override ExcelRowData InstanceForTypeInference => new AttackSkillStat();
    }
}
