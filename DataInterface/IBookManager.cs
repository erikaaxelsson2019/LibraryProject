using System;
using System.Collections.Generic;
using System.Text;

namespace DataInterface
{
    public interface IBookManager
    {
        public Book AddBook(string bookTitle, string bookAuthor, string IsbnNumber, int bookPrice, int purchaseYear, int bookCondition, Shelf shelf, bool isLoaned);
        Book GetBookByBookTitle(string bookTitle);
        public Book GetBookFromLoan(int bookID);
        public void MoveBook(int bookID, int shelfID);
        public void RemoveBook(int bookID);
        public void UpdateBookCondition(int bookID, int bookCondition);

        //List<Book> GetScrapList();
        //Book GetBookByBookCondition(int bookCondition);
       
    }
}
