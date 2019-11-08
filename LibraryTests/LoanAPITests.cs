using DataInterface;
using LibraryProject;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;


namespace UnitTests
{
    [TestClass]
    public class LoanAPITests
    {
        private static Mock<ILoanManager> SetupMock(Loan loan)
        {
            var loanManagerMock = new Mock<ILoanManager>();

            loanManagerMock.Setup(m =>
                    m.GetLoanByBookAndCustomer(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(loan);

            loanManagerMock.Setup(m =>
                m.AddLoan(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>()));
            return loanManagerMock;
        }

        private static Mock<ICustomerManager> SetupMock(Customer customer)
        {
            var customerManagerMock = new Mock<ICustomerManager>();

            customerManagerMock.Setup(m =>
                    m.GetCustomerByCustomerName(It.IsAny<string>()))
                .Returns(customer);

            customerManagerMock.Setup(m =>
                m.AddCustomer(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<Customer>()));
            return customerManagerMock;
        }

        private static Mock<IBookManager> SetupMock(Book book)
        {
            var bookManagerMock = new Mock<IBookManager>();

            bookManagerMock.Setup(m =>
                    m.GetBookByBookTitle(It.IsAny<string>()))
                .Returns(book);

            bookManagerMock.Setup(m =>
                m.AddBook(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(),
                It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Shelf>(), It.IsAny<bool>()));
            return bookManagerMock;
        }

        [TestMethod]
        public void TestAddLoanOk()
        {
            Mock<ILoanManager> loanManagerMock = SetupMock((Loan)null);
            var customerManagerMock = SetupMock(new Customer { CustomerName = "Erika Axelsson" });
            var bookManagerMock = SetupMock(new Book
            {
                BookID = 1,
                BookTitle = "Peter Pan",
                IsLoaned = false
            });

            loanManagerMock.Setup(m =>
                  m.GetLoanFromCustomer(It.IsAny<string>()))
               .Returns(new List<Loan>
               {
                   new Loan
                   {

                   }    
               });

            var successfull = AddLoanNumberOne(loanManagerMock);
            Assert.AreEqual(LoanedBookStatusCodes.Ok, successfull);
            loanManagerMock.Verify(
                m => m.AddLoan(It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.Is<string>(i => i == "Erika Axelsson"), It.Is<string>(i => i == "Peter Pan")),
                    Times.Once());
        }

        private LoanedBookStatusCodes AddLoanNumberOne(Mock<ILoanManager> loanManagerMock) 
        {
            var customerManagerMock = SetupMock(new Customer { CustomerName = "Erika Axelsson" });
            var bookManagerMock = SetupMock(new Book
            {
                BookTitle = "Peter Pan",
                IsLoaned = false
            });

            var loanAPI = new LoanAPI(loanManagerMock.Object, bookManagerMock.Object, customerManagerMock.Object);
            var successfull = loanAPI.AddLoan("Erika Axelsson", "Peter Pan");
            return successfull; 
        }

        [TestMethod]
        public void TestAddLoanNoSuchBook()
        {
            Mock<ILoanManager> loanManagerMock = SetupMock(new Loan());
            var customerManagerMock = SetupMock(new Customer 
            { 
                CustomerID = 4,
                CustomerName = "Lovisa Lund" 
            });
            var bookManagerMock = SetupMock((Book)null);
            
            var loanApi = new LoanAPI(null, bookManagerMock.Object, customerManagerMock.Object);
            var result = loanApi.AddLoan("Lovisa Lund", "ABC Boken");
            Assert.AreEqual(LoanedBookStatusCodes.NoSuchBook, result);
            loanManagerMock.Verify(
              m => m.AddLoan(It.IsAny<DateTime>(), It.IsAny<DateTime>(),
              It.Is<string>(i => i == "Lovisa Lund"), It.Is<string>(i => i == "ABC")),
                  Times.Never());
        }

        [TestMethod]
        public void TestAddLoanNoSuchCustomer()
        {
            Mock<ILoanManager> loanManagerMock = SetupMock(new Loan());
            var customerManagerMock = SetupMock((Customer)null);
            var bookManagerMock = SetupMock(new Book
            {
                BookID = 4,
                BookTitle = "Moby Dick"
            });
           
            var loanApi = new LoanAPI(null, bookManagerMock.Object, customerManagerMock.Object);
            var result = loanApi.AddLoan("Bobo Krut", "Moby Dick");
            Assert.AreEqual(LoanedBookStatusCodes.NoSuchCustomer, result);
            loanManagerMock.Verify(
               m => m.AddLoan(It.IsAny<DateTime>(), It.IsAny<DateTime>(),
               It.Is<string>(i => i == "Gunvor Gnut"), It.Is<string>(i => i == "Moby Dick")),
                   Times.Never());
        }

        [TestMethod]
        public void TestAddExistingLoan()
        {
            var loanManagerMock = new Mock<ILoanManager>();
            var customerManagerMock = SetupMock(new Customer { CustomerName = "Lovisa Lund"});
            var bookManagerMock = SetupMock(new Book
            {
                BookTitle = "Peter Pan",
                IsLoaned = true
            });

            var loanApi = new LoanAPI(loanManagerMock.Object, bookManagerMock.Object, customerManagerMock.Object);
            var result = loanApi.AddLoan("Lovisa Lund", "Peter Pan");
            Assert.AreEqual(LoanedBookStatusCodes.BookIsLoaned, result);
            loanManagerMock.Verify(
                m => m.AddLoan(It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.Is<string>(i => i == "Lovisa Lund"), It.Is<string>(i => i == "Peter Pan")), 
                    Times.Never());
        }

        [TestMethod]
        public void TestAddExistingLoanExtend() 
        {
            Mock<ILoanManager> loanManagerMock = SetupMock(new Loan());
            var customerManagerMock = SetupMock(new Customer { CustomerName = "Erika Axelsson" });
            var bookManagerMock = SetupMock(new Book
            {
                BookTitle = "Peter Pan",
                IsLoaned = true
            });

            var loanApi = new LoanAPI(loanManagerMock.Object, bookManagerMock.Object, customerManagerMock.Object);
            var result = loanApi.AddLoan("Erika Axelsson", "Peter Pan");
            Assert.AreEqual(LoanedBookStatusCodes.ExtendLoan, result);
        }

        [TestMethod]
        public void TestCustomerHasToManyLoans()
        {
            Mock<ILoanManager> loanManagerMock = SetupMock(new Loan());
            var customerManagerMock = SetupMock(new Customer { CustomerName = "Erika Axelsson" });
            var bookManagerMock = SetupMock(new Book
            {
                BookID = 7,
                BookTitle = "Oliver Twist"
            });

            loanManagerMock.Setup(m =>
            m.GetLoanFromCustomer(It.IsAny<string>()))
                .Returns(new List<Loan>
                {
                    new Loan
                    {
                        CustomerID = 1,
                        BookID = 1
                    },
                    new Loan
                    {
                        CustomerID = 1,
                        BookID = 2
                    },
                    new Loan
                    {
                        CustomerID = 1,
                        BookID = 3
                    },
                    new Loan
                    {
                        CustomerID = 1,
                        BookID = 4
                    },
                    new Loan
                    {
                        CustomerID = 1,
                        BookID = 5
                    },
                });

            var loanApi = new LoanAPI(loanManagerMock.Object, bookManagerMock.Object, customerManagerMock.Object);
            var result = loanApi.AddLoan("Erika Axelsson", "Oliver Twist");
            Assert.AreEqual(LoanedBookStatusCodes.CustomerHasToManyLoans, result);
            loanManagerMock.Verify(
                m => m.AddLoan(It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.Is<string>(i => i == "Erika Axelsson"), It.Is<string>(i => i == "Oliver Twist")),
                    Times.Never());
        }

        [TestMethod]
        public void TestReturnLoanedBook() 
        {
            var customerManagerMock = SetupMock(new Customer { CustomerName = "Erika Axelsson" });
            var bookManagerMock = SetupMock(new Book { BookTitle = "Peter Pan" });
            var loanManagerMock = SetupMock(new Loan
            {
                LoanID = 1,
                Book = new Book()
            });

            var loanAPI = new LoanAPI(loanManagerMock.Object, bookManagerMock.Object, customerManagerMock.Object);
            var successfull = loanAPI.ReturnLoanedBook("Peter Pan", "Erika Axelsson", 2);
            Assert.AreEqual(LoanedBookStatusCodes.Ok, successfull);
            loanManagerMock.Verify(m =>
                m.ReturnLoanedBook(It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public void TestReturnNonExistingLoan()
        {
            var customerManagerMock = SetupMock(new Customer { CustomerName = "Erika Axelsson" });
            var bookManagerMock = SetupMock(new Book { BookTitle = "ABC" });
            var loanManagerMock = SetupMock((Loan)null);

            var loanAPI = new LoanAPI(loanManagerMock.Object, bookManagerMock.Object, customerManagerMock.Object);
            var successfull = loanAPI.ReturnLoanedBook("ABC", "Erika Axelsson", 4);
            Assert.AreEqual(LoanedBookStatusCodes.NoSuchLoan, successfull);
            loanManagerMock.Verify(m =>
               m.ReturnLoanedBook(It.IsAny<int>()), Times.Never);
        }
    }
}
