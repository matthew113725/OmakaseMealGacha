using Microsoft.AspNetCore.Mvc;

namespace OmakaseMealGacha.Controllers
{
    public class GachaController : Controller
    {
        public IActionResult Gacha()
        {
            ViewBag.LunchMenu = new List<string> { "カレー", "ラーメン", "ハンバーグ" };
            ViewBag.DinnerMenu = new List<string> { "鍋", "寿司", "ステーキ" };
            return View();
        }

        [HttpPost]
        public JsonResult GetMenu(string category, string userName)
        {
            var menu = category == "Lunch"
                ? new List<string> { "カレー", "ラーメン", "ハンバーグ" }
                : new List<string> { "鍋", "寿司", "ステーキ" };

            var random = new Random();
            var selected = menu[random.Next(menu.Count)];
            return Json(new { error = false, result = selected });
        }
    }
}