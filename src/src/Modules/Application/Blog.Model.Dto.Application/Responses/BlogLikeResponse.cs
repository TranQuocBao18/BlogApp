using System;
using Blog.Model.Dto.Application.Dtos;

namespace Blog.Model.Dto.Application.Responses;

public class BlogLikeResponse : BlogLikeDto
{
    public Guid Id { get; set; }
}
