namespace Library;

public class Program
{
    public void AddBook()
    {
        Console.WriteLine("Book Added");

    }

    public void DeleteBook()
    {
        Console.WriteLine("Book Deleted");
    }

    public string SearchBook()
    {
        return "Book Found";
    }

    public string ShowAllBooks()
    {
        return "List of Books";
    }

    public bool BorrowBook()
    {
        Console.WriteLine("Book Borrowed");
    }
    
    public bool ReturnBook()
    {
        Console.WriteLine("Book Returned");
    }
}
