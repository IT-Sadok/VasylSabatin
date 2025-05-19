namespace MyLibrary;
using System.Text.Json;

public class FileHandler
{
    public void SavingFile(List<Book> book)
    {
        string json = JsonSerializer.Serialize(book);
        File.WriteAllText("books.json", json);
    }
}