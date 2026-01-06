using System;
using Blog.Service.Communication.UseCases.NotificationMessage.Queries;
using FluentValidation;

namespace Blog.Service.Communication.UseCases.NotificationMessage.Validatiors;

public class GetListNotificationByIdValidator : AbstractValidator<GetNotificationByIdQuery>
{
    public GetListNotificationByIdValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .NotEmpty()
            .WithMessage("Id is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("Id must not be empty.");
    }
}
