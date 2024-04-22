using UnityEngine;

namespace Roguelike.Core
{
    public class AnimationController2D : AnimationContorller
    {
        [SerializeField] private DefaultDirection2D _defaultDirection;
        private bool _filpX;
        private Entity owner;

        public bool FilpX
        {
            get { return _filpX; }
            set
            {
                if (Model == null) return;

                if (_filpX != value)
                {
                    float absoluteX = Mathf.Abs(Model.localScale.x);
                    float scaleX = value ? -absoluteX : absoluteX;
                    Model.localScale = new Vector3(scaleX, Model.localScale.y, Model.localScale.z);
                }
                _filpX = value;
            }
        }

        public bool IsFlipX { get { return Model.localScale.x < 0f; } }

        private void Awake()
        {
            owner = GetComponent<Entity>();
        }

        public override void MoveStart(Vector3 direction)
        {
            base.MoveStart(direction);

            if (direction.x != 0f)
            {
                bool isLeft = direction.x < 0f;
                if (_defaultDirection == DefaultDirection2D.Left)
                {
                    isLeft = !isLeft;
                }
                
                FilpX = isLeft ? true : false;
            }
        }

        public override void SetAnimator()
        {
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

        protected override void OnDeadAnimationEnd()
        {
            owner.Release();
        }
    }
}
