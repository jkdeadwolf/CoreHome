﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoreHome.Data.Model
{
    public class Article
    {
        public int Id { get; set; }

        [Required]
        public Guid ArticleCode { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public DateTime Time { get; set; }

        public int CategoryId { get; set; }

        [Required]
        public Category Category { get; set; }

        public List<ArticleTag> ArticleTags { get; set; }

        [Required]
        public string Overview { get; set; }

        [Url]
        public string CoverUrl { get; set; }

        [Required]
        public string Content { get; set; }

        public List<Comment> Comments { get; set; }
    }
}
