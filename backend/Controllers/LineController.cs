using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LineController : ControllerBase
    {
        private readonly ILogger<LineController> _logger;
        private readonly AppDbContext _dbContext;

        public LineController(ILogger<LineController> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet("dashboard")]
        public IActionResult GetDashboardData()
        {
            var data = new DashboardData
            {
                Message = "Connected to C# Azure Backend!",
                UpdateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                ServerStatus = "Online",
                Notifications = 5
            };
            return Ok(data);
        }

        [HttpPost("record")]
        public async Task<IActionResult> RecordAccount([FromBody] UserProfile profile)
        {
            if (profile == null || string.IsNullOrEmpty(profile.UserId))
            {
                return BadRequest("Invalid profile data");
            }

            try
            {
                // 檢查是否已存在該使用者
                var existingProfile = await _dbContext.UserProfiles
                    .Include(u => u.Customers!)
                        .ThenInclude(c => c.ErpUser)
                    .FirstOrDefaultAsync(u => u.UserId == profile.UserId);

                if (existingProfile != null)
                {
                    existingProfile.DisplayName = profile.DisplayName;
                    existingProfile.PictureUrl = profile.PictureUrl;
                    existingProfile.StatusMessage = profile.StatusMessage;
                    existingProfile.UpdateTime = DateTime.UtcNow;

                    _dbContext.UserProfiles.Update(existingProfile);
                    profile = existingProfile; // 使用資料庫中的完整物件
                }
                else
                {
                    profile.CreateTime = DateTime.UtcNow;
                    profile.UpdateTime = DateTime.UtcNow;
                    await _dbContext.UserProfiles.AddAsync(profile);
                }

                await _dbContext.SaveChangesAsync();

                return Ok(new 
                { 
                    success = true, 
                    data = new {
                        profile.UserId,
                        profile.DisplayName,
                        profile.PictureUrl,
                        profile.StatusMessage,
                        customers = profile.Customers?.Select(c => new {
                            c.CustomerUid,
                            erpUser = new {
                                c.ErpUser?.ErpCode,
                                c.ErpUser?.Name
                            }
                        })
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error recording user profile");
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpPost("bind-erp")]
        public async Task<IActionResult> BindERP([FromBody] ErpBindingRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.UserId) || string.IsNullOrEmpty(request.ErpCode))
            {
                return BadRequest("Invalid binding data");
            }

            try
            {
                // 1. 確保人員存在
                var profile = await _dbContext.UserProfiles
                    .FirstOrDefaultAsync(u => u.UserId == request.UserId);

                if (profile == null)
                {
                    return NotFound(new { success = false, message = "User profile not found" });
                }

                // 2. 確保 ERP 系統配置存在
                var erpSystem = await _dbContext.ErpUsers
                    .FirstOrDefaultAsync(e => e.ErpCode == request.ErpCode);

                if (erpSystem == null)
                {
                    return NotFound(new { success = false, message = "ERP System configuration not found" });
                }

                // 3. 驗證帳號密碼是否存在於預先建立的資料中 (Level 1 管理者提供的資料)
                var validAccount = await _dbContext.UserCustomers
                    .FirstOrDefaultAsync(uc => uc.ErpUserId == erpSystem.Id && 
                                              uc.CustomerUid == request.Uid && 
                                              uc.CustomerUpwd == request.Upwd);

                if (validAccount == null)
                {
                    return BadRequest(new { success = false, message = "綁定失敗：帳號或密碼錯誤，或該帳號尚未由管理員建立。" });
                }

                // 4. 檢查該帳號是否已被其他人綁定
                if (validAccount.UserProfileId != null && validAccount.UserProfileId != profile.Id)
                {
                    return BadRequest(new { success = false, message = "綁定失敗：此 ERP 帳號已被其他 LINE 用戶綁定。" });
                }

                // 5. 執行綁定 (更新 UserProfileId)
                validAccount.UserProfileId = profile.Id;
                _dbContext.UserCustomers.Update(validAccount);
                await _dbContext.SaveChangesAsync();

                return Ok(new { success = true, message = "ERP 帳號驗證並綁定成功" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error binding ERP");
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
    }

    public class ErpBindingRequest
    {
        public string UserId { get; set; } = string.Empty; // Line UserId
        public string ErpCode { get; set; } = string.Empty; // Level 1 ERP Code
        public string Uid { get; set; } = string.Empty; // L2 account
        public string Upwd { get; set; } = string.Empty; // L2 password
    }
}
