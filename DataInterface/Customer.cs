using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataInterface
{
   public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int CustomerID { get; set; }

        public string CustomerName { get; set; }

        public string CustomerAdress { get; set; }

        public string BirthDate { get; set; }

        public int Debt { get; set; }

        public Customer Guardian { get; set; }

        public ICollection<Loan> Loan { get; set; }
    }
}
