using UnityEngine;

namespace Roguelike.Core
{
    public class Skill_Drone : AttackSkill
    {
        private Actor drone;
        private Actor player;

        public Skill_Drone(Actor owner) : base(owner)
        {
        }

        public override void OnActivate()
        {

        }

        public override void OnAdd()
        {
            player = GameManager.Instance.Player;
            
            GameObject droneObj = ObjectPool.Instance.Allocate(EntityType.Actor);
            var collider = droneObj.GetComponent<CircleCollider2D>();
            collider.enabled = false;
            drone = droneObj.GetComponent<Actor>();
            drone.Stat = new ActorStats(DataManager.Instance.ActorSettings.ActorInfos[(int)Stat.SummonActor].Stats);
            drone.Team = Team.Alliance;
            drone.SetActorKind(Stat.SummonActor);
            drone.AddSkill(Stat.SummonSkill);

            droneObj.AddComponent<AI>();
        }

        public override void OnLevelUp()
        {

        }

        public override void OnRemove()
        {

        }

        public override void OnUpdate()
        {
            if (drone == null) return;
            if (player == null) return;

            float speed = (drone.transform.position - player.DronePosition).magnitude;

            drone.MovementSpeed = speed;
        }
    }
}
