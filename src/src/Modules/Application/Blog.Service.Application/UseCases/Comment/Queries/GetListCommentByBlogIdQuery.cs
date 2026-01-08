using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Model.Dto.Application.Responses;
using MediatR;

namespace Blog.Service.Application.UseCases.Comment.Queries;

public class GetListCommentByBlogIdQuery : IRequest<PagedResponse<IReadOnlyList<CommentResponse>>>
{
    public Guid Id { get; set; }
    public string? Search { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
