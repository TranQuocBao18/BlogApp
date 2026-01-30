using System;
using Blog.Domain.Application.Dtos;

namespace Blog.Domain.Application.Requests;

public class CategoryRequest : CategoryDto
{
    public Guid? Id { get; set; }
}
