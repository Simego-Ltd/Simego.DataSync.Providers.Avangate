using Simego.DataSync.Providers.Avangate.Interfaces;
using Simego.DataSync.Providers.Avangate.Parsers;
using System.Collections.Generic;

namespace Simego.DataSync.Providers.Avangate.Datasources
{
    public class AvangateSubscriptionDatasource : IAvangateDatasourceInfo
    {
        public string AvangateEndpointUrl => $"https://api.avangate.com/rest/4.0/subscriptions/";
        
        public string Name => "Subscription";
        public string IdColumnName => "SubscriptionReference";
        public string DataCollectionKey => null;
        public string GetAvangateItemEndpointUrl(string id) => $"https://api.avangate.com/rest/4.0/subscriptions/{id}/";

        public IDictionary<string, AvangateDataSchemaItem> GetAvangateDataSchema(HttpWebRequestHelper helper)
        {
            var result = new Dictionary<string, AvangateDataSchemaItem>();

            result["SubscriptionReference"] = new AvangateDataSchemaItem { Id = "SubscriptionReference", Name = "SubscriptionReference", FieldType = AvangateDataSchemaItemType.FieldString, Parser = new DefaultValueParser(), Key = true };
            result["StartDate"] = new AvangateDataSchemaItem { Id = "StartDate", Name = "StartDate", FieldType = AvangateDataSchemaItemType.FieldDateTime, Parser = new DefaultValueParser() };
            result["ExpirationDate"] = new AvangateDataSchemaItem { Id = "ExpirationDate", Name = "ExpirationDate", FieldType = AvangateDataSchemaItemType.FieldDateTime, Parser = new DefaultValueParser() };
            result["AvangateCustomerReference"] = new AvangateDataSchemaItem { Id = "AvangateCustomerReference", Name = "AvangateCustomerReference", FieldType = AvangateDataSchemaItemType.FieldString, Parser = new DefaultValueParser() };
            result["ExternalCustomerReference"] = new AvangateDataSchemaItem { Id = "ExternalCustomerReference", Name = "ExternalCustomerReference", FieldType = AvangateDataSchemaItemType.FieldString, Parser = new DefaultValueParser() };
            result["RecurringEnabled"] = new AvangateDataSchemaItem { Id = "RecurringEnabled", Name = "RecurringEnabled", FieldType = AvangateDataSchemaItemType.FieldBoolean, Parser = new DefaultValueParser() };
            result["SubscriptionEnabled"] = new AvangateDataSchemaItem { Id = "SubscriptionEnabled", Name = "SubscriptionEnabled", FieldType = AvangateDataSchemaItemType.FieldBoolean, Parser = new DefaultValueParser() };

            result["ProductCode"] = new AvangateDataSchemaItem { Id = "ProductCode", ObjectName = "Product", Name = "ProductCode", FieldType = AvangateDataSchemaItemType.FieldString, Parser = new DefaultValueParser() };
            result["ProductId"] = new AvangateDataSchemaItem { Id = "ProductId", ObjectName = "Product", Name = "ProductId", FieldType = AvangateDataSchemaItemType.FieldString, Parser = new DefaultValueParser() };
            result["ProductName"] = new AvangateDataSchemaItem { Id = "ProductName", ObjectName = "Product", Name = "ProductName", FieldType = AvangateDataSchemaItemType.FieldString, Parser = new DefaultValueParser() };
            result["ProductVersion"] = new AvangateDataSchemaItem { Id = "ProductVersion", ObjectName = "Product", Name = "ProductVersion", FieldType = AvangateDataSchemaItemType.FieldString, Parser = new DefaultValueParser() };
            result["ProductQuantity"] = new AvangateDataSchemaItem { Id = "ProductQuantity", ObjectName = "Product", Name = "ProductQuantity", FieldType = AvangateDataSchemaItemType.FieldInteger, Parser = new DefaultValueParser() };

            result["FirstName"] = new AvangateDataSchemaItem { Id = "FirstName", ObjectName = "EndUser", Name = "FirstName", FieldType = AvangateDataSchemaItemType.FieldString, Parser = new DefaultValueParser() };
            result["LastName"] = new AvangateDataSchemaItem { Id = "LastName", ObjectName = "EndUser", Name = "LastName", FieldType = AvangateDataSchemaItemType.FieldString, Parser = new DefaultValueParser() };
            result["Company"] = new AvangateDataSchemaItem { Id = "Company", ObjectName = "EndUser", Name = "Company", FieldType = AvangateDataSchemaItemType.FieldString, Parser = new DefaultValueParser() };
            result["Email"] = new AvangateDataSchemaItem { Id = "Email", ObjectName = "EndUser", Name = "Email", FieldType = AvangateDataSchemaItemType.FieldEmail, Parser = new DefaultValueParser() };
            result["Phone"] = new AvangateDataSchemaItem { Id = "Phone", ObjectName = "EndUser", Name = "Phone", FieldType = AvangateDataSchemaItemType.FieldString, Parser = new DefaultValueParser() };
            result["Fax"] = new AvangateDataSchemaItem { Id = "Fax", ObjectName = "EndUser", Name = "Fax", FieldType = AvangateDataSchemaItemType.FieldString, Parser = new DefaultValueParser() };
            result["Address1"] = new AvangateDataSchemaItem { Id = "Address1", ObjectName = "EndUser", Name = "Address1", FieldType = AvangateDataSchemaItemType.FieldString, Parser = new DefaultValueParser() };
            result["Address2"] = new AvangateDataSchemaItem { Id = "Address2", ObjectName = "EndUser", Name = "Address2", FieldType = AvangateDataSchemaItemType.FieldString, Parser = new DefaultValueParser() };
            result["Zip"] = new AvangateDataSchemaItem { Id = "Zip", ObjectName = "EndUser", Name = "Zip", FieldType = AvangateDataSchemaItemType.FieldString, Parser = new DefaultValueParser() };
            result["City"] = new AvangateDataSchemaItem { Id = "City", ObjectName = "EndUser", Name = "City", FieldType = AvangateDataSchemaItemType.FieldString, Parser = new DefaultValueParser() };
            result["State"] = new AvangateDataSchemaItem { Id = "State", ObjectName = "EndUser", Name = "State", FieldType = AvangateDataSchemaItemType.FieldString, Parser = new DefaultValueParser() };
            result["CountryCode"] = new AvangateDataSchemaItem { Id = "CountryCode", ObjectName = "EndUser", Name = "CountryCode", FieldType = AvangateDataSchemaItemType.FieldString, Parser = new DefaultValueParser() };
            result["Language"] = new AvangateDataSchemaItem { Id = "Language", ObjectName = "EndUser", Name = "Language", FieldType = AvangateDataSchemaItemType.FieldString, Parser = new DefaultValueParser() };

            result["SKU"] = new AvangateDataSchemaItem { Id = "SKU", Name = "SKU", FieldType = AvangateDataSchemaItemType.FieldString, Parser = new DefaultValueParser() };

            result["Codes"] = new AvangateDataSchemaItem { Id = "Codes", ObjectName = "DeliveryInfo", Name = "Codes", FieldType = AvangateDataSchemaItemType.FieldStringArray, Parser = new CodeValueParser() };

            result["ReceiveNotifications"] = new AvangateDataSchemaItem { Id = "ReceiveNotifications", Name = "ReceiveNotifications", FieldType = AvangateDataSchemaItemType.FieldBoolean, Parser = new DefaultValueParser() };
            result["Lifetime"] = new AvangateDataSchemaItem { Id = "Lifetime", Name = "Lifetime", FieldType = AvangateDataSchemaItemType.FieldBoolean, Parser = new DefaultValueParser() };
            result["PartnerCode"] = new AvangateDataSchemaItem { Id = "PartnerCode", Name = "PartnerCode", FieldType = AvangateDataSchemaItemType.FieldString, Parser = new DefaultValueParser() };
            result["TestSubscription"] = new AvangateDataSchemaItem { Id = "TestSubscription", Name = "TestSubscription", FieldType = AvangateDataSchemaItemType.FieldBoolean, Parser = new DefaultValueParser() };
            result["IsTrial"] = new AvangateDataSchemaItem { Id = "IsTrial", Name = "IsTrial", FieldType = AvangateDataSchemaItemType.FieldBoolean, Parser = new DefaultValueParser() };

            return result;
        }                   
    }
}
