using DataInterface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class ShelfManager : IShelfManager
    {
        public Shelf AddShelf(int shelfNumber, int aisleNumber)
        {
            using (var libraryContext = new LibraryContext())
            {
                var shelf = new Shelf();
                shelf.ShelfNumber = shelfNumber;
                shelf.AisleID = (new AisleManager()).GetAisleByAisleNumber(aisleNumber).AisleID;
                libraryContext.Shelfs.Add(shelf);
                libraryContext.SaveChanges();
                return shelf;
            }
        }

        public Shelf GetShelfByShelfNumber(int shelfNumber, int aisleNumber)
        {
            using var context = new LibraryContext();
            return (from s in context.Shelfs
                    join a in context.Aisles
                    on s.AisleID equals a.AisleID
                    where s.ShelfNumber == shelfNumber && a.AisleNumber == aisleNumber
                    select s)
                    .Include(s => s.Book)
                    .FirstOrDefault();

        }

        public void MoveShelf(int shelfID, int aisleID)
        {
            using var context = new LibraryContext();
            var shelf = (from s in context.Shelfs
                         where s.ShelfID == shelfID
                         select s)
                         .First();
            shelf.AisleID = aisleID;
            context.SaveChanges();
        }

        public void RemoveShelf(int shelfID)
        {
            using var context = new LibraryContext();
            var shelf = (from s in context.Shelfs
                         where s.ShelfID == shelfID
                         select s).FirstOrDefault();
            context.Shelfs.Remove(shelf);
            context.SaveChanges();
        }
    }
}
