using System;
using System.Collections.Generic;
using System.Text;

namespace DataInterface
{
    public interface IAisleManager
    {
        public Aisle AddAisle(int aisleNumber);
        public Aisle GetAisleByAisleNumber(int aisleNumber);

        void RemoveAisle(int aisleID);
    }
}
