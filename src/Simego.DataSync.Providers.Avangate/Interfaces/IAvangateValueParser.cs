using Newtonsoft.Json.Linq;

namespace Simego.DataSync.Providers.Avangate.Interfaces
{
    public interface IAvangateValueParser
    {
        object ParseValue(JToken token);
        object ConvertValue(object value);
    }
}
