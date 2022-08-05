using Simego.DataSync.Providers.Avangate.Interfaces;
using System;

namespace Simego.DataSync.Providers.Avangate.Datasources
{
    public class AvangateDatasourceFactory
    {
        public static IAvangateDatasourceInfo GetDatasourceInfo(string name)
        {
            switch (name.ToLower())
            {
                case "subscription":
                    {
                        return new AvangateSubscriptionDatasource();
                    }
                case "promotion":
                    {
                        return new AvangatePromotionDatasource();
                    }
            }

            throw new ArgumentException($"Invalid Avangate datasource: {name}", name);
        }
    }
}
