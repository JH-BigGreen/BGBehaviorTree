//----------------------------------------
//author: BigGreen
//date: 2023-12-25 14:44
//----------------------------------------

using System;
using System.Reflection;
using BG.BT;
using UnityEngine.UIElements;

namespace BG.BTEditor
{
    [ResolverMatch(resolver = typeof(double))]
    public class DoubleResolver : FieldResolver<DoubleField, double>
    {
        public DoubleResolver(BehaviorTreeAsset asset, Type fieldType, MemberInfo info) : base(asset, fieldType, info)
        {
        }

        public override DoubleField CreateEditorField()
        {
            return new DoubleField();
        }
    }
}