using System;
using Blog.Service.Identity.UseCases.Identity.Commands;
using FluentValidation;

namespace Blog.Service.Identity.UseCases.Identity.Validators;

public class LoginUserValidator : AbstractValidator<LoginUserCommand>
{
  public LoginUserValidator()
  {
    RuleFor(p => p.Email)
      .NotEmpty().WithMessage("{PropertyName} is required.")
      .NotNull()
      .MaximumLength(256).WithMessage("{PropertyName} must not exceed 256 characters.");

    RuleFor(p => p.Password)
        .NotEmpty().WithMessage("{PropertyName} is required.")
        .NotNull()
        .MaximumLength(250).WithMessage("{PropertyName} must not exceed 250 characters.");
  }
}
