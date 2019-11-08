using System;
using System.Collections.Generic;
using System.Text;
using DataInterface;

namespace LibraryProject
{
    public class LibraryAPI
    {
        private IAisleManager aisleManager;
        private IShelfManager shelfManager;
        
        public LibraryAPI(IAisleManager aisleManager, IShelfManager shelfManager)
        {
            this.aisleManager = aisleManager;
            this.shelfManager = shelfManager;
        }

        public LibraryAPI(IShelfManager shelfManager, IBookManager bookManager)
        {
            this.shelfManager = shelfManager;
        }

        public bool AddAisle(int aisleNumber)
        {
            var existingAisle = aisleManager.GetAisleByAisleNumber(aisleNumber);
            if (existingAisle != null)
                return false;
            aisleManager.AddAisle(aisleNumber);
            return true;
        }

        public AisleStatusCodes RemoveAisle(int aisleNumber)
        {
            var newAisle = aisleManager.GetAisleByAisleNumber(aisleNumber);
            if (newAisle == null)
                return AisleStatusCodes.NoSuchAisle;
            if (newAisle.Shelf.Count > 0)
                return AisleStatusCodes.AisleHasShelfs;

            aisleManager.RemoveAisle(newAisle.AisleID);

            return AisleStatusCodes.Ok;
        }

        public ShelfStatusCodes AddShelf(int shelfNumber, int aisleNumber)
        {
            var existingShelf = shelfManager.GetShelfByShelfNumber(shelfNumber, aisleNumber);
            var newAisle = aisleManager.GetAisleByAisleNumber(aisleNumber);

            if (existingShelf != null)
                return ShelfStatusCodes.ShelfAlreadyExistInAisle;
            if (newAisle == null)
                return ShelfStatusCodes.NoSuchAisle;
            shelfManager.AddShelf(shelfNumber, aisleNumber);
            return ShelfStatusCodes.Ok;
        }

        public ShelfStatusCodes MoveShelf(int shelfNumber, int aisleNumber)
        {
            var newAisle = aisleManager.GetAisleByAisleNumber(aisleNumber);
            var shelf = shelfManager.GetShelfByShelfNumber(shelfNumber, aisleNumber);

            if (newAisle == null)
                return ShelfStatusCodes.NoSuchAisle;
            if (shelf == null)
                return ShelfStatusCodes.NoSuchShelf;
            if (shelf.Aisle.AisleNumber == aisleNumber)
                return ShelfStatusCodes.ShelfAlreadyExistInAisle;

            shelfManager.MoveShelf(shelf.ShelfID, newAisle.AisleID);
            return ShelfStatusCodes.Ok;
        }

        public ShelfStatusCodes RemoveShelf(int shelfNumber, int aisleNumber)
        {
            var newShelf = shelfManager.GetShelfByShelfNumber(shelfNumber, aisleNumber);
            if (newShelf == null)
                return ShelfStatusCodes.NoSuchShelf;
            if (newShelf.Book.Count > 0)
                return ShelfStatusCodes.ShelfHasBooks;

            shelfManager.RemoveShelf(newShelf.ShelfID);
            return ShelfStatusCodes.Ok;
        }
    }
}
