using System;
using System.Collections.Generic;
using System.Text;

namespace DataInterface
{
    public interface IShelfManager
    {
        public Shelf AddShelf(int shelfNumber, int aisleNumber);
        void MoveShelf(int shelfID, int aisleID);
        Shelf GetShelfByShelfNumber(int shelfNumber, int aisleNumber);
        public void RemoveShelf(int shelfID);
    }
}
