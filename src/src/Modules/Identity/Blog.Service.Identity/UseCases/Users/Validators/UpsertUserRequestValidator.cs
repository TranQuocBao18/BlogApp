using System;
using Blog.Domain.Identity.Requests;
using Blog.Presentation.Shared.ErrorCodes;
using Blog.Presentation.Shared.Extensions;
using Blog.Service.Identity.UseCases.Users.Commands;
using FluentValidation;

namespace Blog.Service.Identity.UseCases.Users.Validators;

public class UpsertUserRequestValidator : AbstractValidator<UpsertUserCommand>
{
    public UpsertUserRequestValidator()
    {
        RuleFor(x => x.Payload)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .NotEmpty()
            .WithErrorEnum(CommonValidationCode.MSG_019, "Payload");

        RuleFor(x => x.Payload.Username)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .NotEmpty()
            .WithErrorEnum(CommonValidationCode.MSG_019, nameof(UserRequest.Username));

        RuleFor(x => x.Payload.Email)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .NotEmpty()
            .WithErrorEnum(CommonValidationCode.MSG_019, nameof(UserRequest.Email));
    }
}
