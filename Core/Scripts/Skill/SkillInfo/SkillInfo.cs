using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Roguelike.Core
{
   
    public abstract class SkillInfo : ScriptableObject
    {
        [Header("Informations")]
        [SerializeField] private SkillKind _kind;
        [SerializeField] private SkillType _type;
        [SerializeField] private SkillBase _base;
        [SerializeField] private Sprite _icon;
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [SerializeField] private Level _maxLevel;
        [SerializeField] private AssetReference _assetReference;
        [SerializeField] private bool _isMonsterSkill;
        [SerializeField] private bool _isEquipmentSkill;
        [SerializeField] private Grade _grade;

        public SkillKind Kind { get { return _kind; } set { _kind = value; } }
        public SkillType Type => _type;
        public SkillBase Base => _base;
        public Sprite Icon => _icon;
        public string Name => _name;
        public string Description => _description;
        public abstract List<SkillStat> Stats { get; }
        public Level MaxLevel => _maxLevel;
        public AssetReference AssetReference => _assetReference;
        public bool IsMonsterSkill => _isMonsterSkill;
        public bool IsAttackSkill => Base != SkillBase.Buff;
        public bool IsBuffSkill => Base == SkillBase.Buff;
        public bool IsEquipmentSkill => _isEquipmentSkill;
        public Grade Grade => _grade;

    }


}
