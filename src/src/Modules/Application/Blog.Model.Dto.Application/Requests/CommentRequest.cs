using System;
using Blog.Model.Dto.Application.Dtos;

namespace Blog.Model.Dto.Application.Requests;

public class CommentRequest : CommentDto
{
    public Guid? Id { get; set; }
}
