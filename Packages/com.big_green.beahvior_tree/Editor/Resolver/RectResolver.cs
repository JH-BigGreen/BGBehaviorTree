//----------------------------------------
//author: BigGreen
//date: 2023-12-25 14:48
//----------------------------------------

using System;
using System.Reflection;
using BG.BT;
using UnityEngine;
using UnityEngine.UIElements;

namespace BG.BTEditor
{
    [ResolverMatch(resolver = typeof(Rect))]
    public class RectResolver : FieldResolver<RectField, Rect>
    {
        public RectResolver(BehaviorTreeAsset asset, Type fieldType, MemberInfo info) : base(asset, fieldType, info)
        {
        }

        public override RectField CreateEditorField()
        {
            return new RectField();
        }
    }
}