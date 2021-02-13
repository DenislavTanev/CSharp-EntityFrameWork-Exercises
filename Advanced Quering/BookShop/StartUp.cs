namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using BookShopContext db = new BookShopContext();

            //Seeder
            //DbInitializer.ResetDatabase(db);

            //---P04,P10
            //int input = int.Parse(Console.ReadLine());

            //---P01,P05,P06,P07,P08,P09
            //string input = Console.ReadLine();

            //---P01,P04,P05,P06,P07,P08,P09
            //string result = ---- (db, input);

            //---P02,P03,P11,P12,P13
            //string result = ---- (db);

            //----P15
            int result = RemoveBooks(db);

            //---P01,P02,P03,P04,P05,P06,P07,P08,P09,P11,P12,P13,P15
            Console.WriteLine(result);

            //---P10
            //int result = CountBooks(db, input);
            //Console.WriteLine($"There are {result} books with longer title than {input} symbols");

            //---P14
            //IncreasePrices(db);


        }

        //P01
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            List<string> bookTitles = context
                .Books
                .AsEnumerable()
                .Where(b => b.AgeRestriction.ToString().ToLower() == command.ToLower())
                .Select(b => b.Title)
                .OrderBy(bt => bt)
                .ToList();
            return String.Join(Environment.NewLine, bookTitles);
        }

        //P02
        public static string GetGoldenBooks(BookShopContext context)
        {
            List<string> bookTitles = context
               .Books
               .ToList()
               .Where(b=>b.EditionType==EditionType.Gold&&b.Copies<5000)
               .OrderBy(bt => bt.BookId)
               .Select(b => b.Title)
               .ToList();
            return String.Join(Environment.NewLine, bookTitles);
        }

        //P03
        public static string GetBooksByPrice(BookShopContext context)
        {
            var bookTitles = context.Books
                  .Where(b => b.Price > 40)
                  .OrderByDescending(b => b.Price)
                  .Select(b => new { b.Title, b.Price })
                  .ToList();
            return String.Join(Environment.NewLine, bookTitles.Select(b => $"{b.Title} - ${b.Price:f2}"));
        }

        //P04
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var bookTitles = context.Books
                  .Where(b => b.ReleaseDate.Value.Year!=year)
                  .OrderBy(b => b.BookId)
                  .Select(b => b.Title)
                  .ToList();
            return String.Join(Environment.NewLine, bookTitles);
        }

        //P05
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            string[] categories = input
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(c => c.ToLower())
                .ToArray();
            List<string> bookTitles = new List<string>();
            foreach  (string cat in categories)
            {
                List<string> currentCat = context
                    .Books
                    .Where(b => b.BookCategories.Any(bc => bc.Category.Name.ToLower() == cat))
                    .Select(b => b.Title)
                    .ToList();
                bookTitles.AddRange(currentCat);
            }
            bookTitles = bookTitles
                .OrderBy(bt => bt)
                .ToList();
            return String.Join(Environment.NewLine, bookTitles);
        }

        //P06
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            DateTime currDate = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            return string.Join(Environment.NewLine, context.Books
                .Where(b => b.ReleaseDate < currDate)
                .OrderByDescending(b => b.ReleaseDate)
                .Select(b => $"{b.Title} - {b.EditionType} - ${b.Price:F2}"));
        }

        //P07
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var bookTitles = context.Authors
               .ToList()
               .Where(a => a.FirstName != null && a.FirstName.EndsWith(input))
               .Select(a => a.FirstName == null ? a.LastName : $"{a.FirstName} {a.LastName}")
               .OrderBy(n => n)
               .ToList();
            
            return string.Join(Environment.NewLine, bookTitles);
        }

        //P08
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            input = input.ToLower();
            var bookTitles = context.Books
              .ToList()
              .Where(b=>b.Title.ToLower().Contains(input))
              .Select(b => b.Title)
              .OrderBy(t => t)
              .ToList();
            return string.Join(Environment.NewLine, bookTitles);
        }

        //P09
        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            input = input.ToLower();
            var bookTitles = context.Books
                .Where(b => b.Author.LastName.ToLower().StartsWith(input))
                .OrderBy(b => b.BookId)
                .Select(b => b.Author.FirstName == null
                    ? $"{b.Title} ({b.Author.LastName})"
                    : $"{b.Title} ({b.Author.FirstName} {b.Author.LastName})");
            return string.Join(Environment.NewLine, bookTitles);
        }

        //P10
        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var bookTitles=context.Books
                 .Where(b => b.Title.Length > lengthCheck)
                 .Count();
            return context.Books
                 .Where(b => b.Title.Length > lengthCheck)
                 .Count();
        }

        //P11
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var bookTitles = context.Authors
                .Select(a => new
                {
                    Name = a.FirstName == null
                        ? a.LastName
                        : $"{a.FirstName} {a.LastName}",
                    Copies = a.Books
                        .Select(b => b.Copies)
                        .Sum()
                })
                .OrderByDescending(a => a.Copies)
                .Select(a => $"{a.Name} - {a.Copies}");
                  return string.Join(Environment.NewLine, bookTitles);
        }

        //P12
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var bookTitles = context.Categories
                .Select(c => new
                {
                    Name = c.Name,
                    TotalProffit = c.CategoryBooks
                        .Select(cb => cb.Book.Price * cb.Book.Copies)
                        .Sum()
                })
                .OrderByDescending(c => c.TotalProffit)
                .ThenBy(c => c.Name)
                .Select(c => $"{c.Name} ${c.TotalProffit:F2}");
            return string.Join(Environment.NewLine, bookTitles);
        }

        //P13
        public static string GetMostRecentBooks(BookShopContext context)
        {
             var bookTitles=context.Categories
                .Select(c => new
                {
                    Name = c.Name,
                    BookCount = c.CategoryBooks
                        .Select(cb => cb.Book)
                        .Count(),
                    TopThreeString = string.Join(Environment.NewLine, c.CategoryBooks
                        .Select(cb => cb.Book)
                        .OrderByDescending(b => b.ReleaseDate)
                        .Take(3)
                        .Select(b => b.ReleaseDate == null
                            ? $"{b.Title} ()"
                            : $"{b.Title} ({b.ReleaseDate.Value.Year})"))
                })
                .OrderBy(c => c.Name)
                .Select(c => $"{c.Name}{Environment.NewLine}{c.TopThreeString}");          
            return "--" + string.Join(Environment.NewLine+"--", bookTitles);
        }

        //P14
        public static void IncreasePrices(BookShopContext context)
        {
            var booksTitles = context.Books
               .Where(b =>b.ReleaseDate.Value.Year < 2010)
               .ToArray();

            foreach (var book in booksTitles)
            {
                book.Price += 5;
            }
            context.SaveChanges();
        }

        //P15
        public static int RemoveBooks(BookShopContext context)
        {
            var bookTitles = context.Books
                .Where(b => b.Copies < 4200)
                .ToArray();

            int result = bookTitles.Count();

            foreach (var book in bookTitles)
            {
                context.Remove(book);
            }
            context.SaveChanges();

            return result;
        }
    }
}
