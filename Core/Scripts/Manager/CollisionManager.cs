using System.Collections.Generic;
using UnityEngine;

namespace Roguelike.Core
{
    public class CollisionManager : MonoSingleton<CollisionManager>
    {
        public static readonly int COLLIDERS_IN_TILE_MAX = 256;
        public static readonly int TILE_SIZE_BIT = 0; //1;
        public static readonly int TILE_NUM_X = 32;
        public static readonly int TILE_NUM_Y = 32;


        public static readonly int TILE_SIZE = 1 << TILE_SIZE_BIT;


        [SerializeField] private GameObject _target;
        [SerializeField] private int compareCount;
        

        private CollisionTile[,] tiles = null;
        private float bottom;
        private float left;
        private int tileSize; // 나누기 대신 bit shift 쓸수 있게 2제곱수로

        LinkedList<ColliderEx> allColliders = new LinkedList<ColliderEx>();

        public GameObject Target { get { return _target; } set { _target = value; } }

        protected override void Awake()
        {
            base.Awake();

            tiles = new CollisionTile[TILE_NUM_X, TILE_NUM_Y];
            for (int x = 0; x < TILE_NUM_X; x++)
            {
                for (int y = 0; y < TILE_NUM_Y; y++)
                {
                    tiles[x, y] = new CollisionTile(x, y);
                }
            }
        }

        private void FixedUpdate()
        {
            if (_target != null)
            {
                CheckCollision(_target.transform.position);
            }
            else
            {
                CheckCollision(Vector3.zero);
            }

        }

        public void Register(ColliderEx collider)
        {
            if (collider == null)
            {
                //ERROR!!!
                Debug.LogError("No collider");
                return;
            }
            if (collider.MaxRadius * 2 >= TILE_SIZE)
            {
                //ERROR!!!
                Debug.LogError("ColliderEx's max radius must be smaller than tile's size");
                return;
            }

            //return allCollider index;
            allColliders.AddLast(collider);
        }

        public void Unregister(ColliderEx collider)
        {
            collider.Remove();
        }


