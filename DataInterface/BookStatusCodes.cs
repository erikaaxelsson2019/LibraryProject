using System;
using System.Collections.Generic;
using System.Text;

namespace DataInterface
{
    public enum BookStatusCodes
    {
        Ok,
        NoSuchBook,
        BookAlreadyExist,
        BookAlreadyExistInShelf,
        InvalidIsbnNumber,
        NoSuchShelf,
        BookIsLoaned,
    }
}
