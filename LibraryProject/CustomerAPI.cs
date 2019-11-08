using DataInterface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace LibraryProject
{
    public class CustomerAPI
    {
        private ICustomerManager customerManager;

        public CustomerAPI(ICustomerManager customerManager)
        {
            this.customerManager = customerManager;
        }

        public CustomerStatusCodes AddCustomer(string customerName, string customerAdress, string birthDate, int debt, Customer guardian)
        {
            var existingCustomer = customerManager.GetCustomerByCustomerName(customerName);
            
            if (existingCustomer != null)
                return CustomerStatusCodes.CustomerAldreadyExist;
            if(CheckValidDate(birthDate) == false)
                return CustomerStatusCodes.InvalidBirthDate;
            if (GetAge(birthDate) < 15)
            {
                customerManager.AddCustomer(customerName, customerAdress, birthDate, debt, guardian);
                return CustomerStatusCodes.CustomerIsJuvenile;
            }
                
            customerManager.AddCustomer(customerName, customerAdress, birthDate, debt, null);
            return CustomerStatusCodes.Ok;
        }

        public CustomerStatusCodes RemoveCustomer(string customerName)
        {
            var newCustomer = customerManager.GetCustomerByCustomerName(customerName);

            if (newCustomer == null)
                return CustomerStatusCodes.NoSuchCustomer;
            if (newCustomer.Loan.Count > 0)
                return CustomerStatusCodes.CustomerHasLoan;
            if (newCustomer.Debt > 0)
                return CustomerStatusCodes.CustomerHasDebt;
            customerManager.RemoveCustomer(newCustomer.CustomerID);
            return CustomerStatusCodes.Ok;
        }

       public int GetAge(string birthDate)
       {
            DateTime temp;
            DateTime.TryParse(birthDate, out temp);

            int now = int.Parse(DateTime.Now.ToString("yyyymmdd"));
            int dateOfBirth = int.Parse(temp.ToString("yyyymmdd"));
            int age = (now - dateOfBirth) / 10000;
            return age;
       }

        public bool CheckValidDate(string birthDate)
        {
            DateTime temp;

            if (DateTime.TryParseExact(birthDate, "yyyy-M-d", null, DateTimeStyles.None, out temp) == true)
                return true;
            else
                return false;           
        }
    }
}
