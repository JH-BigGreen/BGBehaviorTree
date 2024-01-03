//----------------------------------------
//author: BigGreen
//date: 2023-10-23 21:03
//----------------------------------------

using System;
using System.Collections.Generic;

namespace BG.BT
{
    [Serializable]
    public class Parallel : Composite
    {
        private List<int> m_RunningList = new List<int>();

        /// <summary>
        /// 有任意失败则失败，并终止其它子节点。全部成功则成功。平行节点每次节点都要Evaluate
        /// </summary>
        /// <returns></returns>
        public override BTNodeState Evaluate()
        {
            m_RunningList.Clear();
            var anyFailure = false;

            for (int i = 0; i < m_Children.Count; i++)
            {
                var child = m_Children[i];
                var state = child.Evaluate();
                if (state == BTNodeState.Running)
                {
                    m_RunningList.Add(i);
                }
                else if (state == BTNodeState.Failure)
                {
                    anyFailure = true;
                }
            }

            if (anyFailure)
            {
                foreach (var runningIndex in m_RunningList)
                {
                    var runningNode = m_Children[runningIndex];
                    runningNode.Abort();
                }

                State = BTNodeState.Failure;
            }
            else
            {
                State = m_RunningList.Count > 0 ? BTNodeState.Running : BTNodeState.Success;
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