using Blog.Domain.Application.Responses;
using Blog.Infrastructure.Shared.Wrappers;
using MediatR;

namespace Blog.Service.Application.UseCases.Comment.Queries;

public class GetAllCommentsQuery : IRequest<Response<IReadOnlyList<CommentResponse>>>
{
    public string? Search { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}