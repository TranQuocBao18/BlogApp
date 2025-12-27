using System;
using Blog.Model.Dto.Application.Dtos;

namespace Blog.Model.Dto.Application.Requests;

public class BlogTagRequest : BlogTagDto
{
    public Guid? Id { get; set; }
}
