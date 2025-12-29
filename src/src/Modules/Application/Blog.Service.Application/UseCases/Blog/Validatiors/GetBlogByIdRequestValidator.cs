using System;
using Blog.Presentation.Shared.ErrorCodes;
using Blog.Presentation.Shared.Extensions;
using Blog.Service.Application.UseCases.Blog.Queries;
using FluentValidation;

namespace Blog.Service.Application.UseCases.Blog.Validatiors;

public class GetBlogByIdRequestValidator : AbstractValidator<GetBlogByIdQuery>
{
    public GetBlogByIdRequestValidator()
    {
        RuleFor(x => x.Id)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty()
                .WithErrorEnum(CommonValidationCode.MSG_011, "Id");
    }
}
