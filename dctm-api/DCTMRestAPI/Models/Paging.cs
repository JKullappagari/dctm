using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DCTMRestAPI.Models
{
    // Bound from the query string, so it needs settable properties.
    public class PagingParams
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public record LinkInfo
    {
        public string Href { get; init; }
        public string Rel { get; init; }
        public string Method { get; init; }
    }

    public record PagingHeader(int TotalItems, int PageNumber, int PageSize, int TotalPages)
    {
        public string ToJson() => JsonConvert.SerializeObject(
            this,
            new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
    }

    public class AssetOutputModel
    {
        public PagingHeader Paging { get; set; }
        public List<LinkInfo> Links { get; set; }
        public List<TblAsset> Items { get; set; }
    }
}
