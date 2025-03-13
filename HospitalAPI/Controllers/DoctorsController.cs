using HospitalAPI.Data;
using HospitalAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace HospitalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly DoctorsRepository _doctorsRepository;

        public DoctorsController(DoctorsRepository doctorsRepository)
        {
            _doctorsRepository = doctorsRepository;
        }

        #region GETALL
        [HttpGet]
        public IActionResult GetAllDoctors()
        {
            var doctors = _doctorsRepository.SelectAll();
            return Ok(doctors);
        }
        #endregion

        #region GetDoctorByPK
        [HttpGet("{id}")]
        public IActionResult GetDoctorById(int id)
        {
            var doctor = _doctorsRepository.SelectByPK(id);
            if (doctor == null)
            {
                return NotFound();
            }

            Console.WriteLine($"Doctor Image Path: {doctor.ImagePath}"); // Debugging line
            return Ok(doctor);
        }

        #endregion

        #region Delete
        [HttpDelete("{id}")]
        public IActionResult DeleteDoctor(int id)
        {
            var isDeleted = _doctorsRepository.Delete(id);
            if (!isDeleted)
            {
                return NotFound();
            }
            return NoContent();
        }
        #endregion

        #region Insert
        [HttpPost("InsertDoctor")]
        public IActionResult InsertDoctor([FromForm] DoctorsModel doctor)
        {
            if (doctor.file != null && doctor.file.Length > 0)
            {
                var folderPath = Path.Combine("wwwroot", "uploads", "doctors");
                Directory.CreateDirectory(folderPath);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(doctor.file.FileName)}";
                var filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    doctor.file.CopyTo(stream);
                }

                doctor.ImagePath = $"/uploads/doctors/{fileName}"; // Assign the image path
            }
            else
            {
                doctor.ImagePath = null; // Or a default value
            }

            var result = _doctorsRepository.Insert(doctor);

            if (result)
                return Ok(new { Message = "Doctor inserted successfully", doctor });
            else
                return StatusCode(500, "Error inserting doctor.");
        }
        #endregion

        #region Update
        //[HttpPut("{id}")]
        //public IActionResult UpdateDoctor(int id, [FromForm] DoctorsModel doctor)
        //{
        //    if (doctor == null || id != doctor.DoctorID)
        //        return BadRequest("Invalid doctor details.");

        //    var existingDoctor = _doctorsRepository.SelectByPK(id);
        //    if (existingDoctor == null)
        //        return NotFound("Doctor not found.");

        //    // If a new file is uploaded, delete the old image and save the new one
        //    if (doctor.file != null && doctor.file.Length > 0)
        //    {
        //        var folderPath = Path.Combine("wwwroot", "uploads", "doctors");

        //        // Delete the old image if it exists
        //        if (!string.IsNullOrEmpty(existingDoctor.ImagePath))
        //        {
        //            var oldFilePath = Path.Combine("wwwroot", existingDoctor.ImagePath.TrimStart('/'));
        //            if (System.IO.File.Exists(oldFilePath))
        //            {
        //                System.IO.File.Delete(oldFilePath);
        //            }
        //        }

        //        // Save the new file
        //        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(doctor.file.FileName)}";
        //        var filePath = Path.Combine(folderPath, fileName);

        //        using (var stream = new FileStream(filePath, FileMode.Create))
        //        {
        //            doctor.file.CopyTo(stream);
        //        }

        //        // Update the image path
        //        doctor.ImagePath = $"/uploads/doctors/{fileName}";
        //    }
        //    else
        //    {
        //        // If no new file is uploaded, retain the existing image path
        //        doctor.ImagePath = existingDoctor.ImagePath;
        //    }

        //    var isUpdated = _doctorsRepository.Update(doctor);
        //    if (!isUpdated)
        //        return StatusCode(500, "Error updating doctor.");

        //    return Ok(new { Message = "Doctor updated successfully", doctor });
        //}

        [HttpPut("{id}")]
        public IActionResult UpdateDoctor(int id, [FromForm] DoctorsModel doctor)
        {
            if (doctor == null || id != doctor.DoctorID)
                return BadRequest("Invalid doctor details.");

            var existingDoctor = _doctorsRepository.SelectByPK(id);
            if (existingDoctor == null)
                return NotFound("Doctor not found.");

            // Preserve old image if no new file is uploaded
            if (doctor.file != null && doctor.file.Length > 0)
            {
                var folderPath = Path.Combine("wwwroot", "uploads", "doctors");

                // Delete old image
                if (!string.IsNullOrEmpty(existingDoctor.ImagePath))
                {
                    var oldFilePath = Path.Combine("wwwroot", existingDoctor.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                // Save the new file
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(doctor.file.FileName)}";
                var filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    doctor.file.CopyTo(stream);
                }

                doctor.ImagePath = $"/uploads/doctors/{fileName}"; // Assign new image path
            }
            else
            {
                doctor.ImagePath = existingDoctor.ImagePath; // Retain old image
            }

            var isUpdated = _doctorsRepository.Update(doctor);
            if (!isUpdated)
                return StatusCode(500, "Error updating doctor.");

            return Ok(new { Message = "Doctor updated successfully", doctor });
        }

        #endregion

        #region Upload Image
        [HttpPost("UploadImage")]
        public IActionResult UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            // Define the upload folder path
            var folderPath = Path.Combine("wwwroot", "uploads", "doctors");
            Directory.CreateDirectory(folderPath); // Ensure the directory exists

            // Generate a unique filename
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(folderPath, fileName);

            // Save the file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            // Generate the relative path for accessing the image
            var imagePath = $"/uploads/doctors/{fileName}";

            return Ok(new { Message = "Image uploaded successfully", ImagePath = imagePath });
        }
        #endregion

        #region Get Doctor Image
        [HttpGet("doctor/image/{id}")]
        public IActionResult GetDoctorImage(int id)
        {
            var doctor = _doctorsRepository.SelectByPK(id);
            if (doctor == null || string.IsNullOrEmpty(doctor.ImagePath))
            {
                return NotFound("Image not found.");
            }

            var filePath = Path.Combine("wwwroot", doctor.ImagePath.TrimStart('/'));

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("Image file not found.");
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "image/jpeg"); // or image/png
        }
        #endregion

        #region Dropdown Doctor
        [HttpGet("doctors")]
        public IActionResult GetDoctors()
        {
            var doctors = _doctorsRepository.GetDoctor();
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
            var patients = _doctorsRepository.GetPatient();
            if (!patients.Any())
            {
                return NotFound("No Patients Found");
            }
            return Ok(patients);
        }
        #endregion
    }
}
