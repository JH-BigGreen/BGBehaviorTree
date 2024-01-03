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
    [ResolverMatch(resolver = typeof(Vector3Int))]
    public class Vector3IntResolver : FieldResolver<Vector3IntField, Vector3Int>
    {
        public Vector3IntResolver(BehaviorTreeAsset asset, Type fieldType, MemberInfo info) : base(asset, fieldType, info)
        {
        }

        public override Vector3IntField CreateEditorField()
        {
            return new Vector3IntField();
        }
    }
}