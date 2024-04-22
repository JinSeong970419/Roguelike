using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Roguelike.Core
{
    public class Actor : Entity
    {
        [SerializeField] private HpBar hpBar;
        [SerializeField] private DirectionIndicator directionIndicator;
        public Team Team { get; set; }
        public ActorKind Kind { get; set; }
        public ActorInfo Info { get { return DataManager.Instance.ActorSettings.ActorInfos[(int)Kind]; } }

        private ActorStats stat;
        public ActorStats Stat
        {
            get { return stat; }
            set
            {
                stat = value;
                MovementSpeed = stat.MovementSpeed;
            }
        }

        public AssetReference AssetReference { get; set; }
        public GameObject Model { get; set; }
        public bool IsPlayer
        {
            get
            {
                if (this == null) return false;
                if (GameManager.Instance.Player == null) return false;
                return GameManager.Instance.Player == this;
            }
        }

        #region Knockback
        private bool knockbackFlag = false;
        private float knockbackTick = 0f;
        private float knockbackSpeed = 3f;
        private float knockbackDuration = 0.1f;
        private Vector3 knockbackDirection = Vector3.zero;
        #endregion

        #region Attack
        private bool attackFlag = false;
        private float attackTick = 0f;
        private float attackDelay = 1f;
        #endregion

        #region Skill
        private int skillCount = 0;
        private int attackSkillCount = 0;
        private int buffSkillCount = 0;
        private Skill[] skills = new Skill[(int)SkillKind.End];
        private bool[] disabledSkills = new bool[(int)SkillKind.End];

        public Skill[] Skills { get { return skills; } }
        // UI용
        public List<Skill> AttackSkills = new List<Skill>();
        public List<Skill> BuffSkills = new List<Skill>();
        #endregion

        #region MaterialPropertyController
        [SerializeField] private ActorMaterialPropertyController propertyController;
        #endregion

        #region Blink
        private bool blinkFlag = false;
        private float blinkTick = 0f;
        private float blinkDuration = 1f;
        private float subBlinkTick = 0f;
        private float subBlinkDuration = 0.1f;
        #endregion

        #region Death
        private float deathTick = 0f;
        private float deathDuration = 3f;
        #endregion

        #region Dust
        private float dustTick = 0f;
        private float dustDelay = 0.1f;
        #endregion

        #region Animation
        private Animator animator;

        #endregion

        #region Drone
        public Vector3 DroneOffset
        {
            get
            {
                float x = Direction.x < 0f ? 1f : -1f;

                return new Vector3(x, 0.5f);
            }
        }

        public Vector3 DronePosition
        {
            get { return transform.position + DroneOffset; }
        }
        #endregion

        #region Projectile
        public Vector3 ProjectileSpawnOffset
        {
            get { return Info.ProjectileSpawnOffset; }
        }
        #endregion

        public DirectionIndicator DirectionIndicator
        {
            get { return directionIndicator; }
        }

        protected override void Awake()
        {
            base.Awake();
            hpBar = GetComponentInChildren<HpBar>();
            hpBar.gameObject.SetActive(false);
            directionIndicator = GetComponentInChildren<DirectionIndicator>();
            directionIndicator.gameObject.SetActive(false);


        }

        protected override void FixedUpdate()
        {
            PrecessBlink();
            if (IsDead)
            {
                ProcessDeath();
                return;
            }
            ProcessSkill();
            ProcessAttack();
            ProcessKnockback();
            
            ProcessLevelUp();
            ProcessDust();
            base.FixedUpdate();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            AnimationContorller.OnDeadAnimationExit.AddListener(OnDeadAnimationExit);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            ReleaseSkill();

            if (AssetReference != null)
            {
                if (AssetReference.RuntimeKeyIsValid())
                {
                    AssetReference.ReleaseInstance(Model);
                    Model = null;
                }
            }

            AnimationContorller.OnDeadAnimationExit.RemoveListener(OnDeadAnimationExit);

        }

        private void OnValidate()
        {
            if (propertyController == null)
                propertyController = GetComponent<ActorMaterialPropertyController>();
        }

        private void ReleaseSkill()
        {
            int count = skills.Length;
            for (int i = 0; i < count; i++)
            {
                skills[i] = null;
            }
        }

        private void ProcessSkill()
        {
            int count = skills.Length;
            for (int i = 0; i < count; i++)
            {
                var skill = skills[i];
                if (skill == null) continue;
                if (skill.SkillType == SkillType.Active) continue;
                skill.Update();
            }
        }

        private void ProcessKnockback()
        {
            if (knockbackFlag)
            {
                Velocity += knockbackDirection * knockbackSpeed * Time.fixedDeltaTime;

                knockbackTick += Time.fixedDeltaTime;
                if (knockbackTick > knockbackDuration)
                {
                    knockbackTick = 0f;
                    knockbackFlag = false;
                }
            }
        }

        private void ProcessAttack()
        {
            if (attackFlag == false)
            {
                attackTick += Time.fixedDeltaTime;
                if (attackTick > attackDelay)
                {
                    attackTick = 0f;
                    attackFlag = true;
                }
            }

        }

        private void PrecessBlink()
        {
            if (blinkFlag)
            {
                blinkTick += Time.fixedDeltaTime;
                subBlinkTick += Time.fixedDeltaTime;
                if (blinkTick > blinkDuration)
                {
                    blinkFlag = false;
                    propertyController.BlinkRatio = 0f;
                    propertyController.UpdateProperties();
                }
                else
                {
                    if(subBlinkTick > subBlinkDuration)
                    {
                        subBlinkTick = 0f;
                        propertyController.BlinkRatio = propertyController.BlinkRatio > 0f ? 0f : 1f;
                        propertyController.UpdateProperties();
                    }
                }
            }
        }

        private void ProcessLevelUp()
        {
            var playerInfo = DataManager.Instance.PlayerInfo;
            if (playerInfo.Exp.Value > playerInfo.MaxExp.Value)
            {
                if (playerInfo.Level.Value == playerInfo.MaxLevel) return;
                LevelUp();
            }
        }

        private void ProcessDeath()
        {
            deathTick += Time.fixedDeltaTime;
            if (deathTick > deathDuration)
            {
                deathTick = 0f;

            }
        }

        private void ProcessDust()
        {
            if (IsMoving == false) return;
            dustTick += Time.fixedDeltaTime;
            if (dustTick > dustDelay)
            {
                dustTick = 0f;
                int count = Info.DustEffects.Count;
                for (int i = 0; i < count; i++)
                {
                    GameObject effectObj = ObjectPool.Instance.Allocate(EntityType.EffectObject);
                    var effect = effectObj.GetComponent<EffectObject>();
                    effect.SetEffectKind(Info.DustEffects[i]);
                    effect.transform.position = transform.position;
                }
            }
        }

        public void ShowHpBar()
        {
            hpBar?.gameObject.SetActive(true);
        }

        public void ShowDirectionIndicator()
        {
            directionIndicator?.gameObject.SetActive(true);
        }

        public void SetActorKind(ActorKind kind)
        {
            Kind = kind;

            var info = DataManager.Instance.ActorSettings.ActorInfos[(int)kind];
            var itemAssetRef = info.AssetReference;
            if (itemAssetRef.RuntimeKeyIsValid())
            {
                itemAssetRef.InstantiateAsync(transform).Completed += (handle) =>
                {
                    if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
                    {
                        _animationContorller.SetAnimator();
                        _animationContorller.SetModel(handle.Result.transform);
                        handle.Result.transform.SetParent(transform);
                        AssetReference = itemAssetRef;
                        Model = handle.Result;

                        var colliders = GetComponents<CircleCollider2D>();
                        if (colliders != null)
                        {
                            int count = colliders.Length;
                            for (int i = 0; i < count; i++)
                            {
                                var collider = colliders[i];
                                collider.radius = info.ColliderRadius;
                                collider.offset = info.ColliderOffset;
                            }
                        }

                        var rigid = GetComponent<Rigidbody2D>();
                        if (rigid != null)
                        {
                            rigid.mass = info.ColliderMass;
                        }

                        blinkFlag = false;
                        blinkTick = 0f;
                        subBlinkTick = 0f;

                        propertyController.OutlineColor = info.OutlineColor;
                        propertyController.OutlineTickness = info.OutlineTickness;
                        propertyController.BlinkRatio = 0f;
                        propertyController.BlinkColor = Color.white;
                        blinkDuration = info.BlinkDuration;
                        propertyController.UpdateProperties();
                    }
                };
            }
        }

        public void MoveStart(Vector3 direction)
        {
            direction = direction.normalized;
            Move(direction);
        }

        public void MoveStop()
        {
            Stop();
        }

        public void AddSkill(SkillKind kind)
        {
            if (FindSkill(kind) != null) return;
            Skill skill = Skill.Create(kind, this);
            skills[(int)kind] = skill;
            if (IsPlayer)
            {
                GameManager.Instance.UpdateSkills(kind);
            }

            if (skill.SkillBase == SkillBase.Buff)
            {
                buffSkillCount++;
                BuffSkills.Add(skill);
            }
            else
            {
                attackSkillCount++;
                AttackSkills.Add(skill);
            }

            skillCount++;
            skill.OnAdd();
        }

        public void RemoveSkill(SkillKind kind)
        {
            Skill skill = FindSkill(kind);
            if (skill == null) return;

            skills[(int)kind] = null;
            if (IsPlayer)
            {
                GameManager.Instance.UpdateSkills(kind);
            }

            if (skill.SkillBase == SkillBase.Buff)
            {
                buffSkillCount--;
                BuffSkills.Remove(skill);
            }
            else
            {
                attackSkillCount--;
                AttackSkills.Remove(skill);
            }

            skillCount--;
            skill.OnRemove();
        }

        public Skill FindSkill(SkillKind kind)
        {
            return skills[(int)kind];
        }

        public void TakeDamage(Actor attacker, float damage)
        {
            if (Stat.IsInvincibility) return;

            Stat.Hp -= damage;
            if (Stat.Hp <= 0f)
            {
                Stat.Hp = 0f;
                OnDeath_Internal(attacker);
            }

            if (hpBar != null)
            {
                hpBar.Value = Stat.Hp / Stat.MaxHp;
            }

            var font = DamageFont.Create(DamageFontType.Default, (int)damage);
            font.transform.position = transform.position;

            if (Info.HitEffect != EffectKind.None)
            {
                GameObject effectObj = ObjectPool.Instance.Allocate(EntityType.EffectObject);
                var effect = effectObj.GetComponent<EffectObject>();
                effect.SetEffectKind(Info.HitEffect);
                effect.transform.position = transform.position + new Vector3(Info.HitEffectOffset.x, Info.HitEffectOffset.y);
            }

            Blink(Color.white);
        }

        public void RecoverHp()
        {
            float recovery = Stat.MaxHp * Stat.Recovery;
            Stat.Hp += recovery;
            if (Stat.Hp > Stat.MaxHp)
            {
                Stat.Hp = Stat.MaxHp;
            }
        }

        public void GainExp(float exp)
        {
            var playerInfo = DataManager.Instance.PlayerInfo;
            playerInfo.Exp.Value += exp;
        }

        public void GetGold(int gold)
        {
            var playerInfo = DataManager.Instance.PlayerInfo;
            playerInfo.Coin.Value += gold;
        }

        public void LevelUp()
        {
            if (IsPlayer == false) return;
            

            var playerInfo = DataManager.Instance.PlayerInfo;
            playerInfo.Level.Value += 1;
            playerInfo.Exp.Value = playerInfo.Exp.Value - playerInfo.MaxExp.Value;
            playerInfo.MaxExp.Value = playerInfo.ExpTable[playerInfo.Level.Value - 1];
            // TODO : 스킬 진화 시 코드 변경 있어야 함.


            var popup = PopupManager.Instance.Popups[(int)PopupKind.PopupSkillSelect] as PopupSkillSelect;
            popup.Initialize();


            List<SkillInfo> randomSkills = GetRandomSkills(3);
            if (randomSkills.Count == 0) return;

            popup.SetRandomSkills();
            popup.Refresh();

            PopupManager.Instance.OpenPopup((int)PopupKind.PopupSkillSelect);
        }

        public void Knockback(Vector3 direction)
        {
            knockbackFlag = true;
            knockbackDirection = direction;
        }

        public void Blink(Color color)
        {
            blinkFlag = true;
            blinkTick = 0f;
            subBlinkTick = 0f;
        }

        public List<SkillInfo> GetRandomSkills(int count)
        {
            var playerInfo = DataManager.Instance.PlayerInfo;
            List<SkillInfo> list = new List<SkillInfo>();
            if (count <= 0) return list;

            var skillCombinationTableCollection = DataManager.Instance.SkillCombinationTableCollection;
            var skillCombinationTable = skillCombinationTableCollection.Tables.FirstOrDefault();
            if(skillCombinationTable == null)
            {
                Debug.LogError("Skill Combination Table not found in collection!");
                return list;
            }
            var skillInfos = DataManager.Instance.SkillSettings.SkillInfos;
            int len = skillInfos.Count;
            for (int i = 0; i < len; i++)
            {
                SkillInfo skillInfo = skillInfos[i];
                Skill skill = skills[(int)skillInfo.Kind];
                var rowDatas = skillCombinationTable.Rows.Select(o => o as SkillCombinationData).ToList();
                var combination = rowDatas.Where(o => o.ResultSkill == skillInfo.Kind).ToList();
                bool findCombination = combination.Count > 0;

                if (skill != null && skill.IsMaxLevel) continue;
                if (skillInfo.IsMonsterSkill) continue;
                if (skill == null && skillInfo.IsAttackSkill && attackSkillCount == playerInfo.MaxAttackSkillCount) continue;
                if (skill == null && skillInfo.IsBuffSkill && buffSkillCount == playerInfo.MaxBuffSkillCount) continue;
                if (disabledSkills[(int)skillInfo.Kind]) continue;
                if (skill == null && skillInfo.Grade > Grade.Normal && findCombination)
                {
                    var skillA = FindSkill(combination[0].SkillA);
                    var skillB = FindSkill(combination[0].SkillB);
                    if (skillA == null) continue;
                    if (skillB == null) continue;
                    if (skillA.Level < combination[0].LevelA) continue;
                    if (skillB.Level < combination[0].LevelB) continue;

                    if (skillInfos[(int)combination[0].SkillA].IsAttackSkill)
                    {
                        disabledSkills[(int)combination[0].SkillA] = true;
                    }

                    if (skillInfos[(int)combination[0].SkillB].IsAttackSkill)
                    {
                        disabledSkills[(int)combination[0].SkillB] = true;
                    }
                }

                list.Add(skillInfo);
            }

            list = list.OrderBy(x => UnityEngine.Random.value).ToList();

            int resultCount = Math.Min(list.Count, count);
            List<SkillInfo> result = new List<SkillInfo>();
            for (int i = 0; i < resultCount; i++)
            {
                result.Add(list[i]);
            }

            return result;
        }

        public void OnSkillSelect(SkillInfo skillInfo)
        {
            Skill skill = FindSkill(skillInfo.Kind);
            if (skill != null)
            {
                skill.LevelUp();
            }
            else
            {
                AddSkill(skillInfo.Kind);

                if (skillInfo.Grade > Grade.Normal)
                {
                    var skillInfos = DataManager.Instance.SkillSettings.SkillInfos;
                    var skillCombinationTableCollection = DataManager.Instance.SkillCombinationTableCollection;
                    var skillCombinationTables = skillCombinationTableCollection.Tables.Where(o => o.SheetName == "Combine").ToList();
                    if(skillCombinationTables.Count == 0)
                    {
                        Debug.LogError($"Sheet name [Combine] not found!!");
                        return;
                    }
                    var skillCombinationTable = skillCombinationTables[0].Rows.Select(o => o as SkillCombinationData).ToList();
                    if (skillCombinationTable.Count == 0)
                    {
                        Debug.LogError($"SkillCombinationTable not found!!");
                        return;
                    }
                    var combination = skillCombinationTable.Where(o => o.ResultSkill == skillInfo.Kind).ToList();
                    if (combination.Count != 0)
                    {
                        if (skillInfos[(int)combination[0].SkillA].IsAttackSkill)
                        {
                            RemoveSkill(combination[0].SkillA);
                        }

                        if (skillInfos[(int)combination[0].SkillB].IsAttackSkill)
                        {
                            RemoveSkill(combination[0].SkillB);
                        }

                    }
                }

            }
            PopupManager.Instance.ClosePopup((int)PopupKind.PopupSkillSelect);
            GameManager.Instance.Resume();
        }

        public override void OnMoveStart(MoveStartEventArgs args)
        {
            MoveStart(args.Direction);
        }

        public override void OnMoveEnd(MoveEndEventArgs args)
        {
            MoveStop();
        }

        private void OnDeath_Internal(Actor attacker)
        {
            IsDead = true;
            AnimationContorller.Die();
            hpBar.gameObject.SetActive(false);
            Collider.enabled = false;
            OnDeath.Invoke(this);

            if (this == GameManager.Instance.Player)
            {
                GameManager.Instance.OnPlayerDie();
                OnPlayerDied();
            }

            if (attacker.Team == Team.Alliance)
            {
                DataManager.Instance.PlayerInfo.KillCount.Value += 1;
            }

            if (Stat.IsBoss)
            {
                var stageInfo = GameManager.Instance.CurrentStage.StageInfo;
                var playerInfo = DataManager.Instance.PlayerInfo;

                var popup = PopupManager.Instance.Popups[(int)PopupKind.PopupGameResult] as PopupGameResult;
                popup.SetData(new GameResult()
                {
                    IsVictory = true,
                    StageName = stageInfo.Name,
                    StageSubName = stageInfo.SubName,
                    PlayTime = playerInfo.PlayTime.Value,
                    Gold = playerInfo.Coin.Value,
                    Exp = 10, // TODO : 계정 Exp 적용시 변경해야 함
                });

                GameManager.Instance.Pause();
                FirestoreManager.Instance.GetUserDataAsync(DataManager.Instance.Account.UserId.Value, OnGetUserData);
                PopupManager.Instance.OpenPopup((int)PopupKind.PopupGameResult);
                FirestoreManager.Instance.SetLeaderboardData(stageInfo.CodeName, new LeaderboardDataElement()
                {
                    UserId = DataManager.Instance.Account.UserId.Value,
                    Score = DataManager.Instance.PlayerInfo.KillCount.Value,
                });
            }
        }

        private void OnPlayerDied()
        {
            var stageInfo = GameManager.Instance.CurrentStage.StageInfo;
            var playerInfo = DataManager.Instance.PlayerInfo;

            var popup = PopupManager.Instance.Popups[(int)PopupKind.PopupGameResult] as PopupGameResult;
            popup.SetData(new GameResult()
            {
                IsVictory = false,
                StageName = stageInfo.Name,
                StageSubName = stageInfo.SubName,
                PlayTime = playerInfo.PlayTime.Value,
                Gold = playerInfo.Coin.Value,
                Exp = 0,
            });

            GameManager.Instance.Pause();
            PopupManager.Instance.OpenPopup((int)PopupKind.PopupGameResult);
            FirestoreManager.Instance.SetLeaderboardData(stageInfo.CodeName, new LeaderboardDataElement()
            {
                UserId = DataManager.Instance.Account.UserId.Value,
                Score = DataManager.Instance.PlayerInfo.KillCount.Value,
            });
        }

        private void OnGetUserData(UserData userData)
        {
            FirestoreManager.Instance.GetMaxExp(userData.Level, OnGetMaxExp);
        }

        private void OnGetMaxExp(int maxExp)
        {
            Account account = DataManager.Instance.Account;
            account.MaxExp.Value = maxExp;
            account.Exp.Value += 10;
            float exp = account.Exp.Value;
            if (exp >= maxExp)
            {
                account.Exp.Value = 0;
                account.Level.Value += 1;
                account.Stamina.Value = account.MaxStamina.Value;
            }

            FirestoreManager.Instance.SetUserData(account.ToUserData());

            PopupManager.Instance.ClosePopup((int)PopupKind.PopupLoading);
            PopupManager.Instance.OpenPopup((int)PopupKind.PopupGameResult);
        }


        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (IsDead) return;
            base.OnTriggerEnter2D(other);

            Projectile projectile = other.GetComponent<Projectile>();
            DamageArea damageArea = other.GetComponentInParent<DamageArea>();
            if (projectile != null && Team != projectile.Team && projectile.DestroyFlag == false)
            {
                var attacker = projectile.Owner;
                var attackerStat = attacker.Stat;
                float elementalDamage = attackerStat.GetElementalDamage(projectile.Stat.WeaponType);
                float criticalChance = attackerStat.GetTotalCriticalChance();
                float random = UnityEngine.Random.Range(0f, 1f);
                float damage;
                if (random < criticalChance)
                {
                    damage = attackerStat.Damage * projectile.Stat.Damage * elementalDamage * attackerStat.CriticalDamage;
                }
                else
                {
                    damage = attackerStat.Damage * projectile.Stat.Damage * elementalDamage * (1f - Stat.DamageReduction);
                }

                // knockback
                if (projectile.Stat.Knockback && Stat.Knockback)
                {
                    Vector3 to = (transform.position - projectile.transform.position).normalized;
                    Knockback(to);
                }

                TakeDamage(attacker, damage);
                projectile.OnHit();
            }

            if (damageArea != null && Team != damageArea.Team && damageArea.DestroyFlag == false)
            {
                var attacker = damageArea.Owner;
                var attackerStat = attacker.Stat;
                float elementalDamage = attackerStat.GetElementalDamage(damageArea.Stat.WeaponType);
                float criticalChance = attackerStat.GetTotalCriticalChance();
                float random = UnityEngine.Random.Range(0f, 1f);
                float damage;
                if (random < criticalChance)
                {
                    damage = attackerStat.Damage * damageArea.Stat.Damage * elementalDamage * attackerStat.CriticalDamage;
                }
                else
                {
                    damage = attackerStat.Damage * damageArea.Stat.Damage * elementalDamage * (1f - Stat.DamageReduction);
                }

                // knockback
                if (damageArea.Stat.Knockback && Stat.Knockback)
                {
                    Vector3 to = (transform.position - damageArea.transform.position).normalized;
                    Knockback(to);
                }

                TakeDamage(attacker, damage);
                damageArea.OnHit();
            }
        }

        protected override void OnCollisionStay2D(Collision2D collision)
        {

            if (IsDead) return;
            base.OnCollisionStay2D(collision);

            //Debug.Log($"OnEntityTriggerStay {self.gameObject.name} {target.gameObject.name}");
            //float power = Vector3.Distance(bounds.max, bounds.min);
            var attacker = collision.collider.GetComponent<Actor>();
            if (attacker == null) return;
            if (Team == Team.Alliance && attacker.Team == Team.Enemy)
            {
                // process damage from enemy to player
                if (attackFlag)
                {
                    attackTick = 0f;
                    attackFlag = false;
                    TakeDamage(attacker, attacker.Stat.Damage);
                }
            }

            // Push
            if (Team == Team.Enemy && attacker.Team != Team.Neutral || Team == Team.Alliance && attacker.Team == Team.Neutral)
            {
                //Vector3 to = (transform.position - targetActor.transform.position);
                //float dist = to.magnitude;
                //Vector3 direction = to / dist;
                //float power = (Radius + targetActor.Radius - dist) * 5f;
                //Velocity += direction * power * Time.fixedDeltaTime;
            }
        }

        protected override void OnTriggerExit2D(Collider2D other)
        {
            base.OnTriggerExit2D(other);
        }

        private void OnDeadAnimationExit()
        {
            AnimationContorller.Revival();
            IsDead = false;
            Collider.enabled = true;
            Release();
        }
    }
}
