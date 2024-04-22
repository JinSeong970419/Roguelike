using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Roguelike.Core
{
    [CreateAssetMenu(fileName = "Actor Stat Table", menuName = "TheSalt/Excel/Actor Stat Table")]
    public class ActorStatTable : ExcelTable
    {
        [ArrayElementTitle("tid")]
        [SerializeField] private ActorStats[] rows;


        public override ExcelRowData[] Rows { get => rows; set => rows = value.Select(o => o as ActorStats).ToArray(); }
        public override ExcelRowData InstanceForTypeInference => new ActorStats();
    }
}
