using HospitalAPI.Data;
using HospitalAPI.Model;
using Microsoft.AspNetCore.Mvc;

namespace HospitalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly PatientsRepository _patientsRepository;

        public PatientsController(PatientsRepository patientsRepository)
        {
            _patientsRepository = patientsRepository;
        }

        #region GETALL
        [HttpGet]
        public IActionResult GetAllPatients()
        {
            var patients = _patientsRepository.SelectAll();
            return Ok(patients);
        }
        #endregion

        #region GetPatientByPK
        [HttpGet("{id}")]
        public IActionResult GetPatientById(int id)
        {
            var patient = _patientsRepository.SelectByPK(id);
            if (patient == null)
            {
                return NotFound();
            }
            return Ok(patient);
        }
        #endregion

        #region Delete
        [HttpDelete("{id}")]
        public IActionResult DeletePatient(int id)
        {
            var isDeleted = _patientsRepository.Delete(id);
            if (!isDeleted)
            {
                return NotFound();
            }
            return NoContent();
        }
        #endregion

        #region Insert
        [HttpPost]
        public IActionResult InsertPatient([FromBody] PatientsModel patient)
        {
            if (patient == null)
                return BadRequest();

            bool isInserted = _patientsRepository.Insert(patient);

            if (isInserted)
                return Ok(new { Message = "Patient inserted successfully!" });

            return StatusCode(500, "An error occurred while inserting the patient.");
        }
        #endregion

        #region Update
        [HttpPut("{id}")]
        public IActionResult UpdatePatient(int id, [FromBody] PatientsModel patient)
        {
            if (patient == null || id != patient.PatientID)
                return BadRequest();

            var isUpdated = _patientsRepository.Update(patient);
            if (!isUpdated)
                return NotFound();

            return NoContent();
        }
        #endregion

        #region Dropdown
        [HttpGet("patients")]
        public IActionResult GetPatients()
        {
            var patients = _patientsRepository.GetPatient();
            if(!patients.Any())
            {
                return NotFound("No Patients Found");
            }
            return Ok(patients);
        }
        #endregion
    }
}