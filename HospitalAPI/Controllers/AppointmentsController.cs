using HospitalAPI.Data;
using HospitalAPI.Model;
using Microsoft.AspNetCore.Mvc;

namespace HospitalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly AppointmentsRepository _appointmentsRepository;

        public AppointmentsController(AppointmentsRepository appointmentsRepository)
        {
            _appointmentsRepository = appointmentsRepository;
        }

        #region GETALL
        [HttpGet]
        public IActionResult GetAllAppointments(
            string? patientName = null,
            string? doctorName = null,
            DateTime? appointmentDate = null,
            string? tokenNumber = null,
            string? status = null)
        {
            var appointments = _appointmentsRepository.SelectAll(patientName, doctorName, appointmentDate, tokenNumber, status);
            return Ok(appointments);
        }
        #endregion

        #region GetAppointmentByPK
        [HttpGet("{id}")]
        public IActionResult GetAppointmentById(int id)
        {
            var appointment = _appointmentsRepository.SelectByPK(id);
            if (appointment == null)
            {
                return NotFound();
            }
            return Ok(appointment);
        }
        #endregion

        #region Delete
        [HttpDelete("{id}")]
        public IActionResult DeleteAppointment(int id)
        {
            var isDeleted = _appointmentsRepository.Delete(id);
            if (!isDeleted)
            {
                return NotFound();
            }
            return NoContent();
        }
        #endregion

        #region Insert
        [HttpPost]
        public IActionResult InsertAppointment([FromBody] AppointmentsModel appointment)
        {
            if (appointment == null)
                return BadRequest();

            bool isInserted = _appointmentsRepository.Insert(appointment);

            if (isInserted)
                return Ok(new { Message = "Appointment inserted successfully!" });

            return StatusCode(500, "An error occurred while inserting the appointment.");
        }
        #endregion

        #region Update
        [HttpPut("{id}")]
        public IActionResult UpdateAppointment(int id, [FromBody] AppointmentsModel appointment)
        {
            if (appointment == null || id != appointment.AppointmentID)
                return BadRequest();

            var isUpdated = _appointmentsRepository.Update(appointment);
            if (!isUpdated)
                return NotFound();

            return NoContent();
        }
        #endregion

        #region Dropdown Docotr
        [HttpGet("doctors")]
        public IActionResult GetDoctors()
        {
            var doctors = _appointmentsRepository.GetDoctor();
            if (!doctors.Any())
            {
                return NotFound("No Doctors Found");
            }
            return Ok(doctors);
        }
        #endregion

        #region Dropdown Patients
        [HttpGet("patients")]
        public IActionResult GetPatients()
        {
            var patients = _appointmentsRepository.GetPatient();
            if (!patients.Any())
            {
                return NotFound("No Patients Found");
            }
            return Ok(patients);
        }
        #endregion
    }
}
