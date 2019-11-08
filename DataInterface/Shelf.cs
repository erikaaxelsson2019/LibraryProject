using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataInterface
{
    public class Shelf
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ShelfID { get; set; }

        public int ShelfNumber { get; set; }

        public int AisleID { get; set; }

        public int AisleNumber { get; set; }

        public Aisle Aisle { get; set; }

        public ICollection<Book> Book { get; set; }
    }
}
