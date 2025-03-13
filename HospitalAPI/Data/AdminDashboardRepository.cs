using HospitalAPI.Model;
using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace HospitalAPI.Data
{
    public class AdminDashboardRepository
    {
        private readonly string _connectionString;

        public AdminDashboardRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionString");
        }

        #region Get Dashboard Counts
        public async Task<AdminDashboardModel> GetDashboardCounts()
        {
            var dashboard = new AdminDashboardModel();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("sp_GetDashboardCounts", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.HasRows && await reader.ReadAsync())
                        {
                            dashboard.TotalPatients = reader["TotalPatients"] != DBNull.Value ? Convert.ToInt32(reader["TotalPatients"]) : 0;
                            dashboard.TotalDoctors = reader["TotalDoctors"] != DBNull.Value ? Convert.ToInt32(reader["TotalDoctors"]) : 0;
                            dashboard.TotalPayments = reader["TotalPayments"] != DBNull.Value ? Convert.ToInt32(reader["TotalPayments"]) : 0;
                            dashboard.TotalRooms = reader["TotalRooms"] != DBNull.Value ? Convert.ToInt32(reader["TotalRooms"]) : 0;
                            dashboard.TotalAppointments = reader["TotalAppointments"] != DBNull.Value ? Convert.ToInt32(reader["TotalAppointments"]) : 0;

                        }
                    }
                }
            }

            return dashboard;
        }
        #endregion

        #region Get Recent Entries
        public async Task<RecentEntriesModel> GetRecentEntries()
        {
            var recentEntries = new RecentEntriesModel();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("sp_GetRecentEntries", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        // Recent Patients
                        if (reader.HasRows)
                        {
                            while (await reader.ReadAsync())
                            {
                                recentEntries.RecentPatients.Add(new PatientModel
                                {
                                    PatientName = reader["PatientName"].ToString(),
                                    Age = reader["Age"] != DBNull.Value ? Convert.ToInt32(reader["Age"]) : 0,
                                    Phone = reader["Phone"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    Gender = reader["Gender"].ToString()
                                });
                            }
                        }

                        // Recent Doctors
                        if (await reader.NextResultAsync() && reader.HasRows)
                        {
                            while (await reader.ReadAsync())
                            {
                                recentEntries.RecentDoctors.Add(new DoctorModel
                                {
                                    DoctorName = reader["DoctorName"].ToString(),
                                    Specialization = reader["Specialization"].ToString(),
                                    Experience = reader["Experience"] != DBNull.Value ? Convert.ToInt32(reader["Experience"]) : 0,
                                    ContactNumber = reader["ContactNumber"].ToString(),
                                    Email = reader["Email"].ToString()
                                });
                            }
                        }

                        // Recent Payments
                        if (await reader.NextResultAsync() && reader.HasRows)
                        {
                            while (await reader.ReadAsync())
                            {
                                recentEntries.RecentPayments.Add(new PaymentModel
                                {
                                    PatientName = reader["PatientName"].ToString(),
                                    DoctorName = reader["DoctorName"].ToString(),
                                    ServiceName = reader["ServiceName"].ToString(),
                                    Amount = reader["Amount"] != DBNull.Value ? Convert.ToDecimal(reader["Amount"]) : 0m,
                                    PaymentStatus = reader["PaymentStatus"].ToString(),
                                    PaymentDate = reader["PaymentDate"] != DBNull.Value ? Convert.ToDateTime(reader["PaymentDate"]) : DateTime.MinValue
                                });
                            }
                        }
                    }
                }
            }

            return recentEntries;
        }
        #endregion

        #region Get Monthly Stats
        public async Task<MonthlyStatsModel> GetMonthlyStats()
        {
            var monthlyStats = new MonthlyStatsModel();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("sp_GetMonthlyStats", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        // Monthly Admissions
                        if (reader.HasRows)
                        {
                            while (await reader.ReadAsync())
                            {
                                monthlyStats.Admissions.Add(new MonthlyAdmissions
                                {
                                    Month = reader["Month"].ToString(), // 'yyyy-MM' format
                                    TotalAdmissions = reader["TotalAdmissions"] != DBNull.Value ? Convert.ToInt32(reader["TotalAdmissions"]) : 0
                                });
                            }
                        }

                        // Move to the next result set for payments
                        if (await reader.NextResultAsync() && reader.HasRows)
                        {
                            while (await reader.ReadAsync())
                            {
                                monthlyStats.Payments.Add(new MonthlyPayments
                                {
                                    Month = reader["Month"].ToString(), // 'yyyy-MM' format
                                    TotalRevenue = reader["TotalRevenue"] != DBNull.Value ? Convert.ToDecimal(reader["TotalRevenue"]) : 0m
                                });
                            }
                        }
                    }
                }
            }

            return monthlyStats;
        }

        #endregion

        #region Update Dashboard Data
        public async Task<bool> UpdateDashboardData()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("sp_UpdateDashboardData", conn)) // Stored Procedure
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    return rowsAffected > 0; // Returns true if the update was successful
                }
            }
        }
        #endregion

    }
}
