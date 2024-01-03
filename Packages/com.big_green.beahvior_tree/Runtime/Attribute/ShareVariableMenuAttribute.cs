//----------------------------------------
//author: BigGreen
//date: 2023-12-22 16:45
//----------------------------------------

using System;

namespace BG.BT
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ShareVariableMenuAttribute : Attribute
    {
        public string menuName;

        public ShareVariableMenuAttribute(string menuName)
        {
            this.menuName = menuName;
        }
    }
}