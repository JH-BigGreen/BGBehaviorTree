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
    public class Vector2Resolver : FieldResolver<Vector2Field, Vector2>
    {
        public Vector2Resolver(BehaviorTreeAsset asset, Type fieldType, MemberInfo info) : base(asset, fieldType, info)
        {
        }

        public override Vector2Field CreateEditorField()
        {
            return new Vector2Field();
        }
    }
}