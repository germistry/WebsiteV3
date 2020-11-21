﻿using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using WebsiteV3.Data.Repository;

namespace WebsiteV3.ViewComponents
{
    [ViewComponent(Name = "CategoryLinksPortfolioViewComponent")]
    public class CategoryLinksPortfolioViewComponent : ViewComponent
    {
        private readonly IRepository _repo;

        public CategoryLinksPortfolioViewComponent(IRepository repo)
        {
            _repo = repo;
        }
        public Task<IViewComponentResult> InvokeAsync(int id)
        {
            var model = _repo.GetCategoryLinks();
            var result = model.Where(x => x.Id != id).ToList();

            return Task.FromResult<IViewComponentResult>(View("CategoryLinksPortfolioVC", result));
        }
    }
}
