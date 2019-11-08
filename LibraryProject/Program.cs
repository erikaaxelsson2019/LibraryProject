using DataAccess;
using DataInterface;
using System;
using System.Linq;

namespace LibraryProject
{
    class Program
    {
        static void Main(string[] args)
        {
            IAisleManager aisleManager = new AisleManager();
            aisleManager.AddAisle(1); 
            aisleManager.AddAisle(2);
            aisleManager.AddAisle(3);
            aisleManager.AddAisle(4);

            IShelfManager shelfManager = new ShelfManager();
            var shelf1 = shelfManager.AddShelf(101, 1);
            var shelf2 = shelfManager.AddShelf(102, 1);
            var shelf3 = shelfManager.AddShelf(101, 2);
            var shelf4 = shelfManager.AddShelf(101, 3);
            var shelf5 = shelfManager.AddShelf(102, 3);
            var shelf6 = shelfManager.AddShelf(103, 3);

            IBookManager bookManager = new BookManager();
            bookManager.AddBook("Clean Code", "Robert C. Martin", "9780132350884", 452, 2019, 5, shelf1, false);
            bookManager.AddBook("Peter Pan", "Barrie, J. M.", "9781405279574", 258, 2003, 3, shelf1, true);
            bookManager.AddBook("Harry Potter och frången från Azkaban", "J.K. Rowling", "9789129704211", 243, 2017, 4, shelf1, false);
            bookManager.AddBook("Harrp Potter och den vises sten", "J.K. Rowling", "9789129697704", 243, 2015, 3, shelf1, false);
            bookManager.AddBook("Bränn alla mina brev", "Alex Schulman", "9789188745583", 58, 2019, 5, shelf2, false);
            bookManager.AddBook("1984", "George Orwell", "9780451524935", 92, 1950, 1, shelf2, false);
            bookManager.AddBook("Oliver Twist", "Charles Dickens", "9789163802225", 28, 1994, 1, shelf2, false);
            bookManager.AddBook("Lord of the rings box set", "J.R.R. Tolkien", "9780007581146", 937, 2014, 2, shelf2, false);
            bookManager.AddBook("Moby Dick", "Herman Melville", "9789187193170", 89, 2016, 3, shelf3, false);
            bookManager.AddBook("Lord of the flies", "William Goldberg", "9780571200535", 85, 1999, 2, shelf3, false);
            bookManager.AddBook("To Kill a Mockingbird", "Harper Lee", "9781784752637", 98, 2015, 4, shelf4, false);
            bookManager.AddBook("Hjärnstark", "Anders Hansen", "9789175038452", 58, 2018, 5, shelf4, true);
            bookManager.AddBook("Gone Girl", "Gillian Flynn", "9780753827666", 76, 2013, 4, shelf4, false);
            bookManager.AddBook("Middagstipset", "Jenny Warsén", "9789174247947", 180, 2018, 5, shelf5, true);
            bookManager.AddBook("Omgiven av idioter", "Thomas Erikson", "9789175038407", 58, 2018, 5, shelf5, true);
            bookManager.AddBook("Hon som måste dö", "David Lagercrantz", "9789113073743", 189, 2019, 5, shelf5, false);

            ICustomerManager customerManager = new CustomerManager(); 
            customerManager.AddCustomer("Erika Axelsson", "Restalundsvägen 2", "1996-4-4", 0, null);
            var guardian1 = customerManager.AddCustomer("Knut Knutsson", "Rudbecksgatan 10", "1974-10-10", 0, null);
            var guardian2 = customerManager.AddCustomer("Johan Johansson", "Kungsgatan 1", "1982-1-1", 0, null);
            var guardian3 = customerManager.AddCustomer("Isak Isaksson", "Trädgårdsgatan 6", "1976-10-23", 0, null);
            customerManager.AddCustomer("Anders Andersson", "Drottninggatan 1", "1965-5-10", 0, null);
            customerManager.AddCustomer("Lovisa Lund", "Peppargatan 13", "1992-4-11", 0, null);
            customerManager.AddCustomer("Rebecca Rudolfsson", "Sörbyvägen 3", "1970-1-10", 75, null);
            customerManager.AddCustomer("Britta Bo", "Oskarsvägen 7", "1950-7-17", 0, null);

            customerManager.AddCustomer("Kasper Knutsson", "RudbecksGatan 10", "2007-1-15", 0, guardian1);
            customerManager.AddCustomer("Maja Johansson", "Kungsgatan 1", "2010-4-24", 0, guardian2);
            customerManager.AddCustomer("Lisa Isaksson", "Bolundstigen 11", "2006-8-8", 0, guardian3);

            ILoanManager loanManager = new LoanManager();  
            loanManager.AddLoan(DateTime.Parse("2019-10-21"), DateTime.Parse("2019-11-21"), "Erika Axelsson", "Peter Pan");
            loanManager.AddLoan(DateTime.Parse("2019-9-24"), DateTime.Parse("2019-10-24"), "Anders Andersson", "Omgiven av idioter");
            loanManager.AddLoan(DateTime.Parse("2019-8-30"), DateTime.Parse("2019-9-30"), "Lovisa Lund", "Middagstipset");
            loanManager.AddLoan(DateTime.Parse("2019-10-5"), DateTime.Parse("2019-11-5"), "Lovisa Lund", "Hjärnstark");
            loanManager.AddLoan(DateTime.Parse("2019-11-1"), DateTime.Parse("2019-12-1"), "Kapser Knutsson", "Lord of the flies");

            Console.WriteLine("Press enter:");
            Console.ReadLine();
        }
    }
}
