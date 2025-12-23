using System;
using Blog.Model.Dto.Application.Dtos;

namespace Blog.Model.Dto.Application.Requests;

public class LikeRequest : LikeDto
{
    public Guid Id { get; set; }
}
