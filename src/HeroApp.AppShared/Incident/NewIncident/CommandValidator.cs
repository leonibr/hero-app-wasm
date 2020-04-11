using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HeroApp.AppShared.Incident.NewIncident
{
    public class CommandValidator: AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(c => c.Title).NotEmpty();
            RuleFor(c => c.Description).NotEmpty();
            RuleFor(c => c.Value).NotEmpty();
            RuleFor(c => c.Value).Must(BeCastableToDouble).WithMessage("Is not a valid number");

        }

        private bool BeCastableToDouble(string arg)
        {
            return double.TryParse(arg, out _);
        }
    }
}
