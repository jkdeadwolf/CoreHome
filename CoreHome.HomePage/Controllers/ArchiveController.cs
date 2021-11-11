﻿using CoreHome.Data.DatabaseContext;
using CoreHome.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoreHome.HomePage.Controllers
{
    public class ArchiveController : Controller
    {
        private readonly ArticleDbContext articleDbContext;

        public ArchiveController(ArticleDbContext articleDbContext)
        {
            this.articleDbContext = articleDbContext;
        }

        /// <summary>
        /// 归档时间线
        /// </summary>
        /// <returns>归档时间线页面</returns>
        public async Task<IActionResult> Index()
        {
            ViewBag.PageTitle = "Archive";

            List<Year> years = await articleDbContext.Years
                .OrderByDescending(i => i.Value)
                .Include(i => i.Months)
                .ThenInclude(i => i.Articles)
                .ToListAsync();

            return View(years);
        }
    }
}