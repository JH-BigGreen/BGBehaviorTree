//----------------------------------------
//author: BigGreen
//date: 2023-12-25 14:45
//----------------------------------------

using System;
using System.Reflection;
using BG.BT;
using UnityEngine;
using UnityEngine.UIElements;

namespace BG.BTEditor
{
    [ResolverMatch(resolver = typeof(string))]
    public class StringResolver : FieldResolver<TextField, string>
    {
        private bool m_Multiline;
        
        public StringResolver(BehaviorTreeAsset asset, Type fieldType, MemberInfo info) : base(asset, fieldType, info)
        {
            m_Multiline = info.GetCustomAttribute<MultilineAttribute>() != null;
        }

        public override TextField CreateEditorField()
        {
            var textField = new TextField();
            if (m_Multiline)
            {
                textField.multiline = true;
                textField.style.minHeight = 150;
                textField.style.whiteSpace = WhiteSpace.Normal;
            }

            return textField;
        }
    }
}