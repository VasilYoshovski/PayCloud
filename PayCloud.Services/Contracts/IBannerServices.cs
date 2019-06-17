using PayCloud.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PayCloud.Services.Contracts
{
    public interface IBannerServices
    {
        Task<Banner> FindBannerByIDAsync(int id);
        //Task<Banner> GetBannerByIDAsync(int id);
        //Task<Banner> FindBannerByUrlAsync(string searchString);
        //Task<Banner> GetBannerByUrlAsync(string searchString);
        Task<Banner> FindBannerByImageLocationPathAsync(string searchString);
        //Task<Banner> GetBannerByImageLocationPathAsync(string searchString);
        Task<Banner> CreateBannerAsync(string imgPath, string urlLink, DateTime startDate, DateTime endDate);
        //Task<Banner> UpdateBannerAsync(int id, string imgPath, string urlLink, DateTime startDate, DateTime endDate);
        Task<bool> DeleteBannerAsync(int id);
        Task<List<Banner>> GetAllBannersAsync();
        Task<(List<Banner> filteredList, int allCount)> GetAllBannersByFilterAsync(int from, int to, string contains);
        Task<(List<Banner> filteredList, int allCount, int pageNumber, int elementsPerPage)> GetPagedAllBannersByFilterAsync(int pageNumber, int elementsPerPage, string contains);
        Task<List<Banner>> GetAllActiveBannersAsync();
        Task<(List<Banner> filteredList, int allCount)> GetAllActiveBannersByFilterAsync(int from, int to, string contains);
        Task<(List<Banner> filteredList, int allCount, int pageNumber, int elementsPerPage)> GetPagedAllActiveBannersByFilterAsync(int pageNumber, int elementsPerPage, string contains);
        Task<List<Banner>> GetAllInactiveBannersAsync();
        Task<(List<Banner> filteredList, int allCount)> GetAllInactiveBannersByFilterAsync(int from, int to, string contains);
        Task<(List<Banner> filteredList, int allCount, int pageNumber, int elementsPerPage)> GetPagedAllInactiveBannersByFilterAsync(int pageNumber, int elementsPerPage, string contains);
        Task<List<Banner>> GetRandomSublistOfActiveBannersAsync(int bannersCount);
    }
}
