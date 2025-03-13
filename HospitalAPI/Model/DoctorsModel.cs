using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalAPI.Model
{
    public class DoctorsModel
    {
        public int DoctorID { get; set; }
        public string DoctorName { get; set; } 
        public string Gender { get; set; } 
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
        public string Specialization { get; set; } 
        public int Experience { get; set; }
        public string DoctorDetail { get; set; }
        public string Address { get; set; } 
        public string ContactNumber { get; set; } 
        public string Email { get; set; } 
        public bool IsConfirmed { get; set; }
        public string? ImagePath { get; set; }
        public IFormFile? file { get; set; }
    }

    public class DoctorsDropDown
    {
        public int DoctorID { get; set; }
        public string DoctorName { get; set; }
    }
}
