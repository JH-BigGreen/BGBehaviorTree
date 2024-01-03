//----------------------------------------
//author: BigGreen
//date: 2023-10-24 21:40
//----------------------------------------

using System;

namespace BG.BT
{
    [Serializable]
    public class Inverter : Decorator
    {
        protected override BTNodeState OnDecorate(BTNodeState childState)
        {
            if (childState == BTNodeState.Success)
            {
                State = BTNodeState.Failure;
            }
            else if (childState == BTNodeState.Failure)
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