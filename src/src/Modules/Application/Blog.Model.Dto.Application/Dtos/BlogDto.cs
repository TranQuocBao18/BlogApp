using System;
using Blog.Domain.Application.Enum;

namespace Blog.Model.Dto.Application.Dtos;

public class BlogDto
{
    public required string Title { get; set; }
    public required string Content { get; set; }
    public string? Slug { get; set; }
    public BlogStatus Status { get; set; }
    public int? LikeCount { get; set; }
    public bool? IsLikeByCurrentUser { get; set; }
    public int CommentCount { get; set; }
    public CategoryDto? Category { get; set; }
    public BannerDto? Banner { get; set; }
    public List<TagDto> Tags { get; set; } = new();

}
