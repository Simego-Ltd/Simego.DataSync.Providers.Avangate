using System.Collections.Generic;

namespace Simego.DataSync.Providers.Avangate.Interfaces
{
    public interface IAvangateDatasourceInfo
    {
        IDictionary<string, AvangateDataSchemaItem> GetAvangateDataSchema(HttpWebRequestHelper helper);
        string AvangateEndpointUrl { get; }
        string GetAvangateItemEndpointUrl(string id);
        string Name { get; }
        string IdColumnName { get; }
        string DataCollectionKey { get; }
    }
}
