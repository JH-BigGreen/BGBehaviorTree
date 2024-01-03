//----------------------------------------
//author: BigGreen
//date: 2023-10-24 21:43
//----------------------------------------

using System;

namespace BG.BT
{
    [Serializable]
    public class Repeater : Decorator
    {
        [BTNodeDataField]
        public SharedInt repeatCount = new SharedInt();
        [BTNodeDataField]
        public SharedBool endOnFailure = new SharedBool();

        private int m_Count;
        
        public override void Init(BehaviorTree tree, BehaviorTreeAsset treeAsset)
        {
            base.Init(tree, treeAsset);
            BindTreeVariable(repeatCount);
            BindTreeVariable(endOnFailure);
        }

        public override BTNodeState Evaluate()
        {
            if (repeatCount.Value <= 0)
            {
                State = BTNodeState.Failure;
                return State;
            }
            
            var state = m_Child.Evaluate();
            if (state == BTNodeState.Success)
            {
                m_Count++;
                m_ChildRunning = false;
                if (m_Count >= repeatCount.Value)
                {
                    m_Count = 0;
                    State = BTNodeState.Success;
                }
                else
                {
                    State = BTNodeState.Running;
                }
                
                return State;
            }

            if (state == BTNodeState.Failure)
            {
                if (endOnFailure.Value)
                {
                    m_Count = 0;
                    m_ChildRunning = false;
                    State = BTNodeState.Failure;
                    return State;
                }
                else
                {
                    m_Count++;
                    m_ChildRunning = false;
                    if (m_Count >= repeatCount.Value)
                    {
                        m_Count = 0;
                        State = BTNodeState.Failure;
                    }
                    else
                    {
                        State = BTNodeState.Running;
                    }
                    return State;
                }
            }

            m_ChildRunning = true;
            State = BTNodeState.Running;
            return State;
        }

        public override void Abort()
        {
            m_Count = 0;
            base.Abort();
        }
    }
}