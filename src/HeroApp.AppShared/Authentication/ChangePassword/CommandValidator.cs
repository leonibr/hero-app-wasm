using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HeroApp.AppShared.Authentication.ChangePassword
{
    static class Size
    {
        public static int Min => 6;
        public static int Max => 20;
    }
    public class CommandValidator : AbstractValidator<Command>
    {
        private readonly string newPasswordLenth = $"The new password must be between {Size.Min} and {Size.Max} characters";
        public CommandValidator()
        {
            CascadeMode = CascadeMode.Continue;
            RuleFor(c => c.UserName)
                .NotEmpty().WithMessage("The user id/username is required");
            RuleFor(c => c.CurrentPassword)
                .NotEmpty().WithMessage("The current password is required");
            RuleFor(c => c.NewPassword)
                .NotEmpty().WithMessage("The new password is required");
            RuleFor(c => c.NewPassword)
                    .MinimumLength(Size.Min)
                    .MaximumLength(Size.Max)
                    .WithMessage(newPasswordLenth);
            RuleFor(c => c.ConfirmNewPassord)
                    .MinimumLength(Size.Min)
                    .MaximumLength(Size.Max)
                    .WithMessage(newPasswordLenth);
            RuleFor(c => c.ConfirmNewPassord)
                .NotEmpty().WithMessage("The password confirmation is required");

        }
    }


}
