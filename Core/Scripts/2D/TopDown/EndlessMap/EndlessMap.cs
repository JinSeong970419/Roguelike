using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Roguelike.Core
{
    public enum EndlessMapType
    {
        All,
        Horizontal,
        Vertical,
    }
    public class EndlessMap : MonoBehaviour
    {
        [SerializeField] private GameEvent<string> sceneChangedEvent;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private List<EntityType> _propTypeList = new List<EntityType>();

        private static IObjectPool<EndlessMap> _managedPool = new ObjectPool<EndlessMap>(CreateInfiniteTile, OnGetInfiniteTile, OnReleaseInfiniteTile, OnDestroyInfiniteTile, maxSize: 16);
        private static List<EndlessMap> _endlessMaps = new List<EndlessMap>();
        private static Vector2 TileSize;
        private static GameObject _tilePrefab;
        private static EndlessMapType _type;

        private int _row;
        private int _column;
        private List<Entity> _propList = new List<Entity>();

        public Vector2 Size { get { return new Vector2(_spriteRenderer.size.x * transform.localScale.x, _spriteRenderer.size.y * transform.localScale.y); } }
        public int Row
        {
            get { return _row; }
            set
            {
                _row = value;
                var originPosition = transform.position;
                transform.position = new Vector3(originPosition.x, _row * Size.y, originPosition.z);
            }
        }
        public int Column
        {
            get { return _column; }
            set
            {
                _column = value;
                var originPosition = transform.position;
                transform.position = new Vector3(_column * Size.x, originPosition.y, originPosition.z);
            }
        }

        private void OnValidate()
        {
            if (_spriteRenderer == null)
            {
                _spriteRenderer = GetComponent<SpriteRenderer>();
            }
        }

        private void Start()
        {
            TileSize = Size;
            int propCount = _propTypeList.Count;
            for (int i = 0; i < propCount; i++)
            {
                var propType = _propTypeList[i];
                var prop = ObjectPool.Instance.Allocate(propType);
                var propEntity = prop.GetComponent<Entity>();
                prop.transform.SetParent(transform, false);
                _propList.Add(propEntity);
            }
        }

        private void OnEnable()
        {
            sceneChangedEvent.AddListener(OnSceneChanged);
        }

        private void OnDisable()
        {
            sceneChangedEvent.RemoveListener(OnSceneChanged);
        }

        private void OnDestroy()
        {
            int propCount = _propList.Count;
            for (int i = 0; i < propCount; i++)
            {
                var prop = _propList[i];
                ObjectPool.Instance.Free(prop.gameObject);
            }
        }

        public void Destroy()
        {
            _managedPool.Release(this);
        }

        public static void Initialize(GameObject tilePrefab, EndlessMapType type)
        {
            _managedPool.Clear();
            _endlessMaps.Clear();
            _tilePrefab = tilePrefab;
            _type = type;

            switch (_type)
            {
                case EndlessMapType.All:
                    {
                        for (int i = -1; i < 2; i++)
                        {
                            for (int j = -1; j < 2; j++)
                            {
                                Allocate(i, j);
                            }
                        }
                    }
                    break;
                case EndlessMapType.Horizontal:
                    {
                        for (int j = -1; j < 2; j++)
                        {
                            Allocate(0, j);
                        }
                    }
                    break;
                case EndlessMapType.Vertical:
                    {
                        for (int i = -1; i < 2; i++)
                        {
                            Allocate(i, 0);
                        }
                    }
                    break;
                default:
                    Debug.LogError("[Error] Invalid EndlessMapType!!");
                    break;
            }

            
        }

        public static void Release()
        {
            _managedPool.Clear();
        }

        public static void Create(GameObject target)
        {
            if (target == null) return;

            var targetPosition = target.transform.position;
            Vector2 tileSize = EndlessMap.TileSize;
            int tileSizeX = (int)tileSize.x;
            int tileSizeY = (int)tileSize.y;
            float halfX = tileSize.x * 0.5f;
            float halfY = tileSize.y * 0.5f;
            int targetRow = 0;
            int targetColumn = 0;

            if (tileSizeX != 0)
            {
                targetColumn = (targetPosition.x < 0f) ? -1 : 1;
                targetColumn = (int)(targetPosition.x + (targetColumn * halfX)) / tileSizeX;
            }
            if (tileSizeY != 0)
            {
                targetRow = (targetPosition.y < 0f) ? -1 : 1;
                targetRow = (int)(targetPosition.y + (targetRow * halfY)) / tileSizeY;
            }

            switch (_type)
            {
                case EndlessMapType.All:
                    {
                        for (int i = -1; i < 2; i++)
                        {
                            for (int j = -1; j < 2; j++)
                            {
                                Allocate(targetRow + i, targetColumn + j);
                            }
                        }
                    }
                    break;
                case EndlessMapType.Horizontal:
                    {
                        for (int j = -1; j < 2; j++)
                        {
                            Allocate(targetRow, targetColumn + j);
                        }
                    }
                    break;
                case EndlessMapType.Vertical:
                    {
                        for (int i = -1; i < 2; i++)
                        {
                            Allocate(targetRow + i, targetColumn);
                        }
                    }
                    break;
                default:
                    Debug.LogError("[Error] Invalid EndlessMapType!!");
                    break;
            }
            
        }

        public static void Repeat(GameObject target)
        {
            if (target == null) return;

            Create(target);
            Release(target);
        }

        private static void Release(GameObject target)
        {
            if (target == null) return;

            var targetPosition = target.transform.position;
            List<EndlessMap> removeList = new List<EndlessMap>();
            int count = _endlessMaps.Count;
            for (int i = 0; i < count; i++)
            {
                var tile = _endlessMaps[i];
                float dist = Vector3.Distance(targetPosition, tile.transform.position);
                if (dist > tile.Size.x * 1.5f)
                {
                    removeList.Add(tile);
                }
                else if (dist > tile.Size.y * 1.5f)
                {
                    removeList.Add(tile);
                }
            }

            foreach (var item in removeList)
            {
                Release(item);
            }
        }

        public static EndlessMap Allocate(int row, int col)
        {
            if (FindTile(row, col, out EndlessMap outTile))
            {
                return outTile;
            }
            var tile = _managedPool.Get();
            tile.Row = row;
            tile.Column = col;
            _endlessMaps.Add(tile);
            return tile;
        }

        public static void Release(EndlessMap tile)
        {
            _endlessMaps.Remove(tile);
            _managedPool.Release(tile);
        }

        private static bool FindTile(int row, int col, out EndlessMap outTile)
        {
            int count = _endlessMaps.Count;
            for (int i = 0; i < count; i++)
            {
                var tile = _endlessMaps[i];
                if (tile.Row == row && tile.Column == col)
                {
                    outTile = tile;
                    return true;
                }
            }
            outTile = null;
            return false;
        }

        private static EndlessMap CreateInfiniteTile()
        {
            var instance = Instantiate(_tilePrefab);
            EndlessMap tile = instance.GetComponent<EndlessMap>();
            return tile;
        }

        private static void OnGetInfiniteTile(EndlessMap infiniteTile)
        {
            infiniteTile.gameObject.SetActive(true);
        }

        private static void OnReleaseInfiniteTile(EndlessMap infiniteTile)
        {
            infiniteTile.gameObject.SetActive(false);
        }

        private static void OnDestroyInfiniteTile(EndlessMap infiniteTile)
        {
            
        }

        private void OnSceneChanged(string sceneName)
        {
            _managedPool.Clear();
            _endlessMaps.Clear();
        }

    }
}
