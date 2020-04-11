using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HeroApp.AppShared.Authentication.Login
{
    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(c => c.Email).NotEmpty().WithMessage("'Email' is required");
            RuleFor(c => c.Email).Must(ContaintAt).WithMessage("Please inform a valid email address");
            RuleFor(c => c.Password).NotEmpty().WithMessage("'Password' is required");
            RuleFor(c => c.Password).MinimumLength(6)
                .WithMessage("Inform a password with mininum of 6 characters");
        }

        private bool ContaintAt(string arg)
        {
            if (arg == null) return false;
            return arg.Contains('@');
        }
    }

}
