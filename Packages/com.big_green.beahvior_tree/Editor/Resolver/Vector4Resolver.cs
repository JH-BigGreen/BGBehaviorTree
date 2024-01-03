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
    [ResolverMatch(resolver = typeof(Vector4))]
    public class Vector4Resolver : FieldResolver<Vector4Field, Vector4>
    {
        public Vector4Resolver(BehaviorTreeAsset asset, Type fieldType, MemberInfo info) : base(asset, fieldType, info)
        {
        }

        public override Vector4Field CreateEditorField()
        {
            return new Vector4Field();
        }
    }
}