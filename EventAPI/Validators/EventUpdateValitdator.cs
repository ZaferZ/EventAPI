using EventAPI.Models;
using FluentValidation;

namespace EventAPI.Validators
{
    public class EventUpdateValitdator : AbstractValidator<EventUpdateDTO>
    {
        public EventUpdateValitdator()
        {
            RuleFor(e => e.Id)
                .NotEmpty().WithMessage("Event ID is required.")
                .GreaterThan(0).WithMessage("Event ID must be a positive integer.");
            RuleFor(e => e.Title)
                .NotEmpty().WithMessage("Event name is required.")
                .MaximumLength(100).WithMessage("Event name cannot exceed 100 characters.");
            RuleFor(e => e.Description)
                .NotEmpty().WithMessage("Event description is required.")
                .MaximumLength(500).WithMessage("Event description cannot exceed 500 characters.");
            RuleFor(e => e.Location)
                .NotEmpty().WithMessage("Event location is required.")
                .MaximumLength(200).WithMessage("Event location cannot exceed 200 characters.");
            RuleFor(e => e.StartDate)
                .NotEmpty().WithMessage("Event start date is required.")
                .GreaterThan(DateTime.UtcNow).WithMessage("Event start date must be in the future.");
            RuleFor(e => e.EndDate)
                .NotEmpty().WithMessage("Event end date is required.")
                .GreaterThan(e => e.StartDate).WithMessage("Event end date must be after the start date.");
        }
    }
}
