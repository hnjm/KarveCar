﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using KarveCommon.Generic;
using Prism.Commands;

namespace KarveControls.UIObjects
{
    public class UiEmailDataField: UiDfObject
    {
        public string ButtonImage { get; set; }
        private string _textContent = "";
        public delegate void EmailCheckHandler(string email);

        public event EmailCheckHandler EmailEventHandler;
        public ICommand EmailCommand { get; set; }
        /// <summary>
        ///  UiEmailData Field.
        /// </summary>
        public UiEmailDataField()
        {
            this.EmailCommand = new DelegateCommand<object>(OnEmailOpen);
            DataAllowed = DataType.Email;
        }
        /// <summary>
        ///  TextContent. 
        /// </summary>
        public new string TextContent
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                _textContent = value;
             
            }
            get
            {
                return _textContent;
            }
        }

        private void OnEmailOpen(object var)
        {
            string email = (string) var;
            EmailEventHandler?.Invoke(email);
        }
    }
}
