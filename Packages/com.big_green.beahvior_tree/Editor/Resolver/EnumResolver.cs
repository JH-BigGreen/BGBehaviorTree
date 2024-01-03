//----------------------------------------
//author: BigGreen
//date: 2023-12-25 14:59
//----------------------------------------

using System;
using System.Linq;
using System.Reflection;
using BG.BT;

namespace BG.BTEditor
{
    [ResolverMatch(type = ResolverType.Enum)]
    public class EnumResolver : FieldResolver<EnumField, Enum>
    {
        public EnumResolver(BehaviorTreeAsset asset, Type fieldType, MemberInfo info) : base(asset, fieldType, info)
        {
        }

        public override EnumField CreateEditorField()
        {
            if (!m_FieldType.IsEnum)
            {
                return null;
            }

            var enums = Enum.GetValues(m_FieldType).Cast<Enum>().Select(v => v).ToList();
            return new EnumField("", enums, enums[0]);
        }
    }
}