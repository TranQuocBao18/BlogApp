using System;
using Blog.Model.Dto.Application.Dtos;

namespace Blog.Model.Dto.Application.Responses;

public class BlogTagResponse : BlogTagDto
{
    public Guid Id { get; set; }
}
