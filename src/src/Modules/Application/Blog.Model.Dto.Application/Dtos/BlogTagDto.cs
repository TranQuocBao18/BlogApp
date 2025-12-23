using System;

namespace Blog.Model.Dto.Application.Dtos;

public class BlogTagDto
{
    public Guid BlogId { get; set; }
    public Guid TagId { get; set; }
}
