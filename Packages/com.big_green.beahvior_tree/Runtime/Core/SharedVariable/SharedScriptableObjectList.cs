//----------------------------------------
//author: BigGreen
//date: 2023-12-27 16:12
//----------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BG.BT
{
    [Serializable]
    [ShareVariableMenu("ScriptableObjectList")]
    public class SharedScriptableObjectList : SharedVariable<List<ScriptableObject>>
    {

    }
}