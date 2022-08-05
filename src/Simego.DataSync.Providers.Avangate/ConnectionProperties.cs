using Simego.DataSync.Providers.Avangate.TypeConverters;
using System.Collections.Generic;
using System.ComponentModel;

namespace Simego.DataSync.Providers.Avangate
{
    class ConnectionProperties
    {
        private readonly AvangateDatasourceReader _reader;
        
        [Category("Settings")]
        public string VendorCode { get { return _reader.VendorCode; } set { _reader.VendorCode = value; } }

        [Category("Settings")]
        public string VendorSecret { get { return _reader.VendorSecret; } set { _reader.VendorSecret = value; } }

        [Category("Settings")]
        [TypeConverter(typeof(AvangateListTypeConverter))]
        public string List
        {
            get { return _reader.List; }
            set { _reader.List = value; }
        }
        public ConnectionProperties(AvangateDatasourceReader reader)
        {
            _reader = reader;
        }

        public IList<string> GetObjectNames()
        {
            return _reader.GetObjectNames();
        }
    }
}
