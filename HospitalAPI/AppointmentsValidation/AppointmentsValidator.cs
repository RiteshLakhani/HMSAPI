using HospitalAPI.Model;
using FluentValidation;

namespace HospitalAPI.AppointmentsValidation
{
    public class AppointmentsValidator : AbstractValidator<AppointmentsModel>
    {
        public AppointmentsValidator()
        {

            RuleFor(appointment => appointment.AppointmentTime)
                .NotEmpty().WithMessage("Appointment time is required.");


            RuleFor(appointment => appointment.Problem)
                .NotEmpty().WithMessage("Problem description is required.")
                .MaximumLength(500).WithMessage("Problem description must not exceed 500 characters.");

            RuleFor(appointment => appointment.Status)
                .NotEmpty().WithMessage("Status is required.")
                .Must(status => status == "Scheduled" || status == "Completed" )
                .WithMessage("Status must be either Scheduled, Completed");

            RuleFor(appointment => appointment.IsConfirmed)
                .NotNull().WithMessage("Confirmation status is required.");
        }
    }
}
