using System;
using AutoMapper;
using Blog.Domain.Identity.Entities;
using Blog.Domain.Identity.Dtos;
using Blog.Domain.Identity.Requests;
using Blog.Domain.Identity.Responses;
using Blog.Model.Dto.Shared.Dtos;

namespace Blog.Service.Identity.Mappings;

public class UserMapping : Profile
{
    public UserMapping()
    {
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<User, UserDetailDto>().ReverseMap();
        CreateMap<User, UsersResponse>();
        CreateMap<UserRequest, User>();
        CreateMap<User, UserDto>();
    }
}
