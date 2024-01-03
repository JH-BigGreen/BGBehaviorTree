//----------------------------------------
//author: BigGreen
//date: 2023-10-23 20:36
//----------------------------------------

using System;

namespace BG.BT
{
    [Serializable]
    public class Selector : Composite
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
                    return UpdateWhileFailure(abortIndex);
                }
                
                //没有中断，继续执行running的Node
                {
                    var runningNode = m_Children[m_CurRunningIndex];
                    var state = runningNode.Evaluate();
                    if (state == BTNodeState.Failure)
                    {
                        return UpdateWhileFailure(m_CurRunningIndex + 1);
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
            }
            //没有running的Node, 从头开始Evaluate
            return UpdateWhileFailure(0);
        }

        /// <summary>
        /// 有成功则返回，全部失败则失败
        /// </summary>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        private BTNodeState UpdateWhileFailure(int startIndex)
        {
            for (int i = startIndex; i < m_Children.Count; i++)
            {
                var child = m_Children[i];
                var state = child.Evaluate();
                if (state == BTNodeState.Failure)
                {
                    continue;
                }

                if (state == BTNodeState.Success)
                {
                    m_CurRunningIndex = -1;
                    State = BTNodeState.Success; 
            
                }
                else
                {
                    m_CurRunningIndex = i;
                    State = BTNodeState.Running;
                }

                return State;
            }
            
            m_CurRunningIndex = -1;
            State = BTNodeState.Failure;
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