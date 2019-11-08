using DataInterface;
using LibraryProject;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace UnitTests
{
    [TestClass]
    public class BookAPITests
    {
        private static Mock<IShelfManager> SetupMock(Shelf shelf)
        {
            var bookManagerMock = new Mock<IShelfManager>();

            bookManagerMock.Setup(m =>
                    m.GetShelfByShelfNumber(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(shelf);

            bookManagerMock.Setup(m =>
                m.AddShelf(It.IsAny<int>(), It.IsAny<int>()));
            return bookManagerMock;
        }

        private static Mock<IBookManager> SetupMock(Book book)
        {
            var bookManagerMock = new Mock<IBookManager>();

            bookManagerMock.Setup(m =>
                    m.GetBookByBookTitle(It.IsAny<string>()))
                .Returns(book);

            bookManagerMock.Setup(m =>
                m.AddBook(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(),
                It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Shelf>(), It.IsAny<bool>()));
            return bookManagerMock;
        }

        [TestMethod]
        public void TestAddBook()
        {
            Mock<IBookManager> bookManagerMock = SetupMock((Book)null);
            var successfull = AddBookNumberOne(bookManagerMock);
            Assert.AreEqual(BookStatusCodes.Ok, successfull);
            bookManagerMock.Verify(
                m => m.AddBook(It.Is<string>( i => i == "Clean Code"), It.Is<string>(i => i == "Robert C. Martin"),
                It.Is<string>(i => i == "9780132350884"), It.Is<int>(i => i == 452), It.Is<int>(i => i == 2019),
                It.Is<int>(i => i == 5), It.IsAny<Shelf>(), It.Is<bool>(i => i == false)),
                    Times.Once());
        }

        private BookStatusCodes AddBookNumberOne(Mock<IBookManager> bookManagerMock)
        {
            var shelfManagerMock = new Mock<IShelfManager>();

            var bookAPI = new BookAPI(shelfManagerMock.Object, bookManagerMock.Object);
            var successfull = bookAPI.AddBook("Clean Code", "Robert C. Martin", "9780132350884", 452, 2019, 5, new Shelf(), false);
            return successfull;
        }

        [TestMethod]
        public void TestAddExistingBook()
        {
            var bookManagerMock = SetupMock(new Book());
            var successfull = AddBookNumberOne(bookManagerMock);
            Assert.AreEqual(BookStatusCodes.BookAlreadyExist, successfull);
            bookManagerMock.Verify(
                m => m.AddBook(It.Is<string>(i => i == "Clean Code"), It.Is<string>(i => i == "Robert C. Martin"),
                It.Is<string>(i => i == "9780132350884"), It.Is<int>(i => i == 452), It.Is<int>(i => i == 2019), 
                It.Is<int>(i => i == 5), It.IsAny<Shelf>(), It.Is<bool>(i => i == false)),
                    Times.Never());
        }

        [TestMethod]
        public void TestMoveBookOk()
        {
            var shelfManagerMock = new Mock<IShelfManager>();
            var bookManagerMock = new Mock<IBookManager>();

            shelfManagerMock.Setup(m =>
            m.GetShelfByShelfNumber(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new Shelf { ShelfID = 2 });

            bookManagerMock.Setup(m =>
            m.GetBookByBookTitle(It.IsAny<string>()))
                .Returns(new Book
                {
                    BookID = 2,
                    Shelf = new Shelf()
                });

            var bookAPI = new BookAPI(shelfManagerMock.Object, bookManagerMock.Object);
            var result = bookAPI.MoveBook("Moby Dick", 101, 2);
            Assert.AreEqual(BookStatusCodes.Ok, result);
            bookManagerMock.Verify(m =>
            m.MoveBook(2, 2), Times.Once());
        }

        [TestMethod]
        public void TestMoveBookNoSuchBook()
        {
            var shelfManagerMock = SetupMock(new Shelf { ShelfNumber = 101 });
            var bookManagerMock = SetupMock((Book)null);
           
            var bookApi = new BookAPI(shelfManagerMock.Object, bookManagerMock.Object);
            var result = bookApi.MoveBook("ABC", 101, 3);
            Assert.AreEqual(BookStatusCodes.NoSuchBook, result);
            bookManagerMock.Verify(
               m => m.MoveBook(It.Is<int>(i => i == 9), It.Is<int>(i => i == 3)),
                   Times.Never());
        }

        [TestMethod]
        public void TestMoveBookNoSuchShelf()
        {
            var shelfManagerMock = SetupMock((Shelf)null);
            var bookManagerMock = SetupMock(new Book
            {
                BookID = 4,
                BookTitle = "Moby Dick"
            });

            var bookApi = new BookAPI(shelfManagerMock.Object, bookManagerMock.Object);
            var result = bookApi.MoveBook("Moby Dick", 101, 3);
            Assert.AreEqual(BookStatusCodes.NoSuchShelf, result);
            bookManagerMock.Verify(
               m => m.MoveBook(It.Is<int>(i => i == 4), It.Is<int>(i => i == 12)),
                   Times.Never());
        }

        [TestMethod]
        public void TestRemoveLonelyBook() 
        {
            var bookManagerMock = new Mock<IBookManager>();
            var loanManagerMock = new Mock<ILoanManager>();

            bookManagerMock.Setup(m =>
          m.GetBookByBookTitle(It.IsAny<string>()))
                .Returns(new Book
                {

                    BookTitle = "Oliver Twist",
                    Loan = new List<Loan>()

                });

            var libraryAPI = new BookAPI(bookManagerMock.Object, loanManagerMock.Object);
            var successfull = libraryAPI.RemoveBook("Oliver Twist");
            Assert.AreEqual(BookStatusCodes.Ok, successfull);
            bookManagerMock.Verify(m =>
                m.RemoveBook(It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public void TestRemoveBookThatIsLoaned()
        {
            var bookManagerMock = new Mock<IBookManager>();
            var loanManagerMock = new Mock<ILoanManager>();

            bookManagerMock.Setup(m =>
          m.GetBookByBookTitle(It.IsAny<string>()))
                .Returns(new Book
                {

                    BookTitle = "Middagstipset",
                    Loan = new List<Loan>
                    {
                        new Loan()
                    }

                });

            var bookAPI = new BookAPI(bookManagerMock.Object, loanManagerMock.Object);
            var successfull = bookAPI.RemoveBook("Middagstipset");
            Assert.AreEqual(BookStatusCodes.BookIsLoaned, successfull);
            bookManagerMock.Verify(m =>
               m.RemoveBook(It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public void TestRemoveNonExistingBook()
        {
            var bookManagerMock = new Mock<IBookManager>();
            var loanManagerMock = new Mock<ILoanManager>();


            bookManagerMock.Setup(m =>
               m.GetBookByBookTitle(It.IsAny<string>()))
                .Returns((Book)null);

            var bookAPI = new BookAPI(bookManagerMock.Object, loanManagerMock.Object);
            var successfull = bookAPI.RemoveBook("ABC");
            Assert.AreEqual(BookStatusCodes.NoSuchBook, successfull);
            bookManagerMock.Verify(m =>
               m.RemoveBook(It.IsAny<int>()), Times.Never);
        }
    }
}
