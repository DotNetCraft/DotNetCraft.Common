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

        private static readonly MethodInfo OrderByMethod = typeof(Queryable).GetMethods()
            .Where(method => method.Name == "OrderBy")
            .Where(method => method.GetParameters().Length == 2)
            .Single();

        public static IQueryable<TSource> OrderByProperty<TSource>(this IQueryable<TSource> query, string propertyName)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(TSource), "posting");
            Expression orderByProperty = Expression.Property(parameter, propertyName);

            LambdaExpression lambda = Expression.Lambda(orderByProperty, new[] { parameter });
            MethodInfo genericMethod = OrderByMethod.MakeGenericMethod(new[] { typeof(TSource), orderByProperty.Type });
            object ret = genericMethod.Invoke(null, new object[] { query, lambda });
            return (IQueryable<TSource>)ret;
        }
    }
}
