﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace KarveDataServices.DataTransferObject
{
    /// <summary>
    ///  Data Transfer object for the visit.
    /// </summary>
    public class VisitsDto: BaseDto
    {
        /// <summary>
        ///  Identifier for the visit.
        /// </summary>
        [PrimaryKey]
        [Display(GroupName = "Codigo Visita")]
        public string VisitId { get; set; }
        /// <summary>
        ///  Client identifer
        /// </summary>
        /// [PrimaryKey]
        [Display(GroupName = "Codigo Cliente")]
        public string ClientId { get; set; }

        private ContactsDto _contacts;

        /// <summary>
        ///  Contact identifier
        /// </summary>
        [Display(GroupName = "Codigo Contacto")]
        public string ContactId { get; set; }

        private ResellerDto _reseller;
        private ClientDto _clients;

        /// <summary>
        ///  This is the order of the things.
        /// </summary>
        private bool IsOrder { set; get; }
        /// <summary>
        ///  Seller identifier
        /// </summary>
        [Display(GroupName = "Codigo Vendedor")]
        public string SellerId { get; set; }
        /// <summary>
        ///  Type of the visit
        /// </summary>
        [Display(GroupName = "Tipo Visita")]
        public string VisitType { get; set; }
        /// <summary>
        ///  Name of the contact.
        /// </summary>
        [Display(GroupName = "Nombre Contact")]
        public string ContactName { set; get; }
        /// <summary>
        /// Visit date.
        /// </summary>
        [Display(GroupName = "Fecha Alta")]
        public DateTime? Date { set; get; }
        [Display(GroupName = "Vendedor")]

        public ResellerDto SellerSource {
            set
            {
                _reseller = value;
                SellerId = _reseller.Code;
                RaisePropertyChanged();
            }
            get
            {
                return _reseller;
            }
        }
        [Display(GroupName = "Contacto")]
        public ContactsDto ContactsSource
        {
            set
            {
                _contacts = value;
                ContactId = _contacts.ContactId;
                RaisePropertyChanged();
            }
            get
            {
                return _contacts;
            }
        }
        [Display(GroupName = "Cliente")]
        public ClientDto ClientSource
        {
            get
            {
                
                return _clients;
            }
            set
            {
                _clients = value;
                ClientId = _clients.NUMERO_CLI;
                RaisePropertyChanged();
            }
        }

     }
}