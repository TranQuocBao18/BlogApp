using System;
using AutoMapper;
using Blog.Domain.Application.Entities;
using Blog.Model.Dto.Application.Dtos;
using Blog.Model.Dto.Application.Requests;
using Blog.Model.Dto.Application.Responses;

namespace Blog.Service.Application.Mappings;

public class BlogMapping : Profile
{
    public BlogMapping()
    {
        CreateMap<BlogEntity, BlogDto>().ReverseMap();
        CreateMap<BlogRequest, BlogEntity>().ReverseMap();
        CreateMap<BlogResponse, BlogEntity>().ReverseMap();
    }
}
