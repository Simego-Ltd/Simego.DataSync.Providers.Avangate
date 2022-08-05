using Newtonsoft.Json.Linq;
using Simego.DataSync.Providers.Avangate.Interfaces;

namespace Simego.DataSync.Providers.Avangate.Parsers
{
    public class DefaultValueParser : IAvangateValueParser
    {        
        public object ConvertValue(object value)
        {
            return value;
        }

        public object ParseValue(JToken token)
        {
            return token?.ToObject<object>();
        }
    }
}
