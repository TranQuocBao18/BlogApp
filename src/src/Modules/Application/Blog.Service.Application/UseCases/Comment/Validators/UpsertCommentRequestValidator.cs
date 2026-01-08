using System;
using Blog.Model.Dto.Application.Requests;
using Blog.Presentation.Shared.ErrorCodes;
using Blog.Presentation.Shared.Extensions;
using Blog.Service.Application.UseCases.Comment.Commands;
using FluentValidation;

namespace Blog.Service.Application.UseCases.Comment.Validators;

public class UpsertCommentRequestValidator : AbstractValidator<UpsertCommentCommand>
{
    public UpsertCommentRequestValidator()
    {
        RuleFor(x => x.Payload)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty()
                .WithErrorEnum(CommonValidationCode.MSG_011, "Payload");

        RuleFor(x => x.Payload!.Content)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty()
                .WithErrorEnum(CommonValidationCode.MSG_011, nameof(CommentRequest.Content));
    }
}
