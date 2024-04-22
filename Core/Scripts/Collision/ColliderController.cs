using System;
using System.Collections.Generic;
using UnityEngine;

namespace Roguelike.Core
{
    [Obsolete("Deprecated")]
    public class ColliderController : MonoBehaviour
    {
        [Header("Game Event")]
        [SerializeField] private GameEvent<TriggerEnterEventArgs> _entityTriggerEnterEvent;
        [SerializeField] private GameEvent<TriggerStayEventArgs> _entityTriggerStayEvent;
        [SerializeField] private GameEvent<TriggerLeaveEventArgs> _entityTriggerLeaveEvent;

        [Header("Collider")]
        //TODO : 나중에 모든 형태의 충돌체와 연산이 가능하도록 변경해야 함.
        [SerializeField] private CircleCollider2D _collider;
        [SerializeField] private CircleColliderEx _colliderEx;
        
        private Entity _owner;

        private static Dictionary<int, Entity> entityTable = new Dictionary<int, Entity>();

        private void Start()
        {
            _owner = GetComponent<Entity>();
            if (_owner == null) Debug.LogError("ColliderController must have Entity.");
            _collider = GetComponent<CircleCollider2D>();
            //if(_collider == null) Debug.LogError("ColliderController must have Collider.");
            if(_collider != null)
            {
                _owner.CollisionRadius = _collider.radius;
            }
            _colliderEx = GetComponent<CircleColliderEx>();
            if(_colliderEx != null)
            {
                _owner.CollisionRadius = _colliderEx.Radius;
            }
        }

        private void OnEnable()
        {
            if (_owner == null)
            {
                _owner = GetComponent<Entity>();
                if (_owner == null)
                {
                    Debug.LogError("ColliderController must have Entity.");
                }
            }
            entityTable.Add(gameObject.GetInstanceID(), _owner);
            if(_colliderEx == null)
            {
                _colliderEx = GetComponent<CircleColliderEx>();
            }
            _colliderEx?.OnCollisionEnter.AddListener(OnColliisionEnterEx);
            _colliderEx?.OnCollisionStay.AddListener(OnColliisionStayEx);
            _colliderEx?.OnCollisionExit.AddListener(OnColliisionExitEx);
        }

        private void OnDisable()
        {
            entityTable.Remove(gameObject.GetInstanceID());
            _colliderEx?.OnCollisionEnter.RemoveListener(OnColliisionEnterEx);
            _colliderEx?.OnCollisionStay.RemoveListener(OnColliisionStayEx);
            _colliderEx?.OnCollisionExit.RemoveListener(OnColliisionExitEx);
        }

        private void OnColliisionEnterEx(ColliderEx other)
        {
            if (_owner == null) return;
            int hash = other.gameObject.GetInstanceID();
            if (entityTable.TryGetValue(hash, out Entity target) == false) return;
            if (target == null) return;
            _entityTriggerEnterEvent.Invoke(new TriggerEnterEventArgs() { Self = _owner, Target = target });
        }

        private void OnColliisionStayEx(ColliderEx other)
        {
            if (_owner == null) return;
            int hash = other.gameObject.GetInstanceID();
            if (entityTable.TryGetValue(hash, out Entity target) == false) return;
            if (target == null) return;
            _entityTriggerStayEvent.Invoke(new TriggerStayEventArgs() { Self = _owner, Target = target });
        }

        private void OnColliisionExitEx(ColliderEx other)
        {
            if (_owner == null) return;
            int hash = other.gameObject.GetInstanceID();
            if (entityTable.TryGetValue(hash, out Entity target) == false) return;
            if (target == null) return;
            _entityTriggerLeaveEvent.Invoke(new TriggerLeaveEventArgs() { Self = _owner, Target = target });
        }

