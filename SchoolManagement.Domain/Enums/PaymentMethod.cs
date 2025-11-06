using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Domain.Enums
{
    public enum PaymentMethod
    {
        Cash = 1,
        CreditCard = 2,
        DebitCard = 3,
        BankTransfer = 4,
        OnlinePayment = 5,
        Cheque = 6,
        UPI = 7
    }
}
