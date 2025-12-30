using System;
using Blog.Infrastructure.Shared.Wrappers;
using MediatR;

namespace Blog.Service.Application.UseCases.Blog.Commands;

public class DeleteBlogByIdCommand : IRequest<Response<bool>>
{
    public Guid Id { get; set; }
}
