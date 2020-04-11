using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HeroApp.AppShared.Authentication.Register
{
    public class CommandValidator: AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(c => c.Name).NotEmpty();
            RuleFor(c => c.Email).NotEmpty();
            RuleFor(c => c.Password).NotEmpty().WithMessage("'Password' is required");
            RuleFor(c => c.Password).MinimumLength(6)
                .WithMessage("Inform a password with mininum of 6 characters");

            RuleFor(c => c.ConfirmPassword).NotEmpty();
            RuleFor(c => c.ConfirmPassword).Equal(c => c.Password);


            RuleFor(c => c.Whatsapp).NotEmpty();
            RuleFor(c => c.City).NotEmpty();
            RuleFor(c => c.State).NotEmpty().MaximumLength(2);




        }
    }
}
