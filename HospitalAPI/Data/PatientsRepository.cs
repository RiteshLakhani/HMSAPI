using HospitalAPI.Model;
using System.Data;
using Microsoft.Extensions.Configuration;
using SqlClient = Microsoft.Data.SqlClient;

namespace HospitalAPI.Data
{
    public class PatientsRepository
    {
        private readonly string _connectionString;

        public PatientsRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionString");
        }

        #region GetAllPatients
        public IEnumerable<PatientsModel> SelectAll()
        {
            var patients = new List<PatientsModel>();
            using (var conn = new SqlClient.SqlConnection(_connectionString))
            {
                var cmd = new SqlClient.SqlCommand("PR_Patient_SelectAll", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                conn.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    patients.Add(new PatientsModel
                    {
                        PatientID = Convert.ToInt32(reader["PatientID"]),
                        PatientName = reader["PatientName"].ToString(),
                        DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
                        Age = Convert.ToInt32(reader["Age"]),
                        Phone = reader["Phone"].ToString(),
                        Email = reader["Email"].ToString(),
                        Gender = reader["Gender"].ToString(),
                        Address = reader["Address"].ToString(),
                        IsConfirmed = Convert.ToBoolean(reader["IsConfirmed"])
                    });
                }
            }
            return patients;
        }
        #endregion

        #region SelectByPK
        public PatientsModel SelectByPK(int patientID)
        {
            PatientsModel patient = null;

            using (var conn = new SqlClient.SqlConnection(_connectionString))
            {
                var cmd = new SqlClient.SqlCommand("PR_Patient_SelectByPK", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@PatientID", patientID);
                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    patient = new PatientsModel
                    {
                        PatientID = Convert.ToInt32(reader["PatientID"]),
                        PatientName = reader["PatientName"].ToString(),
                        DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
                        Age = Convert.ToInt32(reader["Age"]),
                        Phone = reader["Phone"].ToString(),
                        Email = reader["Email"].ToString(),
                        Gender = reader["Gender"].ToString(),
                        Address = reader["Address"].ToString(),
                        IsConfirmed = Convert.ToBoolean(reader["IsConfirmed"])
                    };
                }
            }
            return patient;
        }

        #endregion

        #region Patient Delete
        public bool Delete(int patientID)
        {
            using (var conn = new SqlClient.SqlConnection(_connectionString))
            {
                var cmd = new SqlClient.SqlCommand("PR_Patient_Delete", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@PatientID", patientID);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
        #endregion

        #region Patient Insert
        public bool Insert(PatientsModel patient)
        {
            using (var conn = new SqlClient.SqlConnection(_connectionString))
            {
                var cmd = new SqlClient.SqlCommand("PR_Patient_Insert", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@PatientName", patient.PatientName);
                cmd.Parameters.AddWithValue("@DateOfBirth", patient.DateOfBirth);
                cmd.Parameters.AddWithValue("@Age", patient.Age);
                cmd.Parameters.AddWithValue("@Phone", patient.Phone);
                cmd.Parameters.AddWithValue("@Email", patient.Email);
                cmd.Parameters.AddWithValue("@Gender", patient.Gender);
                cmd.Parameters.AddWithValue("@Address", patient.Address);
                cmd.Parameters.AddWithValue("@IsConfirmed", patient.IsConfirmed);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }
        #endregion

        #region Patient Update
        public bool Update(PatientsModel patient)
        {
            using (var conn = new SqlClient.SqlConnection(_connectionString))
            {
                var cmd = new SqlClient.SqlCommand("PR_Patient_Update", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@PatientID", patient.PatientID);
                cmd.Parameters.AddWithValue("@PatientName", patient.PatientName);
                cmd.Parameters.AddWithValue("@DateOfBirth", patient.DateOfBirth);
                cmd.Parameters.AddWithValue("@Age", patient.Age);
                cmd.Parameters.AddWithValue("@Phone", patient.Phone);
                cmd.Parameters.AddWithValue("@Email", patient.Email);
                cmd.Parameters.AddWithValue("@Gender", patient.Gender);
                cmd.Parameters.AddWithValue("@Address", patient.Address);
                cmd.Parameters.AddWithValue("@IsConfirmed", patient.IsConfirmed);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
        #endregion

        #region PatientDropdown
        public IEnumerable<PatientsDropDown> GetPatient()
        {
            var patients = new List<PatientsDropDown>();
            using (var conn = new SqlClient.SqlConnection(_connectionString))
            {
                var cmd = new SqlClient.SqlCommand("PR_Patient_Dropdown", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                conn.Open();
                var reader = cmd.ExecuteReader();

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
