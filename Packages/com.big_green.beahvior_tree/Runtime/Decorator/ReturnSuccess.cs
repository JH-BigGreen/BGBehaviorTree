//----------------------------------------
//author: BigGreen
//date: 2023-10-25 19:59
//----------------------------------------

using System;

namespace BG.BT
{
    [Serializable]
    public class ReturnSuccess : Decorator
    {
        protected override BTNodeState OnDecorate(BTNodeState childState)
        {
            State = BTNodeState.Success; 
            return State;
        }
    }
}