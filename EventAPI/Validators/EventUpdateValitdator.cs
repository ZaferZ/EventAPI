using EventAPI.Models.DTOs;
using FluentValidation;

namespace EventAPI.Validators
{
    public class EventUpdateValitdator : AbstractValidator<UpdateEventDto>
    {
        public EventUpdateValitdator()
        {

            RuleFor(e => e.Title)
                .MaximumLength(100).WithMessage("Event name cannot exceed 100 characters.");
            RuleFor(e => e.Description)
                .MaximumLength(500).WithMessage("Event description cannot exceed 500 characters.");
            RuleFor(e => e.Location)
                .MaximumLength(200).WithMessage("Event location cannot exceed 200 characters.");
            RuleFor(e => e.StartDate)
                .GreaterThan(DateTime.UtcNow).WithMessage("Event start date must be in the future.");
            RuleFor(e => e.EndDate)
                .GreaterThan(e => e.StartDate).WithMessage("Event end date must be after the start date.");
        }
    }
}
