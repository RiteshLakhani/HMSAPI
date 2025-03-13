using HospitalAPI.Model;
using FluentValidation;

namespace HospitalAPI.RoomsValidation
{
    public class RoomsValidator : AbstractValidator<RoomsModel>
    {
        public RoomsValidator()
        {
            RuleFor(room => room.RoomNumber)
                .GreaterThan(0).WithMessage("Room number must be a positive number.");

            RuleFor(room => room.RoomType)
                .NotEmpty().WithMessage("Room type is required.")
                .MaximumLength(50).WithMessage("Room type must not exceed 50 characters.");

            //RuleFor(room => room.PatientID)
            //    .GreaterThan(0).WithMessage("Patient ID must be a positive number.");

            //RuleFor(room => room.AllotmentDate)
            //    .LessThanOrEqualTo(DateTime.Today)
            //    .WithMessage("Allotment date must be today or in the past.");

            RuleFor(room => room.DischargeDate)
                .GreaterThan(room => room.AllotmentDate)
                .WithMessage("Discharge date must be after the allotment date.");

            //RuleFor(room => room.DoctorID)
            //    .GreaterThan(0).WithMessage("Doctor ID must be a positive number.");

            RuleFor(room => room.IsConfirmed)
                .NotNull().WithMessage("Confirmation status is required.");
        }
    }
}
