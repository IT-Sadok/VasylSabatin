namespace MyLibrary;

public class LibrarySimulation
{
    private readonly Library _library;
    private readonly SemaphoreSlim _semaphore;
    private readonly Random _random = new Random();
    
    public LibrarySimulation(Library library)
    {
        _library = library ?? throw new ArgumentNullException(nameof(library));
        _semaphore = new SemaphoreSlim(5,5);
    }

    public async Task RunSimulation()
    {
        var tasks = new Task[100];

        for (var i = 0; i < 100; i++)
        {
            tasks[i] = SimulateRandomOperation();
        }
        await Task.WhenAll(tasks);
    }

    private async Task SimulateRandomOperation()
    {
        await _semaphore.WaitAsync();

        try
        {
            await Task.Delay(_random.Next(200, 1000));

            if (_random.Next(2) == 0)
            {
                ModifyRandomBook();
            }
            else
            {
                UpdateRandomAuthor();
            }
        }
        finally
        {
            _semaphore.Release();
        }
    }
    
    private void ModifyRandomBook()
    {
        var books = _library.GetAllBooks();
        if (!books.Any()) return;

        var randomBook = books[_random.Next(books.Count)];
        
        randomBook.Title = GenerateRandomTitle();
        randomBook.Year = _random.Next(1900, 2024);
    }

    private void UpdateRandomAuthor()
    {
        var books = _library.GetAllBooks();
        if (!books.Any()) return;
        
        var randomBook = books[_random.Next(books.Count)];
        randomBook.Author = GenerateRandomAuthor();
    }

    private string GenerateRandomTitle()
    {
        var titles = new[]
        {
            "Shadows of Tomorrow",
            "Whispering Winds",
            "Echoes of Eternity",
            "The Last Horizon",
            "Beyond the Silence",
            "Fires of Destiny",
            "The Forgotten Realm",
            "Dance of the Stars",
            "Threads of Time",
            "Secrets Beneath",
            "The Crystal Code",
            "Voyage to Nowhere",
            "Guardians of Light",
            "The Silent Pact",
            "Kingdom of Ashes"
        };
        return titles[_random.Next(titles.Length)];
    }

    private string GenerateRandomAuthor()
    {
        var authors = new[]
        {
            "John Smith",
            "Maria Garcia",
            "David Johnson",
            "Emma Wilson",
            "Michael Brown",
            "Olivia Davis",
            "James Miller",
            "Sophia Martinez",
            "William Anderson",
            "Isabella Taylor",
            "Alexander Thomas",
            "Mia Moore",
            "Benjamin Jackson",
            "Charlotte White",
            "Daniel Harris"
        };
        return authors[_random.Next(authors.Length)];
    }
}