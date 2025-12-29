using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Model.Dto.Application.Requests;
using MediatR;

namespace Blog.Service.Application.UseCases.Blog.Commands;

public partial class UpdateBlogCommand : IRequest<Response<Guid>>
{
    public BlogRequest? Payload { get; set; }
}
