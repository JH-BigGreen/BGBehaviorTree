//----------------------------------------
//author: BigGreen
//date: 2023-12-25 14:41
//----------------------------------------

using System;
using System.Reflection;
using BG.BT;
using UnityEngine.UIElements;

namespace BG.BTEditor
{
    [ResolverMatch(resolver = typeof(float))]
    public class FloatResolver : FieldResolver<FloatField, float>
    {
        public FloatResolver(BehaviorTreeAsset asset, Type fieldType, MemberInfo info) : base(asset, fieldType, info)
        {
        }

        public override FloatField CreateEditorField()
        {
            return new FloatField();
        }
    }
}