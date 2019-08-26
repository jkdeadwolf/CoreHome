﻿using DataContext.DbConfigurator;
using DataContext.ModelDbContext;
using DataContext.Models;
using System.Collections.Generic;
using System.Linq;

namespace DataContext.DbOperator
{
    public class ArticleRepository
    {
        private readonly MysqlDbConfigurator configurator;

        public ArticleRepository()
        {
            configurator = new MysqlDbConfigurator();
        }

        public void Add(Article article)
        {
            using (ArticleDbContext context = configurator.CreateArticleDbContext())
            {
                context.Article.Add(article);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// 指定范围查找
        /// </summary>
        /// <param name="index">查找起点</param>
        /// <param name="pageSize">页面展示内容数量</param>
        /// <returns></returns>
        public List<Article> Find(int index, int pageSize)
        {
            int limit = index * pageSize;
            using (ArticleDbContext context = configurator.CreateArticleDbContext())
            {
                int count = Count() - limit;
                return context.Article.OrderByDescending(i => i.ID).Skip(limit).Take(count > 5 ? pageSize : count).ToList();
            }
        }

        public Article Find(int id)
        {
            using (ArticleDbContext context = configurator.CreateArticleDbContext())
            {
                return context.Article.Single(i => i.ID == id);
            }
        }

        public int Count()
        {
            using (ArticleDbContext context = configurator.CreateArticleDbContext())
            {
                return context.Article.Count();
            }
        }

    }
}