namespace MyLibrary;

public class Library
{
    private List<Book>? _books = new List<Book>();
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
        book.Id = _nextId;
        _books.Add(book);
        _nextId++;
        fileHandler.SaveBooks(_books);
    }

    public void DeleteBook(Book book)
    {
        _books.Remove(book);
        fileHandler.SaveBooks(_books);
    }

    public Book? SearchBook(int id, string author)
    {
        return _books.FirstOrDefault(book => book.Id == id || book.Author == author);
    }

    public List<Book>? GetAllBooks()
    {
        return _books;
    }

    public Dictionary<string, List<Book>> GetBooksByAuthor()
    {
        return _books.GroupBy(book => book.Author)
            .OrderBy(group => group.Key)
            .ToDictionary(group => group.Key,group => group.ToList());
    }

    public Dictionary<int, int> GetBooksCountByYear()
    {
        return _books.GroupBy(book => book.Year)
            .ToDictionary(group => group.Key, group => group.Count());
    }

    public List<Book> GetBooksByYearAndAuthor(int year, string author)
    {
        return _books.Where(book => book.Year == year && book.Author == author).ToList();
    }
}    