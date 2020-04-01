using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HeroApp.AppShared.Authentication.Authenticate
{
    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(c => c.Username).NotNull().WithMessage("O nome do usuário deve ser informado");
            RuleFor(c => c.Password).NotNull().WithMessage("A senha deve ser informada");
        }
    }

}
