using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using SonHoang.Library.Requests;
using SonHoang.Library.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace SonHoang.Library.Helpers
{
    public static class QueryHelper
    {
        public static IQueryable SelectQueryData<T>(this IQueryable<T> queryData, List<string> selectFields)
        {
            if(selectFields is null || selectFields.Count == 0)
            {
                selectFields = typeof(T).GetAllPropertiesName();
            }
            IQueryable selectQuery = queryData.Select(@"new {" + selectFields.ToStringJoin(",") + "}");
            return selectQuery;
        }
        public static IQueryable OrderByQueryData(this IQueryable queryData, List<SortFieldOrderBy> sortFields)
        {
            string sortFieldOrder = null;
            sortFields.ForEach(sf =>
            {
                if(sortFieldOrder is null)
                {
                    sortFieldOrder = sf.ToString();
                }
                else
                {
                    sortFieldOrder = $"{sortFieldOrder},{sf.ToString()}";
                }
            });
            return queryData.OrderBy(sortFieldOrder);
        }
        public static IQueryable OrderByQueryData(this IQueryable queryData, string sortField, string sortOrder)
        {
            queryData = queryData.OrderBy($"{sortField} {sortOrder}");
            return queryData;
        }
        public static IQueryable<T> OrderByQueryData<T>(this IQueryable<T> queryData, string sortField, string sortOrder) where T : class
        {
            queryData = queryData.OrderBy($"{sortField} {sortOrder}");
            return queryData;
        }
        public static SearchListResponse GetSearchResponseQueryData(this IQueryable queryData, int page = 0, int limit = 1)
        {
            return new SearchListResponse
            {
                Data = page != 0 ? queryData.Skip((page - 1) * limit).Take(limit).ToDynamicList() : queryData.ToDynamicList(),
                Info = new Info
                {
                    Page = page != 0 ? page : 1,
                    Limit = page != 0 ? limit : queryData.Count(),
                    TotalRecord = queryData.Count(),
                }
            };
        }
        public static async Task<SearchListResponse> GetSearchResponseQueryDataAsync(this IQueryable queryData, int page = 0, int limit = 1)
        {
            return new SearchListResponse
            {
                Data = page != 0 ? (await queryData.Skip((page - 1) * limit).Take(limit).ToDynamicListAsync()) : (await queryData.ToDynamicListAsync()),
                Info = new Info
                {
                    Page = page != 0 ? page : 1,
                    Limit = page != 0 ? limit : queryData.Count(),
                    TotalRecord = queryData.Count(),
                }
            };
        }
        public static dynamic SelectFields(this object data, List<string> selectFields)
        {
            //List<string> selectFields = selectFields.Split(",").ToList();

            JObject jobjectData = JObject.FromObject(data, new JsonSerializer()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            });
            JObject jobjectResult = new();
            selectFields.ForEach(field =>
            {
                field = field.Trim().LowerCaseFirstLetter();
                if (field.Contains(" as "))
                {
                    string fieldBeforeAs = field.Split(" as ")[0];
                    string fieldAfterAs = field.Split(" as ")[1].Trim().LowerCaseFirstLetter();
                    JToken value = null;
                    if (fieldBeforeAs.Contains('.'))
                    {
                        List<string> props = fieldBeforeAs.Split(".").ToList();
                        props.ForEach(prop =>
                        {
                            prop = prop.Trim().LowerCaseFirstLetter();
                            if (value is null)
                            {
                                value = jobjectData[prop];
                            }
                            else
                            {
                                value = value[prop];
                            }
                        });
                    }
                    else
                    {
                        value = jobjectData[fieldBeforeAs];
                    }
                    jobjectResult[fieldAfterAs] = value;
                }
                else
                {
                    JToken value = null;
                    if (field.Contains('.'))
                    {
                        List<string> props = field.Split(".").ToList();
                        string propLast = null;
                        props.ForEach(prop =>
                        {
                            prop = prop.Trim().LowerCaseFirstLetter();
                            if (value is null)
                            {
                                value = jobjectData[prop];
                            }
                            else
                            {
                                value = value[prop];
                            }
                            propLast = prop;
                        });
                        jobjectResult[propLast] = value;
                    }
                    else
                    {
                        jobjectResult[field] = jobjectData[field];
                    }
                }
            });
            dynamic result = jobjectResult.ToObject<dynamic>();
            return result;
        }
        public static dynamic SelectFields<T>(this object data, GetDetailsRequest<T> getDetailsRequest) where T : class
        {
            List<string> selectFields;
            if (getDetailsRequest is null || getDetailsRequest.SelectFields is null || getDetailsRequest.SelectFields.Count == 0)
            {
                selectFields = typeof(T).GetAllPropertiesName();
            }
            else
            {
                selectFields = getDetailsRequest.SelectFields;
            }
            JObject jobjectData = JObject.FromObject(data, new JsonSerializer()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            });
            JObject jobjectResult = new();
            selectFields.ForEach(field =>
            {
                field = field.Trim().LowerCaseFirstLetter();
                if (field.Contains(" as "))
                {
                    string fieldBeforeAs = field.Split(" as ")[0];
                    string fieldAfterAs = field.Split(" as ")[1].Trim().LowerCaseFirstLetter();
                    JToken value = null;
                    if (fieldBeforeAs.Contains('.'))
                    {
                        List<string> props = fieldBeforeAs.Split(".").ToList();
                        props.ForEach(prop =>
                        {
                            prop = prop.Trim().LowerCaseFirstLetter();
                            if (value is null)
                            {
                                value = jobjectData[prop];
                            }
                            else
                            {
                                value = value[prop];
                            }
                        });
                    }
                    else
                    {
                        value = jobjectData[fieldBeforeAs];
                    }
                    jobjectResult[fieldAfterAs] = value;
                }
                else
                {
                    JToken value = null;
                    if (field.Contains('.'))
                    {
                        List<string> props = field.Split(".").ToList();
                        string propLast = null;
                        props.ForEach(prop =>
                        {
                            prop = prop.Trim().LowerCaseFirstLetter();
                            if (value is null)
                            {
                                value = jobjectData[prop];
                            }
                            else
                            {
                                value = value[prop];
                            }
                            propLast = prop;
                        });
                        jobjectResult[propLast] = value;
                    }
                    else
                    {
                        jobjectResult[field] = jobjectData[field];
                    }
                }
            });
            dynamic result = jobjectResult.ToObject<dynamic>();
            return result;
        }


    }
}
