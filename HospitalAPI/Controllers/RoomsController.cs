using HospitalAPI.Data;
using HospitalAPI.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace HospitalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly RoomsRepository _roomsRepository;

        public RoomsController(RoomsRepository roomsRepository)
        {
            _roomsRepository = roomsRepository;
        }

        #region GetAllRooms
        [HttpGet]
        public IActionResult GetAllRooms(
            int? roomNumber = null,
            string? roomType = null,
            string? patientName = null,
            string? doctorName = null,
            DateTime? allotmentDate = null,
            DateTime? dischargeDate = null,
            bool? isConfirmed = null)
        {
            var rooms = _roomsRepository.SelectAll(roomNumber, roomType, patientName, doctorName, allotmentDate, dischargeDate, isConfirmed);
            return Ok(rooms);
        }
        #endregion

        #region GetRoomByPK
        [HttpGet("{id}")]
        public IActionResult GetRoomById(int id)
        {
            var room = _roomsRepository.SelectByPK(id);
            if (room == null)
            {
                return NotFound(new { Message = "Room not found." });
            }
            return Ok(room);
        }
        #endregion

        #region Delete
        [HttpDelete("{id}")]
        public IActionResult DeleteRoom(int id)
        {
            var isDeleted = _roomsRepository.Delete(id);
            if (!isDeleted)
            {
                return NotFound(new { Message = "Room not found." });
            }
            return Ok(new { Message = "Room deleted successfully!" });
        }
        #endregion

        #region Insert
        [HttpPost]
        public IActionResult InsertRoom([FromBody] RoomsModel room)
        {
            if (room == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            bool isInserted = _roomsRepository.Insert(room);

            if (isInserted)
                return Ok(new { Message = "Room inserted successfully!" });

            return StatusCode(500, "An error occurred while inserting the room.");
        }
        #endregion

        #region Update
        [HttpPut("{id}")]
        public IActionResult UpdateRoom(int id, [FromBody] RoomsModel room)
        {
            if (room == null || id != room.RoomID || !ModelState.IsValid)
                return BadRequest(new { Message = "Invalid room data." });

            var isUpdated = _roomsRepository.Update(room);
            if (!isUpdated)
                return NotFound(new { Message = "Room not found." });

            return Ok(new { Message = "Room updated successfully!" });
        }
        #endregion

        #region Dropdown Doctor
        [HttpGet("doctors")]
        public IActionResult GetDoctors()
        {
            var doctors = _roomsRepository.GetDoctor();
            if (!doctors.Any())
            {
                return NotFound(new { Message = "No Doctors Found." });
            }
            return Ok(doctors);
        }
        #endregion

        #region Dropdown Patients
        [HttpGet("patients")]
        public IActionResult GetPatients()
        {
            var patients = _roomsRepository.GetPatient();
            if (!patients.Any())
            {
                return NotFound(new { Message = "No Patients Found." });
            }
            return Ok(patients);
        }
        #endregion
    }
}


