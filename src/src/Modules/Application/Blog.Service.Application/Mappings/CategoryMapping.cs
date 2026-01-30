using System;
using AutoMapper;
using Blog.Domain.Application.Entities;
using Blog.Domain.Application.Dtos;
using Blog.Domain.Application.Requests;
using Blog.Domain.Application.Responses;

namespace Blog.Service.Application.Mappings;

public class CategoryMapping : Profile
{
    public CategoryMapping()
    {
        CreateMap<Category, CategoryDto>().ReverseMap();
        CreateMap<CategoryRequest, Category>().ReverseMap();
        CreateMap<CategoryResponse, Category>().ReverseMap();
    }
}
