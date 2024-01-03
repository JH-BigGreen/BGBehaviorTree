//----------------------------------------
//author: BigGreen
//date: 2023-12-14 15:20
//----------------------------------------

using System;
using UnityEngine;

namespace BG.BT
{
    [Serializable]
    [BTGroup("Input")]
    public class InputGetKeyDown : Condition
    {
        [BTNodeDataField]
        public KeyCode key;
        
        public override bool IsConditionMet()
        {
            return Input.GetKeyDown(key);
        }
    }
}