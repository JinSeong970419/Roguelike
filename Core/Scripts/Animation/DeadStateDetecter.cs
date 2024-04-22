using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Roguelike.Core
{
    public class DeadStateDetecter : StateMachineBehaviour
    {
        private UnityEvent onDeadAnimationExit = new UnityEvent();
        public UnityEvent OnDeadAnimationExit { get { return onDeadAnimationExit; } }
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // 새로운 상태로 변할 때 실행
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // 처음과 마지막 프레임을 제외한 각 프레임 단위로 실행
            //Debug.Log($"DeadUpdate {stateInfo.normalizedTime}");
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // 상태가 다음 상태로 바뀌기 직전에 실행
            OnDeadAnimationExit.Invoke();
        }

        public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // MonoBehaviour.OnAnimatorMove 직후에 실행
        }

        public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // MonoBehaviour.OnAnimatorIK 직후에 실행
        }

        public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
        {
            // 스크립트가 부착된 상태 기계로 전환이 왔을때 실행
        }

        public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
        {
            // 스크립트가 부착된 상태 기계에서 빠져나올때 실행
        }
    }
}
