using Roguelike.InGame;

namespace Roguelike.Core
{
    public static class StageLogic
    {
        public static void SpawnMonster(SpawnMonsterParam param)
        {
            GameManager.Instance.CurrentStage.SpawnMonster(param.kind, param.count);
        }

        public static void SpawnMonsterOnce(SpawnMonsterParam param)
        {
            GameManager.Instance.CurrentStage.SpawnMonsterOnce(param.kind, param.count, param.distance);
        }
        public static void SpawnFenceAround(SpawnMonsterParam param)
        {
            GameManager.Instance.CurrentStage.SpawnFenceAround(param.kind, param.count, param.distance);
        }

        public static void ChangeSpawnDelay(float delay)
        {
            GameManager.Instance.CurrentStage.ChangedSpawnDelay(delay);
        }

        public static void DestroyAllMonsters()
        {
            GameManager.Instance.CurrentStage.DestroyAllMonsters();
        }

        public static void SpawnItemAtRandomPosition(SpawnItemAtRandomParam param)
        {
            GameManager.Instance.CurrentStage.SpawnItemAtRandomPosition(param.kind);
        }

        public static void SetZoom(ZoomParam param)
        {
            GameManager.Instance.CurrentStage.SetZoom(param.zoom, param.time);
        }

        public static void ShowBossAlert(float second)
        {
            GameManager.Instance.ShowBossAlert(second); 
        }

    }
}
