namespace BookShop
{
    using Data;
    using Initializer;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            DbInitializer.ResetDatabase(db);
            //string command = Console.ReadLine().ToLower();
            var context = new BookShopContext();
            // Console.WriteLine(GetBooksByAgeRestriction(context, command));
            //Console.WriteLine(GetGoldenBooks(context));
            //Console.WriteLine(GetBooksByPrice(context));
            //int year = int.Parse(Console.ReadLine());
            //Console.WriteLine(GetBooksNotReleasedIn(context, year));
            //string input = Console.ReadLine();
            //Console.WriteLine(GetBooksByCategory(context,input));
            //string date = Console.ReadLine();
            //Console.WriteLine(GetBooksReleasedBefore(context, date));
            //string input = Console.ReadLine();
            //Console.WriteLine(GetAuthorNamesEndingIn(context, input));
            //string input = Console.ReadLine();
            //Console.WriteLine(GetBookTitlesContaining(context, input));
            //string input = Console.ReadLine();
            //Console.WriteLine(GetBooksByAuthor(context, input));
            //int lengthCheck = int.Parse(Console.ReadLine());
            //Console.WriteLine(CountBooks(context,lengthCheck));
            //Console.WriteLine(CountCopiesByAuthor(context));
            //Console.WriteLine(GetTotalProfitByCategory(context));
            //Console.WriteLine(GetMostRecentBooks(context));
            //IncreasePrices(context);
            //RemoveBooks(context);

        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var books = context.Books
                .Select(x => new
                {
                    x.AgeRestriction,
                    x.Title
                })
                .OrderBy(x => x.Title)
                .ToList();
            var sb = new StringBuilder();
            foreach (var book in books)
            {
                if (book.AgeRestriction.ToString().ToLower() == command)
                {
                    sb.AppendLine(book.Title);
                }
            }
            return sb.ToString().Trim();
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            var books = context.Books
                .Select(x => new
                {
                    x.Copies,
                    x.Title,
                    x.BookId,
                    x.EditionType
                })
                .Where(x => x.Copies < 5000)
                .OrderBy(x => x.BookId)
                .ToList();
            var sb = new StringBuilder();
            foreach (var book in books.Where(x => x.EditionType.ToString() == "Gold"))
            {
                sb.AppendLine(book.Title);
            }
            return sb.ToString().Trim();
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            var books = context.Books
                .Select(x => new
                {
                    x.Title,
                    x.Price
                })
                .Where(x => x.Price > 40)
                .OrderByDescending(x => x.Price)
                .ToList();
            var sb = new StringBuilder();
            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - ${book.Price:f2}");
            }
            return sb.ToString().Trim();
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var books = context.Books
                .Select(x => new
                {
                    x.Title,
                    releaseDate = (DateTime)x.ReleaseDate,
                    x.BookId
                })
                .Where(x => x.releaseDate.Year != year)
                .OrderBy(x => x.BookId)
                .ToList();
            var sb = new StringBuilder();
            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title}");
            }
            return sb.ToString().Trim();
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            var categories = input
                .ToLower()
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .ToList();
            var books = context.Books
                .Select(x => new
                {
                    x.Title,
                    CategoryName = x.BookCategories
                    .Select(x => x.Category.Name)
                    .SingleOrDefault()
                })
                .Where(c => categories.Any(g => g == c.CategoryName.ToLower()))
                .OrderBy(x => x.Title)
                .ToList();
            var sb = new StringBuilder();
            foreach (var book in books)
            {
                sb.AppendLine(book.Title);
            }
            return sb.ToString().Trim();
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            DateTime dating = DateTime.Parse(date);
            var books = context.Books
                .Select(x => new
                {
                    x.Title,
                    x.EditionType,
                    x.ReleaseDate,
                    x.Price
                })
                .Where(x => x.ReleaseDate < dating)
                .OrderByDescending(x => x.ReleaseDate)
                .ToList();

            var sb = new StringBuilder();
            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - {book.EditionType} ${book.Price:f2}");
            }
            return sb.ToString().Trim();
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {

            var authors = context.Authors
               .Where(a => a.FirstName.EndsWith(input))
                .Select(a => $"{a.FirstName} {a.LastName}")
                .OrderBy(a => a)
                .ToList();

            return string.Join(Environment.NewLine, authors);
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var books = context.Books
                .Where(x => x.Title.ToLower().Contains(input.ToLower()))
                .Select(x => x.Title)
                .OrderBy(x => x)
                .ToList();

            return String.Join(Environment.NewLine, books);
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var books = context.Books
                .Where(x => x.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .Select(x => new
                {
                    x.Author,
                    x.Title,
                    x.BookId
                })
                .OrderBy(x => x.BookId)
                .Select(x =>

                   $"{x.Title} ({x.Author.FirstName} {x.Author.LastName})"
                )
                .ToList();


            return string.Join(Environment.NewLine, books);
        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var books = context.Books
                .Where(x => x.Title.Length > lengthCheck)
                .ToList();

            return books.Count;

        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var authors = context.Authors
                .Select(a => new
                {
                    Name = $"{a.FirstName} {a.LastName}",
                    Copies = a.Books.Sum(b => b.Copies)
                })
                .OrderByDescending(a => a.Copies)
                .Select(a => $"{a.Name} - {a.Copies}")
                .ToList();

            return string.Join(Environment.NewLine, authors);
        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var categories = context.Categories
               .Select(c => new
               {
                   CategoryName = c.Name,
                   Profit = c.CategoryBooks
                       .Select(cb => cb.Book)
                       .Select(b => b.Price * b.Copies)
                       .Sum()
               })
               .OrderByDescending(c => c.Profit)
               .ThenBy(c => c.CategoryName)
               .ToList();

            return string.Join(Environment.NewLine, categories.Select(c => $"{c.CategoryName} ${c.Profit:f2}"));
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {

            var categories = context.Categories
                .Select(x => new
                {
                    CategoryName = x.Name,
                    BookName = x.CategoryBooks
                    .Select(b => new
                    {
                        Title = b.Book.Title,
                        Date = b.Book.ReleaseDate.Value
                    })
                    .OrderByDescending(b => b.Date)
                    .ToList()

                })
                .OrderBy(x => x.CategoryName)
                .Take(3)
                .ToList();

            var sb = new StringBuilder();

            foreach (var c in categories)
            {
                sb.AppendLine($"--{c.CategoryName}");
                foreach (var b in c.BookName)
                {
                    sb.AppendLine($"{b.Title} ({b.Date.Year})");
                }
            }

            return sb.ToString();

        }

        public static void IncreasePrices(BookShopContext context)
        {
            var books = context.Books
                .Where(x => x.ReleaseDate.Value.Year < 2010)
                .ToList();

            foreach (var item in books)
            {
                item.Price += 5;
            }

            context.SaveChanges();
        }

        public static int RemoveBooks(BookShopContext context)
        {
            var books = context.Books
                .Where(x => x.Copies < 4200)
                .ToList();

            context.Books.RemoveRange(books);
            context.SaveChanges();
            return books.Count();
        }
    }
}


