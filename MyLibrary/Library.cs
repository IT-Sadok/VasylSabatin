using MyLibrary;
using System.Collections.Generic;

public class Library
{
    private List<Book> _books = [];
    Random _rand = new Random();
     
    public void AddBook(Book book)
    {
        book.Id = _rand.Next(1, 1000);
        _books.Add(book);
    }

    public void DeleteBook(Book book)
    {
        _books.Remove(book);
    }

    public Book? SearchBook(int id)
    {
        return _books.FirstOrDefault(book => book.Id == id);
    }

    public List<Book> GetAllBooks(Book book)
    {
        return _books;
    }
}    
