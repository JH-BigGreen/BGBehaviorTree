//----------------------------------------
//author: BigGreen
//date: 2023-10-24 20:54
//----------------------------------------

using System;
using System.Collections.Generic;

namespace BG.BT
{
    [Serializable]
    public class WeightedRandomSequence : Composite
    {
        public List<float> weights = new List<float>();

        private List<int> m_List = new List<int>();
        private List<float> m_TempWeights = new List<float>();
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

        private int GetNext()
        {
            m_TempWeights.Clear();
            float total = 0;
            foreach (var nodeIndex in m_List)
            {
                float t = 0;
                if (nodeIndex >= 0 && nodeIndex < weights.Count)
                {
                    t = weights[nodeIndex];
                }
                m_TempWeights.Add(t);
                total += t;
            }

            var r = UnityEngine.Random.Range(0, total);
            for (int i = 0; i < m_TempWeights.Count; i++)
            {
                if (r <= m_TempWeights[i])
                {
                    return m_List[i];
                }

                r -= m_TempWeights[i];
            }

            return m_List[0];
        }

        private BTNodeState UpdateWhileSuccess()
        {
            var next = GetNext();
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