using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Roguelike.Core
{
    public class HorizontalSrollMapStage : IStage
    {
        public HorizontalSrollMapStage(StageKind kind) : base(kind)
        {
        }

        public override StageType Type => StageType.HorizontalMap;

        protected override void OnInitialize()
        {
            GameManager.Instance.CreatePlayer();
            var tilePrefab = GameManager.Instance.CurrentStage.StageInfo.TilePrefab;
            EndlessMap.Initialize(tilePrefab, EndlessMapType.Horizontal);
        }

        protected override void OnRelease()
        {
            EndlessMap.Release();
        }

        protected override void OnUpdate()
        {
            if (GameManager.Instance.Player == null) return;
            EndlessMap.Repeat(GameManager.Instance.Player.gameObject);
            ProcessSetNearestEnemyFromPlayer();
            ProcessSpawnMonster();
            ProcessStageLogic();
        }
    }
}
