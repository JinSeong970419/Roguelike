using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Roguelike.Core
{
    public abstract partial class Item : ScriptableObject
    {
        [Header("Informations")]
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [SerializeField] private Sprite _icon;
        //[SerializeField] private GameObject _prefab;
        [SerializeField] private AssetReference _assetReference;
        [SerializeField] private AssetReference _assetReference1;
        [SerializeField] private AssetReference _assetReference2;
        [SerializeField] private AssetReference _assetReference3;

        public string Name { get { return _name; } }
        public string Description { get { return _description; } }
        public Sprite Icon { get { return _icon; } }
        //public GameObject Prefab { get { return _prefab; } }
        public AssetReference AssetReference { get { return _assetReference;} }
        public AssetReference AssetReference1 { get { return _assetReference1; } }
        public AssetReference AssetReference2 { get { return _assetReference2; } }
        public AssetReference AssetReference3 { get { return _assetReference3; } }

    }
}
