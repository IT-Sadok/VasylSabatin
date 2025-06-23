namespace MyLibrary;

public class Library
{
    private List<Book> _books = new List<Book>();
    private int _nextId;
    private readonly FileHandler _fileHandler;
    private readonly object _lock = new object();

    public Library(FileHandler fileHandler)
    {
        _fileHandler = fileHandler ?? throw new ArgumentNullException(nameof(fileHandler));
        LoadBooks();
    }

    private void SaveChanges()
    {
        lock (_lock)
        {
            _fileHandler.SaveBooks(_books);
        }
    }

    private void LoadBooks()
    {
        _books = _fileHandler.LoadBooks();
        _nextId = _books.Count > 0 ? _books.Max(b => b.Id) + 1 : 1;
        
    }
    
    public void AddBook(Book book)
    {
        lock (_lock)
        {
            book.Id = _nextId;
            _books.Add(book);
            _nextId++;
            SaveChanges(); 
        }
    }

    public void DeleteBook(Book book)
    {
        lock (_lock)
        {
            _books.Remove(book);
            SaveChanges();
        }
    }

    public Book? SearchBookById(int id)
    {
        lock (_lock)
        {
            return _books.FirstOrDefault(b => b.Id == id);
        }
    }

    public Book? SearchBookByAuthor(string author)
    {
        lock (_lock)
        {
            return _books.FirstOrDefault(b => b.Author.Equals(author,  StringComparison.OrdinalIgnoreCase));  
        }
    }
    public List<Book>? GetAllBooks()
    {
        lock (_lock)
        {
            return _books.ToList();
        }
    }

    public Dictionary<string, List<Book>> GetBooksByAuthor()
    {
        lock (_lock)
        {
            return _books.GroupBy(book => book.Author)
                .OrderBy(group => group.Key)
                .ToDictionary(group => group.Key,group => group.ToList());
        }
    }

    public Dictionary<int, int> GetBooksCountByYear()
    {
        lock (_lock)
        {
            return _books.GroupBy(book => book.Year)
                .ToDictionary(group => group.Key, group => group.Count());
        }
    }

    public List<Book> GetBooksByYearAndAuthor(int year, string author)
    {
        lock (_lock)
        {
            return _books.Where(book => book.Year == year && book.Author == author).ToList();
        }
    }
}    