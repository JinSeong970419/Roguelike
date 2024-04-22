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
            // ���ο� ���·� ���� �� ����
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // ó���� ������ �������� ������ �� ������ ������ ����
            //Debug.Log($"DeadUpdate {stateInfo.normalizedTime}");
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // ���°� ���� ���·� �ٲ�� ������ ����
            OnDeadAnimationExit.Invoke();
        }

        public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // MonoBehaviour.OnAnimatorMove ���Ŀ� ����
        }

        public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // MonoBehaviour.OnAnimatorIK ���Ŀ� ����
        }

        public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
        {
            // ��ũ��Ʈ�� ������ ���� ���� ��ȯ�� ������ ����
        }

        public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
        {
            // ��ũ��Ʈ�� ������ ���� ��迡�� �������ö� ����
        }
    }
}
