using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Roguelike.Core
{
    [CreateAssetMenu(fileName = "DamageArea Info", menuName = "TheSalt/Data/DamageArea Info")]
    public class DamageAreaInfo : ScriptableObject
    {
        [Header("Informations")]
        [SerializeField] private Sprite _icon;
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [SerializeField] private AssetReference _assetReference;

        [SerializeField] private float _duration;

        public Sprite Icon => _icon;
        public string Name => _name;
        public string Description => _description;
        public AssetReference AssetReference => _assetReference;

        public float Duration => _duration;
    }
}
