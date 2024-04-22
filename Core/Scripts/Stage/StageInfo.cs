using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Roguelike.Core
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "StageInfo", menuName = "TheSalt/Data/StageInfo")]
    public class StageInfo : ScriptableObject
    {
        [Header("Informations")]
        [SerializeField] private string _codeName;
        [SerializeField] private string _name;
        [SerializeField] private string _subName;
        [SerializeField] private string _description;
        [SerializeField] private Sprite _icon;
        [SerializeField] private StageType _type;
        [SerializeField] private GameObject _tilePrefab;
        [Header("Stage Datas")]
        [SerializeField] private Variable<float> _spawnDelay;
        [SerializeField] private Variable<int> _spawnPerUnit;
        [SerializeField] private int _maxSpawnCount;
        [Header("Stage Logic")]
        [SerializeField] private List<StageLogicExecutor> stageLogics = new List<StageLogicExecutor>();

        public string CodeName { get { return _codeName; } }
        public string Name { get { return _name; } }
        public string SubName { get { return _subName; } }
        public string Description { get { return _description; } }
        public Sprite Icon { get { return _icon; } }
        public StageType Type { get { return _type; } }
        public float SpawnDelay { get { return _spawnDelay.Value; } set { _spawnDelay.Value = value; } }
        public int SpawnPerUnit { get { return _spawnPerUnit.Value; } set { _spawnPerUnit.Value = value; } }
        public int MaxSpawnCount { get { return _maxSpawnCount; } set { _maxSpawnCount = value; } }
        public IReadOnlyList<StageLogicExecutor> StageLogics { get { return stageLogics.AsReadOnly(); } }
        public GameObject TilePrefab { get { return _tilePrefab;} }
    }


    [System.Serializable]
    public class StageLogicExecutor
    {
        [SerializeField] private StageCondition condition;
        [SerializeField] private GameActionBase action;
        [SerializeField] private bool invokeOnce;

        [NonSerialized]
        private bool invokeFlag;

        public StageCondition Condition { get { return condition; } }
        public GameActionBase Action { get { return action; } }
        public bool InvokeOnce { get { return invokeOnce;} }
        public bool InvokeFlag { get { return invokeFlag; } }

        public void Initialize()
        {
            invokeFlag = false;
        }

        public void Execute()
        {
            if (InvokeOnce && InvokeFlag) return;

            if(Condition.Result)
            {
                action?.Invoke();
                //Debug.Log($"{Condition.name} 조건 / {Action.name} 발동");
                invokeFlag = true;
            }
        }

    }

}
