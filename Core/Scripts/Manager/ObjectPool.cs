using System.Collections.Generic;
using UnityEngine;

namespace Roguelike.Core
{
    public class ObjectPool : MonoSingleton<ObjectPool>
    {
        [SerializeField] private GameEvent<string> sceneChangedEvent;
        [SerializeField] private EntitySettings _settings;
        [SerializeField] private int _reserveSize = 10;

        private Dictionary<int, List<GameObject>> _pool = new Dictionary<int, List<GameObject>>();
        private Dictionary<int, EntityType> _keyStore = new Dictionary<int, EntityType>();

        public EntitySettings Settings { get { return _settings; } }

        protected override void Awake()
        {
            base.Awake();

        }

        private void OnEnable()
        {
            sceneChangedEvent.AddListener(OnSceneChanged);
        }

        private void OnDisable()
        {
            sceneChangedEvent.RemoveListener(OnSceneChanged);
        }

        public void Initialize()
        {
            DestroyAll();
        }

        public GameObject Allocate(EntityType type)
        {
            int key = (int)type;
            if (_pool.ContainsKey(key) == false)
            {
                _pool.Add(key, new List<GameObject>());
            }

            if (_pool[key].Count == 0)
            {
                Reserve(type, _reserveSize);
            }

            GameObject gameObject = _pool[key].PopBack();
            _keyStore.Add(gameObject.GetInstanceID(), type);
            gameObject.SetActive(true);
            return gameObject;
        }

        public void Free(GameObject target)
        {
            if (_keyStore.TryGetValue(target.GetInstanceID(), out EntityType type) == false) return;
            int key = (int)type;
            if (!_pool.ContainsKey(key))
            {
                _pool.Add(key, new List<GameObject>());
            }
            target.SetActive(false);
            _pool[key].PushBack(target);
            _keyStore.Remove(target.GetInstanceID());
        }

        public void Reserve(EntityType type, int reserveSize)
        {
            int key = (int)type;
            if (_pool.ContainsKey(key) == false)
            {
                _pool.Add(key, new List<GameObject>());
            }

            GameObject prefab = _settings.Prefabs[key];
            for (int i = 0; i < reserveSize; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                _pool[key].Add(obj);
            }
        }

        public void DestroyAll()
        {
            foreach (var item in _pool)
            {
                int count = item.Value.Count;
                for(int i=0;i < count; i++)
                {
                    GameObject obj = item.Value[i];
                    Destroy(obj);
                }
            }
            _pool.Clear();
        }

        private void OnSceneChanged(string sceneName)
        {
            if (sceneName == "InGame")
            {

            }
            else
            {
                DestroyAll();
            }
        }
    }
}
