//----------------------------------------
//author: BigGreen
//date: 2023-10-25 20:00
//----------------------------------------

using System;

namespace BG.BT
{
    [Serializable]
    public class ReturnFailure : Decorator
    {
        protected override BTNodeState OnDecorate(BTNodeState childState)
        {
            State = BTNodeState.Failure; 
            return State;
        }
    }
}