using System;
using Blog.Model.Dto.Application.Dtos;

namespace Blog.Model.Dto.Application.Responses;

public class CommentResponse : CommentDto
{
    public Guid Id { get; set; }
}
