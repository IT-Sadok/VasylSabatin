using Library;
using System.Collections.Generic;

namespace MyLibrary;

public class Library
{
    private List<Book> _books = []; 
    
    public void AddBook(Book book)
    {
        _books.Add(book);
    }

    public void DeleteBook(Book book)
    {
        _books.Remove(book);
    }

    public Book? SearchBook(int id)
    {
        return _books.FirstOrDefault(book => Book.Id == id);
    }

    public List<Book> GetAllBooks(Book book)
    {
        return _books;
    }
}
