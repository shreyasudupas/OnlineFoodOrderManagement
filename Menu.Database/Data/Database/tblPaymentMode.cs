using System;
using System.Collections.Generic;
using System.Text;

namespace OrderAPI.BuisnessLayer.DBModels
{
    public class tblPaymentMode
    {
        public int PaymentModeId { get; set; }
        public string PaymenentType { get; set; }
        public bool isActive { get; set; }
    }
}
