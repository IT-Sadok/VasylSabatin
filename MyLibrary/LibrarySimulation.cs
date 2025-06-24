using System.Diagnostics;

namespace MyLibrary;

public class LibrarySimulation
{
    private readonly Library _library;
    private readonly SemaphoreSlim _semaphore;
    
    public LibrarySimulation(Library library)
    {
        _library = library ?? throw new ArgumentNullException(nameof(library));
        _semaphore = new SemaphoreSlim(3, 3);
    }

    public async Task RunSimulationAsync()
    {
        var tasks = new Task[100];

        for (var i = 0; i < 100; i++)
        {
            tasks[i] = SimulateRandomOperationAsync();
        }
        await Task.WhenAll(tasks);
    }

    private async Task SimulateRandomOperationAsync()
    {
        var localRandom = new Random(Guid.NewGuid().GetHashCode());
        await _semaphore.WaitAsync();

        try
        {
            await Task.Delay(localRandom.Next(500, 1000));
            
            if (localRandom.Next(2) == 0)
            {
                ModifyRandomBook(localRandom);
            }
            else
            {
                await UpdateRandomAuthor(localRandom);
            }
        }
        finally
        {
            _semaphore.Release();
        }
    }
    
    private void ModifyRandomBook(Random random)
    {
        var books = _library.GetAllBooks();
        if (!books.Any()) return;

        var randomBook = books[random.Next(books.Count)];
        
        randomBook.Title = GenerateRandomTitle(random);
        randomBook.Year = random.Next(1900, 2024);
    }

    private async Task UpdateRandomAuthor(Random random)
    {
        var books = _library.GetAllBooks();
        if (!books.Any()) return;
        
        var randomBook = books[random.Next(books.Count)];
        randomBook.Author = GenerateRandomAuthor(random);
    }

    private string GenerateRandomTitle(Random randomTitle)
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
        return titles[randomTitle.Next(titles.Length)];
    }

    private string GenerateRandomAuthor(Random randomAuthor)
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
        return authors[randomAuthor.Next(authors.Length)];
    }
}