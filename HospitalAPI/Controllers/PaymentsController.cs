using HospitalAPI.Data;
using HospitalAPI.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly PaymentsRepository _paymentsRepository;

        public PaymentsController(PaymentsRepository paymentsRepository)
        {
            _paymentsRepository = paymentsRepository;
        }

        #region GetAllPayments
        [HttpGet]
        public async Task<IActionResult> GetAllPayments()
        {
            var payments = await _paymentsRepository.SelectAllAsync();
            return Ok(payments);
        }
        #endregion

        #region GetPaymentByPK
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentById(int id)
        {
            var payment = await _paymentsRepository.SelectByPKAsync(id);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }
        #endregion

        #region Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            var isDeleted = await _paymentsRepository.DeleteAsync(id);
            if (!isDeleted)
            {
                return NotFound();
            }
            return NoContent();
        }
        #endregion

        #region Insert
        [HttpPost]
        public async Task<IActionResult> InsertPayment([FromBody] PaymentsModel payment)
        {
            if (payment == null)
                return BadRequest("Payment data is required.");

            bool isInserted = await _paymentsRepository.InsertAsync(payment);

            if (isInserted)
                return Ok(new { Message = "Payment inserted successfully!" });

            return StatusCode(500, "An error occurred while inserting the payment.");
        }
        #endregion

        #region Update
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePayment(int id, [FromBody] PaymentsModel payment)
        {
            if (payment == null || id != payment.PaymentID)
                return BadRequest();

            var isUpdated = await _paymentsRepository.UpdateAsync(payment);
            if (!isUpdated)
                return NotFound();

            return NoContent();
        }
        #endregion

        #region Dropdown Doctor
        [HttpGet("doctors")]
        public async Task<IActionResult> GetDoctors()
        {
            var doctors = await _paymentsRepository.GetDoctorAsync();
            if (!doctors.Any())
            {
                return NotFound("No Doctors Found");
            }
            return Ok(doctors);
        }
        #endregion

        #region Dropdown Patients
        [HttpGet("patients")]
        public async Task<IActionResult> GetPatients()
        {
            var patients = await _paymentsRepository.GetPatientAsync();
            if (!patients.Any())
            {
                return NotFound("No Patients Found");
            }
            return Ok(patients);
        }
        #endregion

    }
}
