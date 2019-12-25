using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Vaccination.Infastructure.Utils
{
    public static class FindAttribute<T> where T : Attribute
    {
        public static T InProp<TProperty>(Expression<Func<TProperty>> property)
        {
            MemberExpression memberExpression = property.GetMemberExpression();
            MemberInfo memberInfo = memberExpression.Member;
            Type propertyOwnerType = memberExpression.Expression.Type;
            return memberInfo.TryGetAttribute<T>(propertyOwnerType);
        }

        public static T InEnum<EnumT>(EnumT enumVal) where EnumT : Enum
        {
            Type type = typeof(EnumT);
            MemberInfo[] memberInfos = type.GetMember(enumVal.ToString());
            var attributes = memberInfos[0].GetCustomAttributes(typeof(T), false);
            return (attributes.Length > 0) ? (T)attributes[0] : null;
        }
    }

    public static class AttributeHelper
    {
        public static TAttribute TryGetAttribute<TAttribute>(this MemberInfo memberInfo, Type classType) where TAttribute : Attribute
        {
            TAttribute attr = (TAttribute)memberInfo.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault();
            return attr;
        }
    }
}
