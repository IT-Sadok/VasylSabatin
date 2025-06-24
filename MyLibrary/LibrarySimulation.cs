using System.Diagnostics;

namespace MyLibrary;

public class LibrarySimulation
{
    private readonly Library _library;

    public LibrarySimulation(Library library)
    {
        _library = library ?? throw new ArgumentNullException(nameof(library));
    }

    public async Task RunSimulationAsync()
    {
        var tasks = new Task[100];
        for (var i = 0; i < 100; i++)
        {
            tasks[i] = SimulateRandomOperationAsync();
        }

        await Task.WhenAll(tasks);

        _library.SaveChanges();
    }

    private async Task SimulateRandomOperationAsync()
    {
        var localRandom = new Random(Guid.NewGuid().GetHashCode());
        Task.Delay(localRandom.Next(500, 1000));

        if (localRandom.Next(2) == 0)
        {
            ModifyRandomBook(localRandom);
        }
        else
        {
            await UpdateRandomAuthor(localRandom);
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
            "00000",
            "11111",
            "22222",
            "33333",
            "44444",
            "55555",
            "66666",
            "77777",
            "88888",
            "99999",
            "10101"
        };
        return titles[randomTitle.Next(titles.Length)];
    }

    private string GenerateRandomAuthor(Random randomAuthor)
    {
        var authors = new[]
        {
            "0123",
            "1234",
            "2345",
            "3456",
            "4567",
            "5678",
            "6789",
            "7890",
            "1122",
            "2211"
        };
        return authors[randomAuthor.Next(authors.Length)];
    }
}