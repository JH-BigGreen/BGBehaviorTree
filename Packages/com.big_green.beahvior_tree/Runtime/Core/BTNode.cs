//----------------------------------------
//author: BigGreen
//date: 2023-10-18 16:43
//----------------------------------------

using System;
using UnityEngine;

namespace BG.BT
{
    [Serializable]
    public abstract class BTNode
    {
        private BehaviorTree m_Tree;
        private BehaviorTreeAsset m_TreeAsset;

        public GameObject Actor => m_Tree.gameObject;

        public BTNodeState State { get; set; }

        #region editor相关

        public string guid = "";
        public Vector2 nodePosition;
        [BTNodeDataField]
        [Multiline]
        public string comment = "";
        
        #endregion
        
        public virtual void Init(BehaviorTree tree, BehaviorTreeAsset treeAsset)
        {
            m_Tree = tree;
            m_TreeAsset = treeAsset;
        }

        public abstract BTNodeState Evaluate();

        public virtual void Abort()
        {
            
        }

        public virtual void SetChildren(BTNode[] btNodes)
        {
            
        }

        public virtual BTNode[] GetChildren()
        {
            return null;
        }

        public virtual void AddChild(BTNode addNode)
        {
            
        }

        public virtual void RemoveChild(BTNode removeNode)
        {
            
        }

        public virtual void SortChildren()
        {
            
        }

        public void BindTreeVariable(SharedVariable variable)
        {
            if (variable == null)
            {
                return;
            }

            if (!variable.Shared)
            {
                return;
            }
            
            SharedVariable source = null;
            foreach (var item in m_TreeAsset.variables)
            {
                if (item.Name == variable.Name && item.GetType() == variable.GetType())
                {
                    source = item;
                    break;
                }
            }

            if (source == null) 
            {
                return;
            }
            
            variable.Bind(source);
        }
    }
}