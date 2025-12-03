using System;
using Blog.Presentation.Shared.ErrorCodes;
using Blog.Presentation.Shared.Extensions;
using Blog.Service.Identity.UseCases.Users.Queries;
using FluentValidation;

namespace Blog.Service.Identity.UseCases.Users.Validators;

public class GetUserByIdRequestValidator : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdRequestValidator()
    {
        RuleFor(x => x.Id)
               .NotNull()
               .NotEmpty()
               .WithErrorEnum(CommonValidationCode.MSG_011, "Id");
    }
}
