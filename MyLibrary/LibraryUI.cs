using System;
using System.Reflection.Metadata.Ecma335;
using MyLibrary;

public class LibraryUI
{ 
    Book book = new Book();
    Library library = new Library(new FileHandler());

    public async Task DisplayMenuAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Welcome to my library!\n" +
                              "1. Add book\n" +
                              "2. Delete book\n" +
                              "3. Search book\n" +
                              "4. View all books\n" + 
                              "5. View all books by author\n" + 
                              "6. View books count by year\n" + 
                              "7. Find books by year and author\n" +
                              "8. Library simulation");
            Console.WriteLine("Select an option (1-8): ");
        
            var option = Console.ReadLine();
        
            if (option?.ToLower() == "quit")
            {
                Console.Clear();
                Console.WriteLine("Goodbye");
                break;
            }

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
                case "5":
                    ViewAllBooksByAuthor();
                    break;
                case "6":
                    ViewBooksCountByYear();
                    break;
                case "7":
                    ViewBooksByYearAndAuthor();
                    break;
                case "8":
                    await RunLibrarySimulationAsync();
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    break;
                
            }

        }
    }

    private void AddBook()
    {
        var newBook = new Book();
        
        Console.Clear();
        Console.WriteLine("Add book\n");
    
        Console.Write("Enter the book's title: ");
        newBook.Title = Console.ReadLine();

        Console.Write("Enter the book's author: ");
        newBook.Author = Console.ReadLine();
    
        Console.Write("Enter the book's publication year: ");
        string? yearInput = Console.ReadLine();
        bool yearResult = int.TryParse(yearInput, out int year);
        if (yearResult == true)
            newBook.Year = year;
        else
        {
            Console.WriteLine("Invalid input, try again");
            Console.ReadKey();
            return;
        }
    
        library.AddBook(newBook);
    
        Console.WriteLine("Book added successfully.");
        Console.WriteLine("Press any key to return to the main menu.");
        Console.ReadKey();
    }
    
    private void DeleteBook()
    {
        Console.Clear();
        Console.WriteLine("Delete book\n");
        
        Console.WriteLine("Enter the book's ID to delete: ");
        int id;
        if (int.TryParse(Console.ReadLine(), out id))
        {
            var foundBook = library.SearchBookById(id);
            
            if (foundBook != null)
            {
                library.DeleteBook(foundBook);
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

    private void SearchBook()
    {
        Console.Clear();
        Console.WriteLine("Search book\n");
        
        Console.WriteLine("Enter the book's author or ID: ");
        var input = Console.ReadLine();

        Book? foundBook;

        if (int.TryParse(input,   out int id))
        {
            foundBook = library.SearchBookById(id);
        }
        else
        {
            foundBook = library.SearchBookByAuthor(input);
        }

        if (foundBook != null)
        {
            Console.WriteLine($"Book found: {foundBook.Title} by {foundBook.Author} ({foundBook.Year})");
        }
        else
        {
            Console.WriteLine("Book not found.");
        }

        Console.WriteLine("Press any key to return to the main menu.");
        Console.ReadKey();
    }

    private void ViewAllBooks()
    {
        Console.Clear();
        Console.WriteLine("All books in library \n");
        
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
                                  $"Year: {book.Year} \n");
            }
        }
        
        Console.WriteLine("Press any key to return to the main menu.");
        Console.ReadKey();
    }

    private void ViewAllBooksByAuthor()
    {
        Console.Clear();
        Console.WriteLine("All books by author\n");
        
        Console.WriteLine("Enter the author's name: ");
        var authorInput = Console.ReadLine();
        
        var authorResult =  library.GetAllBooks()
            .Where(b => b.Author.Equals(authorInput, StringComparison.OrdinalIgnoreCase)).ToList();
        
        Console.Clear();
        
        if (authorResult.Count() == 0)
        {
            Console.WriteLine("No books found for the author.");
        }
        else
            Console.WriteLine($"" +
                              $"Books by {authorInput}:");

        foreach (var searchBook in authorResult)
        {
            Console.WriteLine($"- {searchBook.Title} ({searchBook.Year})");
        }

        Console.WriteLine("Press any key to return to the main menu.");
        Console.ReadKey();
    }
    
    private void ViewBooksCountByYear()
    {
        Console.Clear();
        Console.WriteLine("Books count by year\n");
        
        Console.WriteLine("Enter publication year: ");
        var yearInput = Console.ReadLine();

        if (!int.TryParse(yearInput, out int year))
        {
            Console.WriteLine("Invalid input, please try again.");
            Console.WriteLine("Press any key to return to the main menu.");
            Console.ReadKey();
            return;
        }

        var booksCountByYear = library.GetBooksCountByYear();

        if (booksCountByYear.TryGetValue(year, out int booksCount))
        {
            Console.WriteLine($"Books count: {booksCount}");
        }
        else
        {
            Console.WriteLine($"No books found for year {year}.");
        }
        
        Console.WriteLine("\nPress any key to return to the main menu.");
        Console.ReadKey();
    }

    private void ViewBooksByYearAndAuthor()
    {
        Console.Clear();
        Console.WriteLine("Books by year and author\n");
        
        Console.WriteLine("Enter the author's name: ");
        var authorInput = Console.ReadLine();
        
        Console.WriteLine("Enter the publication year: ");
        var publicationYearInput = Console.ReadLine();

        if (!int.TryParse(publicationYearInput, out int publicationYear))
        {
            Console.WriteLine("Invalid input, please try again.");
            Console.WriteLine("Press any key to return to the main menu.");
            Console.ReadKey();
            return;
        }
        
        Console.Clear();
        
        var viewBooks =  library.GetBooksByYearAndAuthor(publicationYear, authorInput);
        
        if (viewBooks.Count == 0)
        {
            Console.WriteLine("No books found.");
        }
        else
        {
            Console.WriteLine($"Books by {authorInput} in {publicationYear}:");
            foreach (var book in viewBooks)
            {
                Console.WriteLine($"- {book.Title}");
            }

        }
        
        Console.WriteLine("\nPress any key to return to the main menu.");
        Console.ReadKey();
    }

    private async Task RunLibrarySimulationAsync()
    {
        Console.Clear();
        Console.WriteLine("Starting library simulation...\n");

        var simulation = new LibrarySimulation(library);
        
        await simulation.RunSimulationAsync();
        
        Console.WriteLine("Simulation finished. Press any key to return to the main menu.");
        Console.ReadKey();
    }
}