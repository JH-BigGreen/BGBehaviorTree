//----------------------------------------
//author: BigGreen
//date: 2023-10-18 17:01
//----------------------------------------

using System;
using UnityEngine;

namespace BG.BT
{
    [Serializable]
    public abstract class Decorator : BTNode
    {
        [SerializeReference]
        protected BTNode m_Child;

        protected bool m_ChildRunning;
        
        public override void Init(BehaviorTree tree, BehaviorTreeAsset treeAsset)
        {
            base.Init(tree, treeAsset);
            if (m_Child != null)
            {
                m_Child.Init(tree, treeAsset);
            } 
        }

        public override BTNodeState Evaluate()
        {
            var state = BTNodeState.Failure;
            if (m_Child!= null)
            {
                state = m_Child.Evaluate();
                m_ChildRunning = state == BTNodeState.Running;
            }
            return OnDecorate(state);
        }

        protected virtual BTNodeState OnDecorate(BTNodeState childState)
        {
            State = childState;
            return State;
        }

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