namespace HospitalAPI.Model
{
    public class AdminDashboardModel
    {
        public int TotalPatients { get; set; }
        public int TotalDoctors { get; set; }
        public int TotalPayments { get; set; }
        public int TotalRooms { get; set; }
        public int TotalAppointments { get; set; }
    }

    public class RecentEntriesModel
    {
        public List<PatientModel> RecentPatients { get; set; } = new List<PatientModel>();
        public List<DoctorModel> RecentDoctors { get; set; } = new List<DoctorModel>();
        public List<PaymentModel> RecentPayments { get; set; } = new List<PaymentModel>();
    }

    public class MonthlyStatsModel
    {
        public List<MonthlyAdmissions> Admissions { get; set; } = new List<MonthlyAdmissions>();
        public List<MonthlyPayments> Payments { get; set; } = new List<MonthlyPayments>();
    }

    public class PatientModel
    {
        public string PatientName { get; set; }
        public int Age { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
    }

    public class DoctorModel
    {
        public string DoctorName { get; set; }
        public string Specialization { get; set; }
        public int Experience { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
    }

    public class PaymentModel
    {
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public string ServiceName { get; set; }
        public decimal Amount { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime PaymentDate { get; set; }
    }

    public class MonthlyAdmissions
    {
        public string Month { get; set; }
        public int TotalAdmissions { get; set; }
    }

    public class MonthlyPayments
    {
        public string Month { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
