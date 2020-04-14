using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Domain.Books;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace PersonalWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBookService bookService;

        public HomeController(IBookService bookService)
        {
            this.bookService = bookService;
        }

        public async Task<IActionResult> Index()
        {
            var books = await bookService.GetBooksAsync();
            var model = new HomeViewModel
            {
                BookReviewsNum = books.Count()
            };
            //using (var unit = unitOfWorkFactory.Create())
            //{
            //    var book = new Book
            //    {
            //        Name = "a book",
            //        PurchaseLink = "a purchase link",
            //        Review = new Review
            //        {
            //            LearningRating = 2,
            //            ReadabilityRating = 3,
            //            Text = "it was ok"
            //        }
            //    };
            //    await repo.AddAsync(book);
            //    var read = await repo.GetAsync();
            //    await unit.CommitAsync();
            //}



            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
