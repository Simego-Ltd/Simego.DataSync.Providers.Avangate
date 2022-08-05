using Simego.DataSync.Providers.Avangate.Interfaces;
using Simego.DataSync.Providers.Avangate.Parsers;
using System;

namespace Simego.DataSync.Providers.Avangate
{
    public enum AvangateDataSchemaItemType
    {
        FieldInteger,           
        FieldString,            
        FieldStringArray,       
        FieldText,              
        FieldDouble,            
        FieldDecimal,          
        FieldDateTime,          
        FieldEmail,             
        FieldBoolean,
        FieldJsonToken,
    }
    public class AvangateDataSchemaItem
    {
        public bool Key { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string ObjectName { get; set; }
        public AvangateDataSchemaItemType FieldType { get; set; }
        public IAvangateValueParser Parser { get; set; } = new DefaultValueParser();
        public bool ReadOnly { get; set; }
        
        public DataSchemaItem ToDataSchemaItem()
        {
            switch (FieldType)
            {
                case AvangateDataSchemaItemType.FieldInteger:
                    {
                        return new DataSchemaItem(Id, typeof(int), Key, ReadOnly, true, -1);
                    }
                case AvangateDataSchemaItemType.FieldDateTime:
                    {
                        return new DataSchemaItem(Id, typeof(DateTime), Key, ReadOnly, true, -1);
                    }
                case AvangateDataSchemaItemType.FieldStringArray:
                    {
                        return new DataSchemaItem(Id, typeof(string[]), Key, ReadOnly, true, -1);
                    }
                case AvangateDataSchemaItemType.FieldBoolean:
                    {
                        return new DataSchemaItem(Id, typeof(bool), Key, ReadOnly, true, -1);
                    }
                case AvangateDataSchemaItemType.FieldDouble:
                    {
                        return new DataSchemaItem(Id, typeof(double), Key, ReadOnly, true, -1);
                    }
                case AvangateDataSchemaItemType.FieldDecimal:
                    {
                        return new DataSchemaItem(Id, typeof(decimal), Key, ReadOnly, true, -1);
                    }
                case AvangateDataSchemaItemType.FieldJsonToken:
                    {
                        return new DataSchemaItem(Id, typeof(Newtonsoft.Json.Linq.JToken), Key, ReadOnly, true, -1);
                    }
                default:
                    return new DataSchemaItem(Id, typeof(string), Key, ReadOnly, true, -1);

            }
        }
    }
}
