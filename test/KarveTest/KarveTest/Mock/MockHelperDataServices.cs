﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using KarveDataServices;

namespace KarveTest.Mock
{

    /// <summary>
    ///  MockHelperDataServices.
    /// </summary>
    class MockHelperDataServices: IHelperDataServices
    {

        /// <summary>
        /// GetAsyncHelper mock.
        /// </summary>
        /// <param name="assistQuery">Assist Query Value.</param>
        /// <param name="assistTableName">Assist Name Value.</param>
        /// <returns></returns>
        public async Task<DataSet> GetAsyncHelper(string assistQuery, string assistTableName)
        {
            DataSet set = new DataSet();
            await Task.Delay(1);
            return set;
        }
        /// <summary>
        /// GetAsyncHelper. 
        /// </summary>
        /// <param name="viewModelAssitantQueries">AsyncHelper </param>
        /// <returns></returns>
        public async Task<DataSet> GetAsyncHelper(IDictionary<string, string> viewModelAssitantQueries)
        {
            DataSet set = new DataSet();
            await Task.Delay(1);
            return set;
        }
        /// <summary>
        ///  GetAsyncHelper. Asynchronous value for the helper.
        /// </summary>
        /// <typeparam name="T">Type of the helper.</typeparam>
        /// <param name="assistQuery">Query for getting the helper.</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetAsyncHelper<T>(string assistQuery)
        {
            List<T> listOfType = new List<T>();
            await Task.Delay(1);
            for (int i = 0; i < 10; ++i)
            {
                T value = Activator.CreateInstance<T>();
                Type type = value.GetType();
                PropertyInfo[] info = type.GetProperties();
                foreach (PropertyInfo tmp in info)
                {
                    Type t = tmp.GetType();
                    if (t == typeof(int))
                    {
                        tmp.SetValue(value, 1);
                    }
                    if (t == typeof(string))
                    {
                        tmp.SetValue(value, "test");
                    }
                    if (t == typeof(bool))
                    {
                        tmp.SetValue(value, true);
                    }
                }
                listOfType.Add(value);
            }
            return listOfType;
        }
        /// <summary>
        /// Execute async update value.
        /// </summary>
        /// <typeparam name="DtoTransfer">Data transfer object type</typeparam>
        /// <typeparam name="T">Type of the object</typeparam>
        /// <param name="entity">Entity to be executed</param>
        /// <returns></returns>
        public async Task<bool> ExecuteAsyncUpdate<DtoTransfer, T>(DtoTransfer entity) where T : class
        {
            await Task.Delay(1);
            return true;
        }
        /// <summary>
        /// Execute async insert value
        /// </summary>
        /// <typeparam name="DtoTransfer">Data transfer object type</typeparam>
        /// <typeparam name="T">Type of the subject</typeparam>
        /// <param name="entity">Entity to be executed</param>
        /// <returns></returns>
        public async Task<int> ExecuteAsyncInsert<DtoTransfer, T>(DtoTransfer entity) where T : class
        {
            await Task.Delay(1);
            return 1;
        }
        /// <summary>
        /// Execute Asynchronous Delete.
        /// </summary>
        /// <typeparam name="DtoTransfer">Data transfer to be executed</typeparam>
        /// <typeparam name="T">Type of the subject</typeparam>
        /// <param name="entity">Entity to be executed.</param>
        /// <returns></returns>
        public async Task<bool> ExecuteAsyncDelete<DtoTransfer, T>(DtoTransfer entity) where T : class
        {
            await Task.Delay(1);
            return true;
        }

       /// <summary>
       /// Execute asynhrnouse update and insert
       /// </summary>
       /// <typeparam name="DtoTransfer">Data transfer object to be executed</typeparam>
       /// <typeparam name="T">Type of the subject</typeparam>
       /// <param name="entity">Entity to be executed</param>
       /// <returns></returns>
        public async Task<bool> ExecuteInsertOrUpdate<DtoTransfer, T>(DtoTransfer entity) where T : class
        {
            await Task.Delay(1);
            return true;
        }
        /// <summary>
        ///  Get an unique identifier.
        /// </summary>
        /// <typeparam name="T">Type of the subject</typeparam>
        /// <param name="entity">Entity to be executed</param>
        /// <returns></returns>
        public async Task<string> GetUniqueId<T>(T entity) where T : class
        {
            await Task.Delay(1);
            return "";
        }

