//----------------------------------------
//author: BigGreen
//date: 2023-12-27 16:09
//----------------------------------------

using System;
using System.Reflection;
using BG.BT;
using UnityEditor.UIElements;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BG.BTEditor
{
    [ResolverMatch(resolver = typeof(ScriptableObject))]
    public class ScriptableObjectResolver : FieldResolver<ObjectField, Object>
    {
        public ScriptableObjectResolver(BehaviorTreeAsset asset, Type fieldType, MemberInfo info) : base(asset, fieldType, info)
        {
        }

        public override ObjectField CreateEditorField()
        {
            var field = new ObjectField();
            field.objectType = typeof(ScriptableObject);
            return field;
        }
    }
}