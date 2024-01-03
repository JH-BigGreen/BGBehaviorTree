//----------------------------------------
//author: BigGreen
//date: 2023-10-23 19:16
//----------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BG.BT
{
    [Serializable]
    [ShareVariableMenu("GameObjectList")]
    public class SharedGameObjectList : SharedVariable<List<GameObject>>
    {

    }
}