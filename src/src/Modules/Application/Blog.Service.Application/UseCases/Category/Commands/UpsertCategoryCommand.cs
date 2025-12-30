using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Model.Dto.Application.Requests;
using MediatR;

namespace Blog.Service.Application.UseCases.Category.Commands;

public partial class UpsertCategoryCommand : IRequest<Response<Guid>>
{
    public CategoryRequest? Payload { get; set; }
}
