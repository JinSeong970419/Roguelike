using UnityEngine;
using UnityEngine.Events;

namespace Roguelike.Core
{
    public abstract class Entity : MonoBehaviour
    {
        private static int uniqueNum = 0;
        [SerializeField] protected AnimationContorller _animationContorller;

        // TODO : Actor Stat 정보 만들 때 변경해야 함.
        #region State
        [SerializeField] private float _movementSpeed;
        private Vector3 _direction = Vector3.left;
        private bool _isMoving = false;
        private bool _isDead = false;
        public Vector3 Direction { get { return _direction; } set { _direction = value; } }
        public bool IsMoving { get { return _isMoving; } set { _isMoving = value; } }
        public float MovementSpeed { get { return _movementSpeed; } set { _movementSpeed = value; } }

        private bool _destroyFlag = false;
        public bool DestroyFlag { get => _destroyFlag; set { _destroyFlag = value; } }
        public bool IsDead { get { return _isDead; } set { _isDead = value; } }
        #endregion

        private int _id;
        public int Id 
        {
            get { return _id; }
            set { _id = value; }
        }

        #region Physics
        private Vector3 _velocity = Vector3.zero;
        public Vector3 Velocity { get { return _velocity; } set { _velocity = value; } }
        #endregion

        protected AnimationContorller AnimationContorller => _animationContorller;

        #region Collider
        public float CollisionRadius { get; set; }
        private Collider2D _collider;
        public Collider2D Collider { get { return _collider; } }
        #endregion

        #region Event
        private UnityEvent<Entity> onDestroy = new UnityEvent<Entity>();
        public UnityEvent<Entity> OnDestroy { get { return onDestroy; } }

        private UnityEvent<Entity> onDeath = new UnityEvent<Entity>();
        public UnityEvent<Entity> OnDeath { get { return onDeath; } }
        #endregion

        protected virtual void Awake()
        {
            var collider = GetComponent<CircleCollider2D>();
            if (collider != null)
            {
                _collider = collider;
                CollisionRadius = collider.radius;
            }
        }

        protected virtual void OnEnable()
        {
            DestroyFlag = false;
            Direction = Vector3.zero;
            IsMoving = false;
        }

        protected virtual void OnDisable()
        {
            onDestroy.RemoveAllListeners();
            onDeath.RemoveAllListeners();
            IsMoving = false;
            Velocity = Vector3.zero;
        }

        protected virtual void FixedUpdate()
        {
            ProcessMove();
            transform.position += _velocity;
            _velocity = Vector3.zero;
        }

        protected virtual void LateUpdate()
        {
            if (DestroyFlag)
            {
                DestroyFlag = false;
                OnRelease();
            }
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {

        }

        protected virtual void OnTriggerStay2D(Collider2D other)
        {

        }

        protected virtual void OnTriggerExit2D(Collider2D other)
        {

        }
        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            
        }
        protected virtual void OnCollisionStay2D(Collision2D collision)
        {
            
        }

        protected virtual void OnCollisionExit2D(Collision2D collision)
        {
            
        }

        public virtual void Initialize()
        {
            _id = GetUniqueId();
        }
        protected void ProcessMove()
        {
            if (_isMoving)
            {
                Velocity += _direction * _movementSpeed * Time.fixedDeltaTime;
            }
        }

        public void Move(Vector3 direction)
        {
            if (IsDead) return;
            Direction = direction;
            IsMoving = true;
            if (AnimationContorller != null)
            {
                AnimationContorller.MoveStart(direction);
            }
        }

        public void Stop()
        {
            IsMoving = false;
            if (AnimationContorller != null)
            {
                AnimationContorller.MoveEnd();
            }
        }

        public void Release()
        {
            if (DestroyFlag) return;
            DestroyFlag = true;
        }

        protected virtual void OnRelease()
        {
            OnDestroy.Invoke(this);
            ObjectPool.Instance.Free(gameObject);
        }

        public abstract void OnMoveStart(MoveStartEventArgs args);
        public abstract void OnMoveEnd(MoveEndEventArgs args);

        private static int GetUniqueId()
        {
            return uniqueNum++;
        }
    }
}
