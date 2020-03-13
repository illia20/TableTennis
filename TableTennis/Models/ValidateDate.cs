using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TableTennis.Models
{
    public class ValidateDate : Attribute, IModelValidator
    {
        public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
        {
            if (Convert.ToDateTime(context.Model) > DateTime.Now)
                return new List<ModelValidationResult> {
                new ModelValidationResult("", "Incorrect date.")
            };
            else
            {
                return Enumerable.Empty<ModelValidationResult>();
            }
        }
    }
}
