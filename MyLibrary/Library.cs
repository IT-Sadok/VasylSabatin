namespace MyLibrary;
using System.Text.Json;

public class Library
{
    private List<Book> _books = [];
    private int _nextId = 1;
     
    public void AddBook(Book book)
    {
        book.Id = _nextId;
        _nextId++;
        _books.Add(book);
        
        string json = JsonSerializer.Serialize(_books);
        File.WriteAllText("books.json", json);
        Console.WriteLine("Book added");
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
