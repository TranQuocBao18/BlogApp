using System;
using Blog.Service.Identity.UseCases.Roles.Queries;
using FluentValidation;

namespace Blog.Service.Identity.UseCases.Roles.Validatiors;

public class GetRolesByUserIdValidator : AbstractValidator<GetRolesByUserIdQuery>
{
    public GetRolesByUserIdValidator()
    {
        RuleFor(p => p.UserId)
          .NotEmpty()
          .NotNull();
    }
}
