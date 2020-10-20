using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsiteV3.Data.Repository;

namespace WebsiteV3.ViewComponents
{
    [ViewComponent(Name = "PostAssetsViewComponent")]
    public class PostAssetsViewComponent : ViewComponent
    {
        private readonly IRepository _repo;

        public PostAssetsViewComponent(IRepository repo)
        {
            _repo = repo;
        }

        public Task<IViewComponentResult> InvokeAsync(int id)
        {
            var model = _repo.GetAllPostAssets();
            var result = model.Where(x => x.Post.Id == id).ToList();

            return Task.FromResult<IViewComponentResult>(View("PostAssetsVC", result));
        }
    }
}
