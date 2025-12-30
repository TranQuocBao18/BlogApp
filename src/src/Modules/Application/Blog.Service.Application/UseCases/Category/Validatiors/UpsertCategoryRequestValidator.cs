using System;
using Blog.Model.Dto.Application.Requests;
using Blog.Presentation.Shared.ErrorCodes;
using Blog.Presentation.Shared.Extensions;
using Blog.Service.Application.UseCases.Category.Commands;
using FluentValidation;

namespace Blog.Service.Application.UseCases.Category.Validatiors;

public class UpsertCategoryRequestValidator : AbstractValidator<UpsertCategoryCommand>
{
    public UpsertCategoryRequestValidator()
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
                .WithErrorEnum(CommonValidationCode.MSG_011, nameof(CategoryRequest.Name));
    }
}
