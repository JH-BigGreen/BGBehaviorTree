//----------------------------------------
//author: BigGreen
//date: 2023-12-25 18:25
//----------------------------------------

using System;
using System.Collections.Generic;

namespace BG.BT
{
    [Serializable]
    [ShareVariableMenu("BoolList")]
    public class SharedBoolList : SharedVariable<List<bool>>
    {
    }
}