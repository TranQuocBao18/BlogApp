using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Domain.Application.Responses;
using MediatR;

namespace Blog.Service.Application.UseCases.Blog.Queries;

public class GetBlogBySlugQuery : IRequest<Response<BlogResponse>>
{
    public string Slug { get; set; }
}
