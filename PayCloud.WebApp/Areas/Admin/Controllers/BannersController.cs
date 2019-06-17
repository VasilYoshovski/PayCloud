using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PayCloud.Data.DbContext;
using PayCloud.Data.Models;
using PayCloud.Services.Contracts;
using PayCloud.Services.Providers;
using PayCloud.WebApp.Areas.Admin.Models.BannerViewModels;
using PayCloud.WebApp.Mappers;
using PayCloud.WebApp.Utils;

namespace PayCloud.WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [JWTAuthorizeAttribute("Admin")]

    public class BannersController : Controller
    {
        private readonly IViewModelMapper<IReadOnlyCollection<Banner>, BannersCollectionViewModel> bannersCollectionMapper;
        private readonly IViewModelMapper<Banner, BannerViewModel> bannerMapper;
        private readonly IBannerServices bannerServices;
        private readonly IFileServicesProvider fileServicesProvider;
        private readonly IDateTimeNowProvider dateTimeNowProvider;
        private readonly ILogger<BannersController> logger;

        public BannersController(
            IViewModelMapper<IReadOnlyCollection<Banner>, BannersCollectionViewModel> bannersCollectionMapper,
            IViewModelMapper<Banner, BannerViewModel> bannerMapper,
            IBannerServices bannerServices,
            IFileServicesProvider fileServicesProvider,
            IDateTimeNowProvider dateTimeNowProvider,
            ILogger<BannersController> logger)
        {
            this.bannersCollectionMapper = bannersCollectionMapper ?? throw new ArgumentNullException(nameof(bannersCollectionMapper));
            this.bannerMapper = bannerMapper ?? throw new ArgumentNullException(nameof(bannerMapper));
            this.bannerServices = bannerServices ?? throw new ArgumentNullException(nameof(bannerServices));
            this.fileServicesProvider = fileServicesProvider ?? throw new ArgumentNullException(nameof(fileServicesProvider));
            this.dateTimeNowProvider = dateTimeNowProvider ?? throw new ArgumentNullException(nameof(dateTimeNowProvider));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET: Products list JSON for AJAX
        public async Task<IActionResult> GetBannersList(string term, int bannerTypeSelector, int? pageNumber, int? totalPages, int? pageSize)
        {
            string sortOrder = "";
            string currentFilter = "";
            this.ViewData["CurrentSort"] = sortOrder;
            this.ViewData["LengthOptions"] = new int[] { 5, 10, 15, 20 };

            this.ViewData["CurrentFilter"] = currentFilter;
            //this.ViewData["ClientId"] = clientId;

            this.ViewData["pageSize"] = pageSize;

            var tmpDateTime = this.dateTimeNowProvider.Now;
            var tmpStartDateTime = new DateTime(
                tmpDateTime.Year,
                tmpDateTime.Month,
                tmpDateTime.Day,
                tmpDateTime.Hour,
                tmpDateTime.Minute,
                0, 0);
            var tmpEndDateTime = tmpStartDateTime
                .AddHours(24)
                .AddMinutes(60 - tmpStartDateTime.Minute);
            var banner = new Banner()
            {
                UrlLink = "",
                ImgLocationPath = "",
                StartDate = tmpStartDateTime,
                EndDate = tmpEndDateTime
            };
            var x = this.bannerMapper.MapFrom(banner);
            var BannerIndexViewModelObject = new BannerIndexViewModel()
            {
                BannerViewModelObject = x,
                PageIndex = pageNumber ?? 1,
                TotalPages = totalPages ?? 1,
                HasNextPage = false,
                HasPreviousPage = false,
                ElementsPerPage = pageSize ?? 5,
                ImageMaxSize = 32 * 1024,
                BannerActivityType = bannerTypeSelector,
                SearchString = term,
                AllDatabaseBannersCount = 0,
                BannersList = null
            };
            switch (BannerIndexViewModelObject.BannerActivityType)
            {
                case 1:
                    {
                        (List<Banner> filteredList, int allCount, int pageNum, int elementsPerPage) = await this.bannerServices.GetPagedAllActiveBannersByFilterAsync(
                            BannerIndexViewModelObject.PageIndex,
                            BannerIndexViewModelObject.ElementsPerPage,
                            BannerIndexViewModelObject.SearchString);
                        BannerIndexViewModelObject.BannersList = this.bannersCollectionMapper.MapFrom(filteredList);
                        BannerIndexViewModelObject.AllDatabaseBannersCount = allCount;
                        BannerIndexViewModelObject.TotalPages = (BannerIndexViewModelObject.AllDatabaseBannersCount + BannerIndexViewModelObject.ElementsPerPage - 1) / BannerIndexViewModelObject.ElementsPerPage;
                        break;
                    }
                case 2:
                    {
                        (List<Banner> filteredList, int allCount, int pageNum, int elementsPerPage) = await this.bannerServices.GetPagedAllInactiveBannersByFilterAsync(
                            BannerIndexViewModelObject.PageIndex,
                            BannerIndexViewModelObject.ElementsPerPage,
                            BannerIndexViewModelObject.SearchString);
                        BannerIndexViewModelObject.BannersList = this.bannersCollectionMapper.MapFrom(filteredList);
                        BannerIndexViewModelObject.AllDatabaseBannersCount = allCount;
                        BannerIndexViewModelObject.TotalPages = (BannerIndexViewModelObject.AllDatabaseBannersCount + BannerIndexViewModelObject.ElementsPerPage - 1) / BannerIndexViewModelObject.ElementsPerPage;
                        break;
                    }
                case 0:
                default:
                    {
                        BannerIndexViewModelObject.BannerActivityType = 0;
                        (List<Banner> filteredList, int allCount, int pageNum, int elementsPerPage) = await this.bannerServices.GetPagedAllBannersByFilterAsync(
                            BannerIndexViewModelObject.PageIndex - 1,
                            BannerIndexViewModelObject.ElementsPerPage,
                            BannerIndexViewModelObject.SearchString);
                        BannerIndexViewModelObject.BannersList = this.bannersCollectionMapper.MapFrom(filteredList);
                        BannerIndexViewModelObject.AllDatabaseBannersCount = allCount;
                        BannerIndexViewModelObject.TotalPages = (BannerIndexViewModelObject.AllDatabaseBannersCount + BannerIndexViewModelObject.ElementsPerPage - 1) / BannerIndexViewModelObject.ElementsPerPage;
                        break;
                    }
            }
            if ((BannerIndexViewModelObject.PageIndex * BannerIndexViewModelObject.ElementsPerPage) > BannerIndexViewModelObject.AllDatabaseBannersCount)
            {
                if (BannerIndexViewModelObject.AllDatabaseBannersCount < BannerIndexViewModelObject.ElementsPerPage)
                {
                    //BannerIndexViewModelObject.PageIndex = 1;
                }
                else
                {
                    //BannerIndexViewModelObject.PageIndex = BannerIndexViewModelObject.AllDatabaseBannersCount / BannerIndexViewModelObject.ElementsPerPage;
                }
            }
            BannerIndexViewModelObject.HasPreviousPage = (BannerIndexViewModelObject.PageIndex > 1) ? true : false;
            BannerIndexViewModelObject.HasNextPage = (BannerIndexViewModelObject.PageIndex < BannerIndexViewModelObject.TotalPages) ? true : false;
            var result1 = PaginatedList<BannerViewModel>.Create(BannerIndexViewModelObject.BannersList.Banners.ToList(), BannerIndexViewModelObject.AllDatabaseBannersCount/*accountsCount*/, BannerIndexViewModelObject.PageIndex, BannerIndexViewModelObject.ElementsPerPage/*(int)pageSize*/, true);
            return this.PartialView("_BannersListPartial", result1);
            //return this.Json(new  BannerIndexViewModelObject);
        }

        // GET: Admin/Banners
        [HttpGet]
        public async Task<IActionResult> Index(int? pageNumber, int? totalPages, int? pageSize)
        {
            this.ViewData["Title"] = "Banners administration";
            var tmpDateTime = this.dateTimeNowProvider.Now;
            var tmpStartDateTime = new DateTime(
                tmpDateTime.Year,
                tmpDateTime.Month,
                tmpDateTime.Day,
                tmpDateTime.Hour,
                tmpDateTime.Minute,
                0, 0);
            var tmpEndDateTime = tmpStartDateTime
                .AddHours(24)
                .AddMinutes(60 - tmpStartDateTime.Minute);
            var banner = new Banner()
            {
                UrlLink = "",
                ImgLocationPath = "",
                StartDate = tmpStartDateTime,
                EndDate = tmpEndDateTime
            };
            var x = this.bannerMapper.MapFrom(banner);
            var BannerIndexViewModelObject = new BannerIndexViewModel()
            {
                BannerViewModelObject = x,
                PageIndex = pageNumber??1,
                TotalPages = totalPages??1,
                HasNextPage = false,
                HasPreviousPage = false,
                ElementsPerPage = pageSize??5,
                ImageMaxSize = 32 * 1024,
                BannerActivityType = 0,
                SearchString = "",
                AllDatabaseBannersCount = 0,
                BannersList = null
            };
            (List<Banner> filteredList, int allCount, int pageNumberTmp, int elementsPerPage) = await this.bannerServices.GetPagedAllBannersByFilterAsync(
                BannerIndexViewModelObject.PageIndex - 1,
                BannerIndexViewModelObject.ElementsPerPage,
                BannerIndexViewModelObject.SearchString);
            BannerIndexViewModelObject.BannersList = this.bannersCollectionMapper.MapFrom(filteredList);
            BannerIndexViewModelObject.AllDatabaseBannersCount = allCount;
            BannerIndexViewModelObject.TotalPages = (BannerIndexViewModelObject.AllDatabaseBannersCount + BannerIndexViewModelObject.ElementsPerPage - 1) / BannerIndexViewModelObject.ElementsPerPage;
            if ((BannerIndexViewModelObject.PageIndex * BannerIndexViewModelObject.ElementsPerPage) > BannerIndexViewModelObject.AllDatabaseBannersCount)
            {
                if (BannerIndexViewModelObject.AllDatabaseBannersCount < BannerIndexViewModelObject.ElementsPerPage)
                {
                    //BannerIndexViewModelObject.PageIndex = 1;
                }
                else
                {
                    //BannerIndexViewModelObject.PageIndex = BannerIndexViewModelObject.AllDatabaseBannersCount / BannerIndexViewModelObject.ElementsPerPage;
                }
            }
            BannerIndexViewModelObject.HasPreviousPage = (BannerIndexViewModelObject.PageIndex > 1) ? true : false;
            BannerIndexViewModelObject.HasNextPage = (BannerIndexViewModelObject.PageIndex < BannerIndexViewModelObject.TotalPages) ? true : false;

            return View(BannerIndexViewModelObject);
        }

        // GET: Admin/Banners
        [HttpPost]
        public async Task<IActionResult> Index(BannerIndexViewModel BannerIndexViewModelObject, int? pageNumber, int? totalPages, int? pageSize)
        {
            if (null == BannerIndexViewModelObject)
            {
                return View(BannerIndexViewModelObject);
            }
            if (null == BannerIndexViewModelObject.BannerViewModelObject.UrlLink)
            {
                BannerIndexViewModelObject.BannerViewModelObject.UrlLink = "";
            }
            if (null == BannerIndexViewModelObject.BannerViewModelObject.ImageLocationPath)
            {
                BannerIndexViewModelObject.BannerViewModelObject.ImageLocationPath = "";
            }
            if (null == BannerIndexViewModelObject.BannerViewModelObject.ImageData)
            {
                return View(BannerIndexViewModelObject);
            }
            BannerIndexViewModelObject.PageIndex = pageNumber ?? 1;
            BannerIndexViewModelObject.ElementsPerPage = pageSize ?? 5;
            BannerIndexViewModelObject.TotalPages = totalPages ?? 1;
            BannerIndexViewModelObject.BannerViewModelObject.UrlLink = BannerIndexViewModelObject.BannerViewModelObject.UrlLink.Trim();
            BannerIndexViewModelObject.BannerViewModelObject.ImageLocationPath = BannerIndexViewModelObject.BannerViewModelObject.ImageLocationPath.Trim();
            if (BannerIndexViewModelObject.BannerViewModelObject.UrlLink.Length < 3)
            {
                return View(BannerIndexViewModelObject);
            }
            if (BannerIndexViewModelObject.BannerViewModelObject.ImageLocationPath.Length < 3)
            {
                return View(BannerIndexViewModelObject);
            }

            if (ModelState.IsValid)
            {
            }
            switch (BannerIndexViewModelObject.BannerActivityType)
            {
                case 1:
                    {
                        (List<Banner> filteredList, int allCount, int pageNumber01, int elementsPerPage) = await this.bannerServices.GetPagedAllActiveBannersByFilterAsync(
                            BannerIndexViewModelObject.PageIndex,
                            BannerIndexViewModelObject.ElementsPerPage,
                            BannerIndexViewModelObject.SearchString);
                        BannerIndexViewModelObject.BannersList = this.bannersCollectionMapper.MapFrom(filteredList);
                        BannerIndexViewModelObject.AllDatabaseBannersCount = allCount;
                        BannerIndexViewModelObject.TotalPages = (BannerIndexViewModelObject.AllDatabaseBannersCount + BannerIndexViewModelObject.ElementsPerPage - 1) / BannerIndexViewModelObject.ElementsPerPage;
                        break;
                    }
                case 2:
                    {
                        (List<Banner> filteredList, int allCount, int pageNumber11, int elementsPerPage) = await this.bannerServices.GetPagedAllInactiveBannersByFilterAsync(
                            BannerIndexViewModelObject.PageIndex,
                            BannerIndexViewModelObject.ElementsPerPage,
                            BannerIndexViewModelObject.SearchString);
                        BannerIndexViewModelObject.BannersList = this.bannersCollectionMapper.MapFrom(filteredList);
                        BannerIndexViewModelObject.AllDatabaseBannersCount = allCount;
                        BannerIndexViewModelObject.TotalPages = (BannerIndexViewModelObject.AllDatabaseBannersCount + BannerIndexViewModelObject.ElementsPerPage - 1) / BannerIndexViewModelObject.ElementsPerPage;
                        break;
                    }
                case 0:
                default:
                    {
                        BannerIndexViewModelObject.BannerActivityType = 0;
                        (List<Banner> filteredList, int allCount, int pageNumber12, int elementsPerPage) = await this.bannerServices.GetPagedAllBannersByFilterAsync(
                            BannerIndexViewModelObject.PageIndex - 1,
                            BannerIndexViewModelObject.ElementsPerPage,
                            BannerIndexViewModelObject.SearchString);
                        BannerIndexViewModelObject.BannersList = this.bannersCollectionMapper.MapFrom(filteredList);
                        BannerIndexViewModelObject.AllDatabaseBannersCount = allCount;
                        BannerIndexViewModelObject.TotalPages = (BannerIndexViewModelObject.AllDatabaseBannersCount + BannerIndexViewModelObject.ElementsPerPage - 1) / BannerIndexViewModelObject.ElementsPerPage;
                        break;
                    }
            }
            if ((BannerIndexViewModelObject.PageIndex * BannerIndexViewModelObject.ElementsPerPage) > BannerIndexViewModelObject.AllDatabaseBannersCount)
            {
                if (BannerIndexViewModelObject.AllDatabaseBannersCount < BannerIndexViewModelObject.ElementsPerPage)
                {
                    //BannerIndexViewModelObject.PageIndex = 1;
                }
                else
                {
                    //BannerIndexViewModelObject.PageIndex = BannerIndexViewModelObject.AllDatabaseBannersCount / BannerIndexViewModelObject.ElementsPerPage;
                }
            }
            BannerIndexViewModelObject.HasPreviousPage = (BannerIndexViewModelObject.PageIndex > 1) ? true : false;
            BannerIndexViewModelObject.HasNextPage = (BannerIndexViewModelObject.PageIndex < BannerIndexViewModelObject.TotalPages) ? true : false;
            return View(BannerIndexViewModelObject);
        }

        // GET: Admin/Banners/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banner = await this.bannerServices.FindBannerByIDAsync(id ?? -1);
            if (banner == null)
            {
                return NotFound();
            }

            return View(banner);
        }

        // GET: Admin/Banners/Create
        public IActionResult Create()
        {
            var tmpDateTime = this.dateTimeNowProvider.Now;
            var tmpStartDateTime = new DateTime(
                tmpDateTime.Year,
                tmpDateTime.Month,
                tmpDateTime.Day,
                tmpDateTime.Hour,
                tmpDateTime.Minute,
                0, 0);
            var tmpEndDateTime = tmpStartDateTime
                .AddHours(24)
                .AddMinutes(60-tmpStartDateTime.Minute);
            var banner = new Banner()
            {
                UrlLink = "",
                ImgLocationPath = "",
                StartDate = tmpStartDateTime,
                EndDate = tmpEndDateTime
            };
            return RedirectToAction(nameof(Index), this.bannerMapper.MapFrom(banner));
        }

        // POST: Admin/Banners/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BannerViewModel bannerView)
        {
            if (null == bannerView)
            {
                return RedirectToAction(nameof(Index),bannerView);
            }
            if (null == bannerView.UrlLink)
            {
                bannerView.UrlLink = "";
            }
            if (null == bannerView.ImageLocationPath)
            {
                bannerView.ImageLocationPath = "";
            }
            if (null == bannerView.ImageData)
            {
                return RedirectToAction(nameof(Index), bannerView);
            }
            bannerView.UrlLink = bannerView.UrlLink.Trim();
            bannerView.ImageLocationPath = bannerView.ImageLocationPath.Trim();
            if (bannerView.UrlLink.Length < 3)
            {
                bannerView.ImageData = null;
                bannerView.ImageLocationPath = "";
                return RedirectToAction(nameof(Index), bannerView);
            }

            if (!bannerView.UrlLink.ToLower().StartsWith("http://") && !bannerView.UrlLink.ToLower().StartsWith("https://"))
            {
                bannerView.UrlLink = "http://" + bannerView.UrlLink;
            }

            if (bannerView.ImageLocationPath.Length < 3)
            {
                bannerView.ImageData = null;
                bannerView.ImageLocationPath = "";
                return RedirectToAction(nameof(Index), bannerView);
            }

            if (ModelState.IsValid)
            {
                //save image in file
                if (bannerView.ImageData.Length > 0)
                {
                    //var filePath = AppDomain.CurrentDomain.BaseDirectory;
                    //var filePath = Path.GetTempFileName();
                    var bannerUploadsFolder = System.IO.Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot\\images\\BannersStorage");
                    this.fileServicesProvider.CreateFolder(bannerUploadsFolder);
                    Banner createBannerResult;
                    do
                    {
                        string bannerDBImageLocationPath;
                        string fullFilePath;
                        do
                        {
                            var newFileName = $"{this.dateTimeNowProvider.Now.Ticks.ToString()}_{bannerView.ImageData.FileName}";
                            fullFilePath = Path.Combine(bannerUploadsFolder, newFileName);
                            bannerDBImageLocationPath = $"/images/BannersStorage/{newFileName}";
                        } while (null != (await this.bannerServices.FindBannerByImageLocationPathAsync(bannerDBImageLocationPath)));
                        using (var stream = new FileStream(fullFilePath, FileMode.Create))
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                await bannerView.ImageData.CopyToAsync(memoryStream);
                                // process uploaded files
                                var x = memoryStream.ToArray();
                                stream.Write(x, 0, x.Length);
                            }
                        }

                        bannerView.StartDate = new DateTime(
                            bannerView.StartDate.Year,
                            bannerView.StartDate.Month,
                            bannerView.StartDate.Day,
                            bannerView.StartDate.Hour,
                            bannerView.StartDate.Minute,
                            0, 0);
                        bannerView.EndDate = new DateTime(
                            bannerView.EndDate.Year,
                            bannerView.EndDate.Month,
                            bannerView.EndDate.Day,
                            bannerView.EndDate.Hour,
                            bannerView.EndDate.Minute,
                            0, 0);

                        createBannerResult = await this.bannerServices.CreateBannerAsync(
                            bannerDBImageLocationPath,
                            bannerView.UrlLink,
                            bannerView.StartDate,
                            bannerView.EndDate);
                    } while (null == createBannerResult);
                    return RedirectToAction(nameof(Index), bannerView);
                }
            }
            bannerView.ImageData = null;
            bannerView.ImageLocationPath = "";
            return RedirectToAction(nameof(Index), bannerView);
        }

        //// GET: Admin/Banners/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var banner = await this.bannerServices.GetBannerByIDAsync(id ?? -1);
        //    if (banner == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(banner);
        //}

        //// POST: Admin/Banners/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("BannerId,UrlLink,ImgLocationPath,StartDate,EndDate")] Banner bannerDB)
        //{
        //    if (id != bannerDB.BannerId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            await this.bannerServices.UpdateBannerAsync(
        //                bannerDB.BannerId,
        //                bannerDB.ImgLocationPath,
        //                bannerDB.UrlLink,
        //                bannerDB.StartDate,
        //                bannerDB.EndDate);
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (null == (await this.bannerServices.FindBannerByIDAsync(bannerDB.BannerId)))
        //            {
        //                throw;
        //            }
        //            else
        //            {
        //                return NotFound();
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(bannerDB);
        //}

        //// GET: Admin/Banners/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var banner = await this.bannerServices.FindBannerByIDAsync(id ?? -1);
        //    if (banner == null)
        //    {
        //        // Go to index menu, because the banner has probably been deleted by someone else meanwhile
        //        return RedirectToAction(nameof(Index));
        //        //return NotFound();
        //    }

        //    return View(banner);
        //}

        //// POST: Admin/Banners/Delete/5
        //[HttpPost, ActionName("Delete")]
        ////[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    string bannerFilePath = (await this.bannerServices.GetBannerByIDAsync(id)).ImgLocationPath;
        //    await this.bannerServices.DeleteBannerAsync(id);
        //    // Delete the file from the folder
        //    string bannerUploadsFolder = System.IO.Path.Combine(
        //        Directory.GetCurrentDirectory(),
        //        "wwwroot\\images\\BannersStorage");
        //    string bannerFullFilePath = Path.Combine(bannerUploadsFolder, bannerFilePath);
        //    var (deleteResult, deleteMessage) = this.fileServicesProvider.DeleteFile(bannerFullFilePath);

        //    if (deleteResult)
        //    {
        //    }
        //    else
        //    {
        //        // Do nothing, because the banner has probably been deleted by someone else meanwhile
        //        // deleteMessage could be passed for info
        //    }

        //    return RedirectToAction(nameof(Index));
        //}

        //public async Task<IActionResult> UpdateBanner(int id, [Bind("BannerId,UrlLink,ImgLocationPath,StartDate,EndDate")] Banner bannerDB)
        //{
        //    if (id != bannerDB.BannerId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            await this.bannerServices.UpdateBannerAsync(
        //                bannerDB.BannerId,
        //                bannerDB.ImgLocationPath,
        //                bannerDB.UrlLink,
        //                bannerDB.StartDate,
        //                bannerDB.EndDate);
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (null == (await this.bannerServices.FindBannerByIDAsync(bannerDB.BannerId)))
        //            {
        //                throw;
        //            }
        //            else
        //            {
        //                return NotFound();
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(bannerDB);
        //}

        public async Task<IActionResult> DeleteBanner(int id)
        {
            var bannerTemp = await this.bannerServices.FindBannerByIDAsync(id);
            if (null == bannerTemp)
            {
                var infoText = $"Banner with ID {id} already deleted";
                this.logger.LogInformation(infoText);
                return this.Json(infoText);
            }
            string bannerFilePath = bannerTemp.ImgLocationPath;
            await this.bannerServices.DeleteBannerAsync(id);
            // Delete the file from the folder
            string bannerUploadsFolder = System.IO.Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot");
            //"wwwroot\\images\\BannersStorage");
            string bannerFullFilePath = Path.Combine(bannerUploadsFolder, bannerFilePath.Substring(1));
            var (deleteResult, deleteMessage) = this.fileServicesProvider.DeleteFile(bannerFullFilePath);

            if (deleteResult)
            {
                var infoText = $"Banner with ID {id} deleted by admin {"Neznaen admin"} at {this.dateTimeNowProvider.Now.ToLocalTime()}. {deleteMessage}";
                this.logger.LogInformation(infoText);
                return this.Json(infoText);
            }
            else
            {
                // Do nothing, because the banner has probably been deleted by someone else meanwhile
                // deleteMessage could be passed for info
                var infoText = $"Banner with ID {id} deleted by admin {"Neznaen admin"} at {this.dateTimeNowProvider.Now.ToLocalTime()}. {deleteMessage}";
                this.logger.LogInformation(infoText);
                return this.Json($"Banner with ID {id} deleted. {deleteMessage}");
            }
        }
    }
}
