﻿using Dapper;
using DataAccessLayer.Crud;
using DataAccessLayer.DataObjects;
using DataAccessLayer.Exception;
using DataAccessLayer.SQL;
using KarveDataServices.DataObjects;
using KarveDataServices.ViewObjects;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    /*
     *  Code has been generared by the script BookingStructureGenerator.py in CodeGenerator project.
     *  The generator shall keep present that duplicated query type shall be separated generating 
     *  a temp variable. This is due to how the querystore has been designed. 
     *  Further work might be done.It happens that in case of duplicate ids, the query store it will 
     *  work as a stack (LIFO = Last Input First Output).
     */

    internal partial class BookingDataAccessLayer
    {
        private async Task<IBookingData> BuildAux(IBookingData result)
        {
            var auxQueryStore = QueryStoreFactory.GetQueryStore();
            var dto = result.Value;
            #region KarveCode Generator for query multiple single values
            // Code Generated that concatains multiple queries to be executed by QueryMultipleAsync.

            auxQueryStore.AddParamCount(QueryType.QueryAgencyEmployee, dto.EMPLEAGE_RES2);
            auxQueryStore.AddParamCount(QueryType.QueryBookingMedia, dto.MEDIO_RES1);
            auxQueryStore.AddParamCount(QueryType.QueryBookingType, dto.TIPORES_res1);
            auxQueryStore.AddParamCount(QueryType.QueryBudgetSummaryById, dto.PRESUPUESTO_RES1);
            auxQueryStore.AddParamCount(QueryType.QueryCommissionAgentSummaryById, dto.COMISIO_RES2);

            /*
             * In some databases we have stored the complete city. And some other just the zip.
             * 
             */
            auxQueryStore.AddParamCount(QueryType.QueryCityByName, SanitizeCity(dto.POCOND_RES2));
            auxQueryStore.AddParamCount(QueryType.QueryClientContacts, dto.CONTACTO_RES2);
            auxQueryStore.AddParamCount(QueryType.QueryCompany, dto.SUBLICEN_RES1);
            auxQueryStore.AddParamCount(QueryType.QueryFares, dto.TARIFA_RES1);
            auxQueryStore.AddParamCount(QueryType.QueryPaymentForm, dto.FCOBRO_RES1);
            auxQueryStore.AddParamCount(QueryType.QueryProvince, dto.PROVCOND_RES2);
            auxQueryStore.AddParamCount(QueryType.QueryPrintingType, dto.CONTRATIPIMPR_RES);
            auxQueryStore.AddParamCount(QueryType.QueryOrigin, dto.ORIGEN_RES2);
            auxQueryStore.AddParamCount(QueryType.QueryVehicleActivity, dto.ACTIVEHI_RES1);
            auxQueryStore.AddParamCount(QueryType.QueryVehicleGroup, dto.GRUPO_RES1);
            auxQueryStore.AddParamCount(QueryType.QueryVehicleSummaryById, dto.VCACT_RES1);
            auxQueryStore.AddParamCount(QueryType.QueryRefusedBooking, dto.RECHAZAMOTI);
            
            #endregion


            #region OfficeQuery

            var officeStore = _queryStoreFactory.GetQueryStore();
            officeStore.AddParamCount(QueryType.QueryOffice, dto.OFICINA_RES1);

            officeStore.AddParamCount(QueryType.QueryOffice, dto.OFISALIDA_RES1);

            officeStore.AddParamCount(QueryType.QueryOffice, dto.OFIRETORNO_RES1);
            #endregion
            #region  ClientQuery
            var clientStore = _queryStoreFactory.GetQueryStore();
            clientStore.AddParamCount(QueryType.QueryClientSummaryExtById, dto.CLIENTE_RES1);
            clientStore.AddParamCount(QueryType.QueryClientSummaryExtById, dto.CONDUCTOR_RES1);
            clientStore.AddParamCount(QueryType.QueryClientSummaryExtById, dto.OTROCOND_RES2);
            clientStore.AddParamCount(QueryType.QueryClientSummaryExtById, dto.OTRO2COND_RES2);
            clientStore.AddParamCount(QueryType.QueryClientSummaryExtById, dto.OTRO3COND_RES2);
            #endregion
            #region CountryQuery
            var countryStore = _queryStoreFactory.GetQueryStore();
            countryStore.AddParamCount(QueryType.QueryCountry, dto.PAISNIFCOND_RES2);
            countryStore.AddParamCount(QueryType.QueryCountry, dto.PAISCOND_RES2);
            #endregion

            var basicQuery = auxQueryStore.BuildQuery();
            var countryQuery = countryStore.BuildQuery();
            var clientQuery = clientStore.BuildQuery();
            var officeQuery = officeStore.BuildQuery();
           
            var query = basicQuery + countryQuery;
            try
            {
                using (var connection = SqlExecutor.OpenNewDbConnection())
                {
                    if (connection != null)
                    {
                        var multipleResult = await connection.QueryMultipleAsync(query).ConfigureAwait(false);
                        result.Valid = ParseResult(result, multipleResult);
                        result.Valid = result.Valid && ParseCountryResult(result, multipleResult);
                        var officeResult = await connection.QueryMultipleAsync(officeQuery).ConfigureAwait(false);
                        result.Valid = result.Valid && ParseOfficeResult(result, officeResult);
                        var clientResult = await connection.QueryMultipleAsync(clientQuery).ConfigureAwait(false);
                        result.Valid = result.Valid && ParseClientResult(result, clientResult);
                 
               
                        

                    }
                }
            } catch (System.Exception ex)
            {
                var msg = "Error building aux for booking type " + dto.NUMERO_RES + "Reason " + ex.Message;
                throw new DataAccessLayerException(msg, ex);
            }
            try
            {
                result.Valid = result.Valid && await ExecuteSecondDriverResult(result).ConfigureAwait(false);
            } catch(System.Exception ex)
            {
                var msg = "Error building aux for booking type" + dto.NUMERO_RES + " ";
                msg += ex.Message;
            }
            return result;
        }
        private async Task<bool> ExecuteSecondDriverResult(IBookingData request)
        {
            if (request == null)
            {
                return false;
            }
            if (request.Value == null)
            {
                return false;
            }
            try
            {

                using (var connection = SqlExecutor.OpenNewDbConnection())
                {
                    BookingViewObject dto = request.Value;
                    var secondDriverStore = _queryStoreFactory.GetQueryStore();
                    secondDriverStore.AddParamCount(QueryType.QueryCity, dto.DRV2_CITY);
                    secondDriverStore.AddParamCount(QueryType.QueryCountry, dto.DRV2_ID_CARD_COU_CODE);
                    secondDriverStore.AddParamCount(QueryType.QueryProvince, dto.DRV2_ZIP_CODE);
                    var secondDriverQuery = secondDriverStore.BuildQuery();
                    var driverResult = await connection.QueryMultipleAsync(secondDriverQuery).ConfigureAwait(false);
                    request.SecondDriverCityDto = SelectionHelpers.WrappedSelectedDto<POBLACIONES, CityViewObject>(request.Value.DRV2_CITY, _mapper, driverResult);
                    request.SecondDriverCountryDto = SelectionHelpers.WrappedSelectedDto<Country, CountryViewObject>(request.Value.DRV2_ID_CARD_COU_CODE, _mapper, driverResult);
                    request.SecondDriverProvinceDto = SelectionHelpers.WrappedSelectedDto<PROVINCIA, ProvinceViewObject>(request.Value.DRV2_ZIP_CODE, _mapper, driverResult);
                }

            }
            catch (System.Exception ex)
            {
                throw new DataAccessLayerException("Parsing multiple query result error", ex);
            }
            return true;

        }

        private bool ParseCountryResult(IBookingData request, SqlMapper.GridReader reader)
        {
            // null checking as usual.
            if ((request == null) || (reader == null))
            {
                return false;
            }
            if (request.Value == null)
            {
                return false;
            }
            try
            {
                // client queries. Multiple Query are stacked when created so we need to fetch in the reverse order.
                request.CountryDto3 = SelectionHelpers.WrappedSelectedDto<Country, CountryViewObject>(request.Value.PAISCOND_RES2, _mapper, reader);
                request.DriverCountryList = SelectionHelpers.WrappedSelectedDto<Country, CountryViewObject>(request.Value.PAISNIFCOND_RES2, _mapper, reader);
            }
            catch (System.Exception ex)
            {
                throw new DataAccessLayerException("Parsing multiple query result error", ex);
            }
            return true;
        }
        private bool ParseClientResult(IBookingData request, SqlMapper.GridReader reader)
        {
            if ((request == null) || (reader == null))
            {
                return false;
            }
            if (request.Value == null)
            {
                return false;
            }
            try
            {
                // client queries. Multiple Query are stacked when created so we need to fetch in the reverse order.
                request.DriverDto5 = SelectionHelpers.WrappedSelectedDto<ClientSummaryExtended, ClientSummaryExtended>(request.Value.OTRO3COND_RES2, _mapper, reader);

                request.DriverDto4 = SelectionHelpers.WrappedSelectedDto<ClientSummaryExtended, ClientSummaryExtended>(request.Value.OTRO2COND_RES2, _mapper, reader);

                request.DriverDto3 = SelectionHelpers.WrappedSelectedDto<ClientSummaryExtended, ClientSummaryExtended>(request.Value.OTROCOND_RES2, _mapper, reader);
              
                request.DriverDto2 = SelectionHelpers.WrappedSelectedDto<ClientSummaryExtended, ClientSummaryExtended>(request.Value.CONDUCTOR_RES1, _mapper, reader);

                request.Clients = SelectionHelpers.WrappedSelectedDto<ClientSummaryExtended, ClientSummaryExtended>(request.Value.CLIENTE_RES1, _mapper, reader);

              


            } catch (System.Exception ex)
            {
                throw new DataAccessLayerException("Parsing multiple query result error", ex);
            }
            return true;
        }
        private bool ParseOfficeResult(IBookingData request, SqlMapper.GridReader reader)
        {
            if ((request == null) || (reader == null))
            {
                return false;
            }
            if (request.Value == null)
            {
                return false;
            }
            try
            {
                // office queries. Requests are stacked so, retrieval shall be reversed.
                request.ReservationOfficeArrival = SelectionHelpers.WrappedSelectedDto<OFICINAS, OfficeViewObject>(request.Value.OFIRETORNO_RES1, _mapper, reader);
                request.ReservationOfficeDeparture = SelectionHelpers.WrappedSelectedDto<OFICINAS, OfficeViewObject>(request.Value.OFISALIDA_RES1, _mapper, reader);
                request.OfficeDto = SelectionHelpers.WrappedSelectedDto<OFICINAS, OfficeViewObject>(request.Value.OFICINA_RES1, _mapper, reader);
               
            }
            catch (System.Exception ex)
            {
                throw new DataAccessLayerException("Parsing multiple query result error", ex);
            }
            return true;
        }
        /// <summary>
        ///  In the database can appear multiple values for the field poblacion.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string SanitizeCity(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                var tmp = value.Trim();
                var stringArray = tmp.Split(' ');
                if (stringArray.Length > 0)
                {
                    return stringArray[stringArray.Length - 1];
                }
            }
            return value;
        }
       

        private bool ParseResult(IBookingData request, SqlMapper.GridReader reader)
        {

            if ((request == null) || (reader == null))
            {
                return false;
            }
            if (request.Value == null)
            {
                return false;
            }
            try
            {
                request.AgencyEmployeeDto = SelectionHelpers.WrappedSelectedDto<EAGE, AgencyEmployeeViewObject>(request.Value.EMPLEAGE_RES2, _mapper, reader);

                request.BookingMediaDto = SelectionHelpers.WrappedSelectedDto<MEDIO_RES, BookingMediaViewObject>(request.Value.MEDIO_RES1, _mapper, reader);

                request.BookingTypeDto = SelectionHelpers.WrappedSelectedDto<TIPOS_RESERVAS, BookingTypeViewObject>(request.Value.TIPORES_res1, _mapper, reader);


                
                request.BookingBudget = SelectionHelpers.WrappedSelectedDto<BudgetSummaryViewObject, BudgetSummaryViewObject>(request.Value.PRESUPUESTO_RES1, _mapper, reader);

                request.BrokerDto = SelectionHelpers.WrappedSelectedDto<CommissionAgentSummaryViewObject, CommissionAgentSummaryViewObject>(request.Value.COMISIO_RES2, _mapper, reader);


               request.CityDto3 = SelectionHelpers.WrappedSelectedDto<POBLACIONES, CityViewObject>(SanitizeCity(request.Value.POCOND_RES2), _mapper, reader);

                request.ContactsDto1 = SelectionHelpers.WrappedSelectedDto<CliContactos, ContactsViewObject>(request.Value.CONTACTO_RES2, _mapper, reader);

                request.CompanyDto = SelectionHelpers.WrappedSelectedDto<SUBLICEN, CompanyViewObject>(request.Value.SUBLICEN_RES1, _mapper, reader);

               
                request.FareDto = SelectionHelpers.WrappedSelectedDto<NTARI, FareViewObject>(request.Value.TARIFA_RES1, _mapper, reader);

                request.PaymentFormDto = SelectionHelpers.WrappedSelectedDto<FORMAS, PaymentFormViewObject>(request.Value.FCOBRO_RES1, _mapper, reader);

                request.ProvinceDto3 = SelectionHelpers.WrappedSelectedDto<PROVINCIA, ProvinceViewObject>(request.Value.PROVCOND_RES2, _mapper, reader);

                request.PrintingTypeDto = SelectionHelpers.WrappedSelectedDto<CONTRATIPIMPR, PrintingTypeViewObject>(request.Value.CONTRATIPIMPR_RES, _mapper, reader);

                request.OriginDto = SelectionHelpers.WrappedSelectedDto<ORIGEN, OrigenViewObject>(request.Value.ORIGEN_RES2,_mapper,reader);

                request.VehicleActivitiesDto = SelectionHelpers.WrappedSelectedDto<ACTIVEHI, VehicleActivitiesViewObject>(request.Value.ACTIVEHI_RES1, _mapper, reader);

                request.VehicleGroupDto = SelectionHelpers.WrappedSelectedDto<GRUPOS, VehicleGroupViewObject>(request.Value.GRUPO_RES1, _mapper, reader);

                request.VehicleDto = SelectionHelpers.WrappedSelectedDto<VehicleSummaryViewObject, VehicleSummaryViewObject>(request.Value.VCACT_RES1, _mapper, reader);

                request.BookingRefusedDto = SelectionHelpers.WrappedSelectedDto<MOTANU, BookingRefusedViewObject>(request.Value.RECHAZAMOTI, _mapper, reader);


#pragma warning disable CS0168 // Variable is declared but never used
            }
            catch (System.Exception ex)
#pragma warning restore CS0168 // Variable is declared but never used
            {
                var msg = "Parsing multiple query result error " + ex.Message;
                throw new DataAccessLayerException(msg, ex);
            }
            return true;
        }

    }
}
