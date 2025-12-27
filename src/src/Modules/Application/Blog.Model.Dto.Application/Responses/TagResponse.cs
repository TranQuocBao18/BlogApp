using System;
using Blog.Model.Dto.Application.Dtos;

namespace Blog.Model.Dto.Application.Responses;

public class TagResponse : TagDto
{
    public Guid Id { get; set; }
}
