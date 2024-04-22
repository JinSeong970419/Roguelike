using System.Collections.Generic;
using UnityEngine;

namespace Roguelike.Core
{
    [CreateAssetMenu(fileName = "StageCondition", menuName = "TheSalt/StageLogic/StageCondition")]
    public class StageCondition : ScriptableObject
    {
        [SerializeField] List<Automata.Condition> conditions;

        public bool Result
        {
            get
            {
                for (int i = 0; i < conditions.Count; i++)
                {
                    if (conditions[i].Result == false) return false;
                }
                return true;
            }
        }
    }
}
