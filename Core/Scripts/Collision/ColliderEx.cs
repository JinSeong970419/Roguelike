using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Roguelike.Core
{
    public abstract class ColliderEx : MonoBehaviour
    {
        [SerializeField] protected Vector2 offset;
        protected UnityEvent<ColliderEx> onCollisionEnter = new UnityEvent<ColliderEx>();
        protected UnityEvent<ColliderEx> onCollisionStay = new UnityEvent<ColliderEx>();
        protected UnityEvent<ColliderEx> onCollisionExit = new UnityEvent<ColliderEx>();

        protected int onEdgeX = 0;
        protected int onEdgeY = 0;
        protected bool isActive = true;

        private Vector2 posCache = Vector2.zero;
        public Vector2 PosCache { get { return posCache; } set { posCache = value; } }
        public Vector3 Pos { get { return transform.position; } }
        public bool IsActive { get { return isActive; } }
        public int OnEdgeX { get { return onEdgeX; } set { onEdgeX = value; } }
        public int OnEdgeY { get { return onEdgeY; } set { onEdgeY = value; } }
        public Vector2 Offset { get { return offset; } set { offset = value; } }
        public UnityEvent<ColliderEx> OnCollisionEnter => onCollisionEnter;
        public UnityEvent<ColliderEx> OnCollisionStay => onCollisionStay;
        public UnityEvent<ColliderEx> OnCollisionExit => onCollisionExit;

        /// <summary>
        /// 충돌체의 최대 반경입니다. (만약 큐브라면 피벗에서 가장 먼 꼭지점의 거리)
        /// </summary>
        public abstract float MaxRadius { get; }

        /// <summary>
        /// 충돌 중인 충돌체
        /// </summary>
        private Dictionary<int, ColliderEx> collidingColliders= new Dictionary<int, ColliderEx>();

        protected virtual void OnEnable()
        {
            CollisionManager.Instance.Register(this);
            isActive = true;
        }

        protected virtual void OnDisable()
        {
            CollisionManager.Instance.Unregister(this);
        }

        public abstract bool CheckCollision(ColliderEx target);

        public void OnCollision(ColliderEx other)
        {
            //Debug.Log($"OnCollision {gameObject.name} {other.gameObject.name}");
            if(collidingColliders.TryGetValue(other.GetInstanceID(), out ColliderEx value))
            {
                onCollisionStay.Invoke(other);
            }
            else
            {
                collidingColliders[other.GetInstanceID()] = other;
                onCollisionEnter.Invoke(other);
            }
        }

        public void OnExit(ColliderEx other)
        {
            if (collidingColliders.TryGetValue(other.GetInstanceID(), out ColliderEx value))
            {
                collidingColliders.Remove(other.GetInstanceID());
                onCollisionExit.Invoke(other);
            }
        }

        public void Remove()
        {
            isActive = false;
        }
    }
}
