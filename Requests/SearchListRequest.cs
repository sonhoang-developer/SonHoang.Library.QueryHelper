using Microsoft.Data.SqlClient;
using Project.App.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SonHoang.Library.Helpers
{
    public class SearchListRequest<T> where T : class
    {
        [Range(1, int.MaxValue, ErrorMessage = "Limit must be a positive number")]
        public int Limit { get; set; } = 1; // Number of items per page
        [Range(0, int.MaxValue, ErrorMessage = "Page must be a positive number")]
        public int Page { get; set; } = 1; // Page number
        public List<SortFieldOrderBy>? SortFields { get; set; } // Field to sort
        //public List<SearchValue>? SearchValue { get; set; } // Value to search list
        public List<string>? SelectFields { get; set; } // Selected Fields
        
        public string? ToStringSortFields()
        {
            string result = null;
            if(SortFields is not null)
            {
                SortFields.ForEach(s =>
                {
                    if(result is null)
                    {
                        result = s.ToString();
                    }
                    else
                    {
                        result = $"{result},{s.ToString()}";
                    }
                });
            }
            return result;
        }

    }
    public class SortFieldOrderBy
    {
        public string? SortField { get; set; } // Field sort
        public SortOrder SortOrder { get; set; } = SortOrder.Ascending; //Ascending or decending
        public override string ToString() => $"{SortField} {SortOrder}";
    }
    public class SearchValue
    {
        public List<string> Fields { get; set; }
        public List<ValueCompare> Compares { get; set; }
        public List<ValueCompare> ComparesOr { get; set; }
    }
    public class ValueCompare
    {
        public string Value { get; set; }
        public string Compare { get; set; }
    }
    public enum CompareType
    {
        Equals,
        Contains,


    }
}
