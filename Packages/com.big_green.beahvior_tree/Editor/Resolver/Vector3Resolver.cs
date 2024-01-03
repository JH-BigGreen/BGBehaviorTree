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
    [ResolverMatch(resolver = typeof(Vector3))]
    public class Vector3Resolver : FieldResolver<Vector3Field, Vector3>
    {
        public Vector3Resolver(BehaviorTreeAsset asset, Type fieldType, MemberInfo info) : base(asset, fieldType, info)
        {
        }

        public override Vector3Field CreateEditorField()
        {
            return new Vector3Field();
        }
    }
}