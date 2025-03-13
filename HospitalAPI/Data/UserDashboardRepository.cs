using HospitalAPI.Model;
using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace HospitalAPI.Data
{
    public class UserDashboardRepository
    {
        private readonly string _connectionString;

        public UserDashboardRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionString");
        }

        #region Get Patient Dashboard
        public async Task<PatientDashboardModel> GetPatientDashboard(int patientId)
        {
            var dashboard = new PatientDashboardModel
            {
                Appointments = new List<AppointmentModel>(),
                Payments = new List<PatientPaymentModel>(),
                Rooms = new List<RoomModel>()
            };

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("sp_GetPatientDashboard", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PatientID", patientId);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        // Patient Basic Details
                        if (reader.HasRows && await reader.ReadAsync())
                        {
                            dashboard.PatientID = Convert.ToInt32(reader["PatientID"]);
                            dashboard.PatientName = reader["PatientName"].ToString();
                            dashboard.Email = reader["Email"].ToString();
                            dashboard.Phone = reader["Phone"].ToString();
                        }

                        // Move to next result set for Appointments
                        if (await reader.NextResultAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                dashboard.Appointments.Add(new AppointmentModel
                                {
                                    AppointmentID = Convert.ToInt32(reader["AppointmentID"]),
                                    AppointmentDate = Convert.ToDateTime(reader["AppointmentDate"]),
                                    AppointmentTime = TimeSpan.Parse(reader["AppointmentTime"].ToString()),
                                    Problem = reader["Problem"].ToString(),
                                    Status = reader["Status"].ToString()
                                });
                            }
                        }

                        // Move to next result set for Payments
                        if (await reader.NextResultAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                dashboard.Payments.Add(new PatientPaymentModel
                                {
                                    PaymentID = Convert.ToInt32(reader["PaymentID"]),
                                    ServiceName = reader["ServiceName"].ToString(),
                                    CostOfTreatment = Convert.ToDecimal(reader["CostOfTreatment"]),
                                    PaymentStatus = reader["PaymentStatus"].ToString()
                                });
                            }
                        }

                        // Move to next result set for Room details
                        if (await reader.NextResultAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                dashboard.Rooms.Add(new RoomModel
                                {
                                    RoomID = Convert.ToInt32(reader["RoomID"]),
                                    RoomNumber = Convert.ToInt32(reader["RoomNumber"]),
                                    RoomType = reader["RoomType"].ToString(),
                                    AllotmentDate = Convert.ToDateTime(reader["AllotmentDate"]),
                                    DischargeDate = reader["DischargeDate"] != DBNull.Value
                                        ? Convert.ToDateTime(reader["DischargeDate"])
                                        : (DateTime?)null
                                });
                            }
                        }
                    }
                }
            }

            return dashboard;
        }
        #endregion
    }
}
