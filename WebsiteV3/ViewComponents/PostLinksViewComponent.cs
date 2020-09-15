﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsiteV3.Data.Repository;
using WebsiteV3.ViewModels;

namespace WebsiteV3.ViewComponents
{
    [ViewComponent(Name = "PostLinksViewComponent")]
    public class PostLinksViewComponent : ViewComponent
    {
        private readonly IRepository _repo;

        public PostLinksViewComponent(IRepository repo)
        {
            _repo = repo;
        }
        public Task<IViewComponentResult> InvokeAsync(int id)
        {
            var model = _repo.GetPostLinks();
            var result = model.Where(x => x.Id != id).ToList();
                                            
            return Task.FromResult<IViewComponentResult>(View("PostLinksVC", result));
        }

    }
}
