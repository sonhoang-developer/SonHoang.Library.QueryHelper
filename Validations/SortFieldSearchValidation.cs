using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project.App.Validations
{
    public class SortFieldSearchValidation : ValidationAttribute
    {
        public SortFieldSearchValidation(Type typeT)
        {

        }
        /*protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string sortField = value as string;

        }*/
    }
}
