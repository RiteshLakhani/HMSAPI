using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace HospitalAPI.Model
{
    public class RoomsModel
    {
        public int RoomID { get; set; }
        public int RoomNumber { get; set; }
        public string RoomType { get; set; }
        public int? DoctorID { get; set; }  // Foreign Key from Doctor Table
        public string? DoctorName { get; set; } // To Store Name
        public int? PatientID { get; set; } // Foreign Key from Patient Table
        public string? PatientName { get; set; } // To Store Name
        public DateTime AllotmentDate { get; set; }
        public DateTime? DischargeDate { get; set; }
        public bool IsConfirmed { get; set; }
    }
}

