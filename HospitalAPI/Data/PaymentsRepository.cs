using HospitalAPI.Model;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System;
using System.Threading.Tasks;

namespace HospitalAPI.Data
{
    public class PaymentsRepository
    {
        private readonly string _connectionString;

        public PaymentsRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionString");
        }

        #region GetAllPayments
        public async Task<IEnumerable<PaymentsModel>> SelectAllAsync()
        {
            var payments = new List<PaymentsModel>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("PR_Payment_SelectAll", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    await conn.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            payments.Add(new PaymentsModel
                            {
                                PaymentID = Convert.ToInt32(reader["PaymentID"]),
                                PatientID = Convert.ToInt32(reader["PatientID"]),
                                PatientName = reader["PatientName"] != DBNull.Value ? Convert.ToString(reader["PatientName"]) : string.Empty,
                                DoctorID = Convert.ToInt32(reader["DoctorID"]),
                                DoctorName = reader["DoctorName"] != DBNull.Value ? Convert.ToString(reader["DoctorName"]) : string.Empty,
                                Department = reader["Department"].ToString(),
                                ServiceName = reader["ServiceName"].ToString(),
                                CostOfTreatment = Convert.ToDecimal(reader["CostOfTreatment"]),
                                AdmissionDate = reader["AdmissionDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["AdmissionDate"]),
                                DischargeDate = reader["DischargeDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["DischargeDate"]),
                                AdvancedPaid = reader["AdvancedPaid"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["AdvancedPaid"]),
                                Discount = reader["Discount"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["Discount"]),
                                Amount = reader["Amount"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["Amount"]),
                                PaymentDate = Convert.ToDateTime(reader["PaymentDate"]),
                                PaymentMethod = reader["PaymentMethod"].ToString(),
                                PaymentType = reader["PaymentType"].ToString(),
                                CardOrCheckNo = reader["CardOrCheckNo"].ToString(),
                                PaymentStatus = reader["PaymentStatus"].ToString(),
                                IsConfirmed = Convert.ToBoolean(reader["IsConfirmed"])
                            });
                        }
                    }
                }
            }
            return payments;
        }
        #endregion

        #region SelectByPK
        public async Task<PaymentsModel> SelectByPKAsync(int paymentID)
        {
            PaymentsModel payment = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("PR_Payment_SelectByPK", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PaymentID", paymentID);

                    await conn.OpenAsync();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            payment = new PaymentsModel
                            {
                                PaymentID = Convert.ToInt32(reader["PaymentID"]),
                                PatientID = Convert.ToInt32(reader["PatientID"]),
                                DoctorID = Convert.ToInt32(reader["DoctorID"]),
                                Department = reader["Department"].ToString(),
                                ServiceName = reader["ServiceName"].ToString(),
                                CostOfTreatment = Convert.ToDecimal(reader["CostOfTreatment"]),
                                AdvancedPaid = reader["AdvancedPaid"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["AdvancedPaid"]),
                                Discount = reader["Discount"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["Discount"]),
                                Amount = reader["Amount"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["Amount"]),
                                PaymentDate = Convert.ToDateTime(reader["PaymentDate"]),
                                PaymentMethod = reader["PaymentMethod"].ToString(),
                                PaymentType = reader["PaymentType"].ToString(),
                                CardOrCheckNo = reader["CardOrCheckNo"].ToString(),
                                PaymentStatus = reader["PaymentStatus"].ToString(),
                                IsConfirmed = Convert.ToBoolean(reader["IsConfirmed"]),
                                AdmissionDate = reader["AdmissionDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["AdmissionDate"]),
                                DischargeDate = reader["DischargeDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["DischargeDate"])
                            };
                        }
                    }
                }
            }
            return payment;
        }
        #endregion

        #region Delete
        public async Task<bool> DeleteAsync(int paymentID)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("PR_Payment_Delete", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PaymentID", paymentID);

                    await conn.OpenAsync();
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }
        #endregion

        #region Insert
        public async Task<bool> InsertAsync(PaymentsModel payment)
        {
            try
            {
                decimal amount = payment.CostOfTreatment - (payment.AdvancedPaid ?? 0) - (payment.Discount ?? 0);
                payment.Amount = amount;

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("PR_Payment_Insert", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@PatientID", payment.PatientID);
                        cmd.Parameters.AddWithValue("@DoctorID", payment.DoctorID);
                        cmd.Parameters.AddWithValue("@Department", payment.Department);
                        cmd.Parameters.AddWithValue("@ServiceName", payment.ServiceName);
                        cmd.Parameters.AddWithValue("@CostOfTreatment", payment.CostOfTreatment);
                        cmd.Parameters.AddWithValue("@AdvancedPaid", payment.AdvancedPaid ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Discount", payment.Discount ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Amount", payment.Amount ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@PaymentDate", payment.PaymentDate);
                        cmd.Parameters.AddWithValue("@PaymentMethod", payment.PaymentMethod);
                        cmd.Parameters.AddWithValue("@PaymentType", payment.PaymentType);
                        cmd.Parameters.AddWithValue("@CardOrCheckNo", string.IsNullOrEmpty(payment.CardOrCheckNo) ? (object)DBNull.Value : payment.CardOrCheckNo);
                        cmd.Parameters.AddWithValue("@PaymentStatus", payment.PaymentStatus);
                        cmd.Parameters.AddWithValue("@IsConfirmed", payment.IsConfirmed);
                        cmd.Parameters.AddWithValue("@AdmissionDate", payment.AdmissionDate ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@DischargeDate", payment.DischargeDate ?? (object)DBNull.Value);

                        await conn.OpenAsync();
                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Update
        public async Task<bool> UpdateAsync(PaymentsModel payment)
        {
            try
            {
                decimal amount = payment.CostOfTreatment - (payment.AdvancedPaid ?? 0) - (payment.Discount ?? 0);
                payment.Amount = amount;

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("PR_Payment_Update", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@PaymentID", payment.PaymentID);
                        cmd.Parameters.AddWithValue("@PatientID", payment.PatientID);
                        cmd.Parameters.AddWithValue("@DoctorID", payment.DoctorID);
                        cmd.Parameters.AddWithValue("@Department", payment.Department);
                        cmd.Parameters.AddWithValue("@ServiceName", payment.ServiceName);
                        cmd.Parameters.AddWithValue("@CostOfTreatment", payment.CostOfTreatment);
                        cmd.Parameters.AddWithValue("@AdvancedPaid", payment.AdvancedPaid ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Discount", payment.Discount ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Amount", payment.Amount ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@PaymentDate", payment.PaymentDate);
                        cmd.Parameters.AddWithValue("@PaymentMethod", payment.PaymentMethod);
                        cmd.Parameters.AddWithValue("@PaymentType", payment.PaymentType);
                        cmd.Parameters.AddWithValue("@CardOrCheckNo", string.IsNullOrEmpty(payment.CardOrCheckNo) ? (object)DBNull.Value : payment.CardOrCheckNo);
                        cmd.Parameters.AddWithValue("@PaymentStatus", payment.PaymentStatus);
                        cmd.Parameters.AddWithValue("@IsConfirmed", payment.IsConfirmed);
                        cmd.Parameters.AddWithValue("@AdmissionDate", payment.AdmissionDate ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@DischargeDate", payment.DischargeDate ?? (object)DBNull.Value);

                        await conn.OpenAsync();
                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region DoctorDropdown
        public async Task<IEnumerable<DoctorsDropDown>> GetDoctorAsync()
        {
            var doctors = new List<DoctorsDropDown>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("PR_Doctor_Dropdown", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    await conn.OpenAsync();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            doctors.Add(new DoctorsDropDown
                            {
                                DoctorID = Convert.ToInt32(reader["DoctorID"]),
                                DoctorName = reader["DoctorName"].ToString()
                            });
                        }
                    }
                }
            }
            return doctors;
        }

        #endregion

        #region PatientDropdown
        public async Task<IEnumerable<PatientsDropDown>> GetPatientAsync()
        {
            var patients = new List<PatientsDropDown>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("PR_Patient_Dropdown", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    await conn.OpenAsync();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            patients.Add(new PatientsDropDown
                            {
                                PatientID = Convert.ToInt32(reader["PatientID"]),
                                PatientName = reader["PatientName"].ToString()
                            });
                        }
                    }
                }
            }
            return patients;
        }

        #endregion
    }
}
