﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Controls;
using BookingModule.Views;
using KarveCar.Navigation;
using KarveCommon.Generic;
using KarveCommon.Services;
using KarveCommonInterfaces;
using KarveDataServices;
using KarveDataServices.ViewObjects;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Regions;


namespace BookingModule.ViewModels
{
    public class ReservationRequestsViewModel : KarveRoutingBaseViewModel, ICreateRegionManagerScope, IEventObserver, INavigationAware
    {
        private ICommand _deleteCommand;
        private ICommand _saveCommand;
        private IUnityContainer _container;
        private ReservationRequestViewObject _dataObject;
        private IReservationRequestDataService _dataReservationService;
        private IAssistDataService _assistDataService;
        private IReservationRequest _reservationRequest;
        private IKarveNavigator _navigator;
        private IEnumerable<ResellerViewObject> _reseller;
        private IEnumerable<CompanyViewObject> _company;
        private IEnumerable<VehicleGroupViewObject> _group;
        private IEnumerable<OrigenViewObject> _origin;
        private IEnumerable<FareViewObject> _fare;
        private IEnumerable<RequestReasonViewObject> _requestreason;
        private IEnumerable<ClientSummaryExtended> _client;
        private IEnumerable<OfficeViewObject> _office;
        private IEnumerable<VehicleSummaryViewObject> _vehicleSummary;
        //private IEnumerable<OrigenDto> _origenSummary;
        private IUserSettings _userSettings;
        private IHelperViewFactory _helperViewFactory;
        private ICommand _newCommand;

        public ReservationRequestsViewModel(IDataServices services, IInteractionRequestController controller, IDialogService dialogService, IEventManager eventManager, IKarveNavigator navigation, IConfigurationService configurationService, IRegionManager regionManager, IUnityContainer unityContainer) : base(services, controller, dialogService, eventManager, regionManager, configurationService)
        {
            InitServices(services, configurationService);
            InitCommands();
            InitCrudCommands();
            SubSystem = DataSubSystem.BookingSubsystem;
            ViewModelUri = new Uri("karve://booking/request/viewmodel?id=" + Guid.ToString());
            _navigator = navigation;
            _container = unityContainer;
            _helperViewFactory = _navigator.GetHelperViewFactory();
            AssistMapper = _assistDataService.Mapper;
            CompositeCommandOnly = true;
            VehicleGridColumns = _userSettings.FindSetting<string>(UserSettingConstants.VehicleSummaryGridColumnsKey);
            EventManager.RegisterObserverSubsystem(BookingModule.RequestGroup, this);


        }
        private void InitServices(IDataServices services, IConfigurationService configurationService)
        {
            _userSettings = configurationService.UserSettings;
            _dataReservationService = services.GetReservationRequestDataService();
            _assistDataService = services.GetAssistDataServices();

        }
        private void InitCommands()
        {
            AssistCommand = new DelegateCommand<object>(OnAssistCommand);
            ItemChangedCommand = new DelegateCommand<object>(OnChangedField);
            CreateNewClient = new DelegateCommand(NewClient);
            CreateNewGroup = new DelegateCommand(NewGroup);
            CreateNewRequestReason = new DelegateCommand(NewRequestReason);
            CreateNewReseller = new DelegateCommand(NewReseller);
            CreateNewFare = new DelegateCommand(NewFare);
            CreateNewVehicle = new DelegateCommand(NewVehicle);
            CreateNewOrigin = new DelegateCommand(NewOrigin);
        }
        private void InitCrudCommands()
        {
            _deleteCommand = new DelegateCommand<object>(DeleteViewCommand);
            _saveCommand = new DelegateCommand<object>(SaveViewCommand);
            _newCommand = new DelegateCommand<object>(NewViewCommand);
        }

        private void NewOrigin()
        {
            var uri = "karve://booking/request/origin/";
            _helperViewFactory.NewOriginView(new Uri(uri));
        }
        
        #region Cross reference
        private void NewVehicle()
        {
            _navigator.NewVehicleView(ViewModelUri);
        }
        private void NewFare()
        {
            _navigator.NewFareView(ViewModelUri);
        }
        private void NewReseller()
        {
            var uri = "karve://booking/request/reseller/";
            _helperViewFactory.NewReseller(new Uri(uri));           
        }
        private void NewRequestReason()
        {
            var uri = "karve://booking/request/reason/";
            _helperViewFactory.NewBookingRequestReason(new Uri(uri));
        }
        private void NewGroup()
        {
            DialogService?.ShowErrorMessage("Not implemented");
            //  _navigator.NewHelperView<GRUPOS, VehicleGroupDto>(new GRUPOS(), typeof(HelperModule.Views.VehicleGroup).FullName);

        }

