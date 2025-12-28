using System;
using AutoMapper;
using Blog.Domain.Application.Entities;
using Blog.Model.Dto.Application.Dtos;
using Blog.Model.Dto.Application.Requests;
using Blog.Model.Dto.Application.Responses;

namespace Blog.Service.Application.Mappings;

public class BlogLikeMapping : Profile
{
    public BlogLikeMapping()
    {
        CreateMap<BlogLike, BlogLikeDto>().ReverseMap();
        CreateMap<BlogLikeRequest, BlogLike>().ReverseMap();
        CreateMap<BlogLikeResponse, BlogLike>().ReverseMap();
    }
}
