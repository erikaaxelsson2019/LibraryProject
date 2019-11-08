using System;
using System.Collections.Generic;
using System.Text;

namespace DataInterface
{
    public enum CustomerStatusCodes
    {
        NoSuchCustomer,
        CustomerAldreadyExist,
        InvalidBirthDate,
        CustomerHasLoan,
        CustomerHasDebt,
        CustomerIsJuvenile,
        Ok
    }
}
