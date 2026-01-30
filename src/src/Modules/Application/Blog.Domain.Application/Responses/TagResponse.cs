using System;
using Blog.Domain.Application.Dtos;

namespace Blog.Domain.Application.Responses;

public class TagResponse : TagDto
{
    public Guid Id { get; set; }
}
