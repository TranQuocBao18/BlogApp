using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Model.Dto.Application.Requests;
using Blog.Model.Dto.Application.Responses;

namespace Blog.Service.Application.Interfaces;

public interface IBlogTagService
{
    Task<Response<BlogTagResponse>> CreateBlogTagAsync(BlogTagRequest blogTagRequest, CancellationToken cancellationToken);
    Task<Response<bool>> DeleteBlogTagAsync(Guid? id, CancellationToken cancellationToken);
}
