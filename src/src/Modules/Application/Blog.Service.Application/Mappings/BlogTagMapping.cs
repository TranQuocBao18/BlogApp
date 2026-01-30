using System;
using AutoMapper;
using Blog.Domain.Application.Entities;
using Blog.Domain.Application.Dtos;
using Blog.Domain.Application.Requests;
using Blog.Domain.Application.Responses;

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
