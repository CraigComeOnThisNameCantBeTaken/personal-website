using System;
using System.Diagnostics;
using System.Threading.Tasks;
using DataAccess.Entities;
using DataAccess.Repository;
using DataAccess.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using PersonalWebsite.Models;

namespace PersonalWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository<Book> repo;
        private readonly IScopedUnitOfWorkFactory unitOfWorkFactory;

        public HomeController(IRepository<Book> repo, IScopedUnitOfWorkFactory unitOfWorkFactory)
        {
            this.repo = repo;
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public async Task<IActionResult> Index()
        {
            using (var unit = unitOfWorkFactory.Create())
            {
                var book = new Book
                {
                    Name = "a book",
                    PurchaseLink = "a purchase link",
                    Review = new Review
                    {
                        LearningRating = 2,
                        ReadabilityRating = 3,
                        Text = "it was ok"
                    }
                };
                await repo.AddAsync(book);
                var read = await repo.GetAsync();
                await unit.CommitAsync();
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
