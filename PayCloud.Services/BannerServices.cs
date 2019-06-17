using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PayCloud.Data.DbContext;
using PayCloud.Data.Models;
using PayCloud.Services.Contracts;
using PayCloud.Services.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayCloud.Services
{
    public class BannerServices : IBannerServices
    {
        private readonly PayCloudDbContext context;
        private readonly IDateTimeNowProvider dateTimeNowProvider;
        private readonly ILogger<BannerServices> logger;

        public BannerServices(
            PayCloudDbContext context,
            IDateTimeNowProvider dateTimeNowProvider,
            ILogger<BannerServices> logger)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.dateTimeNowProvider = dateTimeNowProvider ?? throw new ArgumentNullException(nameof(dateTimeNowProvider));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        //public async Task<Banner> FindBannerByUrlAsync(string searchString)
        //{
        //    searchString = string.IsNullOrWhiteSpace(searchString) ? "" : searchString.Trim().ToLower();
        //    return await this.context.Banners.FirstOrDefaultAsync(b => b.UrlLink.ToLower().Contains(searchString));
        //}

        //public async Task<Banner> GetBannerByUrlAsync(string searchString)
        //{
        //    return await FindBannerByUrlAsync(searchString) ?? throw new ArgumentException($"Can not find banner with Url containing {searchString}");
        //}

        public async Task<Banner> FindBannerByImageLocationPathAsync(string searchString)
        {
            searchString = string.IsNullOrWhiteSpace(searchString) ? "": searchString.Trim().ToLower();
            return await this.context.Banners.FirstOrDefaultAsync(b => b.ImgLocationPath.ToLower().Equals(searchString));
        }

        //public async Task<Banner> GetBannerByImageLocationPathAsync(string searchString)
        //{
        //    return await FindBannerByImageLocationPathAsync(searchString) ?? throw new ArgumentException($"Can not find banner with ImageLocationPath containing {searchString}");
        //}

        public async Task<Banner> FindBannerByIDAsync(int id)
        {
            return await this.context.Banners.FirstOrDefaultAsync(b => b.BannerId == id);
        }

        //public async Task<Banner> GetBannerByIDAsync(int id)
        //{
        //    return await FindBannerByIDAsync(id) ?? throw new ArgumentException($"Can not find banner with ID {id}");
        //}

        public async Task<Banner> CreateBannerAsync(
            string imgPath,
            string urlLink,
            DateTime startDate,
            DateTime endDate)
        {
            imgPath = NormalizeString(imgPath, "BannerPath");
            urlLink = NormalizeString(urlLink, "UrlLink");

            if (null != (await FindBannerByImageLocationPathAsync(imgPath)))
            {
                return null;
                //throw new ArgumentException($"Banner with ImageLocationPath {imgPath} already exists!");
            }
            var banner = new Banner
            {
                ImgLocationPath = imgPath,
                UrlLink = urlLink,
                StartDate = startDate,
                EndDate = endDate
            };
            try
            {
                await this.context.Banners.AddAsync(banner);
                await this.context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return null;
            }
            catch (DbUpdateException)
            {
                return null;
            }

            return await FindBannerByIDAsync(banner.BannerId);
        }

        //public async Task<Banner> UpdateBannerAsync(
        //    int id,
        //    string imgPath,
        //    string urlLink,
        //    DateTime startDate,
        //    DateTime endDate)
        //{
        //    imgPath = NormalizeString(imgPath, "BannerPath");
        //    urlLink = NormalizeString(urlLink, "UrlLink");

        //    var banner = await FindBannerByIDAsync(id);
        //    if (banner == null)
        //    {
        //        throw new ArgumentException($"Banner with ID {id} does not exist!");
        //    }
        //    else
        //    {
        //        banner.ImgLocationPath = imgPath;
        //        banner.UrlLink = urlLink;
        //        banner.StartDate = startDate;
        //        banner.EndDate = endDate;
        //        try
        //        {
        //            context.Update(banner);
        //            await context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            throw;
        //        }
        //    }

        //    return await GetBannerByIDAsync(banner.BannerId);
        //}

        public async Task<bool> DeleteBannerAsync(int id)
        {
            var banner = await FindBannerByIDAsync(id);
            if (null != banner)
            {
                try
                {
                    this.context.Banners.Remove(banner);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return false;
                }
                catch (DbUpdateException)
                {
                    return false;
                }
            }
            else
            {
                // Do nothing, because the banner has probably been deleted by someone else meanwhile
                //throw new ArgumentException($"Banner with ID {id} does not exist!");
            }
            return true;
        }

        public async Task<List<Banner>> GetAllBannersAsync()
        {
            return (await this.context.Banners.ToListAsync())
                        .OrderBy(b => b.EndDate)
                        .ThenBy(b => b.UrlLink)
                        .ThenBy(b => b.StartDate)
                        .ThenBy(b => b.ImgLocationPath)
                        .ToList();
        }

        public async Task<List<Banner>> GetAllActiveBannersAsync()
        {
            var dateNow = this.dateTimeNowProvider.Now;
            return (await this.context.Banners.ToListAsync())
                .Where(b => (b.StartDate <= dateNow && b.EndDate >= dateNow))
                        .OrderBy(b => b.EndDate)
                        .ThenBy(b => b.UrlLink)
                        .ThenBy(b => b.StartDate)
                        .ThenBy(b => b.ImgLocationPath)
                        .ToList();
        }

        public async Task<List<Banner>> GetAllInactiveBannersAsync()
        {
            var dateNow = this.dateTimeNowProvider.Now;
            return (await this.context.Banners.ToListAsync())
                .Where(b => (b.StartDate > dateNow || b.EndDate < dateNow))
                        .OrderBy(b => b.EndDate)
                        .ThenBy(b => b.UrlLink)
                        .ThenBy(b => b.StartDate)
                        .ThenBy(b => b.ImgLocationPath)
                        .ToList();
        }

        public async Task<List<Banner>> GetRandomSublistOfActiveBannersAsync(int bannersCount)
        {
            var fullList = await GetAllActiveBannersAsync();
            if (fullList.Count > bannersCount)
            {
                Random random = new Random();
                var randomList = new List<Banner>();
                for (int i = 0; i < bannersCount; i++)
                {
                    var index = random.Next() % fullList.Count;
                    randomList.Add(fullList.ElementAt(index));
                    fullList.RemoveAt(index);
                }
                return randomList;
            }
            return fullList;
        }

        public async Task<(List<Banner> filteredList, int allCount)> GetAllBannersByFilterAsync(
            int from,
            int to,
            string contains)
        {
            var allBanners = await GetAllBannersAsync();
            var allCount = 0;
            if (allBanners != null)
            {
                allCount = allBanners.Count;
                if ((from + to) > allCount)
                {
                    if (allCount < to)
                    {
                        from = 0;
                    }
                    else
                    {
//                        from = allCount - to;
                    }
                }
                contains = string.IsNullOrWhiteSpace(contains) ? "" : contains.Trim().ToLower();
                var filteredBanners = allBanners
                    .Where(x => (x.UrlLink.ToLower().Contains(contains) ||
                    x.ImgLocationPath.ToLower().Contains(contains) ||
                    x.StartDate.ToString().ToLower().Contains(contains) ||
                    x.EndDate.ToString().ToLower().Contains(contains)))
                    .Skip(from)
                    .Take(to);
                return (filteredBanners.ToList(), allCount);
            }
            return (allBanners, allCount);
        }

        public async Task<(List<Banner> filteredList, int allCount)> GetAllActiveBannersByFilterAsync(
            int from,
            int to,
            string contains)
        {
            var allBanners = await GetAllActiveBannersAsync();
            var allCount = 0;
            if (allBanners != null)
            {
                allCount = allBanners.Count;
                if ((from + to) > allCount)
                {
                    if (allCount < to)
                    {
                        from = 0;
                    }
                    else
                    {
//                        from = allCount - to;
                    }
                }
                contains = string.IsNullOrWhiteSpace(contains) ? "" : contains.Trim().ToLower();
                var filteredBanners = allBanners
                    .Where(x => (x.UrlLink.ToLower().Contains(contains) ||
                    x.ImgLocationPath.ToLower().Contains(contains) ||
                    x.StartDate.ToString().ToLower().Contains(contains) ||
                    x.EndDate.ToString().ToLower().Contains(contains)))
                    .Skip(from)
                    .Take(to);
                return (filteredBanners.ToList(), allCount);
            }
            return (allBanners, allCount);
        }

        public async Task<(List<Banner> filteredList, int allCount)> GetAllInactiveBannersByFilterAsync(
            int from,
            int to,
            string contains)
        {
            var allBanners = await GetAllInactiveBannersAsync();
            var allCount = 0;
            if (allBanners != null)
            {
                allCount = allBanners.Count;
                if ((from + to) > allCount)
                {
                    if (allCount < to)
                    {
                        from = 0;
                    }
                    else
                    {
 //                       from = allCount - to;
                    }
                }
                contains = string.IsNullOrWhiteSpace(contains) ? "" : contains.Trim().ToLower();
                var filteredBanners = allBanners
                    .Where(x => (x.UrlLink.ToLower().Contains(contains) ||
                    x.ImgLocationPath.ToLower().Contains(contains) ||
                    x.StartDate.ToString().ToLower().Contains(contains) ||
                    x.EndDate.ToString().ToLower().Contains(contains)))
                    .Skip(from)
                    .Take(to);
                return (filteredBanners.ToList(), allCount);
            }
            return (allBanners, allCount);
        }

        public async Task<(List<Banner> filteredList, int allCount, int pageNumber, int elementsPerPage)> GetPagedAllBannersByFilterAsync(
            int pageNumber,
            int elementsPerPage,
            string contains)
        {
            if (pageNumber < 0)
            {
                pageNumber = 0;
            }
            if (elementsPerPage < 1)
            {
                elementsPerPage = 1;
            }
            (List<Banner> filteredList, int allCount) = await GetAllBannersByFilterAsync(
                pageNumber * elementsPerPage,
                elementsPerPage,
                contains);

            return (filteredList, allCount, pageNumber, elementsPerPage);
        }

        public async Task<(List<Banner> filteredList, int allCount, int pageNumber, int elementsPerPage)> GetPagedAllActiveBannersByFilterAsync(
            int pageNumber,
            int elementsPerPage,
            string contains)
        {
            if (pageNumber < 0)
            {
                pageNumber = 0;
            }
            if (elementsPerPage < 1)
            {
                elementsPerPage = 1;
            }
            (List<Banner> filteredList, int allCount) = await GetAllActiveBannersByFilterAsync(
                pageNumber * elementsPerPage,
                elementsPerPage,
                contains);

            return (filteredList, allCount, pageNumber, elementsPerPage);
        }

        public async Task<(List<Banner> filteredList, int allCount, int pageNumber, int elementsPerPage)> GetPagedAllInactiveBannersByFilterAsync(
            int pageNumber,
            int elementsPerPage,
            string contains)
        {
            if (pageNumber < 0)
            {
                pageNumber = 0;
            }
            if (elementsPerPage < 1)
            {
                elementsPerPage = 1;
            }
            var (filteredList, allCount) = await GetAllInactiveBannersByFilterAsync(
                pageNumber * elementsPerPage,
                elementsPerPage,
                contains);

            return (filteredList, allCount, pageNumber, elementsPerPage);
        }

        private string NormalizeString(string stringToCheck, string exceptionText)
        {
            if (string.IsNullOrEmpty(stringToCheck))
            {
                throw new ArgumentException($"{exceptionText} could not be null or empty!");
            }

            if (string.IsNullOrWhiteSpace(stringToCheck))
            {
                throw new ArgumentException($"{exceptionText} could not be WhiteSpace!");
            }
            return stringToCheck.Trim();
        }
    }
}
