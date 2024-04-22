using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Roguelike.Core
{
    public class Skill_Summon : AttackSkill
    {
        private class SummonTimer
        {
            public float tick;
            public float duration;
            public Actor summon;
        }

        private List<SummonTimer> timerList = new List<SummonTimer>();

        public Skill_Summon(Actor owner) : base(owner)
        {
        }

        public override void OnActivate()
        {
            GameObject summonObj = ObjectPool.Instance.Allocate(EntityType.Actor);
            var collider = summonObj.GetComponent<CircleCollider2D>();
            collider.enabled = false;
            Actor actor = summonObj.GetComponent<Actor>();
            actor.Stat = new ActorStats(DataManager.Instance.ActorSettings.ActorInfos[(int)Stat.SummonActor].Stats);
            actor.Team = Team.Alliance;
            actor.SetActorKind(Stat.SummonActor);
            actor.AddSkill(Stat.SummonSkill);

            summonObj.AddComponent<AI>();

            timerList.Add(new SummonTimer() { tick = 0f, duration = Stat.Duration, summon = actor });
        }

        public override void OnAdd()
        {
            
        }

        public override void OnLevelUp()
        {
            
        }

        public override void OnRemove()
        {
            
        }

        public override void OnUpdate()
        {
            ProcessTimer();
        }

        private void ProcessTimer()
        {
            List<int> removes = new List<int>();

            int count = timerList.Count;
            for(int i= count-1; i >= 0; i--)
            {
                var timer = timerList[i];
                timer.tick += Time.fixedDeltaTime;
                if(timer.tick > timer.duration)
                {
                    removes.Add(i);
                }
            }

            int removeCount = removes.Count;
            for (int i = 0; i < removeCount; i++)
            {
                var index = removes[i];
                timerList[index].summon.Release();
                timerList.RemoveAt(index);
            }
        }
    }
}
