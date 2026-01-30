using System;
using Blog.Domain.Application.Dtos;

namespace Blog.Domain.Application.Responses;

public class CategoryResponse : CategoryDto
{
    public Guid? Id { get; set; }
}
