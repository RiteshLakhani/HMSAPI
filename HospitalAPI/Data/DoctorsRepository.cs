using HospitalAPI.Model;
using System.Data;
using Microsoft.Extensions.Configuration;
using SqlClient = Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalAPI.Data
{
    public class DoctorsRepository
    {
        private readonly string _connectionString;

        public DoctorsRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionString");
        }

        #region GetAllDoctors
        public IEnumerable<DoctorsModel> SelectAll()
        {
            var doctors = new List<DoctorsModel>();
            using (SqlClient.SqlConnection conn = new SqlClient.SqlConnection(_connectionString))
            {
                SqlClient.SqlCommand cmd = new SqlClient.SqlCommand("PR_Doctor_SelectAll", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                conn.Open();
                SqlClient.SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    doctors.Add(new DoctorsModel
                    {
                        DoctorID = Convert.ToInt32(reader["DoctorID"]),
                        DoctorName = reader["DoctorName"].ToString(),
                        Gender = reader["Gender"].ToString(),
                        DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
                        Age = Convert.ToInt32(reader["Age"]),
                        Specialization = reader["Specialization"].ToString(),
                        Experience = Convert.ToInt32(reader["Experience"]),
                        DoctorDetail = reader["DoctorDetail"].ToString(),
                        Address = reader["Address"].ToString(),
                        ContactNumber = reader["ContactNumber"].ToString(),
                        Email = reader["Email"].ToString(),
                        IsConfirmed = Convert.ToBoolean(reader["IsConfirmed"]),
                        ImagePath = reader["ImagePath"].ToString()
                    });
                }
            }
            return doctors;
        }
        #endregion

        #region SelectByPK
        public DoctorsModel SelectByPK(int doctorID)
        {
            DoctorsModel doctor = null;

            using (SqlClient.SqlConnection conn = new SqlClient.SqlConnection(_connectionString))
            {
                SqlClient.SqlCommand cmd = new SqlClient.SqlCommand("PR_Doctor_SelectByPK", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@DoctorID", doctorID);
                conn.Open();
                SqlClient.SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    //var fileData = reader["file"] as byte[];

                    doctor = new DoctorsModel
                    {
                        DoctorID = Convert.ToInt32(reader["DoctorID"]),
                        DoctorName = reader["DoctorName"].ToString(),
                        Gender = reader["Gender"].ToString(),
                        DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
                        Age = Convert.ToInt32(reader["Age"]),
                        Specialization = reader["Specialization"].ToString(),
                        Experience = Convert.ToInt32(reader["Experience"]),
                        DoctorDetail = reader["DoctorDetail"].ToString(),
                        Address = reader["Address"].ToString(),
                        ContactNumber = reader["ContactNumber"].ToString(),
                        Email = reader["Email"].ToString(),
                        IsConfirmed = Convert.ToBoolean(reader["IsConfirmed"]),
                        ImagePath = reader["ImagePath"] == DBNull.Value ? null : reader["ImagePath"].ToString()
                    };

                }
            }
            return doctor;
        }
        #endregion

        #region Doctor Delete
        public bool Delete(int doctorID)
        {
            using (SqlClient.SqlConnection conn = new SqlClient.SqlConnection(_connectionString))
            {
                SqlClient.SqlCommand cmd = new SqlClient.SqlCommand("PR_Doctor_Delete", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@DoctorID", doctorID);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
        #endregion

        #region Doctor Insert
        public bool Insert(DoctorsModel doctor)
        {
            using (SqlClient.SqlConnection conn = new SqlClient.SqlConnection(_connectionString))
            {
                SqlClient.SqlCommand cmd = new SqlClient.SqlCommand("PR_Doctor_Insert", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@DoctorName", doctor.DoctorName);
                cmd.Parameters.AddWithValue("@Gender", doctor.Gender);
                cmd.Parameters.AddWithValue("@DateOfBirth", doctor.DateOfBirth);
                cmd.Parameters.AddWithValue("@Age", doctor.Age);
                cmd.Parameters.AddWithValue("@Specialization", doctor.Specialization);
                cmd.Parameters.AddWithValue("@Experience", doctor.Experience);
                cmd.Parameters.AddWithValue("@DoctorDetail", doctor.DoctorDetail);
                cmd.Parameters.AddWithValue("@Address", doctor.Address);
                cmd.Parameters.AddWithValue("@ContactNumber", doctor.ContactNumber);
                cmd.Parameters.AddWithValue("@Email", doctor.Email);
                cmd.Parameters.AddWithValue("@IsConfirmed", doctor.IsConfirmed);

                // Ensure ImagePath is not null, otherwise pass DBNull.Value
                cmd.Parameters.AddWithValue("@ImagePath", string.IsNullOrEmpty(doctor.ImagePath) ? (object)DBNull.Value : doctor.ImagePath);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }
        #endregion

        #region Doctor Update
        public bool Update(DoctorsModel doctor)
        {
            using (SqlClient.SqlConnection conn = new SqlClient.SqlConnection(_connectionString))
            {
                using (SqlClient.SqlCommand cmd = new SqlClient.SqlCommand("PR_Doctor_Update", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@DoctorID", doctor.DoctorID);
                    cmd.Parameters.AddWithValue("@DoctorName", doctor.DoctorName);
                    cmd.Parameters.AddWithValue("@Gender", doctor.Gender);
                    cmd.Parameters.AddWithValue("@DateOfBirth", doctor.DateOfBirth);
                    cmd.Parameters.AddWithValue("@Age", doctor.Age);
                    cmd.Parameters.AddWithValue("@Specialization", doctor.Specialization);
                    cmd.Parameters.AddWithValue("@Experience", doctor.Experience);
                    cmd.Parameters.AddWithValue("@DoctorDetail", doctor.DoctorDetail);
                    cmd.Parameters.AddWithValue("@Address", doctor.Address);
                    cmd.Parameters.AddWithValue("@ContactNumber", doctor.ContactNumber);
                    cmd.Parameters.AddWithValue("@Email", doctor.Email);
                    cmd.Parameters.AddWithValue("@IsConfirmed", doctor.IsConfirmed);
                    cmd.Parameters.AddWithValue("@ImagePath", string.IsNullOrEmpty(doctor.ImagePath) ? (object)DBNull.Value : doctor.ImagePath);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0; // Return true if update was successful
                }
            }
        }
        #endregion

        #region DoctorDropdown
        public IEnumerable<DoctorsDropDown> GetDoctor()
        {
            var doctors = new List<DoctorsDropDown>();
            using (SqlClient.SqlConnection conn = new SqlClient.SqlConnection(_connectionString))
            {
                SqlClient.SqlCommand cmd = new SqlClient.SqlCommand("PR_Doctor_Dropdown", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                conn.Open();
                SqlClient.SqlDataReader reader = cmd.ExecuteReader();

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
            using (SqlClient.SqlConnection conn = new SqlClient.SqlConnection(_connectionString))
            {
                SqlClient.SqlCommand cmd = new SqlClient.SqlCommand("PR_Patient_Dropdown", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                conn.Open();
                SqlClient.SqlDataReader reader = cmd.ExecuteReader();

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