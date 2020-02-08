using System;
using System.Collections.Generic;
using System.Linq;
using AspNetCore.IQueryable.Extensions.Attributes;

namespace AspNetCore.IQueryable.Extensions.Filter
{

    internal class WhereFactory<TSearchModel>
        where TSearchModel : new()

    {
        private readonly TSearchModel _searchModel;

        public WhereFactory(TSearchModel searchModel)
        {
            _searchModel = searchModel;
        }

        public List<WhereClause> GetCriterias()
        {
            var type = _searchModel.GetType();
            var criterias = new List<WhereClause>();
            // Iterate through all the methods of the class.
            foreach (var propertyInfo in type.GetProperties())
            {
                if (Convert.GetTypeCode(propertyInfo.GetValue(_searchModel, null)) == TypeCode.Object)
                    continue;

                var criteria = new WhereClause();

                var attr = Attribute.GetCustomAttributes(propertyInfo).FirstOrDefault();
                // Check for the AnimalType attribute.
                if (attr?.GetType() == typeof(RestAttribute))
                {
                    var data = (RestAttribute)attr;
                    criteria.UpdateAttributeData(data);
                }

                var customValue = propertyInfo.GetValue(_searchModel, null);
                if (customValue == null)
                    continue;

                criteria.UpdateValues(propertyInfo);
                criterias.Add(criteria);
            }

            return criterias;
        }
    }
}
