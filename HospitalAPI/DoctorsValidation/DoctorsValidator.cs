using HospitalAPI.Model;
using FluentValidation;
using System;

namespace HospitalAPI.DoctorsValidation
{
    public class DoctorsValidator : AbstractValidator<DoctorsModel>
    {
        public DoctorsValidator()
        {
            RuleFor(doctor => doctor.DoctorName)
                .NotEmpty().WithMessage("Doctor Name is required.")
                .MaximumLength(100).WithMessage("Doctor Name must not exceed 100 characters.");


            RuleFor(doctor => doctor.DateOfBirth)
                .NotEmpty().WithMessage("Date of Birth is required.")
                .LessThan(DateTime.Today).WithMessage("Date of Birth must be in the past.");

            RuleFor(doctor => doctor.Specialization)
                .NotEmpty().WithMessage("Specialization is required.")
                .MaximumLength(100).WithMessage("Specialization must not exceed 100 characters.");

            RuleFor(doctor => doctor.Experience)
                .GreaterThanOrEqualTo(0).WithMessage("Experience must be a non-negative number.");

            RuleFor(doctor => doctor.DoctorDetail)
                .MaximumLength(500).WithMessage("Doctor detail must not exceed 500 characters.");

            RuleFor(doctor => doctor.Address)
                .NotEmpty().WithMessage("Address is required.");

            RuleFor(doctor => doctor.ContactNumber)
                .NotEmpty().WithMessage("Contact number is required.")
                .Matches(@"^\+?[1-9][0-9]{9,14}$").WithMessage("Invalid contact number format. Example: +1234567890");

            RuleFor(doctor => doctor.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email address format.");

            RuleFor(doctor => doctor.IsConfirmed)
                .NotNull().WithMessage("Confirmation status is required.");
        }
    }
}
