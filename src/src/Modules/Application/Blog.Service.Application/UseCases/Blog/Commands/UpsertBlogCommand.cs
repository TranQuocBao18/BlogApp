using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Model.Dto.Application.Requests;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Blog.Service.Application.UseCases.Blog.Commands;

public partial class UpsertBlogCommand : IRequest<Response<Guid>>
{
    public BlogRequest? Payload { get; set; }
    public IFormFile? BannerImage { get; set; }
}
