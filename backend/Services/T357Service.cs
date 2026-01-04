using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using backend.Models;
using backend.Models.T357;

namespace backend.Services
{
    public interface IT357Service
    {
        Task<T357Response<T>> ExecuteAsync<T>(T357Settings settings, string tag, List<T> masterData);
        Task<T357Response<T>> ExecuteForUserAsync<T>(string lineUserId, string tag, List<T> masterData);
        Task<T357Response<ProductQueryMaster>> QueryProductsForUserAsync(string lineUserId, string? prodId = null);
    }

    public class T357Service : IT357Service
    {
        private readonly HttpClient _httpClient;
        private readonly backend.Data.AppDbContext _db;
        private readonly ILogger<T357Service> _logger;

        public T357Service(
            HttpClient httpClient, 
            backend.Data.AppDbContext db,
            ILogger<T357Service> logger)
        {
            _httpClient = httpClient;
            _db = db;
            _logger = logger;
        }

        private async Task<T357Settings> GetUserSettingsAsync(string lineUserId)
        {
            // 透過關聯查詢，一次抓取 UserProfiles -> UserCustomers -> ErpUsers
            var binding = await _db.UserCustomers
                .Include(uc => uc.ErpUser)
                .Include(uc => uc.UserProfile)
                .AsNoTracking()
                .FirstOrDefaultAsync(uc => uc.UserProfile!.UserId == lineUserId);

            if (binding == null || binding.ErpUser == null)
            {
                throw new Exception("使用者未綁定 ERP 或帳號不存在。");
            }

            return new T357Settings
            {
                BaseUrl = binding.ErpUser.BaseUrl,
                CID = binding.ErpUser.CID,
                UID = binding.CustomerUid,
                UPWD = binding.CustomerUpwd,
                LoginType = "Net"
            };
        }

        public async Task<T357Response<T>> ExecuteForUserAsync<T>(string lineUserId, string tag, List<T> masterData)
        {
            var settings = await GetUserSettingsAsync(lineUserId);
            return await ExecuteAsync(settings, tag, masterData);
        }

        public async Task<T357Response<T>> ExecuteAsync<T>(T357Settings settings, string tag, List<T> masterData)
        {
            var request = new T357Request<T>
            {
                CID = settings.CID,
                UID = settings.UID,
                UPWD = settings.UPWD,
                LoginType = settings.LoginType,
                Tag = tag,
                Data = new T357Data<T>
                {
                    MasterData = masterData
                }
            };

            try
            {
                _logger.LogInformation("Sending T357 API Request: Tag={Tag}, Host={Host}", tag, settings.BaseUrl);
                
                var response = await _httpClient.PostAsJsonAsync(settings.BaseUrl, request);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<T357Response<T>>();

                if (result == null)
                {
                    throw new Exception("Received empty response from T357 API.");
                }

                // Check for business errors
                var apiStatus = result.result.FirstOrDefault();
                if (apiStatus != null && apiStatus.IfSucceed != "True")
                {
                    _logger.LogError("T357 API Error: {Message}", apiStatus.ErrMessage);
                    throw new Exception($"T357 API Error: {apiStatus.ErrMessage}");
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling T357 API for Tag={Tag}", tag);
                throw;
            }
        }

        public async Task<T357Response<ProductQueryMaster>> QueryProductsForUserAsync(string lineUserId, string? prodId = null)
        {
            var queryList = new List<ProductQueryMaster>();
            if (!string.IsNullOrEmpty(prodId))
            {
                queryList.Add(new ProductQueryMaster { ProdID = prodId });
            }

            return await ExecuteForUserAsync(lineUserId, T357Tags.ProductQuery, queryList);
        }
    }
}
