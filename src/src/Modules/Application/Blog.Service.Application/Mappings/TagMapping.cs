using System;
using AutoMapper;
using Blog.Domain.Application.Entities;
using Blog.Model.Dto.Application.Dtos;
using Blog.Model.Dto.Application.Requests;
using Blog.Model.Dto.Application.Responses;

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
