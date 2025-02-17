﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarveDataServices.DataObjects;
using KarveDataServices.DataTransferObject;

namespace KarveDataServices
{
    public interface ICompanyDataServices
    {
        /// <summary>
        ///  This give us the summary query.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<CompanySummaryDto>> GetAsyncAllCompanySummary();
        /// <summary>
        /// This return a client with a new code.
        /// </summary>
        /// <param name="code">Code to return.</param>
        /// <returns></returns>
        ICompanyData GetNewCompanyDo(string code);
        /// <summary>
        ///  This is a delete client.
        /// </summary>
        Task<bool> DeleteCompanyAsyncDo(ICompanyData companyData);
        /// <summary>
        ///  This save the client data.
        /// </summary>
        /// <param name="clientData"></param>
        /// <returns>true or false in case of a client.</returns>
        Task<bool> SaveAsync(ICompanyData clientData);
        /// <summary>
        ///  Client data object
        /// </summary>
        /// <param name="clientIndentifier">Identifier a client</param>
        /// <returns>Client data to receive</returns>
        Task<ICompanyData> GetAsyncCompanyDo(string clientIndentifier);
        /// <summary>
        ///  This generate an unique id following the entity.
        /// </summary>
        /// <returns></returns>
        string GetNewId();
    }
}
