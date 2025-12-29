using System;
using Blog.Model.Dto.Application.Requests;
using Blog.Presentation.Shared.ErrorCodes;
using Blog.Presentation.Shared.Extensions;
using Blog.Service.Application.UseCases.Blog.Commands;
using FluentValidation;

namespace Blog.Service.Application.UseCases.Blog.Validatiors;

public class InsertBlogRequestValidatior : AbstractValidator<InsertBlogCommand>
{
    public InsertBlogRequestValidatior()
    {
        RuleFor(x => x.Payload)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty()
                .WithErrorEnum(CommonValidationCode.MSG_011, "Payload");

        RuleFor(x => x.Payload.Title)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty()
                .WithErrorEnum(CommonValidationCode.MSG_011, nameof(BlogRequest.Title));

        RuleFor(x => x.Payload.Content)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty()
                .WithErrorEnum(CommonValidationCode.MSG_011, nameof(BlogRequest.Content));
    }
}
