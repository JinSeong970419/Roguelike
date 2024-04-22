using UnityEngine;

namespace Roguelike.Core
{
    public class FiniteMapStage : IStage
    {
        private GameObject _tileObject;
        public FiniteMapStage(StageKind kind) : base(kind)
        {
        }

        public override StageType Type => StageType.FiniteMap;

        protected override void OnInitialize()
        {
            var tilePrefab = GameManager.Instance.CurrentStage.StageInfo.TilePrefab;
            _tileObject = Object.Instantiate(tilePrefab);
            GameManager.Instance.CreatePlayer();
        }

        protected override void OnRelease()
        {
            Object.Destroy(_tileObject);
        }

        protected override void OnUpdate()
        {
            if (GameManager.Instance.Player == null) return;
            ProcessSetNearestEnemyFromPlayer();
            ProcessSpawnMonster();
            ProcessStageLogic();
        }
    }
}