        public void CheckCollision(Vector3 center)
        {
            this.bottom = center.y - ((TILE_SIZE * TILE_NUM_Y) >> 1);
            this.left = center.x - ((TILE_SIZE * TILE_NUM_X) >> 1);

            int tileX;
            int tileY;
            ColliderEx collider;
            LinkedListNode<ColliderEx> node = allColliders.First;

            while (node != null)
            {
                collider = node.Value;
                if (collider == null)
                {
                    node = node.Next;
                    continue;
                }
                
                if (collider.IsActive == false)
                {
                    LinkedListNode<ColliderEx> toRemove = node;
                    node = node.Next;
                    allColliders.Remove(toRemove);
                    continue;
                }

                collider.PosCache = collider.Pos;
                tileX = ((int)(collider.PosCache.x - left)) >> TILE_SIZE_BIT;     // bit shift
                tileY = ((int)(collider.PosCache.y - bottom)) >> TILE_SIZE_BIT;
                if (tileX < 0 || tileX >= TILE_NUM_X || tileY < 0 || tileY >= TILE_NUM_Y)
                {
                    // outside of field
                    node = node.Next;
                    continue;
                }
                tiles[tileX, tileY].AddCollider(collider, collider.PosCache.x - (left + tileX * TILE_SIZE), collider.PosCache.y - (bottom + tileY * TILE_SIZE));

                node = node.Next;
            }


            compareCount = 0;

            // check collision
            CollisionTile tile;
            ColliderEx other;
            for (int x = 0; x < TILE_NUM_X; x++)
            {
                for (int y = 0; y < TILE_NUM_Y; y++)
                {
                    tile = tiles[x, y];

                    for (int i = 0; i < tile.ColliderCount - 1; i++)
                    {
                        collider = tile.GetColliderAt(i);

                        for (int j = i + 1; j < tile.ColliderCount; j++)
                        {
                            other = tile.GetColliderAt(j);
                            compareCount++;
                            if (collider.CheckCollision(other))
                            {
                                collider.OnCollision(other);
                                other.OnCollision(collider);
                            }
                            else
                            {
                                collider.OnExit(other);
                                other.OnExit(collider);
                            }
                        }

                        int edgeCheck = 0;
                        // 여기 할차례!!!!!
                        if (collider.OnEdgeX != 0 && x + collider.OnEdgeX >= 0 && x + collider.OnEdgeX < TILE_NUM_X)
                        {
                            edgeCheck++;
                            for (int j = 0; j < tiles[x + collider.OnEdgeX, y].ColliderCount; j++)
                            {
                                other = tiles[x + collider.OnEdgeX, y].GetColliderAt(j);
                                if (collider.OnEdgeX < 0 && collider.MaxRadius <= other.MaxRadius)
                                {
                                    // 이미 체크했으므로 패스
                                    continue;
                                }

                                compareCount++;
                                if (collider.CheckCollision(other))
                                {
                                    //TODO : 두번 호출 안하도록 체크!!
                                    collider.OnCollision(other);
                                    other.OnCollision(collider);
                                }
                                else
                                {
                                    collider.OnExit(other);
                                    other.OnExit(collider);
                                }
                            }
                        }


                        if (collider.OnEdgeY != 0 && y + collider.OnEdgeY >= 0 && y + collider.OnEdgeY < TILE_NUM_Y)
                        {
                            edgeCheck++;
                            for (int j = 0; j < tiles[x, y + collider.OnEdgeY].ColliderCount; j++)
                            {
                                other = tiles[x, y + collider.OnEdgeY].GetColliderAt(j);
                                if (collider.OnEdgeY < 0 && collider.MaxRadius <= other.MaxRadius)
                                {
                                    // 이미 체크했으므로 패스
                                    continue;
                                }

                                compareCount++;
                                if (collider.CheckCollision(other))
                                {
                                    collider.OnCollision(other);
                                    other.OnCollision(collider);
                                }
                                else
                                {
                                    collider.OnExit(other);
                                    other.OnExit(collider);
                                }
                            }
                        }

                        if (edgeCheck >= 2)
                        {
                            for (int j = 0; j < tiles[x + collider.OnEdgeX, y + collider.OnEdgeY].ColliderCount; j++)
                            {
                                other = tiles[x + collider.OnEdgeX, y + collider.OnEdgeY].GetColliderAt(j);
                                if (collider.OnEdgeY < 0 && collider.MaxRadius <= other.MaxRadius)
                                {
                                    // 이미 체크했으므로 패스
                                    continue;
                                }

                                compareCount++;
                                if (collider.CheckCollision(other))
                                {
                                    collider.OnCollision(other);
                                    other.OnCollision(collider);
                                }
                                else
                                {
                                    collider.OnExit(other);
                                    other.OnExit(collider);
                                }
                            }
                        }
                    }
                }
            }

            // reset
            for (int x = 0; x < TILE_NUM_X; x++)
            {
                for (int y = 0; y < TILE_NUM_Y; y++)
                {
                    tiles[x, y].RemoveAll();
                }
            }
        }
    }

    public class CollisionTile
    {
        ColliderEx[] colliders = new ColliderEx[CollisionManager.COLLIDERS_IN_TILE_MAX];

        int colliderCount = 0;
        public int ColliderCount { get { return colliderCount; } }
        int tileIndexX = 0;
        int tileIndexY = 0;

        public CollisionTile(int x, int y)
        {
            tileIndexX = x;
            tileIndexY = y;
            colliderCount = 0;
        }

        public void AddCollider(ColliderEx collider, float colliderXInTile, float colliderYInTile)
        {
            if (colliderCount >= CollisionManager.COLLIDERS_IN_TILE_MAX)
            {
                // ERROR
                Debug.LogError("Too many colliders on 1 tile");
                return;
            }

            if (colliderYInTile - collider.MaxRadius < 0) collider.OnEdgeY = -1;
            else if (colliderYInTile + collider.MaxRadius > CollisionManager.TILE_SIZE) collider.OnEdgeY = 1;
            else collider.OnEdgeY = 0;

            if (colliderXInTile - collider.MaxRadius < 0) collider.OnEdgeX = -1;
            else if (colliderXInTile + collider.MaxRadius > CollisionManager.TILE_SIZE) collider.OnEdgeX = 1;
            else collider.OnEdgeX = 0;

            colliders[colliderCount] = collider;
            colliderCount++;

        }
        public ColliderEx GetColliderAt(int i)
        {
            if (colliderCount <= i)
            {
                //ERROR
                Debug.LogError("No such collider collider index");
                return null;
            }

            return colliders[i];
        }

        public void RemoveAll()
        {
            colliderCount = 0;
        }
    }
}