        private void NewClient()
        {
            _navigator.NewClientView(ViewModelUri);

        }
        #endregion


        private void OnChangedField(object ev)
        {
            if (ev is IDictionary<string, object> eventData)
            {            
                OnChangedCommand(DataObject,
                                       eventData,
                                       DataSubSystem.BookingSubsystem,
                                       BookingModule.RequestGroup,
                                       ViewModelUri.ToString());
            }
        }

        public override void DisposeEvents()
        {
            base.DisposeEvents();
            EventManager.DeleteObserverSubSystem(BookingModule.RequestGroup, this);
        }
       
        private void OnAssistCommand(object param)
        {
            IDictionary<string, string> values = (Dictionary<string, string>)param;
            string assistTableName = values.ContainsKey("AssistTable") ? values["AssistTable"] as string : null;
            string assistQuery = values.ContainsKey("AssistQuery") ? values["AssistQuery"] as string : null;
            this.AssistNotifierInitialized = NotifyTaskCompletion.Create<bool>(AssistQueryRequestHandler(assistTableName, assistQuery), (sender, ev) => {
              
            });
        }
        private async Task<bool> AssistQueryRequestHandler(string assistTableName, string assistQuery)
        {
            var collectionValue = await AssistMapper.ExecuteAssistGeneric(assistTableName, assistQuery);
            switch (assistTableName)
            {
                case "VEHICLE_GROUP_ASSIST":
                    {
                        GroupDto = (IEnumerable<VehicleGroupViewObject>)collectionValue;
                        break;
                    };
                case "RESELLER_ASSIST":
                    {
                        ResellerDto = (IEnumerable<ResellerViewObject>)collectionValue;
                        break;
                    }
                case "FARE_ASSIST":
                    {
                        FareDto = (IEnumerable<FareViewObject>)collectionValue;
                        break;
                    }
                case "CLIENT_ASSIST":
                    {
                        ClientDto = (IEnumerable<ClientSummaryExtended>)collectionValue;
                        break;
                    }
                case "ORIGIN_ASSIST":
                    {
                        OriginDto = (IEnumerable<OrigenViewObject>)collectionValue;
                        break;
                    }
                case "COMPANY_ASSIST":
                    {
                        CompanyDto = (IEnumerable<CompanyViewObject>)collectionValue;
                        break;
                    }
                case "OFFICE_ASSIST":
                    {
                        OfficeDto = (IEnumerable<OfficeViewObject>)collectionValue;
                        break;
                    }
                case "VEHICLE_ASSIST":
                    {
                        VehicleSummaryDto = (IEnumerable<VehicleSummaryViewObject>)collectionValue;
                        break;
                    }
                case "REQUEST_REASON":
                    {
                        RequestReasonDto = (IEnumerable<RequestReasonViewObject>)collectionValue;
                        break;
                    }
            }
            return true;
        }
        /// <summary>
        ///  This delete asynchronously. 
        /// </summary>
        /// <param name="primaryKey">Value of the primary key.</param>
        /// <param name="subSystem">Kind of subsystem</param>
        /// <param name="eventSubSystem">Event subsystem</param>
        /// <returns></returns>
        private async Task<bool> DeleteAsync(string primaryKey, DataSubSystem subSystem, string eventSubSystem)
        {
            if (string.IsNullOrEmpty(primaryKey))
                throw new ArgumentNullException();
            var deleted = false;
            try
            {
                deleted = await _dataReservationService.DeleteAsync(_reservationRequest).ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DialogService?.ShowErrorMessage(e.Message);
            }
            return deleted;
        }

        public bool CreateRegionManagerScope => true;


