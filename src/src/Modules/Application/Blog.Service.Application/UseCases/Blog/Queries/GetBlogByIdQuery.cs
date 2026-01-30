using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Domain.Application.Responses;
using MediatR;

namespace Blog.Service.Application.UseCases.Blog.Queries;

public class GetBlogByIdQuery : IRequest<Response<BlogResponse>>
{
    public Guid Id { get; set; }
}
