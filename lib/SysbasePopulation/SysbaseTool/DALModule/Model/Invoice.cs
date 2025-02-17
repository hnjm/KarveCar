﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarveDataAccessLayer;
using KarveDataServices.DataObjects;
using KarveDataServices;



namespace DataAccessLayer.Model
{
    public class Invoice : DomainObject, IInvoiceData
    {
        private string code;
        public Invoice(string code, InvoiceDto value)
        {
            this.code = code;

        }
        InvoiceDto IInvoiceData.Value { get ; set ; }
        public IEnumerable<InvoiceItem> InvoiceItems { set; get; }
    }
}
