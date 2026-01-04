using Microsoft.AspNetCore.Mvc;
using backend.Services;
using backend.Models.T357;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IT357Service _t357Service;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IT357Service t357Service, ILogger<ProductController> logger)
        {
            _t357Service = t357Service;
            _logger = logger;
        }

        /// <summary>
        /// 查詢產品資料 (Product_Query)
        /// </summary>
        /// <param name="userId">Line 使用者唯一識別碼 (UserId)</param>
        /// <param name="prodId">產品編號 (支援 Like 模糊查詢，留空則全查)</param>
        /// <returns></returns>
        [HttpGet("query")]
        public async Task<IActionResult> QueryProducts([FromQuery] string userId, [FromQuery] string? prodId = null)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new { message = "必須提供 userId" });
            }

            try
            {
                var response = await _t357Service.QueryProductsForUserAsync(userId, prodId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error querying products from T357");
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
