namespace MyLibrary;
using System.Text.Json;

public class FileHandler
{
    private const string FileName = "books.json";
    public void SaveBooks(List<Book> books)
    {
        var json = JsonSerializer.Serialize(books, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FileName, json);
    }

    public List<Book> LoadBooks()
    {
        if (File.Exists(FileName))
        {
            var json = File.ReadAllText(FileName);
            return JsonSerializer.Deserialize<List<Book>>(json);
        }
        else
        {
            return new List<Book>();
        }
    }
}