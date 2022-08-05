using Simego.DataSync.Providers.Avangate.Interfaces;
using Simego.DataSync.Providers.Avangate.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simego.DataSync.Providers.Avangate.Datasources
{
    public class AvangatePromotionDatasource : IAvangateDatasourceInfo
    {
        public string AvangateEndpointUrl => $"https://api.avangate.com/rest/4.0/promotions/";

        public string Name => "Promotion";
        public string IdColumnName => "Code";
        public string DataCollectionKey => "items";
        public string GetAvangateItemEndpointUrl(string id) => $"https://api.avangate.com/rest/4.0/promotions/{id}/";

        public IDictionary<string, AvangateDataSchemaItem> GetAvangateDataSchema(HttpWebRequestHelper helper)
        {
            var result = new Dictionary<string, AvangateDataSchemaItem>();

            result["Code"] = new AvangateDataSchemaItem { Id = "Code", Name = "Code", FieldType = AvangateDataSchemaItemType.FieldString, Parser = new DefaultValueParser(), Key = true };
            result["Name"] = new AvangateDataSchemaItem { Id= "Name", Name = "Name", FieldType = AvangateDataSchemaItemType.FieldString, Parser = new DefaultValueParser() };
            result["Description"] = new AvangateDataSchemaItem { Id = "Description", Name = "Description", FieldType = AvangateDataSchemaItemType.FieldString, Parser = new DefaultValueParser() };
            result["StartDate"] = new AvangateDataSchemaItem { Id = "StartDate", Name = "StartDate", FieldType = AvangateDataSchemaItemType.FieldDateTime, Parser = new DefaultValueParser() };
            result["EndDate"] = new AvangateDataSchemaItem { Id = "EndDate", Name = "EndDate", FieldType = AvangateDataSchemaItemType.FieldDateTime, Parser = new DefaultValueParser() };
            result["MaximumOrdersNumber"] = new AvangateDataSchemaItem { Id = "MaximumOrdersNumber", Name = "MaximumOrdersNumber", FieldType = AvangateDataSchemaItemType.FieldInteger, Parser = new DefaultValueParser() };
            result["MaximumQuantity"] = new AvangateDataSchemaItem { Id = "MaximumQuantity", Name = "MaximumQuantity", FieldType = AvangateDataSchemaItemType.FieldInteger, Parser = new DefaultValueParser() };
            result["InstantDiscount"] = new AvangateDataSchemaItem { Id = "InstantDiscount", Name = "InstantDiscount", FieldType = AvangateDataSchemaItemType.FieldBoolean, Parser = new DefaultValueParser() };
            result["CouponType"] = new AvangateDataSchemaItem { Id = "CouponType", ObjectName = "Coupon", Name = "Type", FieldType = AvangateDataSchemaItemType.FieldString, Parser = new DefaultValueParser() };
            result["CouponCode"] = new AvangateDataSchemaItem { Id = "CouponCode", ObjectName = "Coupon", Name = "Code", FieldType = AvangateDataSchemaItemType.FieldString, Parser = new DefaultValueParser() };
            result["DiscountLabel"] = new AvangateDataSchemaItem { Id = "DiscountLabel", Name = "DiscountLabel", FieldType = AvangateDataSchemaItemType.FieldString, Parser = new DefaultValueParser() };
            result["Enabled"] = new AvangateDataSchemaItem { Id = "Enabled", Name = "Enabled", FieldType = AvangateDataSchemaItemType.FieldBoolean, Parser = new DefaultValueParser() };
            result["CouponCodes"] = new AvangateDataSchemaItem { Id = "CouponCodes", Name = "CouponCodes", FieldType = AvangateDataSchemaItemType.FieldString, Parser = new DefaultValueParser() };
            result["ChannelType"] = new AvangateDataSchemaItem { Id = "ChannelType", Name = "ChannelType", FieldType = AvangateDataSchemaItemType.FieldString, Parser = new DefaultValueParser() };
            result["Discount"] = new AvangateDataSchemaItem { Id = "Discount", Name = "Discount", FieldType = AvangateDataSchemaItemType.FieldJsonToken, Parser = new DefaultValueParser() };
            result["DiscountType"] = new AvangateDataSchemaItem { Id = "DiscountType", ObjectName = "Discount", Name = "Type", FieldType = AvangateDataSchemaItemType.FieldString, Parser = new DefaultValueParser() };
            result["DiscountValue"] = new AvangateDataSchemaItem { Id = "DiscountValue", ObjectName = "Discount", Name = "Value", FieldType = AvangateDataSchemaItemType.FieldString, Parser = new DefaultValueParser() };
            result["Type"] = new AvangateDataSchemaItem { Id = "Type", Name = "Type", FieldType = AvangateDataSchemaItemType.FieldString, Parser = new DefaultValueParser() };
            result["Products"] = new AvangateDataSchemaItem { Id = "Products", Name = "Products", FieldType = AvangateDataSchemaItemType.FieldJsonToken, Parser = new DefaultValueParser() };
            
            return result;
        }
    }
}