using System;
using MyLibrary;

public class LibraryUI
{ 
    Book book = new Book();
    Library library = new Library();

    public void DisplayMenu()
    {
        Console.Clear();
        Console.WriteLine("Welcome to my library!\n" +
                          "1. Add book\n" +
                          "2. Delete book\n" +
                          "3. Search book\n" +
                          "4. View all books");
        Console.WriteLine("Select an option (1-4): ");

        string option = Console.ReadLine();
        switch (option)
        {
            case "1":
                AddBook();
                break;
            case "2":
                DeleteBook();
                break;
            case "3":
                SearchBook();
                break;
            case "4":
                ViewAllBooks();
                break;
            default:
                Console.WriteLine("Invalid option. Please try again.");
                break;
            }
        }

    void AddBook()
    {
        Console.Clear();
        Console.WriteLine("Add book");
    
        Console.Write("Enter the book's title: ");
        book.Title = Console.ReadLine();

        Console.Write("Enter the book's author: ");
        book.Author = Console.ReadLine();
    
        Console.Write("Enter the book's publication year: ");
        string? yearInput = Console.ReadLine();
        bool yearResult = int.TryParse(yearInput, out int year);
        if (yearResult == true)
            book.Year = year;
        else
            Console.WriteLine("Invalid input, try again");
    
        library.AddBook(book);
    
        Console.WriteLine("Book added successfully.");
        Console.WriteLine("Press any key to return to the main menu.");
        Console.ReadKey();
    }
    
    void DeleteBook()
    {
        Console.Clear();
        Console.WriteLine("Delete book");
        
        Console.WriteLine("Enter the book's ID to delete: ");
        int id;
        if (int.TryParse(Console.ReadLine(), out id))
        {
            var firstOrDefault = library.GetAllBooks().FirstOrDefault(b => b.Id == id);
            if (firstOrDefault != null)
            {
                library.DeleteBook(book);
                Console.WriteLine($"Book with ID {id} has been deleted.");
                
            }
            else
            {
                Console.WriteLine("Book not found.");
            }
        }
        else 
        {
            Console.WriteLine("Invalid input, please try again.");
        }
        
        Console.WriteLine("Press any key to return to the main menu.");
        Console.ReadKey();
    }

    void SearchBook()
    {
        Console.Clear();
        Console.WriteLine("Search book");
        
        Console.WriteLine("Enter the book's author or ID: ");
        var input = Console.ReadLine();

        Book? foundBook = null;
        if (int.TryParse(input, out int id))
        {
            foundBook = library.GetAllBooks().
                FirstOrDefault(b => b.Id == id);
        }
        else
        {
            foundBook = library.GetAllBooks().
                FirstOrDefault(b => b.Author.Equals(input, StringComparison.OrdinalIgnoreCase));
        }

        if (foundBook != null)
        {
            Console.WriteLine($"Book found: {foundBook.Title}, {foundBook.Author}, {foundBook.Year}");
        }
        else
        {
            Console.WriteLine("No book found with that ID or author name.");
        }
        
        Console.WriteLine("Press any key to return to the main menu.");
        Console.ReadKey();
    }

    void ViewAllBooks()
    {
        Console.Clear();
        Console.WriteLine("All books in library");
        
        if (library.GetAllBooks().Count == 0)
        {
            Console.WriteLine("No books found.");
        }
        else
        {
            foreach (var book in library.GetAllBooks())
            {
                Console.WriteLine($"Title: {book.Title} \n" +
                                  $"Author: {book.Author} \n" +
                                  $"Year: {book.Year}");
            }
        }
        
        Console.WriteLine("Press any key to return to the main menu.");
        Console.ReadKey();
    }
}