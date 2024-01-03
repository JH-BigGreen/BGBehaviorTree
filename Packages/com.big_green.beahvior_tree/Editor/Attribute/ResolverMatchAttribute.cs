//----------------------------------------
//author: BigGreen
//date: 2023-12-25 14:13
//----------------------------------------

using System;
using System.Collections.Generic;
using System.Reflection;
using Object = UnityEngine.Object;

namespace BG.BTEditor
{
    public enum ResolverType
    {
        Common,
        Enum,
        List,
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class ResolverMatchAttribute : Attribute
    {
        public ResolverType type = ResolverType.Common;
        public Type resolver;

        public bool IsMatch(Type fieldType)
        {
            if (type == ResolverType.Common)
            {
                return resolver == fieldType;
            }
            else if (type == ResolverType.Enum)
            {
                return fieldType.IsEnum;
            }
            else if (type == ResolverType.List)
            {
                return FieldResolverFactory.Instance.IsList(fieldType);
            }
            
            return false;
        }
    }
}