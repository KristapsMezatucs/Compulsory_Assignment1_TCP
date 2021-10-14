using Compulsory_Assignment1_Model_Book;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compulsory_Assignment1_TCP
{
    public class BookRepo
    {
        public static readonly List<Book> BookList = new List<Book>()
        {
            new Book() { Author="J.K.Rowling", Title="Harry Potter", PageNr = 300, ISBN13 = "12345767862"},
            new Book() { Author="Andrew Wiggins", Title="Race of a century", PageNr = 200, ISBN13 = "12345767842"},
            new Book() { Author="Pedro Rainho", Title="C# for dummies", PageNr = 400, ISBN13 = "12309767862"}
        };

        public List<Book> GetAll()
        {
            List<Book> result = new List<Book>(BookList);
            return result;
        }

        public Book Get(string isbn13)
        {
            return BookList.Find(book => book.ISBN13 == isbn13);
        }

        public Book Add(Book b)
        {
            BookList.Add(b);
            return b;
        }

    }
}