        private List<TDtoTransfer> CraftListOfType<TDtoTransfer>()
        {
            List<TDtoTransfer> listOfType = new List<TDtoTransfer>();
            for (int i = 0; i < 10; ++i)
            {
                TDtoTransfer value = Activator.CreateInstance<TDtoTransfer>();
                Type type = value.GetType();
                PropertyInfo[] info = type.GetProperties();
                foreach (PropertyInfo tmp in info)
                {
                    Type t = tmp.GetType();
                    if (t == typeof(int))
                    {
                        tmp.SetValue(value, 1);
                    }
                    if (t == typeof(string))
                    {
                        tmp.SetValue(value, "test");
                    }
                    if (t == typeof(bool))
                    {
                        tmp.SetValue(value, true);
                    }
                }
                listOfType.Add(value);
            }
            return listOfType;
        }

        /// <summary>
        /// GetMappedAsyncHelper
        /// </summary>
        /// <typeparam name="DtoTransfer">Type of the data trasnfer object</typeparam>
        /// <typeparam name="T">Type of the subject</typeparam>
        /// <param name="query">Query to be executed</param>
        /// <returns></returns>
        public async Task<IEnumerable<DtoTransfer>> GetMappedAsyncHelper<DtoTransfer, T>(string query) where DtoTransfer : class
        {
            await Task.Delay(1);
            List<DtoTransfer> listOfType = new List<DtoTransfer>();
            for (int i = 0; i < 10; ++i)
            {
                DtoTransfer value = Activator.CreateInstance<DtoTransfer>();
                Type type = value.GetType();
                PropertyInfo[] info = type.GetProperties();
                foreach (PropertyInfo tmp in info)
                {
                    Type t = tmp.GetType();
                    if (t == typeof(int))
                    {
                        tmp.SetValue(value, 1);
                    }
                    if (t == typeof(string))
                    {
                        tmp.SetValue(value, "test");
                    }
                    if (t == typeof(bool))
                    {
                        tmp.SetValue(value, true);
                    }
                }
                listOfType.Add(value);
            }
            return listOfType;
        }
        /// <summary>
        ///  GetMappedUniqueId. Identifier for the unique mapping.
        /// </summary>
        /// <typeparam name="DtoTransfer">Data Transfer Object type to map</typeparam>
        /// <typeparam name="T">Entity type to map.</typeparam>
        /// <param name="entity">Entity value to map.</param>
        /// <returns>A list of </returns>
        public async Task<string> GetMappedUniqueId<DtoTransfer, T>(DtoTransfer entity) where T : class
        {
            await Task.Delay(1);
            RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();
            byte[] data = new byte[4];
            rngCsp.GetBytes(data);
            string value = Convert.ToString(data);
            return value;
        }

        public Task<DtoTransfer> GetSingleMappedAsyncHelper<DtoTransfer, T>(string code) where DtoTransfer : class, new() where T : class
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// GetMapped unique async helper.
        /// </summary>
        /// <typeparam name="DtoTransfer">Data Transfer Object to map</typeparam>
        /// <typeparam name="T">Entity type to map</typeparam>
        /// <returns></returns>
        public async Task<IEnumerable<DtoTransfer>> GetMappedAllAsyncHelper<DtoTransfer, T>() where DtoTransfer : class where T : class
        {
          var list = CraftListOfType<DtoTransfer>();
          await Task.Delay(1);
          return list;
        }

        public Task<IDictionary<string, DtoTransfer>> GetAsyncBatchData<DtoTransfer>(IDictionary<string, string> value)
        {
            throw new NotImplementedException();
        }

        public void ExecuteBulkDelete<DtoTransfer, T>(IEnumerable<DtoTransfer> objectValues) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExecuteBulkUpdateAsync<DtoTransfer, T>(IEnumerable<DtoTransfer> entity) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExecuteBulkDeleteAsync<DtoTransfer, T>(IEnumerable<DtoTransfer> objectValues) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExecuteBulkInsertAsync<DtoTransfer, T>(IEnumerable<DtoTransfer> entity) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DtoTransfer>> GetPagedSummaryDoAsync<DtoTransfer, T>(int pageIndex, int pageSize)
            where DtoTransfer : class
            where T : class
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DtoTransfer>> GetPagedSummaryDoAsync<DtoTransfer, T>(long pageIndex, int pageSize)
            where DtoTransfer : class
            where T : class
        {
            throw new NotImplementedException();
        }

        public Task<int> GetItemsCount<T>()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetPagedQueryDoAsync<T>(string query, int pageIndex, int pageSize) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Dto>> GetPagedAsyncHelper<Dto, Entity>(string query, int pageIndex, int pageSize)
            where Dto : class
            where Entity : class
        {
            throw new NotImplementedException();
        }
    }
}
