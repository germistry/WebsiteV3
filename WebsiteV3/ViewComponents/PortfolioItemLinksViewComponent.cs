using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsiteV3.Data.Repository;

namespace WebsiteV3.ViewComponents
{
    [ViewComponent(Name = "PortfolioItemLinksViewComponent")]
    public class PortfolioItemLinksViewComponent : ViewComponent
    {
        private readonly IRepository _repo;

        public PortfolioItemLinksViewComponent(IRepository repo)
        {
            _repo = repo;
        }
        public Task<IViewComponentResult> InvokeAsync(int id)
        {
            var model = _repo.GetPortfolioItemLinks();
            var result = model.Where(x => x.Id != id).ToList();

            return Task.FromResult<IViewComponentResult>(View("PortfolioItemLinksVC", result));
        }
    }
}
