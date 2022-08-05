using Newtonsoft.Json.Linq;
using Simego.DataSync.Providers.Avangate.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Simego.DataSync.Providers.Avangate.Parsers
{
    public class CodeValueParser : IAvangateValueParser
    {
        public object ConvertValue(object value)
        {
            return value;
        }

        public object ParseValue(JToken token)
        {
            var array = token as JArray;
            if (array != null)
            {
                var codes = new List<string>();
                foreach (var val in array)
                {
                    var value = val["Code"]?.ToObject<string>();
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        codes.Add(value);
                    }
                }
                if (codes.Any())
                {
                    return codes.ToArray();
                }
            }

            return null;
        }
    }
}
