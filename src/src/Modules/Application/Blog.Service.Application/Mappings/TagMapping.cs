using System;
using AutoMapper;
using Blog.Domain.Application.Entities;
using Blog.Domain.Application.Dtos;
using Blog.Domain.Application.Requests;
using Blog.Domain.Application.Responses;

namespace Blog.Service.Application.Mappings;

public class TagMapping : Profile
{
    public TagMapping()
    {
        CreateMap<Tag, TagDto>().ReverseMap();
        CreateMap<TagRequest, Tag>().ReverseMap();
        CreateMap<TagResponse, Tag>().ReverseMap();
    }
}
