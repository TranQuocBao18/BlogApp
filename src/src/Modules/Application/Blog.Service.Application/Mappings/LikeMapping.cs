using System;
using AutoMapper;
using Blog.Domain.Application.Entities;
using Blog.Model.Dto.Application.Dtos;
using Blog.Model.Dto.Application.Requests;
using Blog.Model.Dto.Application.Responses;

namespace Blog.Service.Application.Mappings;

public class LikeMapping : Profile
{
    public LikeMapping()
    {
        CreateMap<Like, LikeDto>().ReverseMap();
        CreateMap<LikeRequest, Like>().ReverseMap();
        CreateMap<LikeResponse, Like>().ReverseMap();
    }
}
