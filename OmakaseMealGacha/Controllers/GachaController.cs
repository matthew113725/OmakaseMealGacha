using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OmakaseMealGacha.Models;

namespace OmakaseMealGacha.Controllers
{
    public class GachaController : Controller
    {
        private readonly MealGachaDbContext _context;

        public GachaController(MealGachaDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Gacha()
        {
            var menus = await _context.Menus
                .OrderBy(m => m.Id)
                .ToListAsync();

            var histories = await _context.History
                .OrderByDescending(h => h.RolledAt)
                .Take(10)
                .ToListAsync();

            ViewBag.Menus = menus;
            ViewBag.History = histories;
            return View();
        }

        [HttpPost]
        public JsonResult SpinGacha(string userName)
        {
            var allMenusInDb = _context.Menus.ToList();
            var allMenus = allMenusInDb
                .Where(m => m.IsInGacha)
                .Select(m => m.Name)
                .ToList();

            if (allMenusInDb.Count == 0)
            {
                return Json(new { error = true, message = "メニューが1件も登録されていません。" });
            }
            else if (allMenus.Count == 0)
            {
                return Json(new { error = true, message = "ガチャ対象のメニューがありません。" });
            }

            var random = new Random();
            var selected = allMenus[random.Next(allMenus.Count)];

            _context.History.Add(new History
            {
                UserName = userName,
                MenuName = selected,
                RolledAt = DateTime.Now
            });
            _context.SaveChanges();

            return Json(new { error = false, result = selected, allMenus });
        }

        [HttpPost]
        public async Task<JsonResult> AddMenu(string newItem)
        {
            bool exists = await _context.Menus.AnyAsync(m => m.Name == newItem);

            if (exists)
            {
                return Json(new { error = true, message = $"「{newItem}」というメニューはすでに存在します。" });
            }

            var menu = new Menu
            {
                Name = newItem,
                IsInGacha = true,
                CreatedOn = DateTime.Now,
                LastModifiedOn = DateTime.Now
            };
            _context.Menus.Add(menu);
            await _context.SaveChangesAsync();

            return Json(new { error = false, message = $"「{newItem}」をメニューに追加しました。" });
        }

        [HttpPost]
        public async Task<JsonResult> EditMenu(int id, string newName)
        {
            // 同じ名前の別のメニューが存在するか確認
            bool duplicateExists = await _context.Menus
                .AnyAsync(m => m.Id != id && m.Name == newName);

            if (duplicateExists)
            {
                return Json(new { error = true, message = $"「{newName}」というメニューはすでに存在しています。" });
            }

            var menu = await _context.Menus.FindAsync(id);
            if (menu == null)
            {
                return Json(new { error = true, message = "指定されたメニューが見つかりませんでした。" });
            }

            menu.Name = newName;
            menu.LastModifiedOn = DateTime.Now;
            await _context.SaveChangesAsync();

            return Json(new { error = false, message = $"「{newName}」に更新しました。" });
        }

        [HttpPost]
        public async Task<JsonResult> ToggleGacha(int id, bool isInGacha)
        {
            var menu = await _context.Menus.FindAsync(id);
            if (menu != null)
            {
                menu.IsInGacha = isInGacha;
                menu.LastModifiedOn = DateTime.Now;
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public async Task<JsonResult> DeleteMenu(int id)
        {
            var menu = await _context.Menus.FindAsync(id);
            if (menu != null)
            {
                _context.Menus.Remove(menu);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        public async Task SaveHistoryAsync(string userName, string menuName)
        {
            var history = new History
            {
                UserName = userName,
                MenuName = menuName,
                RolledAt = DateTime.Now
            };

            _context.History.Add(history);
            await _context.SaveChangesAsync();
        }

        [HttpGet]
        public async Task<JsonResult> GetHistory()
        {
            var histories = await _context.History
                .OrderByDescending(h => h.RolledAt)
                .Take(10)
                .ToListAsync();

            var result = histories.Select(h => new
            {
                userName = h.UserName,
                menuName = h.MenuName,
                rolledAt = h.RolledAt.ToString("yyyy/MM/dd HH:mm:ss")
            });

            return Json(result);
        }
    }
}