using System;
using Blog.Service.Communication.UseCases.NotificationMessage.Queries;
using FluentValidation;

namespace Blog.Service.Communication.UseCases.NotificationMessage.Validatiors;

public class GetListNotificationByUserValidator : AbstractValidator<GetListNotificationByUserIdQuery>
{
    public GetListNotificationByUserValidator()
    {
        RuleFor(x => x.UserId)
            .NotNull()
            .NotEmpty()
            .WithMessage("UserId is required.");

        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage("PageNumber must be greater than 0.");

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .WithMessage("PageSize must be greater than 0.");
    }
}
