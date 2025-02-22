using Microsoft.AspNetCore.Mvc;
using HttpResponseMessage = System.Net.Http.HttpResponseMessage;

namespace EndPoint.Controllers
{
    public class HomeController : Controller
    {
        // متد GET
        public IActionResult Index()
        {
            var test = "test";
            return View();
        }

        // متد POST برای دریافت داده‌ها
        [HttpPost]
        public IActionResult SubmitData()
        {
            // پردازش داده‌ها
            // مثلا ذخیره یا انجام کارهایی با داده‌ها
            var test = "test";

            // بازگشت به ویو با پیام
            return Content(Response, test);
        }

        private IActionResult Content(HttpResponse response, string test)
        {
            throw new NotImplementedException();
        }
    }
}