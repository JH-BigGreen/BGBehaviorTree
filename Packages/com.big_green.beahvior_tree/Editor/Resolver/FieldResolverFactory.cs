//----------------------------------------
//author: BigGreen
//date: 2023-12-25 16:16
//----------------------------------------

using System;
using System.Collections.Generic;
using System.Reflection;
using BG.BT;

namespace BG.BTEditor
{
    public class FieldResolverFactory
    {
        private static FieldResolverFactory m_Instance;

        public static FieldResolverFactory Instance
        {
            get
            {
                if (m_Instance==null)
                {
                    m_Instance = new FieldResolverFactory();
                }

                return m_Instance;
            }
        }

        private List<Type> m_ResolverTypeList;

        private FieldResolverFactory()
        {
            m_ResolverTypeList = ReflectionUtility.GetTypesByAttribute<ResolverMatchAttribute>(GetType().Assembly);
        }

        public IFieldResolver GetResolver(BehaviorTreeAsset asset, Type fieldType, MemberInfo info)
        {
            Type type = null;
            var isList = false;
            foreach (var item in m_ResolverTypeList)
            {
                var attr = item.GetCustomAttribute<ResolverMatchAttribute>();
                if (attr.IsMatch(fieldType))
                {
                    type = item;
                    isList = attr.type == ResolverType.List;
                    break;
                }
            }

            if (type == null)
            {
                return null;
            }

            if (isList)
            {
                var genericArg = fieldType.GenericTypeArguments[0];
                type = type.MakeGenericType(genericArg);
            }
            
            var resolver = (IFieldResolver) Activator.CreateInstance(type, new object[]{asset, fieldType, info});
            return resolver;
        }

        public bool IsList(Type type)
        {
            return type.IsGenericType &&
                   type.GetGenericTypeDefinition() == typeof(List<>);
        }
    }
}