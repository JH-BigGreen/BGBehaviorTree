//----------------------------------------
//author: BigGreen
//date: 2023-10-18 16:57
//----------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BG.BT
{
    [Serializable]
    public abstract class Composite : BTNode
    {
        [SerializeReference]
        protected List<BTNode> m_Children;

        public override void Init(BehaviorTree tree, BehaviorTreeAsset treeAsset)
        {
            base.Init(tree, treeAsset);
            if (m_Children != null)
            {
                for (int i = 0; i < m_Children.Count; i++)
                {
                    var child = m_Children[i]; 
                    child.Init(tree, treeAsset);
                }
            }
        }
        
        public override void SetChildren(BTNode[] btNodes)
        {
            if (btNodes == null)
            {
                m_Children = null;
                return;
            }

            if (m_Children == null)
            {
                m_Children = new List<BTNode>();
            }
            else
            {
                m_Children.Clear();
            }

            foreach (var node in btNodes)
            {
                m_Children.Add(node);
            }
        }
        
        public override BTNode[] GetChildren()
        {
            if (m_Children == null || m_Children.Count == 0)
            {
                return null;
            }
            return m_Children.ToArray();
        }
        
        public override void AddChild(BTNode addNode)
        {
            if (m_Children == null)
            {
                m_Children = new List<BTNode>();
            }
            m_Children.Add(addNode);

            SortChildren();
        }

        public override void RemoveChild(BTNode removeNode)
        {
            if (m_Children == null)
            {
                return;
            }

            m_Children.Remove(removeNode);
        }
        
        public override void SortChildren()
        {
            m_Children.Sort((a, b) => { return a.nodePosition.x.CompareTo(b.nodePosition.x);});
        }

    }
}