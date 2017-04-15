using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DotNetCraft.Common.DataAccessLayer.Extentions
{
    public static class DotNetCraftQueryableExtentions
    {
        public static IQueryable<T> Simplified<T>(this IQueryable<T> query, string propertyName, object propertyValue)
        {
            PropertyInfo propertyInfo = typeof(T).GetProperty(propertyName);
            return Simplified<T>(query, propertyInfo, propertyValue);
        }

        public static IQueryable<T> Simplified<T>(this IQueryable<T> query, PropertyInfo propertyInfo, object propertyValue)
        {
            ParameterExpression e = Expression.Parameter(typeof(T), "e");
            MemberExpression m = Expression.MakeMemberAccess(e, propertyInfo);
            ConstantExpression c = Expression.Constant(propertyValue, propertyValue.GetType());
            BinaryExpression b = Expression.Equal(m, c);

            Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(b, e);
            return query.Where(lambda);
        }
    }
}
