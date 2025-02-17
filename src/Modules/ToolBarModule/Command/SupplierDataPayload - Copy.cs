﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using KarveCommon.Generic;
using KarveCommon.Services;
using KarveDataServices;
using KarveDataServices.DataObjects;

namespace ToolBarModule.Command
{
    internal class SupplierDataPayload : ToolbarDataPayload
    {
        private ISupplierDataServices _dataServices = null;
        private IEventManager _manager = null;
        private DataPayLoad _payload;
        private INotifyTaskCompletion<DataPayLoad> _initializationNotifier;

        public SupplierDataPayload()
        {
        }

        public event ErrorExecuting OnErrorExecuting;
        /// <summary>
        ///  This updates a data set.
        /// </summary>
        /// <param name="payLoad"></param>
        /// <param name="set"></param>
        /// <param name="queries"></param>
        private void HandleDataSetUpdate(DataPayLoad payLoad, DataSet set, IDictionary<string, string> queries)
        {
            try
            {
                _dataServices.UpdateDataSet(queries, set);
                payLoad.Queries = queries;
                payLoad.Sender = ToolBarModule.NAME;
                payLoad.PayloadType = DataPayLoad.Type.UpdateView;
                NotifyEventManager(_manager, payLoad);
            }
            catch (Exception e)
            {
                OnErrorExecuting?.Invoke(e.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="payLoad"></param>
        /// <param name="dataSetList"></param>
        /// <param name="queries"></param>
        private void HandleDataSetListUpdate(DataPayLoad payLoad, IList<DataSet> dataSetList, IDictionary<string, string> queries)
        {
            foreach (DataSet set in dataSetList)
            {
                try
                {
                    _dataServices.UpdateDataSet(queries, set);
                    DataPayLoad newSet = (DataPayLoad)payLoad.Clone();
                    newSet.HasDataSetList = false;
                    newSet.HasDataSet = true;
                    payLoad.Sender = ToolBarModule.NAME;
                    payLoad.PayloadType = DataPayLoad.Type.UpdateView;
                    NotifyEventManager(_manager, payLoad);
                }
                catch (Exception e)
                {
                    OnErrorExecuting?.Invoke(e.Message);
                }
            }


        }
        private void NotifyEventManager(IEventManager manager, DataPayLoad payLoad)
        {
            manager.SendMessage(EventSubsystem.SuppliersSummaryVm, payLoad);
        }

        /// <summary>
        /// This execute the payload and notify the event manager
        /// </summary>
        /// <param name="services">Services to be used</param>
        /// <param name="manager">Manager to be notified</param>
        /// <param name="payLoad">Payload to execute.</param>
        public override void ExecutePayload(IDataServices services, IEventManager manager, DataPayLoad payLoad)
        {

            _dataServices= services.GetSupplierDataServices();
            _payload = payLoad;
            EventManager = manager;
            DataServices = services;
            _initializationNotifier = NotifyTaskCompletion.Create<DataPayLoad>(HandleSaveOrUpdate(payLoad), ExecutedPayloadHandler);

        }
       
        protected override async Task<DataPayLoad> HandleSaveOrUpdate(DataPayLoad payLoad)
        {
            bool result = false;
            bool isInsert = false;
            ISupplierData supplierData = (ISupplierData)payLoad.DataObject;
          
            if (DataServices == null)
            {
                DataPayLoad nullDataPayLoad = new NullDataPayload();
                return nullDataPayLoad;
            }
            switch (payLoad.PayloadType)
            {
                case DataPayLoad.Type.Update:
                {
                    result = await DataServices.GetSupplierDataServices().SaveChanges(supplierData)
                        .ConfigureAwait(false);
                        break;
                }
                case DataPayLoad.Type.Insert:
                {
                    isInsert = true;
                    result = await DataServices.GetSupplierDataServices().Save(supplierData).ConfigureAwait(true);
                    break;
                }
            }
            if (result)
            {
                payLoad.Sender = ToolBarModule.NAME;
                payLoad.PayloadType = DataPayLoad.Type.UpdateView;
                CurrentPayload = payLoad;

            }
            else
            {
                string message = isInsert ? "Error during the insert" : "Error during the update";
                OnErrorExecuting?.Invoke(message);
            }
            return payLoad;
        }
    }
}
