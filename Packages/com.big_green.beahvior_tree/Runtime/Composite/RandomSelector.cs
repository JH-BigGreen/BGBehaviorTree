//----------------------------------------
//author: BigGreen
//date: 2023-10-24 20:10
//----------------------------------------

using System;
using System.Collections.Generic;

namespace BG.BT
{
    [Serializable]
    public class RandomSelector : Composite
    {
        private List<int> m_List = new List<int>();
        private int m_CurRunningIndex = -1;
        
        public override BTNodeState Evaluate()
        {
            ResetList();
            //有running的Node则继续执行
            if (m_CurRunningIndex >= 0)
            {
                var runningNode = m_Children[m_CurRunningIndex];
                var state = runningNode.Evaluate();
                if (state == BTNodeState.Failure)
                {
                    m_List.Remove(m_CurRunningIndex);
                    m_CurRunningIndex = -1;
                    return UpdateWhileFailure();
                }
                    
                if (state == BTNodeState.Success)
                {
                    m_CurRunningIndex = -1;
                    State = BTNodeState.Success;
                }
                else
                {
                    State = BTNodeState.Running; 
                }
                    
                return State;
            }

            //没有running的Node
            return UpdateWhileFailure();
        }
        
        private void ResetList()
        {
            m_List.Clear();
            for (int i = 0; i < m_Children.Count; i++)
            {
                m_List.Add(i);
            }
        }
        
        private BTNodeState UpdateWhileFailure()
        {
            var index = UnityEngine.Random.Range(0, m_List.Count);
            var next = m_List[index];
            m_List.Remove(next);
            var node = m_Children[next];
            var state = node.Evaluate();
            if (state == BTNodeState.Failure)
            {
                if (m_List.Count == 0)
                {
                    //所有的子节点都failure
                    m_CurRunningIndex = -1;
                    State = BTNodeState.Failure;
                    return State;
                }
                else
                {
                    //否则继续随机evaluate下一个节点
                    return UpdateWhileFailure();
                }
            }
                    
            if (state == BTNodeState.Success)
            {
                m_CurRunningIndex = -1;
                State = BTNodeState.Success;
            }
            else
            {
                m_CurRunningIndex = next;
                State = BTNodeState.Running; 
            }
            return State;
        }
        
        public override void Abort()
        {
            if (m_CurRunningIndex >= 0)
            {
                var runningNode = m_Children[m_CurRunningIndex];
                runningNode.Abort();
                m_CurRunningIndex = -1;
            }
        }
    }
}