using UnityEngine;

namespace Roguelike.Core
{
    public class StageController : MonoBehaviour
    {
        [SerializeField] private GameEvent<StageKind> selectStageEvent;
        [SerializeField] private Variable<int> selectedStageIndex;

        public void SelectStage()
        {
            Debug.Log($"Stage: {selectedStageIndex.Value}");
            selectStageEvent.Invoke((StageKind)selectedStageIndex.Value);
        }

    }
}
