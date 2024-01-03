//----------------------------------------
//author: BigGreen
//date: 2023-10-23 19:26
//----------------------------------------

using System;

namespace BG.BT
{
    [Serializable]
    public class Sequence : Composite
    {
        private int m_CurRunningIndex = -1;
        
        public override BTNodeState Evaluate()
        {
            //如果有Running的Node, 判断有没有中断
            if (m_CurRunningIndex >= 0)
            {
                var abortIndex = -1;
                for (int i = 0; i < m_CurRunningIndex; i++)
                {
                    var child = m_Children[i];
                    if (child is Condition condition && (condition.abortType== AbortType.Priority || condition.abortType == AbortType.Both))
                    {
                        if (condition.IsConditionMet())
                        {
                            abortIndex = i;
                            break;
                        }
                    }
                }

                //有中断，中断running的Node
                if (abortIndex >= 0)
                {
                    var runningNode = m_Children[m_CurRunningIndex];
                    runningNode.Abort();

                    var c = (Condition)m_Children[abortIndex];
                    c.AbortPriority();
                    //从高优先级(中断)处开始Evaluate
                    return UpdateWhileSuccess(abortIndex);
                }
                
                //没有中断，继续执行running的Node
                {
                    var runningNode = m_Children[m_CurRunningIndex];
                    var state = runningNode.Evaluate();
                    if (state == BTNodeState.Success)
                    {
                        return UpdateWhileSuccess(m_CurRunningIndex + 1);
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
            }
            //没有running的Node, 从头开始Evaluate
            return UpdateWhileSuccess(0);
        }

        /// <summary>
        /// 有失败则返回，全部成功则成功
        /// </summary>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        private BTNodeState UpdateWhileSuccess(int startIndex)
        {
            for (int i = startIndex; i < m_Children.Count; i++)
            {
                var child = m_Children[i];
                var state = child.Evaluate();
                if (state == BTNodeState.Success)
                {
                    continue;
                }

                if (state == BTNodeState.Failure)
                {
                    m_CurRunningIndex = -1;
                    State = BTNodeState.Failure;
                }
                else
                {
                    m_CurRunningIndex = i;
                    State = BTNodeState.Running;
                }

                return State;
            }
            
            m_CurRunningIndex = -1;
            State = BTNodeState.Success; 
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