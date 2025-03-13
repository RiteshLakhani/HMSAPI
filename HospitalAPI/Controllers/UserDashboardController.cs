using HospitalAPI.Data;
using HospitalAPI.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace HospitalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDashboardController : ControllerBase
    {
        private readonly UserDashboardRepository _userDashboardRepository;

        public UserDashboardController(UserDashboardRepository userDashboardRepository)
        {
            _userDashboardRepository = userDashboardRepository;
        }

        [HttpGet("GetPatientDashboard/{patientId}")]
        public async Task<IActionResult> GetPatientDashboard(int patientId)
        {
            try
            {
                var dashboard = await _userDashboardRepository.GetPatientDashboard(patientId);

                if (dashboard == null)
                {
                    return NotFound(new { message = "Patient dashboard not found." });
                }

                return Ok(dashboard);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching the patient dashboard.", error = ex.Message });
            }
        }
    }
}
