using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Roguelike.Core
{
    [CreateAssetMenu(fileName = "Actor Info", menuName = "TheSalt/Data/Actor Info")]
    public class ActorInfo : ScriptableObject
    {
        [Header("Informations")]
        [SerializeField] private Sprite _icon;
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [SerializeField] private AssetReference _assetReference;
        [Header("Collider")]
        [SerializeField] private Vector2 _colliderOffset;
        [SerializeField] private float _colliderRadius;
        [SerializeField] private float _colliderMass;
        [Header("Rendering")]
        [SerializeField] private float _outlineTickness;
        [ColorUsage(true,true)]
        [SerializeField] private Color _outlineColor;
        [SerializeField] private float _blinkDuration;
        [ColorUsage(true,true)]
        [SerializeField] private Color _blinkColor;
        [Header("Projectile")]
        [SerializeField] private Vector2 _projectileSpawnOffset;
        [Header("Hit Effect")]
        [SerializeField] private EffectKind _hitEffect;
        [SerializeField] private Vector2 _hitEffectOffset;
        [Header("Dust Effect")]
        [SerializeField] private List<EffectKind> _dustEffects;
        [Header("AI")]
        [SerializeField] private List<AiLogicExcutor> _ai;
        [Header("Stats")]
        [SerializeField] private string _stats;

        public Sprite Icon => _icon;
        public string Name => _name;
        public string Description => _description;
        public AssetReference AssetReference => _assetReference;
        public Vector2 ColliderOffset => _colliderOffset;
        public float ColliderRadius => _colliderRadius;
        public float ColliderMass => _colliderMass;
        public float OutlineTickness => _outlineTickness;
        public Color OutlineColor => _outlineColor;
        public float BlinkDuration => _blinkDuration;
        public Color BlinkColor => _blinkColor;
        public Vector2 ProjectileSpawnOffset => _projectileSpawnOffset;
        public EffectKind HitEffect => _hitEffect;
        public Vector2 HitEffectOffset => _hitEffectOffset;
        public List<EffectKind> DustEffects => _dustEffects;
        public List<AiLogicExcutor> AI => _ai;
        public ActorStats Stats
        {
            get
            {
                if(DataManager.Instance.Storage.ActorStats.TryGetValue(_stats, out var value))
                {
                    return value;
                }
                return null;
            }
        }
    }

    [System.Serializable]
    public class AiLogicExcutor
    {
        [SerializeField] private List<AiCondition> conditions;
        [SerializeField] private List<AIActionElement> actions;

        public List<AiCondition> Conditions => conditions;
        public List<AIActionElement> Actions => actions;

        public void Initialize()
        {
            int actionCount = actions.Count;
            for(int i=0; i<actionCount; i++)
            {
                var action = actions[i];
                action.InvokeFlag = false;
            }
        }

        public void Execute(AI ai)
        {

            int conditionCount = conditions.Count;
            for (int i = 0; i < conditionCount; i++)
            {
                AiCondition condition = conditions[i];
                if (condition.GetResult(ai) == false) return;
            }

            int actionsCount = actions.Count;
            for(int i = 0; i < actionsCount;i++)
            {
                AIActionElement action = actions[i];
                if (action.InvokeOnce && action.InvokeFlag) continue;
                action.Action.Invoke(ai);
                action.InvokeFlag = true;
            }
        }
    }

    [System.Serializable]
    public class AIActionElement
    {
        [SerializeField] public AiActionBase Action;
        [SerializeField] public bool InvokeOnce;
        [NonSerialized] public bool InvokeFlag;
    }

}
