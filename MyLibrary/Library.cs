using MyLibrary;
using System.Collections.Generic;

public class Library
{
    private List<Book> _books = [];
    public int nextId = 1;
     
    public void AddBook(Book book)
    {
        book.Id = nextId;
        nextId++;
        _books.Add(book);
    }

    public void DeleteBook(Book book)
    {
        _books.Remove(book);
    }

    public Book? SearchBook(int id, string author)
    {
        return _books.FirstOrDefault(book => book.Id == id ||  book.Author == author);
    }

    public List<Book> GetAllBooks(Book book)
    {
        return _books;
    }
}    