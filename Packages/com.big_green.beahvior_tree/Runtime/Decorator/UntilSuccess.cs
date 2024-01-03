//----------------------------------------
//author: BigGreen
//date: 2023-10-25 20:09
//----------------------------------------

using System;

namespace BG.BT
{
    [Serializable]
    public class UntilSuccess : Decorator
    {
        protected override BTNodeState OnDecorate(BTNodeState childState)
        {
            if (childState == BTNodeState.Success)
            {
                State = BTNodeState.Success;
            }
            else
            {
                State = BTNodeState.Running;
            }
            
            return State;
        }
    }
}