using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using HospitalAPI.Model;

namespace HospitalAPI.Data
{
    public class RoomsRepository
    {
        private readonly string _connectionString;

        public RoomsRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionString");
        }

        #region SelectAll
        public List<RoomsModel> SelectAll(int? roomNumber = null, string? roomType = null, string? patientName = null,
                            string? doctorName = null, DateTime? allotmentDate = null, DateTime? dischargeDate = null,
                            bool? isConfirmed = null)
        {
            List<RoomsModel> rooms = new List<RoomsModel>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("PR_Room_SelectAll", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@RoomNumber", roomNumber.HasValue ? (object)roomNumber.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@RoomType", !string.IsNullOrEmpty(roomType) ? (object)roomType : DBNull.Value);
                    cmd.Parameters.AddWithValue("@PatientName", !string.IsNullOrEmpty(patientName) ? (object)patientName : DBNull.Value);
                    cmd.Parameters.AddWithValue("@DoctorName", !string.IsNullOrEmpty(doctorName) ? (object)doctorName : DBNull.Value);
                    cmd.Parameters.AddWithValue("@AllotmentDate", allotmentDate.HasValue ? (object)allotmentDate.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@DischargeDate", dischargeDate.HasValue ? (object)dischargeDate.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@IsConfirmed", isConfirmed.HasValue ? (object)isConfirmed.Value : DBNull.Value);

                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            rooms.Add(new RoomsModel
                            {
                                RoomID = Convert.ToInt32(reader["RoomID"]),
                                RoomNumber = Convert.ToInt32(reader["RoomNumber"]),
                                RoomType = reader["RoomType"] != DBNull.Value ? Convert.ToString(reader["RoomType"]) : string.Empty,
                                PatientID = reader["PatientID"] != DBNull.Value ? Convert.ToInt32(reader["PatientID"]) : (int?)null,
                                DoctorID = reader["DoctorID"] != DBNull.Value ? Convert.ToInt32(reader["DoctorID"]) : (int?)null,
                                PatientName = reader["PatientName"] != DBNull.Value ? Convert.ToString(reader["PatientName"]) : string.Empty,
                                AllotmentDate = Convert.ToDateTime(reader["AllotmentDate"]),
                                DischargeDate = reader["DischargeDate"] != DBNull.Value ? Convert.ToDateTime(reader["DischargeDate"]) : (DateTime?)null,
                                DoctorName = reader["DoctorName"] != DBNull.Value ? Convert.ToString(reader["DoctorName"]) : string.Empty,
                                IsConfirmed = reader["IsConfirmed"] != DBNull.Value && Convert.ToBoolean(reader["IsConfirmed"])
                            });
                        }
                    }
                }
            }
            return rooms;
        }
        #endregion

        #region SelectByPK
        public RoomsModel SelectByPK(int roomID)
        {
            RoomsModel room = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("PR_Room_SelectByPK", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RoomID", roomID);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        room = new RoomsModel
                        {
                            RoomID = Convert.ToInt32(reader["RoomID"]),
                            RoomNumber = Convert.ToInt32(reader["RoomNumber"]),
                            RoomType = reader["RoomType"].ToString(),
                            PatientID = reader["PatientID"] != DBNull.Value ? Convert.ToInt32(reader["PatientID"]) : 0,
                            DoctorID = reader["DoctorID"] != DBNull.Value ? Convert.ToInt32(reader["DoctorID"]) : 0,
                            AllotmentDate = Convert.ToDateTime(reader["AllotmentDate"]),
                            DischargeDate = reader["DischargeDate"] != DBNull.Value ? Convert.ToDateTime(reader["DischargeDate"]) : (DateTime?)null,
                            IsConfirmed = Convert.ToBoolean(reader["IsConfirmed"])
                        };
                    }
                }
            }
            return room;
        }
        #endregion

        #region Insert
        public bool Insert(RoomsModel room)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(@"INSERT INTO Room (RoomNumber, RoomType, PatientID, AllotmentDate, DischargeDate, DoctorID, IsConfirmed) 
                                                     VALUES (@RoomNumber, @RoomType, @PatientID, @AllotmentDate, @DischargeDate, @DoctorID, @IsConfirmed)", con))
                    {
                        cmd.Parameters.AddWithValue("@RoomNumber", room.RoomNumber);
                        cmd.Parameters.AddWithValue("@RoomType", room.RoomType);
                        cmd.Parameters.AddWithValue("@PatientID", (object?)room.PatientID ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@AllotmentDate", room.AllotmentDate);
                        cmd.Parameters.AddWithValue("@DischargeDate", (object?)room.DischargeDate ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@DoctorID", (object?)room.DoctorID ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@IsConfirmed", room.IsConfirmed);

                        con.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #region Update
        public bool Update(RoomsModel room)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(@"UPDATE Room SET RoomNumber=@RoomNumber, RoomType=@RoomType, 
                                                     PatientID=@PatientID, AllotmentDate=@AllotmentDate, 
                                                     DischargeDate=@DischargeDate, DoctorID=@DoctorID, IsConfirmed=@IsConfirmed 
                                                     WHERE RoomID=@RoomID", con))
                    {
                        cmd.Parameters.AddWithValue("@RoomID", room.RoomID);
                        cmd.Parameters.AddWithValue("@RoomNumber", room.RoomNumber);
                        cmd.Parameters.AddWithValue("@RoomType", room.RoomType);
                        cmd.Parameters.AddWithValue("@PatientID", (object?)room.PatientID ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@AllotmentDate", room.AllotmentDate);
                        cmd.Parameters.AddWithValue("@DischargeDate", (object?)room.DischargeDate ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@DoctorID", (object?)room.DoctorID ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@IsConfirmed", room.IsConfirmed);

                        con.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;  // ✅ Return `true` if updated
                    }
                }
            }
            catch (Exception)
            {
                return false;  // ✅ Return `false` on failure
            }
        }

        #endregion

        #region Delete
        public bool Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("DELETE FROM Room WHERE RoomID = @RoomID", con))
                {
                    cmd.Parameters.AddWithValue("@RoomID", id);
                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;  // Return true if a row was deleted
                }
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
