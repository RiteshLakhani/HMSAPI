using HospitalAPI.Data;
using HospitalAPI.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace HospitalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminDashboardController : ControllerBase
    {
        private readonly AdminDashboardRepository _repository;

        public AdminDashboardController(AdminDashboardRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("dashboardcounts")]
        public async Task<IActionResult> GetDashboardCounts()
        {
            try
            {
                var counts = await _repository.GetDashboardCounts();
                if (counts == null)
                    return NotFound(new { message = "Dashboard counts not available." });

                return Ok(counts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", error = ex.Message });
            }
        }

        [HttpGet("recententries")]
        public async Task<IActionResult> GetRecentEntries()
        {
            try
            {
                var recentEntries = await _repository.GetRecentEntries();
                if (recentEntries == null)
                    return NotFound(new { message = "No recent entries found." });

                return Ok(recentEntries);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", error = ex.Message });
            }
        }

        [HttpGet("monthlystats")]
        public async Task<IActionResult> GetMonthlyStats()
        {
            try
            {
                var monthlyStats = await _repository.GetMonthlyStats();
                if (monthlyStats == null)
                    return NotFound(new { message = "Monthly statistics not available." });

                return Ok(monthlyStats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", error = ex.Message });
            }
        }

        [HttpPost("updatedashboard")]
        public async Task<IActionResult> UpdateDashboard()
        {
            try
            {
                bool isUpdated = await _repository.UpdateDashboardData();
                if (isUpdated)
                    return Ok(new { message = "Dashboard data updated successfully!" });

                return BadRequest(new { message = "No data found to update." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the dashboard.", error = ex.Message });
            }
        }

    }
}
