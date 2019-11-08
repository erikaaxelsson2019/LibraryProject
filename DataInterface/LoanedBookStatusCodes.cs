using System;
using System.Collections.Generic;
using System.Text;

namespace DataInterface
{
    public enum LoanedBookStatusCodes
    {
        NoSuchCustomer,
        NoSuchBook, 
        NoSuchLoan, 
        BookIsLoaned, 
        ExtendLoan, 
        CustomerHasToManyLoans, 
        CustomerHasDebtCanNotExtendLoan,
        CustomerHasDebt,
        Ok 
    }
}
