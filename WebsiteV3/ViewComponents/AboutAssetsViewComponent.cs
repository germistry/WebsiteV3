using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using WebsiteV3.Data.Repository;

namespace WebsiteV3.ViewComponents
{
    [ViewComponent(Name = "AboutAssetsViewComponent")]
    public class AboutAssetsViewComponent : ViewComponent
    {
        private readonly IRepository _repo;

        public AboutAssetsViewComponent(IRepository repo)
        {
            _repo = repo;
        }

        public Task<IViewComponentResult> InvokeAsync(int id)
        {
            var model = _repo.GetAllAboutAssets();
            var result = model.Where(x => x.About.Id == id).ToList();

            return Task.FromResult<IViewComponentResult>(View("AboutAssetsVC", result));
        }
    }
}
