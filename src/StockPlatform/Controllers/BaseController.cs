using Microsoft.AspNetCore.Mvc;
using StockPlatform.Models;
using StockPlatform.Settings;

namespace StockPlatform.Controllers
{
    public class BaseController : Controller
    {
        protected T InitializeViewModel<T>(AppSettings settings, T viewModel = null) where T : LayoutViewModel, new()
        {
            if (viewModel == null)
            {
                viewModel = new T();
            }

            viewModel.StockApi = settings.StockApi;
            return viewModel;
        }
    }
}
