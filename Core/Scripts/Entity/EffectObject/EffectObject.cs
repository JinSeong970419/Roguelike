using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Roguelike.Core
{
    public class EffectObject : Entity
    {
        private float tick;
        public EffectKind Kind { get; private set; }
        public AssetReference AssetReference { get; set; }
        public GameObject Model { get; set; }
        public float Duration { get; private set; }
        public bool Passive { get; private set; }

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
                }
            }
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            if (Passive) return;
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

        public void SetEffectKind(EffectKind kind)
        {
            Kind = kind;
            var effectInfo = DataManager.Instance.EffectSettings.EffectInfos[(int)kind];
            Passive = effectInfo.Passive;
            Duration = effectInfo.Duration;
            var itemAssetRef = effectInfo.AssetReference;
            if (itemAssetRef.RuntimeKeyIsValid())
            {
                itemAssetRef.InstantiateAsync(transform).Completed += (handle) =>
                {
                    if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
                    {
                        //_animationContorller.SetAnimator();
                        //_animationContorller.SetModel(handle.Result.transform);
                        handle.Result.transform.SetParent(transform);
                        AssetReference = itemAssetRef;
                        Model = handle.Result;
                    }
                };
            }
            else
            {
                Debug.LogError("AssetReference Runtime Key is Invalid.");
            }
        }

        public override void OnMoveEnd(MoveEndEventArgs args)
        {
        }

        public override void OnMoveStart(MoveStartEventArgs args)
        {
        }



    }
}
