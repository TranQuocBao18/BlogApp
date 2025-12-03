using System;
using Blog.Model.Dto.Identity.Dtos;
using Blog.Model.Dto.Shared.Dtos;

namespace Blog.Model.Dto.Identity.Responses;

public class UserResponse
{
    public UserDto? UserDto { get; set; }
    public UserDetailDto? UserDetailDto { get; set; }
}
