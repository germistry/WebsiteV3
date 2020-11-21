using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using WebsiteV3.Data.Repository;

namespace WebsiteV3.ViewComponents
{
    [ViewComponent(Name = "PortfolioAssetsViewComponent")]
    public class PortfolioAssetsViewComponent : ViewComponent
    {
        private readonly IRepository _repo;

        public PortfolioAssetsViewComponent(IRepository repo)
        {
            _repo = repo;
        }
        public Task<IViewComponentResult> InvokeAsync(int id)
        {
            var model = _repo.GetAllPortfolioAssets();
            var result = model.Where(x => x.PortfolioItem.Id == id).ToList();

            return Task.FromResult<IViewComponentResult>(View("PortfolioAssetsVC", result));
        }
    }
}
