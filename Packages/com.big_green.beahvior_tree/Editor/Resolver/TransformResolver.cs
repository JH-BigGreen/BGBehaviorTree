//----------------------------------------
//author: BigGreen
//date: 2023-12-26 15:10
//----------------------------------------

using System;
using System.Reflection;
using BG.BT;
using UnityEditor.UIElements;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BG.BTEditor
{
    [ResolverMatch(resolver = typeof(Transform))]
    public class TransformResolver : FieldResolver<ObjectField, Object>
    {
       
        public TransformResolver(BehaviorTreeAsset asset, Type fieldType, MemberInfo info) : base(asset, fieldType, info)
        {
        }

        public override ObjectField CreateEditorField()
        {
            var field = new ObjectField();
            field.objectType = typeof(Transform);
            return field;
        }
    }
}