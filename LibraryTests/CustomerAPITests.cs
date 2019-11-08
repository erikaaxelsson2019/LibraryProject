using DataInterface;
using LibraryProject;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;



namespace UnitTests
{
    [TestClass]
    public class CustomerAPITests
    {
        [TestMethod]
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

        [TestMethod]
        public void TestAddCustomer()
        {
            Mock<ICustomerManager> customerManagerMock = SetupMock((Customer)null);
            var successfull = AddCustomerNumberOne(customerManagerMock);
            Assert.AreEqual(CustomerStatusCodes.Ok, successfull);
            customerManagerMock.Verify(
                m => m.AddCustomer(It.Is<string>(i => i == "Erika Axelsson"), It.Is<string>(i => i == "Restalundsvägen 2"),
                It.Is<string>(i => i == "1996-4-4"), It.IsAny<int>(), It.Is<Customer>(i => i == null)),
                    Times.Once());
        }

        private CustomerStatusCodes AddCustomerNumberOne(Mock<ICustomerManager> customerManagerMock)
        {
            var customerAPI = new CustomerAPI(customerManagerMock.Object);
            var successfull = customerAPI.AddCustomer("Erika Axelsson", "Restalundsvägen 2", "1996-4-4", 0, null);
            return successfull;
        }

        [TestMethod]
        public void TestTestAddExistingCustomer()
        {
            var customerManagerMock = SetupMock(new Customer());
            var successfull = AddCustomerNumberOne(customerManagerMock);
            Assert.AreEqual(CustomerStatusCodes.CustomerAldreadyExist, successfull);
            customerManagerMock.Verify(
                m => m.AddCustomer(It.Is<string>(i => i == "Erika Axelsson"), It.Is<string>(i => i == "Restalundsvägen 2"),
                It.Is<string>(i => i == "1996-4-4"), It.IsAny<int>(), It.Is<Customer>(i => i == null)), 
                    Times.Never());
        }

        [TestMethod]
        public void TestRemoveUnActiveCustomer()
        {
            var customerManagerMock = new Mock<ICustomerManager>();
           
            customerManagerMock.Setup(m =>
          m.GetCustomerByCustomerName(It.IsAny<string>()))
                .Returns(new Customer
                {

                    CustomerName = "Britta Bo",
                    Loan = new List<Loan>()

                });

            var customerAPI = new CustomerAPI(customerManagerMock.Object);
            var successfull = customerAPI.RemoveCustomer("Britta Bo");
            Assert.AreEqual(CustomerStatusCodes.Ok, successfull);
            customerManagerMock.Verify(m =>
                m.RemoveCustomer(It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public void TestRemoveCustomerWithLoan()
        {
            var customerManagerMock = new Mock<ICustomerManager>();
            var loanManagerMock = new Mock<ILoanManager>();

            customerManagerMock.Setup(m =>
          m.GetCustomerByCustomerName(It.IsAny<string>()))
                .Returns(new Customer
                {

                    CustomerName = "Lovisa Lund",
                    Loan = new List<Loan>
                    {
                        new Loan()
                    }

                });

            var customerAPI = new CustomerAPI(customerManagerMock.Object);
            var successfull = customerAPI.RemoveCustomer("Lovisa Lund");
            Assert.AreEqual(CustomerStatusCodes.CustomerHasLoan, successfull);
            customerManagerMock.Verify(m =>
               m.RemoveCustomer(It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public void TestRemoveNonExistingCustomer()
        {
            var customerManagerMock = new Mock<ICustomerManager>();

            customerManagerMock.Setup(m =>
          m.GetCustomerByCustomerName(It.IsAny<string>()))
                .Returns((Customer)null);

            var customerAPI = new CustomerAPI(customerManagerMock.Object);
            var successfull = customerAPI.RemoveCustomer("Ola Lola");
            Assert.AreEqual(CustomerStatusCodes.NoSuchCustomer, successfull);
            customerManagerMock.Verify(m =>
               m.RemoveCustomer(It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public void TestGetAgeOk()
        {
            var customerManagerMock = new Mock<ICustomerManager>();
            var customerAPI = new CustomerAPI(customerManagerMock.Object);
            var result = customerAPI.GetAge("1996-4-4");
            Assert.AreEqual(23, result);
        }

        [TestMethod]
        public void TestGetAgeFail()
        {
            var customerManagerMock = new Mock<ICustomerManager>();
            var customerAPI = new CustomerAPI(customerManagerMock.Object);
            var result = customerAPI.GetAge("1996-4-4");
            Assert.AreNotEqual(24, result);
        }

        [TestMethod]
        public void TestValitadeDateOk()
        {
            var customerManagerMock = new Mock<ICustomerManager>();
            var customerAPI = new CustomerAPI(customerManagerMock.Object);
            bool result = customerAPI.CheckValidDate("1996-4-4");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestValidateDateFail()
        {
            var customerManagerMock = new Mock<ICustomerManager>();
            var customerAPI = new CustomerAPI(customerManagerMock.Object);
            bool result = customerAPI.CheckValidDate("4-4-1996");
            Assert.IsFalse(result);
        }
    }
}
