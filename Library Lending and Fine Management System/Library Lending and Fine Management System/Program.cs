public class Library
{
    public static void Main(string[] args)

    {
        //store 5 books in an array
        string[] books = new string[] { "Science", "Story", "History", "literature", "Maths" };
        int[] booksstalks = new int[] { 5,5,5,5,5 };
        //display books
        for (int i = 0; i < books.Length; i++)
        {
            Console.WriteLine(books[i]);
        }

        //sorting of books
        Console.WriteLine("Sorted-books-List: ");
        Array.Sort(books);
        
        for (int i = 0; i < books.Length; i++)
        {
            Console.WriteLine(books[i]);
        }

        //search by title
        Console.WriteLine("Enter Book Title to Search: ");
        string? x = Console.ReadLine();
        int flag = 0;
        for (int i = 0; i < books.Length; i++)
        {
            if (books[i].ToLower() == x.ToLower())
            {
                Console.WriteLine("book available");
                flag = 1;
                break;
            }
        }
        if(flag==0)
        {
            Console.WriteLine("book not available");
        }
        // Register library members
        List<string> members = new List<string> { "Admin" };
        List<int> borrowinglimits = new List<int> { 0 };

        Console.WriteLine("Register:");
        Console.WriteLine("Enter your Name:");

        string? name = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(name))
        {
            members.Add(name);
            Console.WriteLine("Your ID is: " + (members.Count));
            borrowinglimits.Add(0);
        }
        else
        {
            Console.WriteLine("Name cannot be empty.");
        }
        //borrowing limits
        string? id = Console.ReadLine();
        Console.WriteLine("Enter no of books Borrowed:");
        string? y = Console.ReadLine();
        Console.WriteLine("Enter no of books Borrowed:");
        string? z = Console.ReadLine();
        int z1 = Convert.ToInt32(z);




    }
}