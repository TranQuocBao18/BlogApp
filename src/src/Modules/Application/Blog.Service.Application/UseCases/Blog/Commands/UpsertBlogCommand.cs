using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Model.Dto.Application.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Blog.Service.Application.ModelBinders;

namespace Blog.Service.Application.UseCases.Blog.Commands;

[ModelBinder(BinderType = typeof(UpsertBlogCommandModelBinder))]
public partial class UpsertBlogCommand : IRequest<Response<Guid>>
{
    public BlogRequest? Payload { get; set; }
    public IFormFile? BannerImage { get; set; }
}
