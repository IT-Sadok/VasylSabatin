using Library;

var book = new Book();

Console.Write("Enter the book's title: ");
book.Title = Console.ReadLine();


Console.Write("Enter the book's author: ");
book.Author = Console.ReadLine();

Console.WriteLine("Enter the book's publication year: ");
book.Year = Convert.ToInt32(Console.ReadLine());

Console.WriteLine("Enter the book's ID: ");
book.Id = Convert.ToInt32(Console.ReadLine());



// Intoducing the info about book 

Console.WriteLine("Your book: " + book.Title + ", " + book.Author + ", " + book.Year + ", " + book.Id);