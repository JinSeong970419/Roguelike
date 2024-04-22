using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Roguelike.Core
{
    [CreateAssetMenu(fileName = "Buff Skill Info", menuName = "TheSalt/Data/Skill/Buff Skill Info")]
    public class BuffSkillInfo : SkillInfo
    {
        [Header("Stats")]
        [SerializeField] private List<string> _stats;
        public override List<SkillStat> Stats
        {
            get
            {
                var stat = _stats.Select(o => {
                    if (DataManager.Instance.Storage.BuffSkillStats.TryGetValue(o, out var stats))
                    {
                        return stats as SkillStat;
                    }
                    else
                    {
                        return null;
                    }
                }).ToList();

                return stat;
            }
        }

        private void OnValidate()
        {
            //int count = (int)MaxLevel + 1;
            //int originCount = _stats.Count;

            //List<BuffSkillStat> stats = new List<BuffSkillStat>();
            //for (int i = 0; i < count; i++)
            //{
            //    if (i < originCount)
            //    {
            //        _stats[i].Level = (Level)i;
            //        stats.Add(_stats[i]);
            //    }
            //    else
            //    {
            //        stats.Add(new BuffSkillStat() { Level = (Level)i });
            //    }

            //}
            //_stats = stats;
        }

    }
}
