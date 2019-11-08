using DataInterface;
using LibraryProject;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace LibraryTests
{
    [TestClass]
    public class LibraryAPITests
    {
        private static Mock<IAisleManager> SetupMock(Aisle aisle)
        {
            var libraryManagerMock = new Mock<IAisleManager>();

            libraryManagerMock.Setup(m =>
                    m.GetAisleByAisleNumber(It.IsAny<int>()))
                .Returns(aisle);

            libraryManagerMock.Setup(m =>
                m.AddAisle(It.IsAny<int>()));
            return libraryManagerMock;
        }

        private static Mock<IShelfManager> SetupMock(Shelf shelf)
        {
            var libraryManagerMock = new Mock<IShelfManager>();

            libraryManagerMock.Setup(m =>
                    m.GetShelfByShelfNumber(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(shelf);

            libraryManagerMock.Setup(m =>
                m.AddShelf(It.IsAny<int>(), It.IsAny<int>()));
            return libraryManagerMock;
        }


        [TestMethod]
        public void TestAddAisle()
        {
            Mock<IAisleManager> libraryManagerMock = SetupMock((Aisle)null);
            bool successfull = AddAisleNumberOne(libraryManagerMock);
            Assert.IsTrue(successfull);
            libraryManagerMock.Verify(
                m => m.AddAisle(It.Is<int>(i => i == 1)),
                    Times.Once());
        }

        private bool AddAisleNumberOne(Mock<IAisleManager> libraryManagerMock)
        {
            var libraryAPI = new LibraryAPI(libraryManagerMock.Object, null);
            var successfull = libraryAPI.AddAisle(1);
            return successfull;
        }

        [TestMethod]
        public void TestAddExistingAisle()
        {
            var libraryManagerMock = SetupMock(new Aisle());
            bool successfull = AddAisleNumberOne(libraryManagerMock);
            Assert.IsFalse(successfull);
            libraryManagerMock.Verify(
                m => m.AddAisle(It.Is<int>(i => i == 1)),
                    Times.Never());
        }

        [TestMethod]
        public void TestRemoveEmptyAisle()
        {
            var aisleManagerMock = new Mock<IAisleManager>();
            var shelfManagerMock = new Mock<IShelfManager>();

            aisleManagerMock.Setup(m =>
          m.GetAisleByAisleNumber(It.IsAny<int>()))
                .Returns(new Aisle
                {

                    AisleNumber = 4,
                    Shelf = new List<Shelf>()

                });

            var libraryAPI = new LibraryAPI(aisleManagerMock.Object, shelfManagerMock.Object);
            var successfull = libraryAPI.RemoveAisle(4);
            Assert.AreEqual(AisleStatusCodes.Ok, successfull);
            aisleManagerMock.Verify(m =>
                m.RemoveAisle(It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public void TestRemoveAisleWithOneShelf()
        {
            var aisleManagerMock = new Mock<IAisleManager>();
            var shelfManagerMock = new Mock<IShelfManager>();

            aisleManagerMock.Setup(m =>
          m.GetAisleByAisleNumber(It.IsAny<int>()))
                .Returns(new Aisle
                {

                    AisleNumber = 4,
                    Shelf = new List<Shelf>
                    {
                        new Shelf()
                    }

                });

            var libraryAPI = new LibraryAPI(aisleManagerMock.Object, shelfManagerMock.Object);
            var successfull = libraryAPI.RemoveAisle(4);
            Assert.AreEqual(AisleStatusCodes.AisleHasShelfs, successfull);
            aisleManagerMock.Verify(m =>
               m.RemoveAisle(It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public void TestRemoveNonExistingAisle()
        {
            var aisleManagerMock = new Mock<IAisleManager>();
            var shelfManagerMock = new Mock<IShelfManager>();

            aisleManagerMock.Setup(m =>
          m.GetAisleByAisleNumber(It.IsAny<int>()))
                .Returns((Aisle)null);

            var libraryAPI = new LibraryAPI(aisleManagerMock.Object, shelfManagerMock.Object);
            var successfull = libraryAPI.RemoveAisle(4);
            Assert.AreEqual(AisleStatusCodes.NoSuchAisle, successfull);
            aisleManagerMock.Verify(m =>
               m.RemoveAisle(It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public void TestAddShelfOk()
        {
            Mock<IShelfManager> libraryManagerMock = SetupMock((Shelf)null);
            var aisleManagerMock = SetupMock(new Aisle { AisleNumber = 1 });
            var successfull = AddShelfNumberOne(libraryManagerMock);
            Assert.AreEqual(ShelfStatusCodes.Ok, successfull);
            libraryManagerMock.Verify(
                m => m.AddShelf(It.Is<int>(i => i == 101), It.Is<int>(i => i == 1)),
                    Times.Once());
        }

        private ShelfStatusCodes AddShelfNumberOne(Mock<IShelfManager> shelfManagerMock)
        {
            var aisleManagerMock = SetupMock(new Aisle { AisleNumber = 1 });
            var libraryAPI = new LibraryAPI(aisleManagerMock.Object, shelfManagerMock.Object);
            var successfull = libraryAPI.AddShelf(101, 1);
            return successfull;
        }

        [TestMethod]
        public void TestAddExistingShelf()
        {
            var aisleManagerMock = SetupMock(new Aisle { AisleNumber = 1 });
            var libraryManagerMock = SetupMock(new Shelf());
            var successfull = AddShelfNumberOne(libraryManagerMock);
            Assert.AreEqual(ShelfStatusCodes.ShelfAlreadyExistInAisle, successfull);
            libraryManagerMock.Verify(
                m => m.AddShelf(It.Is<int>(i => i == 101), It.Is<int>(i => i == 1)),
                    Times.Never());
        }


        [TestMethod]
        public void TestMoveShelfOk()
        {
            var aisleManagerMock = new Mock<IAisleManager>();
            var shelfManagerMock = new Mock<IShelfManager>();

            aisleManagerMock.Setup(m =>
               m.GetAisleByAisleNumber(It.IsAny<int>()))
                .Returns(new Aisle { AisleID = 2 });

            shelfManagerMock.Setup(m =>
              m.GetShelfByShelfNumber(It.IsAny<int>(), It.IsAny<int>()))
               .Returns(new Shelf
               {
                   ShelfID = 2,
                   Aisle = new Aisle()
               });

            var libraryAPI = new LibraryAPI(aisleManagerMock.Object, shelfManagerMock.Object);
            var result = libraryAPI.MoveShelf(101, 1);
            Assert.AreEqual(ShelfStatusCodes.Ok, result);
            shelfManagerMock.Verify(m =>
                m.MoveShelf(2, 2), Times.Once());
        }

        [TestMethod]
        public void MoveShelfNoSuchShelf()
        {
            var aisleManagerMock = SetupMock(new Aisle { AisleNumber = 1 });
            var shelfManagerMock = SetupMock((Shelf)null);

            var libraryApi = new LibraryAPI(aisleManagerMock.Object, shelfManagerMock.Object);
            var result = libraryApi.MoveShelf(113, 1);
            Assert.AreEqual(ShelfStatusCodes.NoSuchShelf, result);
        }

        [TestMethod]
        public void MoveShelfNoSuchAisle()
        {
            var aisleManagerMock = SetupMock((Aisle)null);
            var shelfManagerMock = SetupMock(new Shelf { ShelfNumber = 101 });

            var libraryApi = new LibraryAPI(aisleManagerMock.Object, shelfManagerMock.Object);
            var result = libraryApi.MoveShelf(101, 9);
            Assert.AreEqual(ShelfStatusCodes.NoSuchAisle, result);
        }

        [TestMethod]
        public void TestRemoveEmptyShelf()
        {
            var shelfManagerMock = new Mock<IShelfManager>();
            var bookManagerMock = new Mock<IBookManager>();
            

            shelfManagerMock.Setup(m =>
          m.GetShelfByShelfNumber(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new Shelf
                {

                    ShelfNumber = 4,
                    Book = new List<Book>()

                });

            var libraryAPI = new LibraryAPI(shelfManagerMock.Object, bookManagerMock.Object);
            var successfull = libraryAPI.RemoveShelf(103, 3);
            Assert.AreEqual(ShelfStatusCodes.Ok, successfull);
            shelfManagerMock.Verify(m =>
                m.RemoveShelf(It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public void TestRemoveShelfWithOneBook()
        {
            var shelfManagerMock = new Mock<IShelfManager>();
            var bookManagerMock = new Mock<IBookManager>();

            shelfManagerMock.Setup(m =>
          m.GetShelfByShelfNumber(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new Shelf
                {

                    ShelfNumber = 101,
                    Book = new List<Book>
                    {
                        new Book()
                    }

                });

            var libraryAPI = new LibraryAPI(shelfManagerMock.Object, bookManagerMock.Object);
            var successfull = libraryAPI.RemoveShelf(101, 1);
            Assert.AreEqual(ShelfStatusCodes.ShelfHasBooks, successfull);
            shelfManagerMock.Verify(m =>
               m.RemoveShelf(It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public void TestRemoveNonExistingShelf()
        {
            var shelfManagerMock = new Mock<IShelfManager>();
            var bookManagerMock = new Mock<IBookManager>();

            shelfManagerMock.Setup(m =>
          m.GetShelfByShelfNumber(It.IsAny<int>(), It.IsAny<int>()))
                .Returns((Shelf)null);

            var libraryAPI = new LibraryAPI(shelfManagerMock.Object, bookManagerMock.Object);
            var successfull = libraryAPI.RemoveShelf(106, 6);
            Assert.AreEqual(ShelfStatusCodes.NoSuchShelf, successfull);
            shelfManagerMock.Verify(m =>
               m.RemoveShelf(It.IsAny<int>()), Times.Never);
        }
    }
}
