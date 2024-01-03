//----------------------------------------
//author: BigGreen
//date: 2023-10-25 20:10
//----------------------------------------

using System;

namespace BG.BT
{
    [Serializable]
    public class UntilFailure : Decorator
    {
        protected override BTNodeState OnDecorate(BTNodeState childState)
        {
            if (childState == BTNodeState.Failure)
            {
                State = BTNodeState.Failure;
            }
            else
            {
                State = BTNodeState.Running;
            }
            
            return State;
        }
    }
}