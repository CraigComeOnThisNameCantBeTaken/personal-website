using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Domain.Books;
using Domain.Summaries;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace PersonalWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProfileSummaryService profileSummaryService;

        public HomeController(ProfileSummaryService profileSummaryService)
        {
            this.profileSummaryService = profileSummaryService;
        }

        public async Task<IActionResult> Index()
        {
            var data = await profileSummaryService.GetProfileSummaryAsync();
            var model = new HomeViewModel
            {
                BookReviewsNum = data.BookReviewsNum,
                ProjectNum = data.ProjectNum,
                CommitsNum = data.CommitsNum
            };
            
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
