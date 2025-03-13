using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace HospitalAPI.Model
{
    public class PatientsModel
    {
        public int PatientID { get; set; }
        public string PatientName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public bool IsConfirmed { get; set; }
    }

    public class PatientsDropDown
    {
        public int PatientID { get; set; }
        public string PatientName { get; set; }
    }
}
