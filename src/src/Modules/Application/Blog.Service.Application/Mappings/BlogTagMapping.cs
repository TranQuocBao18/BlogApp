using System;
using AutoMapper;
using Blog.Domain.Application.Entities;
using Blog.Model.Dto.Application.Dtos;
using Blog.Model.Dto.Application.Requests;
using Blog.Model.Dto.Application.Responses;

namespace Blog.Service.Application.Mappings;

public class BlogTagMapping : Profile
{
    public BlogTagMapping()
    {
        CreateMap<BlogTag, BlogTagDto>().ReverseMap();
        CreateMap<BlogTagRequest, BlogTag>().ReverseMap();
        CreateMap<BlogTagResponse, BlogTag>().ReverseMap();
    }
}
