using AspNetCore.IQueryable.Extensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace AspNetCore.IQueryable.Extensions.Filter
{
    internal static class ExpressionFactory
    {
        internal static ExpressionParserCollection GetOperators<TEntity>(ICustomQueryable model)
        {
            var expressions = new ExpressionParserCollection();

            var type = model.GetType();
            expressions.ParameterExpression = Expression.Parameter(typeof(TEntity), "model");

            foreach (var propertyInfo in type.GetProperties())
            {
                var criteria = GetCriteria(model, propertyInfo);
                if (criteria == null)
                    continue;

                if (!typeof(TEntity).HasProperty(criteria.FieldName) && !criteria.FieldName.Contains("."))
                    continue;

                dynamic propertyValue = expressions.ParameterExpression;

                foreach (var part in criteria.FieldName.Split('.'))
                {
                    propertyValue = Expression.PropertyOrField(propertyValue, part);
                }

                var expressionData = new ExpressionParser();
                expressionData.FieldToFilter = propertyValue;
                expressionData.FilterBy = GetClosureOverConstant(criteria.Property.GetValue(model, null), GetNonNullable(criteria.Property.PropertyType));
                expressionData.Criteria = criteria;

                if (criteria.Property.GetValue(model, null) != null)
                    expressions.Add(expressionData);
            }

            return expressions;
        }
        private static Type GetNonNullable(Type propertyType)
        {
            if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return Nullable.GetUnderlyingType(propertyType);
            }

            return propertyType;
        }
        static bool IsNullableType(Type t)
        {
            return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
        internal static WhereClause GetCriteria(ICustomQueryable model, PropertyInfo propertyInfo)
        {
            bool isCollection = propertyInfo.IsPropertyACollection();
            //if (!isCollection && propertyInfo.IsPropertyObject(model))
            //    return null;

            var criteria = new WhereClause();

            var attr = Attribute.GetCustomAttributes(propertyInfo);
            // Check for the AnimalType attribute.
            if (attr.Any(a => a.GetType() == typeof(QueryOperatorAttribute)))
            {
                var data = (QueryOperatorAttribute)attr.First(a => a.GetType() == typeof(QueryOperatorAttribute));
                criteria.UpdateAttributeData(data);
                if (data.Operator != WhereOperator.Contains && isCollection)
                    throw new ArgumentException($"{propertyInfo.Name} - For array the only Operator available is Contains");
            }

            if (isCollection)
                criteria.Operator = WhereOperator.Contains;

            var customValue = propertyInfo.GetValue(model, null);
            if (customValue == null)
                return null;

            criteria.UpdateValues(propertyInfo);
            return criteria;
        }

        // Workaround to ensure that the filter value gets passed as a parameter in generated SQL from EF Core
        // See https://github.com/aspnet/EntityFrameworkCore/issues/3361
        // Expression.Constant passed the target type to allow Nullable comparison
        // See http://bradwilson.typepad.com/blog/2008/07/creating-nullab.html
        internal static Expression GetClosureOverConstant<T>(T constant, Type targetType)
        {
            return Expression.Constant(constant, targetType);
        }


        internal static List<WhereClause> GetCriterias(ICustomQueryable searchModel)
        {
            var type = searchModel.GetType();
            var criterias = new List<WhereClause>();
            // Iterate through all the methods of the class.
            foreach (var propertyInfo in type.GetProperties())
            {
                bool isCollection = propertyInfo.IsPropertyACollection();
                if (!isCollection && propertyInfo.IsPropertyObject(searchModel))
                    continue;

                var criteria = new WhereClause();

                var attr = Attribute.GetCustomAttributes(propertyInfo).FirstOrDefault();
                // Check for the AnimalType attribute.
                if (attr?.GetType() == typeof(QueryOperatorAttribute))
                {
                    var data = (QueryOperatorAttribute)attr;
                    criteria.UpdateAttributeData(data);
                    if (data.Operator != WhereOperator.Contains && isCollection)
                        throw new ArgumentException($"{propertyInfo.Name} - For array the only Operator available is Contains");
                }

                if (isCollection)
                    criteria.Operator = WhereOperator.Contains;

                var customValue = propertyInfo.GetValue(searchModel, null);
                if (customValue == null)
                    continue;

                criteria.UpdateValues(propertyInfo);
                criterias.Add(criteria);
            }

            return criterias.OrderBy(o => o.UseOr).ToList();
        }
    }
}
