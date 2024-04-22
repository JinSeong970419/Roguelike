using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace Roguelike.Core
{
    public enum StageType
    {
        None,
        EndlessMap,
        HorizontalMap,
        VerticalMap,
        FiniteMap,
    }

    public abstract class IStage
    {
        public class SpawnInfo
        {
            public ActorKind Kind { get; set; }
            public int Count { get; set; }
        }

        private float spawnTick;
        protected Dictionary<int, Actor> _monsters = new Dictionary<int, Actor>();
        public Actor NearestMonster { get; set; }
        /// <summary>
        /// 플레이어를 기준으로 가까운 순서로 정렬된 몬스터 리스트
        /// </summary>
        public List<Actor> Monsters { get; set; } = new List<Actor>();

        protected List<ItemObject> _items = new List<ItemObject>();

        /// <summary>
        /// 반드시 설정해야 함.
        /// </summary>
        public StageKind Kind { get; set; } = StageKind.End;
        public StageInfo StageInfo
        {
            get
            {
                if (Kind == StageKind.End)
                {
                    Debug.LogError("Stage Kind is not set!!");
                }
                return DataManager.Instance.StageSettings.StageInfos[(int)Kind];
            }
        }
        public StageSettings Settings { get { return DataManager.Instance.StageSettings; } }
        public abstract StageType Type { get; }

        protected SpawnInfo[] _spawnInfos = new SpawnInfo[(int)ActorKind.End];
        public SpawnInfo[] SpawnInfos { get { return _spawnInfos; } }

        #region Camera
        private GameCamera _camera;
        #endregion

        public IStage(StageKind kind)
        {
            Kind = kind;
            int count = (int)ActorKind.End;
            for (int i = 0; i < count; i++)
            {
                _spawnInfos[i] = new SpawnInfo();
                _spawnInfos[i].Kind = (ActorKind)i;
            }
            InitializeStageLogic();
        }

        public void Initialize()
        {
            Settings.StartTime.Value = Time.time; // 플레이 타임을 초기화.
            Settings.SpawnedMonsterCount.Value = 0;
            DataManager.Instance.PlayerInfo.Initialize();
            _camera = Camera.main.GetComponent<GameCamera>();
            GameManager.Instance.Initilaize();

            OnInitialize();
        }

        public void Update()
        {
            Settings.PlayTime.Value = Time.time - Settings.StartTime.Value;
            Settings.SpawnedMonsterCount.Value = _monsters.Count;
            OnUpdate();
            ProcessLimitItem();
        }

        public void Release()
        {
            GameManager.Instance.Release();
            OnRelease();
        }

        protected abstract void OnInitialize();
        protected abstract void OnUpdate();
        protected abstract void OnRelease();

        public static IStage Create(StageType type, StageKind kind)
        {
            IStage stage = null;
            switch (type)
            {
                case StageType.None: return new EmptyStage(kind);
                case StageType.EndlessMap: return new EndlessMapStage(kind);
                case StageType.HorizontalMap: return new HorizontalSrollMapStage(kind);
                case StageType.VerticalMap: return new VerticalScrollMapStage(kind);
                case StageType.FiniteMap: return new FiniteMapStage(kind);
                default:
                    Debug.LogError("[Error] StageType Not Found");
                    break;
            }

            return stage;
        }

        public void SpawnMonster(ActorKind monsterKind, int count)
        {
            _spawnInfos[(int)monsterKind].Kind = monsterKind;
            _spawnInfos[(int)monsterKind].Count = count;
        }

        public void SpawnMonsterOnce(ActorKind monsterKind, int count, float distance)
        {
            for (int j = 0; j < count; j++)
            {
                SpawnMonsterInternal(monsterKind, distance);
            }
        }

        public void SpawnFenceAround(ActorKind actorKind, int count, float distance)
        {
            float angle = 0f;
            float unitAngle = Mathf.PI * 2f / count;
            for (int i = 0; i < count; i++)
            {
                SpawnNPC(actorKind, angle, distance);
                angle += unitAngle;
            }

        }

        public void ChangedSpawnDelay(float delay)
        {
            StageInfo.SpawnDelay = delay;
        }

        public void DestroyAllMonsters()
        {
            int count = (int)ActorKind.End;
            for (int i = 0; i < count; i++)
            {
                _spawnInfos[(int)i].Count = 0;
            }

            foreach (var item in _monsters)
            {
                item.Value.Release();
            }

            _monsters.Clear();
        }

        public void SpawnItemAtRandomPosition(ItemKind kind)
        {
            var player = GameManager.Instance.Player;
            if (player == null)
            {
                return;
            }
            float dist = DataManager.Instance.GameSettings.MonsterSpawnDistance;
            float randomAngle = Random.Range(0f, Mathf.PI * 2f);
            Vector3 offset = new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)) * dist;
            offset += player.transform.position;
            SpawnItem(kind, offset, 1);
        }

        public void SetZoom(float zoom, float time)
        {
            _camera.SetZoom(zoom, time);
        }

        protected void SpawnMonsterInternal(ActorKind monsterKind)
        {
            var player = GameManager.Instance.Player;
            if (player == null)
            {
                return;
            }

            if (Settings.SpawnedMonsterCount.Value >= StageInfo.MaxSpawnCount)
            {
                return;
            }

            ActorInfo actorInfo = DataManager.Instance.ActorSettings.ActorInfos[(int)monsterKind];
            float dist = DataManager.Instance.GameSettings.MonsterSpawnDistance;
            var monsterObj = ObjectPool.Instance.Allocate(EntityType.Actor);
            var monster = monsterObj.GetComponent<Actor>();
            monster.Initialize();
            var newStat = new ActorStats(actorInfo.Stats);
            monster.Stat = newStat;
            monster.SetActorKind(monsterKind);
            monster.OnDeath.AddListener(OnMonsterDeath);
            monster.Team = Team.Enemy;
            float randomAngle = Random.Range(0f, Mathf.PI * 2f);
            Vector3 offset = new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)) * dist;
            offset += player.transform.position;
            monster.transform.position = offset;

            monsterObj.AddComponent<AI>();

            _monsters.Add(monster.Id, monster);
        }

        protected void SpawnMonsterInternal(ActorKind monsterKind, float distance)
        {
            var player = GameManager.Instance.Player;
            if (player == null)
            {
                return;
            }

            if (Settings.SpawnedMonsterCount.Value >= StageInfo.MaxSpawnCount)
            {
                return;
            }

            ActorInfo actorInfo = DataManager.Instance.ActorSettings.ActorInfos[(int)monsterKind];
            float dist = distance;
            var monsterObj = ObjectPool.Instance.Allocate(EntityType.Actor);
            var monster = monsterObj.GetComponent<Actor>();
            monster.Initialize();
            monster.Stat = new ActorStats(actorInfo.Stats);
            monster.SetActorKind(monsterKind);
            monster.OnDeath.AddListener(OnMonsterDeath);
            monster.Team = Team.Enemy;
            float randomAngle = Random.Range(0f, Mathf.PI * 2f);
            Vector3 offset = new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)) * dist;
            offset += player.transform.position;
            monster.transform.position = offset;

            monsterObj.AddComponent<AI>();

            _monsters.Add(monster.Id, monster);
        }

        protected void SpawnNPC(ActorKind npcKind, float radianAngle, float distance)
        {
            var player = GameManager.Instance.Player;
            if (player == null)
            {
                return;
            }

            if (Settings.SpawnedMonsterCount.Value >= StageInfo.MaxSpawnCount)
            {
                return;
            }

            ActorInfo actorInfo = DataManager.Instance.ActorSettings.ActorInfos[(int)npcKind];
            float dist = distance;
            var npcObj = ObjectPool.Instance.Allocate(EntityType.Actor);
            var npc = npcObj.GetComponent<Actor>();
            npc.Stat = new ActorStats(actorInfo.Stats);
            npc.SetActorKind(npcKind);
            npc.Team = Team.Neutral;

            Vector3 offset = new Vector3(Mathf.Cos(radianAngle), Mathf.Sin(radianAngle)) * dist;
            offset += player.transform.position;
            npc.transform.position = offset;

            npcObj.AddComponent<AI>();

        }

        private void SpawnItem(ItemKind itemKind, Vector3 position, int quantity)
        {
            if (itemKind == ItemKind.Exp || itemKind == ItemKind.Gold)
            {
                var itemObj = ObjectPool.Instance.Allocate(EntityType.ItemObject);
                var item = itemObj.GetComponent<ItemObject>();
                item.SetItemKind(itemKind, quantity);

                itemObj.transform.position = position;

                item.OnDestroy.AddListener(OnItemDestroy);
                _items.Add(item);
            }
            else
            {
                for (int i = 0; i < quantity; i++)
                {
                    var itemObj = ObjectPool.Instance.Allocate(EntityType.ItemObject);
                    var item = itemObj.GetComponent<ItemObject>();
                    item.SetItemKind(itemKind, quantity);

                    itemObj.transform.position = position;

                    item.OnDestroy.AddListener(OnItemDestroy);
                    _items.Add(item);
                }
            }

        }


        public void OnMonsterDeath(Entity entity)
        {
            var monster = entity as Actor;

            var kind = monster.Kind;
            var dropItems = DataManager.Instance.DropItemSettings.DropItemInfos[(int)kind].DropItems;
            foreach (var dropItem in dropItems)
            {
                float random = Random.Range(0f, 1f);
                if (random > dropItem.Probability) continue;

                SpawnItem(dropItem.ItemKind, entity.transform.position, dropItem.Quantity);
            }

            var ai = monster.gameObject.GetComponent<AI>();
            if (ai != null)
            {
                Object.Destroy(ai);
            }

            _monsters.Remove(monster.Id);
        }

        private void OnItemDestroy(Entity entity)
        {
            var item = entity as ItemObject;
            _items.Remove(item);

            item.OnDestroy.RemoveAllListeners();
        }

        protected void ProcessSpawnMonster()
        {
            int spawnInfoCount = _spawnInfos.Length;
            spawnTick += Time.deltaTime;
            if (spawnTick > StageInfo.SpawnDelay)
            {
                spawnTick = 0f;
                for (int i = 0; i < spawnInfoCount; i++)
                {
                    var spawnInfo = _spawnInfos[i];
                    for (int j = 0; j < spawnInfo.Count; j++)
                    {
                        SpawnMonsterInternal(spawnInfo.Kind);
                    }
                }
            }
        }

        protected void ProcessSetNearestEnemyFromPlayer()
        {
            var player = GameManager.Instance.Player;
            var monsterList = _monsters.Values.ToList();
            int monsterCount = monsterList.Count;

            NearestMonster = null;
            Monsters = new List<Actor>();

            for (int i = 0; i < monsterCount; i++)
            {
                var monster = monsterList[i];
                if (monster.Stat.IsInvincibility) continue;
                if (monster.IsDead) continue;
                Monsters.Add(monster);
            }

            if(Monsters.Count > 0)
            {
                Monsters = Monsters.OrderBy(o => (o.transform.position - player.transform.position).sqrMagnitude).ToList();
                NearestMonster = Monsters[0];
            }

        }

        protected void ProcessStageLogic()
        {
            int count = StageInfo.StageLogics.Count;
            for (int i = 0; i < count; i++)
            {
                StageLogicExecutor stageLogic = StageInfo.StageLogics[i];
                stageLogic.Execute();
            }
        }

        protected void InitializeStageLogic()
        {
            int count = StageInfo.StageLogics.Count;
            for (int i = 0; i < count; i++)
            {
                StageLogicExecutor stageLogic = StageInfo.StageLogics[i];
                stageLogic.Initialize();
            }
        }

        private void ProcessLimitItem()
        {
            if (_items.Count >= DataManager.Instance.GameSettings.MaxFieldItemCount)
            {
                int removeCount = _items.Count - DataManager.Instance.GameSettings.MaxFieldItemCount;
                for (int i = 0; i < removeCount; i++)
                {
                    var item = _items[i];
                    item.Release();
                }
                _items.RemoveRange(0, removeCount);
            }
        }

    }

}
