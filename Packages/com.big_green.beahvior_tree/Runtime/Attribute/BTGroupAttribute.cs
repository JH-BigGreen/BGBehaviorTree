//----------------------------------------
//author: BigGreen
//date: 2023-12-19 15:54
//----------------------------------------

using System;

namespace BG.BT
{
    [AttributeUsage(AttributeTargets.Class)]
    public class BTGroupAttribute : Attribute
    {
        public string group;

        public BTGroupAttribute(string group)
        {
            this.group = group;
        }
    }
}