using AspNetCore.IQueryable.Extensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AspNetCore.IQueryable.Extensions.Filter
{

    internal static class WhereFactory
    {
        internal static List<WhereClause> GetCriterias(ICustomQueryable searchModel)
        {
            var type = searchModel.GetType();
            var criterias = new List<WhereClause>();
            // Iterate through all the methods of the class.
            foreach (var propertyInfo in type.GetProperties())
            {
                if (Convert.GetTypeCode(propertyInfo.GetValue(searchModel, null)) == TypeCode.Object)
                    continue;

                var criteria = new WhereClause();

                var attr = Attribute.GetCustomAttributes(propertyInfo).FirstOrDefault();
                // Check for the AnimalType attribute.
                if (attr?.GetType() == typeof(QueryOperatorAttribute))
                {
                    var data = (QueryOperatorAttribute)attr;
                    criteria.UpdateAttributeData(data);
                }

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
