//----------------------------------------
//author: BigGreen
//date: 2023-12-25 14:40
//----------------------------------------

using System;
using System.Reflection;
using BG.BT;
using UnityEngine.UIElements;

namespace BG.BTEditor
{
    [ResolverMatch(resolver = typeof(int))]
    public class IntResolver : FieldResolver<IntegerField, int>
    {
        public IntResolver(BehaviorTreeAsset asset, Type fieldType, MemberInfo info) : base(asset, fieldType, info)
        {
        }

        public override IntegerField CreateEditorField()
        {
            return new IntegerField();
        }
    }
}