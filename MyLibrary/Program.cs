using Library;

Console.Write("Enter the book's title: ");
Book.Title =  Console.ReadLine();

Console.Write("Enter the book's author: ");
Book.Author = Console.ReadLine();

Console.WriteLine("Enter the book's publication year: ");
Book.Year = Convert.ToInt32(Console.ReadLine());

Console.WriteLine("Enter the book's ID: ");
Book.Id = Convert.ToInt32(Console.ReadLine());



// Intoducing the info about book 

Console.WriteLine("Your book: " + Book.Title + ", " + Book.Author + ", " + Book.Year + ", " + Book.Id);