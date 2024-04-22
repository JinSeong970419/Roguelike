using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Roguelike.Core
{
    public class Projectile : Entity
    {
        public Actor Owner { get; set; }
        public Team Team { get; set; }
        private AttackSkillStat _stats;
        public AttackSkillStat Stat
        {
            get { return _stats; }
            set
            {
                _stats = value;
                IsMoving = true;
                MovementSpeed = _stats.Speed;
            }
        }

        public SkillKind Kind { get; set; }
        public AssetReference AssetReference { get; set; }
        public GameObject Model { get; set; }

        #region Satellite
        public bool IsSatellite { get; set; }
        public float SatelliteAngle { get; set; }
        public float SatelliteDistance { get; set; }
        #endregion

        #region Guide
        public bool IsGuided { get; set; }
        private Actor target;

        #endregion

        private float _durationTick = 0f;

        protected override void FixedUpdate()
        {
            ProcessMoveAndRotation();
            ProcessSatellite();
            ProcessGuide();
            ProcessDuration();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            IsSatellite = false;

            //if(Model!= null )
            //{
            //    Model.gameObject.SetActive(false);
            //}
            //if (AssetReference != null)
            //{
            //    if (AssetReference.RuntimeKeyIsValid())
            //    {
            //        AssetReference.ReleaseInstance(Model);
            //        AssetReference = null;
            //        Model = null;
            //    }
            //}
        }

        private void ProcessMoveAndRotation()
        {
            ProcessMove();

            float angle = Vector3.Angle(new Vector3(-1.0f, 0.0f, 0.0f), Direction);
            if (Direction.y > 0.0f)
                angle = -angle;
            var rotation = Quaternion.Euler(0f, 0f, angle);
            var position = transform.position + Velocity;
            transform.SetPositionAndRotation(position, rotation);
            Velocity = Vector3.zero;
        }

        private void ProcessSatellite()
        {
            if (IsSatellite)
            {
                SatelliteAngle += Stat.Speed * Time.fixedDeltaTime;
                Vector3 pos = Owner.transform.position + new Vector3(Mathf.Cos(SatelliteAngle), Mathf.Sin(SatelliteAngle)) * SatelliteDistance;
                transform.position = pos;
            }
        }

        private void ProcessGuide()
        {
            if (IsSatellite) return;
            if (IsGuided)
            {
                if (target == null || target.IsDead || target.gameObject.activeSelf == false)
                {
                    target = null;
                    var monsters = GameManager.Instance.Monsters;
                    int count = monsters.Count;
                    float minDist = float.MaxValue;
                    Actor nearest = null;
                    for (int i = 0; i < count; i++)
                    {
                        var monster = monsters[i];
                        if (monster.Stat.IsInvincibility) continue;
                        if (monster.IsDead) continue;
                        
                        float dist = (monster.transform.position - transform.position).sqrMagnitude;
                        if (dist < minDist)
                        {
                            minDist = dist;
                            nearest = monster;
                        }
                    }

                    if (nearest != null)
                    {
                        target = nearest;
                    }
                }
                else
                {
                    Vector3 originDirection = Direction;
                    Vector3 to = target.transform.position - transform.position;
                    originDirection += to.normalized * 10f * Time.fixedDeltaTime; 
                    Direction = originDirection.normalized;
                    
                }
            }
        }

        private void ProcessDuration()
        {
            _durationTick += Time.fixedDeltaTime;
            if (_durationTick > Stat.Duration)
            {
                _durationTick = 0f;
                Release();
            }
        }

        public override void Initialize()
        {
            _durationTick = 0f;
            IsGuided = false;
            transform.localScale = Vector3.one;
        }

        public void OnHit()
        {
            GameObject effectObj = ObjectPool.Instance.Allocate(EntityType.EffectObject);
            var effect = effectObj.GetComponent<EffectObject>();
            effect.SetEffectKind(EffectKind.Explosion);
            effect.transform.position = transform.position;

            --_stats.PiercingCount;
            if (_stats.PiercingCount < 0)
            {
                _durationTick = 0f;
                Release();
            }
        }

        public void SetSkillKind(SkillKind kind)
        {
            Kind = kind;

            var itemAssetRef = DataManager.Instance.SkillSettings.SkillInfos[(int)kind].AssetReference;
            if (itemAssetRef.RuntimeKeyIsValid())
            {
                itemAssetRef.InstantiateAsync(transform).Completed += (handle) =>
                {
                    if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
                    {
                        if (Model != null)
                        {
                            AssetReference.ReleaseInstance(Model);
                        }
                        handle.Result.transform.SetParent(transform);
                        AssetReference = itemAssetRef;
                        Model = handle.Result;
                    }
                };
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
