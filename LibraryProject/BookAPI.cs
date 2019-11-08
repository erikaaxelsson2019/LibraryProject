using DataInterface;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryProject
{
    public class BookAPI
    {
        private IShelfManager shelfManager;
        private IBookManager bookManager;
        private ILoanManager loanManager;

        public BookAPI(IShelfManager shelfManager, IBookManager bookManager)
        {
            this.shelfManager = shelfManager;
            this.bookManager = bookManager;
        }

        public BookAPI(IBookManager bookManager, ILoanManager loanManager)
        {
            this.bookManager = bookManager;
            this.loanManager = loanManager;
        }

        public BookStatusCodes AddBook(string bookTitle, string bookAuthor, string isbnNumber, int bookPrice, int purchaseYear, int bookCondition, Shelf shelf, bool isLoaned)
        {
            var existingBook = bookManager.GetBookByBookTitle(bookTitle);

            if (existingBook != null)
                return BookStatusCodes.BookAlreadyExist;
            if(CheckValidIsbn(isbnNumber) == false)
                return BookStatusCodes.InvalidIsbnNumber;
            bookManager.AddBook(bookTitle, bookAuthor, isbnNumber, bookPrice, purchaseYear, bookCondition, shelf, isLoaned);
            return BookStatusCodes.Ok;
        }

        public BookStatusCodes MoveBook(string bookTitle, int shelfNumber, int aisleNumber)
        {
            var newShelf = shelfManager.GetShelfByShelfNumber(shelfNumber, aisleNumber);

            if (newShelf == null)
                return BookStatusCodes.NoSuchShelf;
            var book = bookManager.GetBookByBookTitle(bookTitle);
            if (book == null)
                return BookStatusCodes.NoSuchBook;
            if (book.Shelf.ShelfNumber == shelfNumber)
                return BookStatusCodes.BookAlreadyExistInShelf;
            bookManager.MoveBook(book.BookID, newShelf.ShelfID);
            return BookStatusCodes.Ok;
        }

        public BookStatusCodes RemoveBook(string bookTitle)
        {
            var newBook = bookManager.GetBookByBookTitle(bookTitle);

            if (newBook == null)
                return BookStatusCodes.NoSuchBook;
            if (newBook.Loan.Count > 0)
                return BookStatusCodes.BookIsLoaned;
            bookManager.RemoveBook(newBook.BookID);
            return BookStatusCodes.Ok;
        }

       public static bool CheckValidIsbn(string isbnNumber)
       {
            bool result = false;

            if(!string.IsNullOrEmpty(isbnNumber))
            {
                long temp;
                if (isbnNumber.Length != 13 || !long.TryParse(isbnNumber, out temp)) return false;

                int sum = 0;
                for(int i = 0; i < 12; i++)
                {
                    sum += int.Parse(isbnNumber[i].ToString()) * (i % 2 == 1 ? 3 : 1);
                }

                int remainder = sum % 10;
                int checkDigit = 10 - remainder;
                if (checkDigit == 10) checkDigit = 0;

                result = (checkDigit == int.Parse(isbnNumber[12].ToString()));
            }

            return result;
       }
    }
}
