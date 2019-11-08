using System;
using System.Collections.Generic;
using System.Text;

namespace DataInterface
{
    public interface ILoanManager
    {
        public Loan AddLoan(DateTime loanDate, DateTime returnDate, string customerName, string bookTitle);
        public Loan GetLoanByBookAndCustomer(int bookID, int customerID);
        public void ReturnLoanedBook(int loanID);
        public void ExtendLoan(int loanID, DateTime loanDate, DateTime returnDate);
        public List<Loan> GetLoanFromCustomer(string CustomerName);

    }
}
