using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Project.App.Validations
{
    public class AllowedFileExtensions : ValidationAttribute
    {
        private readonly string[] _extensions;
        private readonly string _errorMessage;
        public AllowedFileExtensions(string[] extensions, string errorMessage)
        {
            _extensions = extensions;
            _errorMessage = errorMessage;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            IFormFile? file = value as IFormFile;
            
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);
                if (!_extensions.Contains(extension.ToLower()))
                {
                    return new ValidationResult(_errorMessage);
                }
            }
            return ValidationResult.Success;
        }
    }
}
