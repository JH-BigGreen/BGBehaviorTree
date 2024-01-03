//----------------------------------------
//author: BigGreen
//date: 2023-10-18 16:53
//----------------------------------------

using System;
using UnityEngine;

namespace BG.BT
{
    [Serializable]
    public class Root : BTNode
    {
        [SerializeReference]
        private BTNode m_Child;

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
            if (m_Child != null)
            {
                State = m_Child.Evaluate();
            }
            else
            {
                State = BTNodeState.Failure;
            }

            return State;
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