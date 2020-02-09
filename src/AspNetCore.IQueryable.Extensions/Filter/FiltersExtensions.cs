using System;
using System.Linq;
using System.Linq.Expressions;

namespace AspNetCore.IQueryable.Extensions.Filter
{
    public static class FiltersExtensions
    {
        static FiltersExtensions()
        {

        }
        public static IQueryable<TEntity> Filter<TEntity, TSearch>(
            this IQueryable<TEntity> result, TSearch model) where TSearch : new()
        {
            if (model == null)
            {
                return result;
            }

            var criterias = WhereFactory.GetCriterias(model);

            Expression outerExpression = null;
            var parameterExpression = Expression.Parameter(typeof(TEntity), "model");
            foreach (var criteria in criterias)
            {
                if (!typeof(TEntity).HasProperty(criteria.FieldName) && !criteria.FieldName.Contains("."))
                    continue;

                dynamic propertyValue = parameterExpression;
                foreach (var part in criteria.FieldName.Split('.'))
                {
                    propertyValue = Expression.PropertyOrField(propertyValue, part);

                }

                var filterValue = GetClosureOverConstant(criteria.Property.GetValue(model, null), criteria.Property.PropertyType);


                if (!criteria.CaseSensitive)
                {
                    propertyValue = Expression.Call(propertyValue,
                        typeof(string).GetMethods()
                            .First(m => m.Name == "ToUpper" && m.GetParameters().Length == 0));

                    filterValue = Expression.Call(filterValue,
                        typeof(string).GetMethods()
                            .First(m => m.Name == "ToUpper" && m.GetParameters().Length == 0));
                }

                var expression = GetExpression(criteria, filterValue, propertyValue);

                if (criteria.UseNot)
                {
                    expression = Expression.Not(expression);
                }

                if (outerExpression == null)
                {
                    outerExpression = expression;
                }
                else
                {
                    if (criteria.UseOr)
                        outerExpression = Expression.Or(outerExpression, expression);
                    else
                        outerExpression = Expression.And(outerExpression, expression);
                }
            }

            return outerExpression == null
                ? result
                : result.Where(Expression.Lambda<Func<TEntity, bool>>(outerExpression, parameterExpression));
        }

        private static Expression GetExpression(WhereClause filterTerm, dynamic filterValue, Expression propertyValue)
        {
            switch (filterTerm.Operator)
            {
                case WhereOperator.Equals:
                    return Expression.Equal(propertyValue, filterValue);
                case WhereOperator.NotEquals:
                    return Expression.NotEqual(propertyValue, filterValue);
                case WhereOperator.GreaterThan:
                    return Expression.GreaterThan(propertyValue, filterValue);
                case WhereOperator.LessThan:
                    return Expression.LessThan(propertyValue, filterValue);
                case WhereOperator.GreaterThanOrEqualTo:
                    return Expression.GreaterThanOrEqual(propertyValue, filterValue);
                case WhereOperator.LessThanOrEqualTo:
                    return Expression.LessThanOrEqual(propertyValue, filterValue);
                case WhereOperator.Contains:
                    return Expression.Call(propertyValue,
                        typeof(string).GetMethods()
                            .First(m => m.Name == "Contains" && m.GetParameters().Length == 1),
                        filterValue);
                case WhereOperator.StartsWith:
                    return Expression.Call(propertyValue,
                        typeof(string).GetMethods()
                            .First(m => m.Name == "StartsWith" && m.GetParameters().Length == 1),
                        filterValue);
                default:
                    return Expression.Equal(propertyValue, filterValue);
            }
        }

        // Workaround to ensure that the filter value gets passed as a parameter in generated SQL from EF Core
        // See https://github.com/aspnet/EntityFrameworkCore/issues/3361
        // Expression.Constant passed the target type to allow Nullable comparison
        // See http://bradwilson.typepad.com/blog/2008/07/creating-nullab.html
        private static Expression GetClosureOverConstant<T>(T constant, Type targetType)
        {
            return Expression.Constant(constant, targetType);
        }
    }
}
