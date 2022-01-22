using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Project.App.Validations
{
    public class GenderValidation : ValidationAttribute
    {
        private readonly string _errorMessage;
        private readonly string[] _genderAllows;
        public GenderValidation(string errorMessage, string[] genderAllows)
        {
            _errorMessage = errorMessage;
            _genderAllows = genderAllows;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string gender = value as string;
            if (string.IsNullOrEmpty(gender))
            {
                return ValidationResult.Success;
            }
            if (!_genderAllows.Contains(gender))
            {
                return new ValidationResult(_errorMessage);
            }
            return ValidationResult.Success;
        }
    }
}
