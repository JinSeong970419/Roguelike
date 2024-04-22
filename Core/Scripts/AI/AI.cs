using System.Collections.Generic;
using UnityEngine;

namespace Roguelike.Core
{

    public partial class AI : MonoBehaviour
    {
        private Actor owner;
        private List<AiLogicExcutor> logics;

        private void Awake()
        {
            owner = GetComponent<Actor>();
            if (owner == null)
            {
                Debug.LogError("Actor Not Found!!");
            }
        }

        private void OnEnable()
        {
            logics = owner.Info.AI;
            if (logics != null)
            {
                int count = logics.Count;
                for (int i = 0; i < count; i++)
                {
                    var logic = logics[i];
                    logic.Initialize();
                }
            }

        }

        private void OnDisable()
        {
            logics = null;
        }

        private void FixedUpdate()
        {
            if (logics != null)
            {
                int count = logics.Count;
                for (int i = 0; i < count; i++)
                {
                    var logic = logics[i];
                    logic.Execute(this);
                }
            }
        }
    }
}
