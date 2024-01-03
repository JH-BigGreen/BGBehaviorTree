//----------------------------------------
//author: BigGreen
//date: 2023-10-23 21:19
//----------------------------------------

using System;
using System.Collections.Generic;

namespace BG.BT
{
    [Serializable]
    public class ParallelSelector : Composite
    {
        private List<int> m_RunningList = new List<int>();
        
        /// <summary>
        /// 有任意成功则成功，并终止其它子节点。全部失败则失败。平行节点每次节点都要Evaluate
        /// </summary>
        /// <returns></returns>
        public override BTNodeState Evaluate()
        {
            m_RunningList.Clear();
            var anySuccess = false;

            for (int i = 0; i < m_Children.Count; i++)
            {
                var child = m_Children[i];
                var state = child.Evaluate();
                if (state == BTNodeState.Running)
                {
                    m_RunningList.Add(i);
                }
                else if (state == BTNodeState.Success)
                {
                    anySuccess = true;
                }
            }

            if (anySuccess)
            {
                foreach (var runningIndex in m_RunningList)
                {
                    var runningNode = m_Children[runningIndex];
                    runningNode.Abort();
                }

                State = BTNodeState.Success; 
              
            }
            else
            {
                State = m_RunningList.Count > 0 ? BTNodeState.Running : BTNodeState.Failure;
            }

            return State;
        }
        
        public override void Abort()
        {
            foreach (var runningIndex in m_RunningList)
            {
                m_Children[runningIndex].Abort();
            }
            m_RunningList.Clear();
        }
    }
}