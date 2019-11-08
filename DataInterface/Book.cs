using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataInterface
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int BookID { get; set; }

        public string BookTitle { get; set; }

        public string BookAuthor { get; set; }

        public string IsbnNumber { get; set; } 

        public int BookPrice { get; set; }

        public int PurchaseYear { get; set; }

        public int BookCondition { get; set; }

        public bool IsLoaned { get; set; }

        public int ShelfID { get; set; }

        public Shelf Shelf { get; set; }

        public ICollection<Loan> Loan { get; set; }
    }
}
