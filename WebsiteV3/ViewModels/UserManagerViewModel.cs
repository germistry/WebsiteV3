﻿using System;
using System.Collections.Generic;
using WebsiteV3.Models.PortfolioItemComments;
using WebsiteV3.Models.PostComments;

namespace WebsiteV3.ViewModels
{
    public class UserManagerViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool IsBanned { get; set; }
        public DateTime SignUpDate { get; set; }
        public List<PostMainComment> UserPostMainComments { get; set; }
        public List<PostSubComment> UserPostSubComments { get; set; }
        public List<PortfolioItemMainComment> UserPortfolioItemMainComments { get; set; }
        public List<PortfolioItemSubComment> UserPortfolioItemSubComments { get; set; }
    }
}
