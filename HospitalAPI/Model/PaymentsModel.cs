using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalAPI.Model
{
    public class PaymentsModel
    {
        public int PaymentID { get; set; }

        [Required(ErrorMessage = "Patient ID is required.")]
        public int? PatientID { get; set; }

        public string? PatientName { get; set; }

        [Required(ErrorMessage = "Doctor ID is required.")]
        public int? DoctorID { get; set; }

        public string? DoctorName { get; set; }

        public DateTime? AdmissionDate { get; set; }
        public DateTime? DischargeDate { get; set; }

        [Required(ErrorMessage = "Department is required.")]
        public string Department { get; set; }

        [Required(ErrorMessage = "Service Name is required.")]
        public string ServiceName { get; set; }

        [Required(ErrorMessage = "Cost of Treatment is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Cost must be a positive number.")]
        public decimal CostOfTreatment { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Advanced Paid must be a positive number.")]
        public decimal? AdvancedPaid { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Discount must be a positive number.")]
        public decimal? Discount { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Amount must be a positive number.")]
        public decimal? Amount { get; set; }

        [Required(ErrorMessage = "Payment Date is required.")]
        public DateTime PaymentDate { get; set; }

        [Required(ErrorMessage = "Payment Method is required.")]
        public string PaymentMethod { get; set; }

        [Required(ErrorMessage = "Payment Type is required.")]
        public string PaymentType { get; set; }

        [RequiredIfPaymentMethodIsCard]
        public string CardOrCheckNo { get; set; }

        [Required(ErrorMessage = "Payment Status is required.")]
        public string PaymentStatus { get; set; }

        public bool IsConfirmed { get; set; }
    }

    // Custom validation attribute to make CardOrCheckNo required only if the PaymentMethod is Card
    public class RequiredIfPaymentMethodIsCardAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var paymentModel = (PaymentsModel)validationContext.ObjectInstance;

            if (paymentModel.PaymentMethod == "Card" && string.IsNullOrEmpty(value?.ToString()))
            {
                return new ValidationResult("The Card or Check No field is required when the payment method is Card.");
            }

            return ValidationResult.Success;
        }
    }
}