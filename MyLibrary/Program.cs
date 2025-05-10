using MyLibrary;

var book = new Book();
var library = new Library();

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

// Introducing the info about book 

Console.WriteLine("Your book: " 
                  + book.Title + ", " 
                  + book.Author + ", " 
                  + book.Year.ToString() + ", " 
                  + book.Id.ToString());