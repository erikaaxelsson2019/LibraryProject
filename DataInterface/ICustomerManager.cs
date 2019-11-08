using System;
using System.Collections.Generic;
using System.Text;

namespace DataInterface
{
    public interface ICustomerManager
    {
        public Customer AddCustomer(string customerName, string customerAdress, string birthDate, int debt, Customer guardian);
        public Customer GetCustomerByCustomerName(string customerName);
        public void RemoveCustomer(int customerID);
       
    }
}
