using Newtonsoft.Json.Linq;
using Simego.DataSync.Interfaces;
using Simego.DataSync.Providers.Avangate.Datasources;
using Simego.DataSync.Providers.Avangate.Interfaces;
using Simego.DataSync.Providers.Avangate.TypeConverters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Simego.DataSync.Providers.Avangate
{
    [ProviderInfo(Name = "Avangate/2Checkout Datasource", Group ="Avangate", Description = "Avangate/2Checkout REST v4 Datasource")]
    public class AvangateDatasourceReader : DataReaderProviderBase, IDataSourceSetup
    {
        private HttpWebRequestHelper RequestHelper = new HttpWebRequestHelper();
        private IAvangateDatasourceInfo DataSourceInfo { get; set; } = AvangateDatasourceFactory.GetDatasourceInfo("subscription");

        private ConnectionInterface _connectionIf;

        [Category("Settings")]
        [TypeConverter(typeof(AvangateListTypeConverter))]
        public string List
        {
            get { return DataSourceInfo.Name; }
            set { DataSourceInfo = AvangateDatasourceFactory.GetDatasourceInfo(value); }
        }

        [Category("Settings")]
        public string VendorCode { get; set; }

        [Category("Settings")]
        public string AppendQueryString { get; set; }

        [Browsable(false)]
        public string VendorSecret { get; set; }

        [Category("Settings")]
        [Description("The number or records to return per request.")]
        public int PageSize { get; set; } = 100;

        [Description("Enable HTTP Request Tracing")]
        [Category("Debug")]
        public bool TraceEnabled { get { return RequestHelper.TraceEnabled; } set { RequestHelper.TraceEnabled = true; } }

        public override DataTableStore GetDataTable(DataTableStore dt)
        {
            dt.AddIdentifierColumn(typeof(string));

            var mapping = new DataSchemaMapping(SchemaMap, Side);
            var columns = SchemaMap.GetIncludedColumns();
            var schema = DataSourceInfo.GetAvangateDataSchema(RequestHelper);
            
            int limit = 100;
            int page = 1;
            bool abort = false;

            do
            {
                RequestHelper.AddRequestHeader("X-Avangate-Authentication", BuildAuthenticationHeader(DateTime.UtcNow));
                var result = RequestHelper.GetRequestAsJson($"{DataSourceInfo.AvangateEndpointUrl}?Limit={limit}&Page={page}&{AppendQueryString}");

                if (result.Any())
                {
                    //Loop around your data adding it to the DataTableStore dt object.
                    foreach (var item_row in DataSourceInfo.DataCollectionKey == null ? result : result[DataSourceInfo.DataCollectionKey])
                    {
                        if (dt.Rows.AddWithIdentifier(mapping, columns,
                            (item, columnName) =>
                            {
                                var dsi = schema[columnName];
                                if (dsi == null) return null;

                                if (dsi.ObjectName == null)
                                {                                    
                                    return dsi.Parser.ParseValue(item_row[dsi.Name]);
                                }
                                else
                                {
                                    var obj = item_row[dsi.ObjectName] as JObject;
                                    return dsi.Parser.ParseValue(obj?[dsi.Name]);
                                }
                            }
                            , item_row[DataSourceInfo.IdColumnName].ToObject<string>()) == DataTableStore.ABORT)
                        {
                            abort = true;
                            break;
                        }
                    }

                    page++;

                    // Edit if we got to the end of the results.
                    if (result.Count() < limit) break;
                }
                else
                {
                    break;
                }

            } while (!abort);

            return dt;
        }

        public override DataSchema GetDefaultDataSchema()
        {
            var pd_schema = DataSourceInfo.GetAvangateDataSchema(RequestHelper)
                                .Select(p => p.Value)
                                .ToList();

            DataSchema schema = new DataSchema();

            pd_schema.ForEach(p => schema.Map.Add(p.ToDataSchemaItem()));

            return schema;
        }

        public override List<ProviderParameter> GetInitializationParameters()
        {
            //Return the Provider Settings so we can save the Project File.
            return new List<ProviderParameter>
                       {
                            new ProviderParameter("VendorCode", VendorCode, GetConfigKey("VendorCode")),
                            new ProviderParameter("VendorSecret", SecurityService.EncryptValue(VendorSecret), GetConfigKey("VendorSecret")),
                            new ProviderParameter("List", List, GetConfigKey("List")),
                            new ProviderParameter("PageSize", PageSize.ToString(), GetConfigKey("PageSize")),
                            new ProviderParameter("AppendQueryString", AppendQueryString, GetConfigKey("PageSize"))
                       };
        }

        public override void Initialize(List<ProviderParameter> parameters)
        {
            //Load the Provider Settings from the File.
            foreach (ProviderParameter p in parameters)
            {
                AddConfigKey(p.Name, p.ConfigKey);

                switch (p.Name)
                {
                    case "VendorCode":
                        {
                            VendorCode = p.Value;
                            break;
                        }
                    case "VendorSecret":
                        {
                            VendorSecret = SecurityService.DecyptValue(p.Value);
                            break;
                        }
                    case "List":
                        {
                            List = p.Value;
                            break;
                        }
                    case "PageSize":
                        {
                            if (int.TryParse(p.Value, out int value))
                            {
                                PageSize = value;
                            }
                            break;
                        }
                    case "AppendQueryString":
                        {
                            AppendQueryString = p.Value;
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
        }

        public override IDataSourceWriter GetWriter()
        {
            return new AvangateDataSourceWriter { SchemaMap = SchemaMap };
        }

        #region IDataSourceSetup - Render Custom Configuration UI

        public void DisplayConfigurationUI(Control parent)
        {
            if (_connectionIf == null)
            {
                _connectionIf = new ConnectionInterface();
                _connectionIf.PropertyGrid.SelectedObject = new ConnectionProperties(this);
            }

            _connectionIf.Font = parent.Font;
            _connectionIf.Size = new Size(parent.Width, parent.Height);
            _connectionIf.Location = new Point(0, 0);
            _connectionIf.Dock = System.Windows.Forms.DockStyle.Fill;

            parent.Controls.Add(_connectionIf);
        }

        public bool Validate()
        {
            try
            {
                if (string.IsNullOrEmpty(VendorCode))
                {
                    throw new ArgumentException("You must specify a valid Vendor Code.");
                }

                if (string.IsNullOrEmpty(VendorSecret))
                {
                    throw new ArgumentException("You must specify a valid Vendor Secret.");
                }

                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Avangate", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            return false;

        }

        public IDataSourceReader GetReader()
        {
            return this;
        }

        #endregion

        public string BuildAuthenticationHeader(DateTime date)
        {
            return BuildAuthenticationHeader(date.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        private string BuildAuthenticationHeader(string date)
        {
            string hashValidationString = string.Empty;
            string hashString = VendorCode.Length + VendorCode + date.Length + date;

            Encoding encoding = new UTF8Encoding();

            var keyBytes = encoding.GetBytes(VendorSecret);
            var valueBytes = encoding.GetBytes(hashString);

            using (var hmacSignature = new HMACMD5(keyBytes))
            {
                var signature = hmacSignature.ComputeHash(valueBytes);

                string signatureString = string.Empty;
                signatureString = signature.Aggregate(signatureString, (current, b) => current + b.ToString("x2"));

                return $"code=\"{VendorCode}\" date=\"{date}\" hash=\"{signatureString}\"";
            }
        }

        public HttpWebRequestHelper GetWebRequestHelper()
        {
            var requestHelper = RequestHelper.Copy();
            requestHelper.AddRequestHeader("X-Avangate-Authentication", BuildAuthenticationHeader(DateTime.UtcNow));

            return requestHelper;
        }

        public IAvangateDatasourceInfo GetDatasourceInfo()
        {
            return DataSourceInfo;
        }

        public IList<string> GetObjectNames()
        {
            return new List<string>() { "Subscription", "Promotion" }.OrderBy(p => p).ToList();
        }
    }
}