//----------------------------------------
//author: BigGreen
//date: 2023-12-25 15:18
//----------------------------------------

using System;
using System.Collections.Generic;
using System.Reflection;
using BG.BT;

namespace BG.BTEditor
{
    [ResolverMatch(type = ResolverType.List)]
    public class ListResolver<T> : FieldResolver<ListField<T>, List<T>>, IListResolver
    {
        private IFieldResolver m_SubFieldResolver;
        
        public object DataSource { get; set; }

        public ListResolver(BehaviorTreeAsset asset, Type fieldType, MemberInfo info) : base(asset, fieldType, info)
        {
            m_SubFieldResolver = FieldResolverFactory.Instance.GetResolver(asset, typeof(T), info);
        }

        public override ListField<T> CreateEditorField()
        {
            return new ListField<T>("", null, m_TreeAsset, (List<T>)DataSource, m_SubFieldResolver);
        }
    }
}