using DataInterface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class BookManager : IBookManager
    {
        public Book AddBook(string bookTitle, string bookAuthor, string isbnNumber, int bookPrice, int purchaseYear, int bookCondition, Shelf shelf, bool isLoaned)
        {
            using (var libraryContext = new LibraryContext())  
            {
                Book book = new Book();
                book.BookTitle = bookTitle;
                book.BookAuthor = bookAuthor;
                book.IsbnNumber = isbnNumber;
                book.BookPrice = bookPrice;
                book.PurchaseYear = purchaseYear;
                book.BookCondition = bookCondition;
                book.ShelfID = shelf.ShelfID;
                book.IsLoaned = isLoaned;
                libraryContext.Books.Add(book);
                libraryContext.SaveChanges();
                return book;

            }
        }

        public Book GetBookByBookTitle(string bookTitle) 
        {
            using var context = new LibraryContext();
            return (from b in context.Books
                    where b.BookTitle == bookTitle
                    select b)
                         .Include(b => b.Shelf)
                         .FirstOrDefault();
        }

        public Book GetBookFromLoan(int bookID)
        {
            using var context = new LibraryContext();
            return (from l in context.Loans
                    join b in context.Books
                    on l.BookID equals b.BookID
                    where l.BookID == bookID
                    select b).First();
        }

        public Book GetBookByBookCondtion(int bookCondition)
        {
            using var context = new LibraryContext();
            return (from b in context.Books
                    where b.BookCondition == bookCondition
                    select b)
                         .Include(b => b.Shelf)
                         .FirstOrDefault();
        }

        public void MoveBook(int bookID, int shelfID)
        {
            using var context = new LibraryContext();
            var book = (from b in context.Books
                         where b.BookID == bookID
                         select b)
                         .First();
            book.ShelfID = shelfID;
            context.SaveChanges();
        }

        public void RemoveBook(int bookID)
        {
            using var context = new LibraryContext();
            var book = (from b in context.Books
                         where b.BookID == bookID
                         select b).FirstOrDefault();
            context.Books.Remove(book);
            context.SaveChanges();
        }

        public void UpdateBookCondition(int bookID, int bookCondition)
        {
            using var context = new LibraryContext();
            var book = (from b in context.Books
                        where b.BookID == bookID
                        select b).First();
            book.BookCondition = bookCondition;
            context.SaveChanges();
        }
    }
}
