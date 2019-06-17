using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PayCloud.Services.Contracts;
using PayCloud.WebApp.Models;

namespace PayCloud.WebApp.Controllers
{
    public class HomeController : Controller
    {
        readonly IBannerServices bannerServices;

        public HomeController(IBannerServices bannerServices)
        {
            this.bannerServices = bannerServices ?? throw new ArgumentNullException(nameof(bannerServices));
        }

        //[ResponseCache(Duration = 600, Location = ResponseCacheLocation.Client)]
        public async Task<IActionResult> Index(string errorMessage)
        {


            LoginViewModel userModel = new LoginViewModel
            {
                Password = "",
                Username = "",
                Banners = await this.bannerServices.GetRandomSublistOfActiveBannersAsync(5)
            };

            if (errorMessage != null)
            {
                this.ViewData["Error"] = errorMessage;
            }

            return View(userModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string errorMessage)
        {
            return View("ErrorPage",errorMessage);
        }
    }
}
