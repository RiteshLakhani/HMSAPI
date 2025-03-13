using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace HospitalAPI.Model
{
    public class AppointmentsModel
    {
        public int AppointmentID { get; set; }
        public int PatientID { get; set; }
        public string PatientName { get; set; }
        public int DoctorID { get; set; }
        public string DoctorName { get; set; } 
        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
        public string TokenNumber { get; set; }
        public string Problem { get; set; }
        public string Status { get; set; }
        public bool IsConfirmed { get; set; }
    }

}