        public void IncomingPayload(DataPayLoad dataPayLoad)
        {
            // is it null?
            if (dataPayLoad == null) return;
           // is it sent by myself?
            if ((dataPayLoad.Sender != null) && (dataPayLoad.Sender.Equals(ViewModelUri)))
            {
                return;
            }
            if (dataPayLoad.DataObject == null)
            {
                return;
            }
            if (PrimaryKeyValue != null)
            {
                return;
            }
            // is it for me?
            if (PrimaryKeyValue.Length > 0)
            {

                var request = dataPayLoad.DataObject as IReservationRequest;
                if (request != null)
                {
                    var dto = request.Value;
                    // check if the message if for me.
                    if ((dto != null) && (dto.NUMERO != PrimaryKeyValue))
                        return;
                }
            }

            var interpeter = new PayloadInterpeter<DataType>();
            var currentId = _dataReservationService.NewId();
            interpeter.Init = Init;
            interpeter.CleanUp = CleanUp;

            if (string.IsNullOrEmpty(PrimaryKeyValue)) PrimaryKeyValue = dataPayLoad.PrimaryKeyValue;

            if (string.IsNullOrEmpty(PrimaryKeyValue))
                return;
            // check if this message is for me.
            if (PrimaryKeyValue.Length > 0)
            {
                if (dataPayLoad.DataObject is IReservationRequest dto)
                {
                    var currentValue = dto.Value;
                    if (PrimaryKeyValue != currentValue.NUMERO)
                    {
                        return;
                    }
                }
            }
            OperationalState = interpeter.CheckOperationalType(dataPayLoad);
            interpeter.ActionOnPayload(dataPayLoad, PrimaryKeyValue, currentId, SubSystem,
                EventSubsystem.BookingSubsystemVm);
        }

        protected void CleanUp(DataPayLoad payLoad, DataSubSystem subsystem, string eventSubsystem)
        {
            //    DeleteItem(payLoad);
        }
        /// <summary>
        ///  when i shall show or insert.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="payload"></param>
        /// <param name="insertion"></param>
        protected void Init(string value, DataPayLoad payload, bool insertion)
        {
            if (payload.DataObject==null)
            {
                return;
            }
           if (!(payload.DataObject is IReservationRequest))
            {
                return;
            }
            _reservationRequest = payload.DataObject as IReservationRequest;
            DataObject = _reservationRequest.Value;
            ClientDto = _reservationRequest.ClientDto;
            GroupDto = _reservationRequest.GroupDto;
            ResellerDto = _reservationRequest.ResellerDto;
            RequestReasonDto = _reservationRequest.ResquestReasonDto;
            OfficeDto = _reservationRequest.OfficeDto;
            CompanyDto = _reservationRequest.CompanyDto;
            OriginDto = _reservationRequest.OriginDto;
            FareDto = _reservationRequest.FareDto;
            VehicleSummaryDto = _reservationRequest.VehicleDto;
            RegisterToolBar();
            ActiveSubSystem();
        }

        public void NewViewCommand(object key)
        {
            var factory = new ViewFactory<IReservationRequest, ReservationRequestSummary>(RegionManager, _container, EventManager,_dataReservationService, _dataReservationService);


            var activeView = RegionManager.Regions[RegionNames.TabRegion].ActiveViews.FirstOrDefault();
            /// i shall check that i am on foreground
            if (activeView is UserControl control)
            {
                if (control.DataContext is KarveViewModelBase baseViewModel)
                {
                    // its is me....
                    if (baseViewModel.ViewModelUri == ViewModelUri)
                    {
                        factory.NewItem<ReservationRequests>(KarveLocale.Properties.Resources.lrgrReservas,
                            "karve://booking/request/viewmodel?id=",
                            DataSubSystem.BookingSubsystem,
                            BookingModule.BookingSubSystem);
                    }
                }
            }
        }
        public void DeleteViewCommand(object key)
        {
            base.DeleteView(key);
            DataPayLoad payload = new DataPayLoad();
            _reservationRequest.Value = DataObject;
            payload.DataObject = _reservationRequest;
            payload.HasDataObject = true;
            payload.PrimaryKeyValue = DataObject.NUMERO;
            DeleteItem(payload);
        }
        public void SaveViewCommand(object key)
        {
            bool isSaved = false;
            _reservationRequest.Value = DataObject;
            NotifyTaskCompletion.Create(_dataReservationService.SaveAsync(_reservationRequest), (sender, args) =>
            {
                if (sender is INotifyTaskCompletion<bool> taskCompletion)
                {
                    if (taskCompletion.IsFaulted)
                    {
                        DialogService?.ShowErrorMessage("Error during saving: " + taskCompletion.ErrorMessage);
                    }
                    if (taskCompletion.IsSuccessfullyCompleted)
                    {
                        isSaved = true;
                    }
                }
               
            });
          if (isSaved)
            {
                var dataPayLoad = new DataPayLoad
                {
                    Sender = ViewModelUri.ToString(),
                    PayloadType = DataPayLoad.Type.UpdateView
                };
                EventManager.NotifyObserverSubsystem(BookingModule.RequestGroup, dataPayLoad);
                DialogService?.ShowErrorMessage("Data saved with success!");

            }
           
        }
        protected override void DeleteItem(DataPayLoad payLoad)
        {
            bool isDeleted = false;
            NotifyTaskCompletion.Create(DeleteAsync(payLoad),
                (sender, args) =>
                {

                    if (sender is INotifyTaskCompletion<bool> taskCompletion)
                    {
                        if (taskCompletion.IsFaulted)
                        {
                            DialogService?.ShowErrorMessage("Error during aving: " + taskCompletion.ErrorMessage);
                        }

                        if (!taskCompletion.IsSuccessfullyCompleted) return;
                        isDeleted = true;

                    }
                });

            if (isDeleted)
            {
                var dataPayLoad = new DataPayLoad
                {
                    Sender = ViewModelUri.ToString(),
                    PayloadType = DataPayLoad.Type.UpdateView
                };
                EventManager.NotifyObserverSubsystem(BookingModule.RequestGroup, dataPayLoad);
                UnregisterToolBar(payLoad.PrimaryKeyValue);
                DeleteRegion();
                DialogService?.ShowErrorMessage("Data deleted with success!");
            }
        }
        private async Task<bool> DeleteAsync(DataPayLoad payLoad)
        {
            bool retValue = false;
            if (payLoad.DataObject is IReservationRequest request)
            {
                try
                {
                    retValue = await _dataReservationService.DeleteAsync(request).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    var msg = "Error during saving request:" + ex.Message;
                    DialogService?.ShowErrorMessage(msg);
                }
            }
            return retValue;
        }
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        protected override string GetRouteName(string name)
        {
            return ViewModelUri.ToString(); 
        }
       
