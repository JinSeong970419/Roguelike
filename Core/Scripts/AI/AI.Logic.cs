using UnityEngine;

namespace Roguelike.Core
{
    public partial class AI : MonoBehaviour
    {
        public bool alwaysTrue = true;
        public int phase = 0;
        public void FollowPlayer()
        {
            var player = GameManager.Instance.Player;
            if (player == null) return;
            Vector3 direction = player.transform.position - transform.position;
            direction.Normalize();
            owner.Move(direction);
        }

        public void AddSkill(SkillKind kind)
        {
            Skill skill = owner.FindSkill(kind);
            if (skill == null)
            {
                owner.AddSkill(kind);
            }
        }

        public void ActivateSkill(SkillKind kind)
        {
            Skill skill = Skill.Create(kind, owner);
            skill.Activate();
        }

        public void SetPhase(int phase)
        {
            this.phase = phase;
        }

        public void LazyFollowPlayer()
        {
            var player = GameManager.Instance.Player;
            if (player == null) return;
            Vector3 direction = player.DronePosition - transform.position;
            float distance = direction.magnitude;
            if (distance > 1f)
            {
                direction.Normalize();
                owner.Move(direction);
            }
            if (distance < 0.1f)
            {
                owner.Stop();
            }

        }

    }
}
