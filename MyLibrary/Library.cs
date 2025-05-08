using Library;
using System.Collections.Generic;

namespace MyLibrary;

public class Library
{
    private List<Book> _books = []; 
    
    public List<Book> AddBook(Book book)
    {
        _books.Add(book);
        return null;
    }

    public List<Book> DeleteBook(Book book)
    {
        _books.Remove(book);
        return null;
    }

    public List<Book> SearchBook(Book book)
    {
        _books.Sort();
        return null;
    }

    public List<Book> ShowAll(Book book)
    {
        return _books;
    }
}
