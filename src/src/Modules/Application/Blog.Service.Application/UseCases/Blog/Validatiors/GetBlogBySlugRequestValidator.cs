using System;
using Blog.Presentation.Shared.ErrorCodes;
using Blog.Presentation.Shared.Extensions;
using Blog.Service.Application.UseCases.Blog.Queries;
using FluentValidation;

namespace Blog.Service.Application.UseCases.Blog.Validatiors;

public class GetBlogBySlugRequestValidator : AbstractValidator<GetBlogBySlugQuery>
{
    public GetBlogBySlugRequestValidator()
    {
        RuleFor(x => x.Slug)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty()
                .WithErrorEnum(CommonValidationCode.MSG_011, "Slug");
    }
}
