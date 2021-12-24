using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fiorello.ViewComponents
{
    public class FooterViewComponent:ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
