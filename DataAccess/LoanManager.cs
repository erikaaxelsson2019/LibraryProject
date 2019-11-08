using DataInterface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;


namespace DataAccess
{
    public class LoanManager : ILoanManager
    {
        public Loan AddLoan(DateTime loanDate, DateTime returnDate, string customerName, string bookTitle)
        {
            using (var libraryContext = new LibraryContext())
            {
                var customer = (from c in libraryContext.Customers
                                where c.CustomerName == customerName
                                select c).First();

                var book = (from b in libraryContext.Books
                            where b.BookTitle == bookTitle
                            select b).First();

                Loan loan = new Loan();
                loan.LoanDate = loanDate;
                loan.ReturnDate = returnDate;
                loan.CustomerID = customer.CustomerID;
                loan.BookID = book.BookID;
                libraryContext.Loans.Add(loan);
                libraryContext.SaveChanges();
                return loan;

            }
        }

        public Loan GetLoanByBookAndCustomer(int bookID, int customerID)
        {
            using var context = new LibraryContext();
            return (from b in context.Books
                    join l in context.Loans
                    on b.BookID equals l.BookID
                    join c in context.Customers
                    on l.CustomerID equals c.CustomerID
                    where b.BookID == bookID && c.CustomerID == customerID
                    select l).FirstOrDefault();
        }

        public void ReturnLoanedBook(int loanID)
        {
            using var context = new LibraryContext();
            var loan = (from l in context.Loans
                        where l.LoanID == loanID
                        select l).FirstOrDefault();
            context.Loans.Remove(loan);
            context.SaveChanges();
        }

        public void ExtendLoan(int loanID, DateTime loanDate, DateTime returnDate)
        {
            using var context = new LibraryContext();
            var loan = (from l in context.Loans
                        where l.LoanID == loanID
                        select l).First();
            loan.LoanDate = loanDate;
            loan.ReturnDate = returnDate;
            context.SaveChanges();
        }

        public List<Loan> GetLoanFromCustomer(string customerName)
        {
            using var context = new LibraryContext();
            var customer = (from c in context.Customers
                        where c.CustomerName == customerName
                        select c).First();

            return (from l in context.Loans
                    where l.CustomerID == customer.CustomerID
                    select l).ToList();
        }
    }
}
