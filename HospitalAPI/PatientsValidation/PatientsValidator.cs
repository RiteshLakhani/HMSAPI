using HospitalAPI.Model;
using FluentValidation;

namespace HospitalAPI.PatientsValidation
{
    public class PatientsValidator : AbstractValidator<PatientsModel>
    {
        public PatientsValidator()
        {
            RuleFor(patient => patient.PatientName)
                .NotEmpty().WithMessage("Patient Name is required.")
                .MaximumLength(100).WithMessage("Patient Name must not exceed 100 characters.");

            RuleFor(patient => patient.DateOfBirth)
                .LessThan(DateTime.Today).WithMessage("Date of Birth must be in the past.");

           
            RuleFor(patient => patient.Phone)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^\+?[0-9]{10,15}$").WithMessage("Phone number must be valid.");

            RuleFor(patient => patient.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email address format.");

            RuleFor(patient => patient.Gender)
                .NotEmpty().WithMessage("Gender is required.")
                .Must(g => g == "Male" || g == "Female" || g == "Other")
                .WithMessage("Gender must be either Male, Female, or Other.");

            RuleFor(patient => patient.Address)
                .NotEmpty().WithMessage("Address is required.")
                .MaximumLength(200).WithMessage("Address must not exceed 200 characters.");
        }
    }
}
