using DataInterface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class AisleManager : IAisleManager
    {
        public Aisle AddAisle(int aisleNumber)
        {
            using (var libraryContext = new LibraryContext())
            {
                Aisle aisle = new Aisle();
                aisle.AisleNumber = aisleNumber;
                libraryContext.Aisles.Add(aisle);
                libraryContext.SaveChanges();
                return aisle;
            }
        }

        public Aisle GetAisleByAisleNumber(int aisleNumber)
        {
            using var context = new LibraryContext();
            return (from a in context.Aisles
                    where a.AisleNumber == aisleNumber
                    select a)
                    .Include(a => a.Shelf)
                    .FirstOrDefault();
        }

        public void RemoveAisle(int aisleID)
        {
            using var context = new LibraryContext();
            var aisle = (from a in context.Aisles
                         where a.AisleID == aisleID
                         select a).FirstOrDefault();
            context.Aisles.Remove(aisle);
            context.SaveChanges();
        }
    }
}