        private void DeleteCommandFunction(object obj)
        {
        }
        protected override void SetRegistrationPayLoad(ref DataPayLoad payLoad)
        {
            if (payLoad == null)
            {
                payLoad = new DataPayLoad();
            }

            payLoad.Subsystem = DataSubSystem.BookingSubsystem;
            payLoad.PayloadType = DataPayLoad.Type.RegistrationPayload;
            payLoad.HasDataObject = false;
            payLoad.HasRelatedObject = false;
            payLoad.HasNewCommand = true;
            payLoad.NewCommand = _newCommand;
            payLoad.HasSaveCommand = true;
            payLoad.SaveCommand = _saveCommand;
            payLoad.DeleteCommand = _deleteCommand;
            payLoad.HasDeleteCommand = true;
            payLoad.ObjectPath = ViewModelUri;
            payLoad.Sender = ViewModelUri.ToString();
            payLoad.Subsystem = DataSubSystem.BookingSubsystem;
                       
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
          
        }

        public ReservationRequestViewObject DataObject {
            set {
                _dataObject = value; RaisePropertyChanged();
            }
            get
            {
                return _dataObject;
            }
        }

        public IEnumerable<VehicleSummaryViewObject> VehicleSummaryDto { get => _vehicleSummary; set { _vehicleSummary = value; RaisePropertyChanged(); } }
        public IEnumerable<ResellerViewObject> ResellerDto { get=>_reseller; set { _reseller = value; RaisePropertyChanged(); }  }
        public IEnumerable<ClientSummaryExtended> ClientDto { get => _client;  set { _client = value; RaisePropertyChanged(); } }
        public IEnumerable<VehicleGroupViewObject> GroupDto { get => _group; set { _group = value; RaisePropertyChanged(); } }
        public IEnumerable<OrigenViewObject> OriginDto { get => _origin; set { _origin = value; RaisePropertyChanged(); } }
        public IEnumerable<FareViewObject> FareDto { get => _fare; set { _fare = value; RaisePropertyChanged(); } }
        public IEnumerable<CompanyViewObject> CompanyDto { get=> _company; set { _company = value; RaisePropertyChanged(); } }
        public IEnumerable<OfficeViewObject> OfficeDto { get=> _office; set { _office = value; RaisePropertyChanged(); } }
        public IEnumerable<RequestReasonViewObject> RequestReasonDto { get => _requestreason; set { _requestreason = value; RaisePropertyChanged(); } }
        public DelegateCommand CreateNewClient { get; set; }
        public DelegateCommand CreateNewGroup { get; set; }
        public DelegateCommand CreateNewRequestReason { get; set; }
        public DelegateCommand CreateNewReseller { get; set; }
        public DelegateCommand CreateNewFare { get; set; }
        public DelegateCommand CreateNewVehicle { get; set; }
        public DelegateCommand CreateNewOrigin { get; set; }
        public string VehicleGridColumns { get; private set; }
    }
}
