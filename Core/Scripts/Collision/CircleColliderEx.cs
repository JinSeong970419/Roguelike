using UnityEngine;
using UnityEngine.Events;

namespace Roguelike.Core
{
    public class CircleColliderEx : ColliderEx
    {
        [SerializeField] private float radius = 0;   // tileSize 보다 큰 r 은 수용 불가능함!!!!

        public float Radius { get { return radius; } set { radius = value; } }

        public override float MaxRadius { get => radius; }

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            UnityEditor.Handles.color = Color.green;
            UnityEditor.Handles.DrawWireDisc(transform.position + new Vector3(Offset.x, Offset.y), Vector3.forward, Radius);
#endif
        }

        public override bool CheckCollision(ColliderEx target)
        {
            var circle = target as CircleColliderEx;
            if(circle != null)
            {
                return (PosCache.x - circle.PosCache.x) * (PosCache.x - circle.PosCache.x) + (PosCache.y - circle.PosCache.y) * (PosCache.y - circle.PosCache.y) < (Radius + circle.Radius) * (Radius + circle.Radius);
            }

            return false;
        }
    }
}
