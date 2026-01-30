using System;
using Blog.Domain.Application.Requests;
using Blog.Presentation.Shared.ErrorCodes;
using Blog.Presentation.Shared.Extensions;
using Blog.Service.Application.UseCases.Tag.Commands;
using FluentValidation;

namespace Blog.Service.Application.UseCases.Tag.Validators;

public class UpsertTagRequestValidator : AbstractValidator<UpsertTagCommand>
{
    public UpsertTagRequestValidator()
    {
        RuleFor(x => x.Payload)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty()
                .WithErrorEnum(CommonValidationCode.MSG_011, "Payload");

        RuleFor(x => x.Payload.Name)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty()
                .WithErrorEnum(CommonValidationCode.MSG_011, nameof(TagRequest.Name));
    }
}
