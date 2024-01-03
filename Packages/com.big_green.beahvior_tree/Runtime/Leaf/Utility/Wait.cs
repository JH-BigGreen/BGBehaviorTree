//----------------------------------------
//author: BigGreen
//date: 2023-12-14 16:21
//----------------------------------------

using System;
using UnityEngine;

namespace BG.BT
{
    [Serializable]
    [BTGroup("Utility")]
    public class Wait : Leaf
    {
        [BTNodeDataField]
        public SharedFloat waitTime;
        private float m_Timer;

        public override void Init(BehaviorTree tree, BehaviorTreeAsset treeAsset)
        {
            base.Init(tree, treeAsset);
            BindTreeVariable(waitTime);
            ClearTimer();
        }

        public override BTNodeState Evaluate()
        {
            m_Timer += Time.deltaTime;
            if (m_Timer >= waitTime.Value)
            {
                ClearTimer();
                State = BTNodeState.Success;
            }
            else
            {
                State = BTNodeState.Running; 
            }

            return State;
        }

        private void ClearTimer()
        {
            m_Timer = 0;
        }

        public override void Abort()
        {
            ClearTimer();
        }
    }
}