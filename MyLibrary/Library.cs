namespace MyLibrary;

public class Library
{
    private List<Book>? _books = [];
    private int _nextId;
    FileHandler fileHandler = new FileHandler();


    public Library()
    {
        LoadBooks();
    }

    private void LoadBooks()
    {
        _books = fileHandler.LoadBooks();
        _nextId = _books.Count > 0 ? _books.Max(b => b.Id) + 1 : 1;
    }
    
    public void AddBook(Book book)
    {
        book.Id = _nextId++;
        _books.Add(book);
        fileHandler.SavingFile(_books);
    }

    public void DeleteBook(Book book)
    {
        _books.Remove(book);
        fileHandler.SavingFile(_books);
    }

    public Book? SearchBook(int id, string author)
    {
        return _books.FirstOrDefault(book => book.Id == id || book.Author == author);
    }

    public List<Book>? GetAllBooks()
    {
        return _books;
    }
}    