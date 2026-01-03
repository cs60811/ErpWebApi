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
                    .FirstOrDefaultAsync(u => u.UserId == profile.UserId);

                if (existingProfile != null)
                {
                    // 更新現有使用者資料
                    existingProfile.DisplayName = profile.DisplayName;
                    existingProfile.PictureUrl = profile.PictureUrl;
                    existingProfile.StatusMessage = profile.StatusMessage;
                    existingProfile.UpdateTime = DateTime.UtcNow;

                    _dbContext.UserProfiles.Update(existingProfile);
                    _logger.LogInformation("Updating User: {DisplayName} ({UserId})", profile.DisplayName, profile.UserId);
                }
                else
                {
                    // 新增使用者
                    profile.CreateTime = DateTime.UtcNow;
                    profile.UpdateTime = DateTime.UtcNow;
                    
                    await _dbContext.UserProfiles.AddAsync(profile);
                    _logger.LogInformation("Recording new User: {DisplayName} ({UserId})", profile.DisplayName, profile.UserId);
                }

                await _dbContext.SaveChangesAsync();

                return Ok(new { 
                    success = true, 
                    message = $"Account {profile.DisplayName} recorded successfully",
                    data = new {
                        profile.UserId,
                        profile.DisplayName,
                        profile.PictureUrl,
                        profile.StatusMessage,
                        profile.ErpId,
                        profile.IsErpBound
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error recording user profile for {UserId}", profile.UserId);
                return StatusCode(500, new { success = false, message = "Failed to record account", error = ex.Message });
            }
        }
        [HttpPost("bind-erp")]
        public async Task<IActionResult> BindERP([FromBody] ErpBindingRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.UserId) || string.IsNullOrEmpty(request.ErpId))
            {
                return BadRequest("Invalid binding data");
            }

            try
            {
                var profile = await _dbContext.UserProfiles
                    .FirstOrDefaultAsync(u => u.UserId == request.UserId);

                if (profile == null)
                {
                    return NotFound(new { success = false, message = "User profile not found" });
                }

                // Simulate ERP verification (e.g., password check)
                _logger.LogInformation("Verifying ERP ID: {ErpId} for User: {UserId}", request.ErpId, request.UserId);

                // Update profile with ERP info
                profile.ErpId = request.ErpId;
                profile.IsErpBound = true;
                profile.UpdateTime = DateTime.UtcNow;

                _dbContext.UserProfiles.Update(profile);
                await _dbContext.SaveChangesAsync();

                return Ok(new { success = true, message = "ERP account bound successfully", erpId = request.ErpId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error binding ERP for {UserId}", request.UserId);
                return StatusCode(500, new { success = false, message = "Failed to bind ERP account", error = ex.Message });
            }
        }
    }

    public class ErpBindingRequest
    {
        public string UserId { get; set; } = string.Empty;
        public string ErpId { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
