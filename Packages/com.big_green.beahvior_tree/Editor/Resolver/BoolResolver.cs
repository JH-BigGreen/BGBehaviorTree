//----------------------------------------
//author: BigGreen
//date: 2023-12-25 14:38
//----------------------------------------

using System;
using System.Reflection;
using BG.BT;
using UnityEngine.UIElements;

namespace BG.BTEditor
{
    [ResolverMatch(resolver = typeof(bool))]
    public class BoolResolver : FieldResolver<Toggle, bool>
    {
        public BoolResolver(BehaviorTreeAsset asset, Type fieldType, MemberInfo info) : base(asset, fieldType, info)
        {
        }

        public override Toggle CreateEditorField()
        {
            return new Toggle();
        }
    }
}