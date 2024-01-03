//----------------------------------------
//author: BigGreen
//date: 2023-12-25 14:49
//----------------------------------------

using System;
using System.Reflection;
using BG.BT;
using UnityEngine;
using UnityEngine.UIElements;

namespace BG.BTEditor
{
    [ResolverMatch(resolver = typeof(Vector2Int))]
    public class Vector2IntResolver : FieldResolver<Vector2IntField, Vector2Int>
    {
        public Vector2IntResolver(BehaviorTreeAsset asset, Type fieldType, MemberInfo info) : base(asset, fieldType, info)
        {
        }

        public override Vector2IntField CreateEditorField()
        {
            return new Vector2IntField();
        }
    }
}