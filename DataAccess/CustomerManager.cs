using DataInterface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace DataAccess
{
   public class CustomerManager : ICustomerManager
    {
        public Customer AddCustomer(string customerName, string customerAdress, string birthDate, int debt, Customer guardian)
        {
            using (var libraryContext = new LibraryContext())
            {
                Customer customer = new Customer();
                customer.CustomerName = customerName;
                customer.CustomerAdress = customerAdress;
                customer.BirthDate = birthDate;
                customer.Debt = debt;
                customer.CustomerID = guardian.CustomerID;
                libraryContext.Customers.Add(customer);
                libraryContext.SaveChanges();
                return customer;
            }
        }

        public Customer GetCustomerByCustomerName(string customerName)
        {
            using var context = new LibraryContext();
            return (from c in context.Customers
                    where c.CustomerName == customerName
                    select c)
                    .First();
        }

        public void RemoveCustomer(int customerID)
        {
            using var context = new LibraryContext();
            var customer = (from c in context.Customers
                         where c.CustomerID == customerID
                         select c).FirstOrDefault();
            context.Customers.Remove(customer);
            context.SaveChanges();
        }
    }
}
