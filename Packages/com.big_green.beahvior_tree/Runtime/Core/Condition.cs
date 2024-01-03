//----------------------------------------
//author: BigGreen
//date: 2023-10-18 17:00
//----------------------------------------

using System;
using UnityEngine;

namespace BG.BT
{
    [Serializable]
    public abstract class Condition : BTNode
    {
        [BTNodeDataField]
        public AbortType abortType;
        
        [SerializeReference]
        protected BTNode m_Child;

        private bool m_AbortPriority;
        private bool m_ChildRunning;
        
        public override void Init(BehaviorTree tree, BehaviorTreeAsset treeAsset)
        {
            base.Init(tree, treeAsset);
            if (m_Child != null)
            {
                m_Child.Init(tree, treeAsset);
            }
        }

        public void AbortPriority()
        {
            m_AbortPriority = true;
        }

        public void AbortPriorityFinished()
        {
            m_AbortPriority = false;
        }
        
        public override BTNodeState Evaluate()
        {
            //如果是优先级中断
            if (m_AbortPriority)
            {
                if (m_Child == null)
                {
                    return BTNodeState.Success;
                }
                
                var state = EvaluateChild();
                AbortPriorityFinished();
                return state;
            }
            
            if (m_Child == null)
            {
                State = IsConditionMet() ? BTNodeState.Success : BTNodeState.Failure;
                return State;
            }
            
            if (m_ChildRunning)
            {
                //子节点在running,判断是否要中断
                if (abortType == AbortType.Self || abortType == AbortType.Both)
                {
                    if (!IsConditionMet())
                    {
                        //中断
                        m_Child.Abort();
                        m_ChildRunning = false;
                        State = BTNodeState.Failure;
                        return State;
                    }
                }
                
                return EvaluateChild();
            }
            
            //满足条件
            if (IsConditionMet())
            {
                return EvaluateChild();
            }

            //否则failure
            State = BTNodeState.Failure; 
            return State;
        }

        private BTNodeState EvaluateChild()
        {
            State = m_Child.Evaluate();
            m_ChildRunning = State == BTNodeState.Running;
            return State;
        }

        public abstract bool IsConditionMet();

        public override void Abort()
        {
            if (m_ChildRunning)
            {
                if (m_Child != null)
                {
                    m_Child.Abort();
                }

                m_ChildRunning = false;
            }
        }
        
        public override void SetChildren(BTNode[] btNodes)
        {
            if (btNodes == null || btNodes.Length == 0)
            {
                m_Child = null;
                return;
            }

            m_Child = btNodes[0];
        }
        
        public override BTNode[] GetChildren()
        {
            if (m_Child == null)
            {
                return null;
            }
            return new BTNode[]{m_Child};
        }
        
        public override void AddChild(BTNode addNode)
        {
            m_Child = addNode;
        }

        public override void RemoveChild(BTNode removeNode)
        {
            if (m_Child == removeNode)
            {
                m_Child = null;
            }
        }
    }
}