using HospitalAPI.Model;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;

namespace HospitalAPI.Data
{
    public class AppointmentsRepository
    {
        private readonly string _connectionString;

        public AppointmentsRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionString");
        }

        #region GetAllAppointments
        public IEnumerable<AppointmentsModel> SelectAll(
            string? patientName = null,
            string? doctorName = null,
            DateTime? appointmentDate = null,
            string? tokenNumber = null,
            string? status = null)
        {
            var appointments = new List<AppointmentsModel>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_Appointment_SelectAll", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Add parameters for filtering
                cmd.Parameters.AddWithValue("@PatientName", (object?)patientName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@DoctorName", (object?)doctorName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@AppointmentDate", (object?)appointmentDate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@TokenNumber", (object?)tokenNumber ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Status", (object?)status ?? DBNull.Value);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    appointments.Add(new AppointmentsModel
                    {
                        AppointmentID = Convert.ToInt32(reader["AppointmentID"]),
                        PatientID = Convert.ToInt32(reader["PatientID"]),
                        PatientName = reader["PatientName"].ToString(),
                        DoctorID = Convert.ToInt32(reader["DoctorID"]),
                        DoctorName = reader["DoctorName"].ToString(),
                        AppointmentDate = Convert.ToDateTime(reader["AppointmentDate"]),
                        AppointmentTime = TimeSpan.Parse(reader["AppointmentTime"].ToString()),
                        TokenNumber = reader["TokenNumber"].ToString(),
                        Problem = reader["Problem"].ToString(),
                        Status = reader["Status"].ToString(),
                        IsConfirmed = Convert.ToBoolean(reader["IsConfirmed"])
                    });
                }
            }

            return appointments;
        }
        #endregion

        #region SelectByPK
        public AppointmentsModel SelectByPK(int appointmentID)
        {
            AppointmentsModel appointment = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_Appointment_SelectByPK", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@AppointmentID", appointmentID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    appointment = new AppointmentsModel
                    {
                        AppointmentID = Convert.ToInt32(reader["AppointmentID"]),
                        PatientID = Convert.ToInt32(reader["PatientID"]),
                        DoctorID = Convert.ToInt32(reader["DoctorID"]),
                        AppointmentDate = Convert.ToDateTime(reader["AppointmentDate"]),
                        AppointmentTime = TimeSpan.Parse(reader["AppointmentTime"].ToString()),
                        TokenNumber = reader["TokenNumber"].ToString(),
                        Problem = reader["Problem"].ToString(),
                        Status = reader["Status"].ToString(),
                        IsConfirmed = Convert.ToBoolean(reader["IsConfirmed"])
                    };
                }
            }
            return appointment;
        }
        #endregion

        #region Delete
        public bool Delete(int appointmentID)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_Appointment_Delete", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@AppointmentID", appointmentID);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
        #endregion

        #region Insert
        public bool Insert(AppointmentsModel appointment)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("PR_Appointment_Insert", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    // Pass the parameters needed for insertion, excluding TokenNumber
                    cmd.Parameters.AddWithValue("@PatientID", appointment.PatientID);
                    cmd.Parameters.AddWithValue("@DoctorID", appointment.DoctorID);
                    cmd.Parameters.AddWithValue("@AppointmentDate", appointment.AppointmentDate);
                    cmd.Parameters.AddWithValue("@AppointmentTime", appointment.AppointmentTime);
                    cmd.Parameters.AddWithValue("@Problem", appointment.Problem);
                    cmd.Parameters.AddWithValue("@Status", appointment.Status);
                    cmd.Parameters.AddWithValue("@IsConfirmed", appointment.IsConfirmed);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                // Log exception (you could log to a file or database)
                Console.WriteLine(ex.Message);
                return false; // In case of an error, return false
            }
        }
        #endregion

        #region Update
        public bool Update(AppointmentsModel appointment)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_Appointment_Update", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@AppointmentID", appointment.AppointmentID);
                cmd.Parameters.AddWithValue("@PatientID", appointment.PatientID);
                cmd.Parameters.AddWithValue("@DoctorID", appointment.DoctorID);
                cmd.Parameters.AddWithValue("@AppointmentDate", appointment.AppointmentDate);
                cmd.Parameters.AddWithValue("@AppointmentTime", appointment.AppointmentTime);
                cmd.Parameters.AddWithValue("@Problem", appointment.Problem);
                cmd.Parameters.AddWithValue("@Status", appointment.Status);
                cmd.Parameters.AddWithValue("@IsConfirmed", appointment.IsConfirmed);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
        #endregion

        #region DoctorDropdown
        public IEnumerable<DoctorsDropDown> GetDoctor()
        {
            var doctors = new List<DoctorsDropDown>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_Doctor_Dropdown", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    doctors.Add(new DoctorsDropDown
                    {
                        DoctorID = Convert.ToInt32(reader["DoctorID"]),
                        DoctorName = reader["DoctorName"].ToString()
                    });
                }
            }
            return doctors;
        }
        #endregion

        #region PatientDropdown
        public IEnumerable<PatientsDropDown> GetPatient()
        {
            var patients = new List<PatientsDropDown>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_Patient_Dropdown", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    patients.Add(new PatientsDropDown
                    {
                        PatientID = Convert.ToInt32(reader["PatientID"]),
                        PatientName = reader["PatientName"].ToString()
                    });
                }
            }
            return patients;
        }
        #endregion
    }
}
