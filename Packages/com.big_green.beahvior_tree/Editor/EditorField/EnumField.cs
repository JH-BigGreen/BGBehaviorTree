//----------------------------------------
//author: BigGreen
//date: 2023-12-25 14:58
//----------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace BG.BTEditor
{
    public class EnumField : PopupField<Enum>
    {
        public EnumField(string label, List<Enum> choices, Enum defaultValue)
            : base(label, choices, defaultValue, null, null)
        {
        }
    }
}