using DataInterface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace LibraryProject
{
    public class LoanAPI
    {
        private ILoanManager loanManager;
        private IBookManager bookManager;
        private ICustomerManager customerManager;

        public LoanAPI(ILoanManager loanManager, IBookManager bookManager, ICustomerManager customerManager)
        {
            this.loanManager = loanManager;
            this.bookManager = bookManager;
            this.customerManager = customerManager;
        }

        public LoanedBookStatusCodes AddLoan(string customerName, string bookTitle) 
        {
            var newLoanDate = DateTime.Today;
            var newReturnDate = DateTime.Today.AddDays(30);

            var newBook = bookManager.GetBookByBookTitle(bookTitle);
            if (newBook == null)
                return LoanedBookStatusCodes.NoSuchBook;
            var newCustomer = customerManager.GetCustomerByCustomerName(customerName);
            if (newCustomer == null)
                return LoanedBookStatusCodes.NoSuchCustomer;

            var existingLoan = loanManager.GetLoanByBookAndCustomer(newBook.BookID, newCustomer.CustomerID);
            if (newBook.IsLoaned == true)
                if (existingLoan != null && newCustomer.Debt == 0)
                {
                    loanManager.ExtendLoan(existingLoan.LoanID, newLoanDate, newReturnDate);
                    return LoanedBookStatusCodes.ExtendLoan;
                }
                else if (existingLoan != null && newCustomer.Debt > 0)
                {
                    return LoanedBookStatusCodes.CustomerHasDebtCanNotExtendLoan;
                }
                else
                {
                    return LoanedBookStatusCodes.BookIsLoaned;
                }
         
            if (loanManager.GetLoanFromCustomer(customerName).Count >= 5)
                return LoanedBookStatusCodes.CustomerHasToManyLoans;
            if (newCustomer.Debt > 0)
                return LoanedBookStatusCodes.CustomerHasDebt;
            loanManager.AddLoan(newLoanDate, newReturnDate, customerName, bookTitle);
            return LoanedBookStatusCodes.Ok;
        }
  
        public LoanedBookStatusCodes ReturnLoanedBook(string bookTitle, string customerName, int bookCondtion)
        {
            var newCustomer = customerManager.GetCustomerByCustomerName(customerName);
            var newBook = bookManager.GetBookByBookTitle(bookTitle);
            var existingLoan = loanManager.GetLoanByBookAndCustomer(newBook.BookID, newCustomer.CustomerID);
            int newBookCondition = bookCondtion;
          
            if (existingLoan == null)
                return LoanedBookStatusCodes.NoSuchLoan;
          
            else
            {
                loanManager.ReturnLoanedBook(existingLoan.LoanID);
                bookManager.UpdateBookCondition(newBook.BookID, newBookCondition);
                return LoanedBookStatusCodes.Ok;
            }
        }

        public bool IsLoaned(int bookID)
        {
            var loanedBook = bookManager.GetBookFromLoan(bookID);
            if (loanedBook != null)
                return false; 
            else
                return true; 
        }
    }
}
