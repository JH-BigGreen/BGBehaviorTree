//----------------------------------------
//author: BigGreen
//date: 2023-12-26 15:05
//----------------------------------------

using System;
using System.Reflection;
using BG.BT;
using UnityEditor.UIElements;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BG.BTEditor
{
    [ResolverMatch(resolver = typeof(GameObject))]
    public class GameObjectResolver : FieldResolver<ObjectField, Object>
    {
        public GameObjectResolver(BehaviorTreeAsset asset, Type fieldType, MemberInfo info) : base(asset, fieldType, info)
        {
        }

        public override ObjectField CreateEditorField()
        {
            var field = new ObjectField();
            field.objectType = typeof(GameObject);
            return field;
        }
    }
}