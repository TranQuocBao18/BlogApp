using System;
using Blog.Model.Dto.Application.Dtos;

namespace Blog.Model.Dto.Application.Responses;

public class CategoryResponse : CategoryDto
{
    public Guid? Id { get; set; }

}
