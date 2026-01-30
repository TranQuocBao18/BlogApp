using System;
using Blog.Domain.Identity.Dtos;
using Blog.Model.Dto.Shared.Dtos;

namespace Blog.Domain.Identity.Responses;

public class UserResponse
{
    public UserDto? UserDto { get; set; }
    public UserDetailDto? UserDetailDto { get; set; }
}
