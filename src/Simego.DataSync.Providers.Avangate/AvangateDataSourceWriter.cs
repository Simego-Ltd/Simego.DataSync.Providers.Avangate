using Simego.DataSync.Engine;
using Simego.DataSync.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Simego.DataSync.Providers.Avangate
{
    public class AvangateDataSourceWriter : DataWriterProviderBase
    {
        private AvangateDatasourceReader DataSourceReader { get; set; }
        private DataSchemaMapping Mapping { get; set; }

        public override void AddItems(List<DataCompareItem> items, IDataSynchronizationStatus status)
        {
            if (items != null && items.Count > 0)
            {
                int currentItem = 0;

                foreach (var item in items)
                {
                    if (!status.ContinueProcessing)
                        break;

                    var itemInvariant = new DataCompareItemInvariant(item);

                    try
                    {
                        
                        //Call the Automation BeforeAddItem (Optional only required if your supporting Automation Item Events)
                        Automation?.BeforeAddItem(this, itemInvariant, null);

                        if (itemInvariant.Sync)
                        {
                            #region Add Item

                            //Get the Target Item Data
                            Dictionary<string, object> targetItem = AddItemToDictionary(Mapping, itemInvariant);

                            //TODO: Write the code to Add the Item to the Target




                            //Call the Automation AfterAddItem (pass the created item identifier if possible)
                            Automation?.AfterAddItem(this, itemInvariant, null);

                        }

                        #endregion

                        ClearSyncStatus(item); //Clear the Sync Flag on Processed Rows

                    }
                    catch (WebException e)
                    {
                        Automation?.ErrorItem(this, itemInvariant, null, e);
                        HandleError(status, e);
                    }
                    catch (SystemException e)
                    {
                        Automation?.ErrorItem(this, itemInvariant, null, e);
                        HandleError(status, e);
                    }
                    finally
                    {
                        status.Progress(items.Count, ++currentItem); //Update the Sync Progress
                    }

                }
            }
        }

        public override void UpdateItems(List<DataCompareItem> items, IDataSynchronizationStatus status)
        {
            if (items != null && items.Count > 0)
            {
                int currentItem = 0;

                foreach (var item in items)
                {
                    if (!status.ContinueProcessing)
                        break;

                    var itemInvariant = new DataCompareItemInvariant(item);

                    var item_id = itemInvariant.GetTargetIdentifier<string>();

                    try
                    {
                        
                        //Call the Automation BeforeUpdateItem (Optional only required if your supporting Automation Item Events)
                        Automation?.BeforeUpdateItem(this, itemInvariant, item_id);

                        if (itemInvariant.Sync)
                        {
                            #region Update Item

                            //Get the Target Item Data
                            Dictionary<string, object> targetItem = UpdateItemToDictionary(Mapping, itemInvariant);

                            //TODO: Write the code to Update the Item in the Target using item_id as the Key to the item.

                            //Call the Automation AfterUpdateItem 
                            Automation?.AfterUpdateItem(this, itemInvariant, item_id);

                            
                            #endregion
                        }

                        ClearSyncStatus(item); //Clear the Sync Flag on Processed Rows
                    }
                    catch (WebException e)
                    {
                        Automation?.ErrorItem(this, itemInvariant, item_id, e);
                        HandleError(status, e);
                    }
                    catch (SystemException e)
                    {
                        Automation?.ErrorItem(this, itemInvariant, item_id, e);
                        HandleError(status, e);
                    }
                    finally
                    {
                        status.Progress(items.Count, ++currentItem); //Update the Sync Progress
                    }

                }
            }
        }

        public override void DeleteItems(List<DataCompareItem> items, IDataSynchronizationStatus status)
        {
            if (items != null && items.Count > 0)
            {
                int currentItem = 0;

                foreach (var item in items)
                {
                    if (!status.ContinueProcessing)
                        break;

                    var itemInvariant = new DataCompareItemInvariant(item);

                    var item_id = itemInvariant.GetTargetIdentifier<string>();

                    try
                    {

                        //Call the Automation BeforeDeleteItem (Optional only required if your supporting Automation Item Events)
                        Automation?.BeforeDeleteItem(this, itemInvariant, item_id);

                        if (itemInvariant.Sync)
                        {
                            #region Delete Item

                            //TODO: Write the Code to Delete the Item in the Target using item_id as the Key to the item.

                            #endregion

                            //Call the Automation AfterDeleteItem 
                            Automation?.AfterDeleteItem(this, itemInvariant, item_id);
                        }

                        ClearSyncStatus(item); //Clear the Sync Flag on Processed Rows
                    }
                    catch (WebException e)
                    {
                        Automation?.ErrorItem(this, itemInvariant, item_id, e);
                        HandleError(status, e);
                    }
                    catch (SystemException e)
                    {
                        Automation?.ErrorItem(this, itemInvariant, item_id, e);
                        HandleError(status, e);
                    }
                    finally
                    {
                        status.Progress(items.Count, ++currentItem); //Update the Sync Progress
                    }
                }
            }
        }

        public override void Execute(List<DataCompareItem> addItems, List<DataCompareItem> updateItems, List<DataCompareItem> deleteItems, IDataSourceReader reader, IDataSynchronizationStatus status)
        {
            DataSourceReader = reader as AvangateDatasourceReader;

            if (DataSourceReader != null)
            {
                Mapping = new DataSchemaMapping(SchemaMap, DataCompare);

                //Process the Changed Items
                if (addItems != null && status.ContinueProcessing) AddItems(addItems, status);
                if (updateItems != null && status.ContinueProcessing) UpdateItems(updateItems, status);
                if (deleteItems != null && status.ContinueProcessing) DeleteItems(deleteItems, status);

            }
        }

        private static void HandleError(IDataSynchronizationStatus status, Exception e)
        {
            if (!status.FailOnError)
            {
                status.LogMessage(e.Message);
            }
            if (status.FailOnError)
            {
                throw e;
            }
        }

        private void HandleError(IDataSynchronizationStatus status, WebException e)
        {
            if (status.FailOnError)
            {
                throw e;
            }

            if (e.Response != null)
            {
                using (var response = e.Response.GetResponseStream())
                {
                    if (response != null)
                        using (var sr = new StreamReader(response))
                        {
                            string result = sr.ReadToEnd();
                            if (!string.IsNullOrEmpty(result))
                            {
                                status.LogMessage(string.Concat(e.Message, Environment.NewLine, result));
                            }
                        }
                }
            }
            else
            {
                if (!status.FailOnError)
                {
                    status.LogMessage(e.Message);
                }
            }
        }
    }
}
