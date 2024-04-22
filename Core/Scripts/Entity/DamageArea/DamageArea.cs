using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Roguelike.Core
{
    public class DamageArea : Entity
    {
        private float tick;
        public Actor Owner { get; set; }
        public Team Team { get; set; }
        private AttackSkillStat _stats;
        public AttackSkillStat Stat
        {
            get { return _stats; }
            set
            {
                _stats = value;
                IsMoving = true;
                MovementSpeed = _stats.Speed;
            }
        }

        public DamageAreaKind Kind { get; private set; }
        public AssetReference AssetReference { get; set; }
        public GameObject Model { get; set; }

        private SpriteRenderer _spriteRenderer;

        public float Duration { get; set; }

        public bool IsFadeout { get; set; }

        public override void OnMoveEnd(MoveEndEventArgs args)
        {

        }

        public override void OnMoveStart(MoveStartEventArgs args)
        {

        }

        protected override void OnDisable()
        {
            base.OnDisable();
            tick = 0f;
            if (AssetReference != null)
            {
                if (AssetReference.RuntimeKeyIsValid())
                {
                    AssetReference.ReleaseInstance(Model);
                    Model = null;
                    _spriteRenderer = null;
                }
            }
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            if(IsFadeout)
            {
                if (_spriteRenderer != null)
                {
                    float ratio = tick / Duration;
                    Color origin = _spriteRenderer.color;
                    origin.a = 1f - ratio;
                    _spriteRenderer.color = origin;

                }
            }

            if (IsDead == false)
            {
                tick += Time.fixedDeltaTime;
                if (tick > Duration)
                {
                    tick = 0;
                    Release();
                }
            }
        }

        public void SetDamageArea(DamageAreaKind kind)
        {
            Kind = kind;
            var effectInfo = DataManager.Instance.DamageAreaSettings.DamageAreaInfos[(int)kind];
            Duration = effectInfo.Duration;
            var itemAssetRef = effectInfo.AssetReference;
            if (itemAssetRef.RuntimeKeyIsValid())
            {
                itemAssetRef.InstantiateAsync(transform).Completed += (handle) =>
                {
                    if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
                    {
                        handle.Result.transform.SetParent(transform);
                        AssetReference = itemAssetRef;
                        Model = handle.Result;
                        _spriteRenderer = Model.GetComponentInChildren<SpriteRenderer>();
                    }
                };
            }
            else
            {
                Debug.LogError("AssetReference Runtime Key is Invalid.");
            }
        }

        public void OnHit()
        {

        }
    }
}
