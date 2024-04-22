using UnityEngine;
using UnityEngine.Events;

namespace Roguelike.Core
{
    public abstract class AnimationContorller : MonoBehaviour
    {
        [SerializeField] protected Animator _animator;
        protected static int _runHash = Animator.StringToHash("IsRun");
        protected static int _deadHash = Animator.StringToHash("IsDead");

        public Animator Animator { get { return _animator; } }

        public Transform Model { get; set; }

        protected DeadStateDetecter deadStateDetecter;

        private UnityEvent onDeadAnimationExit = new UnityEvent();
        public UnityEvent OnDeadAnimationExit { get { return onDeadAnimationExit; } }

        protected virtual void Start()
        {
            if (_animator == null)
                _animator = GetComponentInChildren<Animator>();
            _runHash = Animator.StringToHash("IsRun");
            _deadHash = Animator.StringToHash("IsDead");
        }

        private void OnEnable()
        {
            if (_animator == null)
                _animator = GetComponentInChildren<Animator>();

            if (_animator != null)
            {
                deadStateDetecter = _animator.GetBehaviour<DeadStateDetecter>();
                if (deadStateDetecter != null)
                {
                    deadStateDetecter.OnDeadAnimationExit.AddListener(OnDeadAnimationExitCallback);
                }
            }
        }

        private void OnDisable()
        {
            _animator = null;
            if (deadStateDetecter != null)
            {
                deadStateDetecter.OnDeadAnimationExit.RemoveListener(OnDeadAnimationExitCallback);
                deadStateDetecter = null;
            }
        }

        protected virtual void OnValidate()
        {
            if (_animator == null) _animator = GetComponentInChildren<Animator>();
            _runHash = Animator.StringToHash("IsRun");
            _deadHash = Animator.StringToHash("IsDead");
        }

        protected virtual void Update()
        {
            //if (Animator != null && deadCall == false)
            //{
            //    if (Animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Dead")
            //    && Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f)
            //    {
            //        Debug.Log(Animator.GetCurrentAnimatorStateInfo(0).);
            //        OnDeadAnimationEnd();
            //        deadCall = true;
            //    }
            //}

        }

        public abstract void SetAnimator();

        public void SetModel(Transform modelTransform)
        {
            Model = modelTransform;
        }

        public virtual void MoveStart(Vector3 direction)
        {
            if (_animator == null) return;
            _animator?.SetBool(_runHash, true);
        }

        public virtual void MoveEnd()
        {
            if (_animator == null) return;
            _animator?.SetBool(_runHash, false);
        }

        public virtual void Die()
        {
            if (_animator == null) return;
            _animator?.SetBool(_deadHash, true);
        }

        public virtual void Revival()
        {
            if (_animator == null) return;
            _animator?.SetBool(_deadHash, false);
        }

        protected abstract void OnDeadAnimationEnd();

        protected void OnDeadAnimationExitCallback()
        {
            OnDeadAnimationExit.Invoke();
        }
    }
}
