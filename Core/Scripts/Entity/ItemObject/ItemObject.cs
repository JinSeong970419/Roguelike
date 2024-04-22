using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Roguelike.Core
{
    public class ItemObject : Entity
    {
        private ItemKind kind;
        public ItemKind Kind { get { return kind; } }
        public float Exp { get; set; }
        public int Gold { get; set; }
        public AssetReference AssetReference { get; set; }
        public GameObject Model { get; set; }

        private bool magnetFlag = false;


        protected override void OnEnable()
        {
            base.OnEnable();
            MovementSpeed = 5f;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            if (AssetReference != null && AssetReference.RuntimeKeyIsValid())
            {
                AssetReference.ReleaseInstance(Model);
                Model = null;
            }

            magnetFlag = false;
            Stop();
        }

        protected override void FixedUpdate()
        {
            var player = GameManager.Instance.Player;
            if (player != null)
            {
                Vector3 to = player.transform.position - transform.position;
                Vector3 direction = to.normalized;
                float distance = to.magnitude;
                if (distance <= player.Stat.Magnet)
                {
                    Move(direction);
                }

            }

            base.FixedUpdate();
        }

        public void SetItemKind(ItemKind kind, int quantity)
        {
            this.kind = kind;

            switch (kind)
            {
                case ItemKind.Exp:
                    Exp = quantity;
                    ProcessExp();
                    return;
                case ItemKind.Gold:
                    Gold = quantity;
                    ProcessGold();
                    return;
                default:
                    break;
            }

            var itemAssetRef = DataManager.Instance.ItemSettings.Items[(int)kind].AssetReference;
            if (itemAssetRef.RuntimeKeyIsValid())
            {
                itemAssetRef.InstantiateAsync(transform).Completed += (handle) =>
                {
                    if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
                    {
                        handle.Result.transform.SetParent(transform);
                        AssetReference = itemAssetRef;
                        Model = handle.Result;
                    }
                };
            }
        }

        private void ProcessExp()
        {
            var itemAssetRef = DataManager.Instance.ItemSettings.Items[(int)kind].AssetReference;

            if (Exp < 6)
            {
                itemAssetRef = DataManager.Instance.ItemSettings.Items[(int)kind].AssetReference;
            }
            else if(Exp < 11)
            {
                itemAssetRef = DataManager.Instance.ItemSettings.Items[(int)kind].AssetReference1;
            }
            else if(Exp < 21)
            {
                itemAssetRef = DataManager.Instance.ItemSettings.Items[(int)kind].AssetReference2;
            }
            else
            {
                itemAssetRef = DataManager.Instance.ItemSettings.Items[(int)kind].AssetReference3;
            }
            
            if (itemAssetRef.RuntimeKeyIsValid())
            {
                itemAssetRef.InstantiateAsync(transform).Completed += (handle) =>
                {
                    if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
                    {
                        handle.Result.transform.SetParent(transform);
                        AssetReference = itemAssetRef;
                        Model = handle.Result;
                    }
                };
            }
        }

        private void ProcessGold()
        {
            var itemAssetRef = DataManager.Instance.ItemSettings.Items[(int)kind].AssetReference;

            if (Gold < 500)
            {
                itemAssetRef = DataManager.Instance.ItemSettings.Items[(int)kind].AssetReference;
            }
            else if (Gold < 1000)
            {
                itemAssetRef = DataManager.Instance.ItemSettings.Items[(int)kind].AssetReference1;
            }
            else if (Gold < 2000)
            {
                itemAssetRef = DataManager.Instance.ItemSettings.Items[(int)kind].AssetReference2;
            }
            else
            {
                itemAssetRef = DataManager.Instance.ItemSettings.Items[(int)kind].AssetReference3;
            }

            if (itemAssetRef.RuntimeKeyIsValid())
            {
                itemAssetRef.InstantiateAsync(transform).Completed += (handle) =>
                {
                    if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
                    {
                        handle.Result.transform.SetParent(transform);
                        AssetReference = itemAssetRef;
                        Model = handle.Result;
                    }
                };
            }
        }

        private void Use(Actor actor)
        {
            switch (kind)
            {
                case ItemKind.Exp:
                    actor.GainExp(Exp);
                    break;
                case ItemKind.Gold:
                    actor.GetGold(Gold);
                    break;
                default:
                    break;
            }
        }

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            base.OnTriggerEnter2D(other);

            var target = other.GetComponent<Actor>();
            // HACK: 유미봇은 플레이어만 아이템을 먹을 수 있다.
            if (target == GameManager.Instance.Player)
            {
                Use(target);
                Release();
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
