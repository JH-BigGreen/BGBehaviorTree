//----------------------------------------
//author: BigGreen
//date: 2023-10-24 19:47
//----------------------------------------

using System;
using System.Collections.Generic;

namespace BG.BT
{
    [Serializable]
    public class RandomSequence : Composite
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
                if (state == BTNodeState.Success)
                {
                    m_List.Remove(m_CurRunningIndex);
                    m_CurRunningIndex = -1;
                    return UpdateWhileSuccess();
                }
                    
                if (state == BTNodeState.Failure)
                {
                    m_CurRunningIndex = -1;
                    State = BTNodeState.Failure;
                }
                else
                {
                    State = BTNodeState.Running; 
                }

                return State;
            }

            //没有running的Node
            return UpdateWhileSuccess();
        }

        private void ResetList()
        {
            m_List.Clear();
            for (int i = 0; i < m_Children.Count; i++)
            {
                m_List.Add(i);
            }
        }

        private BTNodeState UpdateWhileSuccess()
        {
            var index = UnityEngine.Random.Range(0, m_List.Count);
            var next = m_List[index];
            m_List.Remove(next);
            var node = m_Children[next];
            var state = node.Evaluate();
            if (state == BTNodeState.Success)
            {
                if (m_List.Count == 0)
                {
                    //所有的子节点都success
                    m_CurRunningIndex = -1;
                    State = BTNodeState.Success; 
                    return State;
                }
                else
                {
                    //否则继续随机evaluate下一个节点
                    return UpdateWhileSuccess();
                }
            }
                    
            if (state == BTNodeState.Failure)
            {
                m_CurRunningIndex = -1;
                State = BTNodeState.Failure;
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