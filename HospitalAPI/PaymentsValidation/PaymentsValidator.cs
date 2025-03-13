//using HospitalAPI.Model;
//using FluentValidation;

//namespace HospitalAPI.PaymentsValidation
//{
//    public class PaymentsValidator : AbstractValidator<PaymentsModel>
//    {
//        public PaymentsValidator()
//        {
//            RuleFor(payment => payment.DoctorName).NotEmpty().WithMessage("DoctorName is required.");

//            RuleFor(payment => payment.PatientName).NotEmpty().WithMessage("Patient is required.");


//            RuleFor(payment => payment.Department)
//                .NotEmpty().WithMessage("Department is required.")
//                .MaximumLength(100).WithMessage("Department must not exceed 100 characters.");

//            RuleFor(payment => payment.ServiceName)
//                .NotEmpty().WithMessage("Service name is required.")
//                .MaximumLength(100).WithMessage("Service name must not exceed 100 characters.");

//            RuleFor(payment => payment.CostOfTreatment)
//                .GreaterThanOrEqualTo(0).WithMessage("Cost of treatment must be a non-negative value.");

//            RuleFor(payment => payment.AdvancedPaid)
//                .GreaterThanOrEqualTo(0)
//                .When(payment => payment.AdvancedPaid.HasValue)
//                .WithMessage("Advanced payment must be a non-negative value.");

//            RuleFor(payment => payment.Discount)
//                .GreaterThanOrEqualTo(0)
//                .When(payment => payment.Discount.HasValue)
//                .WithMessage("Discount must be a non-negative value.");

//            RuleFor(payment => payment.Amount)
//                .GreaterThanOrEqualTo(0)
//                .When(payment => payment.Amount.HasValue)
//                .WithMessage("Amount must be a non-negative value.");


//            RuleFor(payment => payment.PaymentMethod)
//                .NotEmpty().WithMessage("Payment method is required.")
//                .Must(method => method == "Cash" || method == "Card" || method == "Check")
//                .WithMessage("Payment method must be one of Cash, Card, Check");

//            RuleFor(payment => payment.PaymentType)
//                .NotEmpty().WithMessage("Payment type is required.")
//                .MaximumLength(50).WithMessage("Payment type must not exceed 50 characters.");

//            RuleFor(payment => payment.CardOrCheckNo)
//                .NotEmpty().When(payment => payment.PaymentMethod == "Card" || payment.PaymentMethod == "Check")
//                .WithMessage("Card or Check number is required for Card or Check payment methods.")
//                .MaximumLength(20).WithMessage("Card or Check number must not exceed 20 characters.");

//            RuleFor(payment => payment.PaymentStatus)
//                .NotEmpty().WithMessage("Payment status is required.")
//                .Must(status => status == "Pending" || status == "Paid")
//                .WithMessage("Payment status must be one of Pending or Paid.");

//            RuleFor(payment => payment.IsConfirmed)
//                .NotNull().WithMessage("Confirmation status is required.");
//        }
//    }
//}
