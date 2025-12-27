using System;
using Blog.Model.Dto.Application.Dtos;

namespace Blog.Model.Dto.Application.Responses;

public class LikeResponse : LikeDto
{
    public Guid Id { get; set; }
}