        private void OnCollisionEnter(Collision collision)
        {
            //Debug.Log("OnCollisionEnter");
            if (_owner == null) return;
            int hash = collision.gameObject.GetInstanceID();
            if (entityTable.TryGetValue(hash, out Entity target) == false) return;
            if (target == null) return;
            _entityTriggerEnterEvent.Invoke(new TriggerEnterEventArgs() { Self = _owner, Target = target });
        }
        private void OnCollisionStay(Collision collision)
        {
            //Debug.Log("OnCollisionStay");
            if (_owner == null) return;
            int hash = collision.gameObject.GetInstanceID();
            if (entityTable.TryGetValue(hash, out Entity target) == false) return;
            if (target == null) return;
            _entityTriggerStayEvent.Invoke(new TriggerStayEventArgs() { Self = _owner, Target = target});
        }
        private void OnCollisionExit(Collision collision)
        {
            //Debug.Log("OnCollisionExit");
            if (_owner == null) return;
              int hash = collision.gameObject.GetInstanceID();
            if (entityTable.TryGetValue(hash, out Entity target) == false) return;
            if (target == null) return;
            _entityTriggerLeaveEvent.Invoke(new TriggerLeaveEventArgs() { Self = _owner, Target = target});
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            //Debug.Log("OnCollisionEnter2D");
            if (_owner == null) return;
              int hash = collision.gameObject.GetInstanceID();
            if (entityTable.TryGetValue(hash, out Entity target) == false) return;
            if (target == null) return;
            _entityTriggerEnterEvent.Invoke(new TriggerEnterEventArgs() { Self = _owner, Target = target });
        }
        private void OnCollisionStay2D(Collision2D collision)
        {
            //Debug.Log("OnCollisionStay2D");
            if (_owner == null) return;
              int hash = collision.gameObject.GetInstanceID();
            if (entityTable.TryGetValue(hash, out Entity target) == false) return;
            if (target == null) return;
            _entityTriggerStayEvent.Invoke(new TriggerStayEventArgs() { Self = _owner, Target = target });
        }
        private void OnCollisionExit2D(Collision2D collision)
        {
            //Debug.Log("OnCollisionExit2D");
            if (_owner == null) return;
              int hash = collision.gameObject.GetInstanceID();
            if (entityTable.TryGetValue(hash, out Entity target) == false) return;
            if (target == null) return;
            _entityTriggerLeaveEvent.Invoke(new TriggerLeaveEventArgs() { Self = _owner, Target = target });
        }
        private void OnTriggerEnter(Collider collision)
        {
            //Debug.Log("OnTriggerEnter");
            if (_owner == null) return;
              int hash = collision.gameObject.GetInstanceID();
            if (entityTable.TryGetValue(hash, out Entity target) == false) return;
            if (target == null) return;
            _entityTriggerEnterEvent.Invoke(new TriggerEnterEventArgs() { Self = _owner, Target = target });
        }
        private void OnTriggerStay(Collider collision)
        {
            //Debug.Log("OnTriggerStay");
            if (_owner == null) return;
              int hash = collision.gameObject.GetInstanceID();
            if (entityTable.TryGetValue(hash, out Entity target) == false) return;
            if (target == null) return;
            _entityTriggerStayEvent.Invoke(new TriggerStayEventArgs() { Self = _owner, Target = target });
        }
        private void OnTriggerExit(Collider collision)
        {
            //Debug.Log("OnTriggerExit");
            if (_owner == null) return;
              int hash = collision.gameObject.GetInstanceID();
            if (entityTable.TryGetValue(hash, out Entity target) == false) return;
            if (target == null) return;
            _entityTriggerLeaveEvent.Invoke(new TriggerLeaveEventArgs() { Self = _owner, Target = target });
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            //Debug.Log("OnTriggerEnter2D");
            if (_owner == null) return;
              int hash = collision.gameObject.GetInstanceID();
            if (entityTable.TryGetValue(hash, out Entity target) == false) return;
            if (target == null) return;
            _entityTriggerEnterEvent.Invoke(new TriggerEnterEventArgs() { Self = _owner, Target = target });
        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            //Debug.Log("OnTriggerStay2D");
            if (_owner == null) return;
              int hash = collision.gameObject.GetInstanceID();
            if (entityTable.TryGetValue(hash, out Entity target) == false) return;
            if (target == null) return;
            _entityTriggerStayEvent.Invoke(new TriggerStayEventArgs() { Self = _owner, Target = target });
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            //Debug.Log("OnTriggerExit2D");
            if (_owner == null) return;
              int hash = collision.gameObject.GetInstanceID();
            if (entityTable.TryGetValue(hash, out Entity target) == false) return;
            if (target == null) return;
            _entityTriggerLeaveEvent.Invoke(new TriggerLeaveEventArgs() { Self = _owner, Target = target });
        }
    }
}
