namespace Roguelike.Core
{
    public class EndlessMapStage : IStage
    {
        public EndlessMapStage(StageKind kind) : base(kind)
        {
        }

        public override StageType Type => StageType.EndlessMap;

        protected override void OnInitialize()
        {
            GameManager.Instance.CreatePlayer();
            var tilePrefab = GameManager.Instance.CurrentStage.StageInfo.TilePrefab;
            EndlessMap.Initialize(tilePrefab, EndlessMapType.All);
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
