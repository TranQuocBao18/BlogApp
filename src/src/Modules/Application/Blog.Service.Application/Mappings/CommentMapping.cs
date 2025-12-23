using System;
using AutoMapper;
using Blog.Domain.Application.Entities;
using Blog.Model.Dto.Application.Dtos;
using Blog.Model.Dto.Application.Requests;
using Blog.Model.Dto.Application.Responses;

namespace Blog.Service.Application.Mappings;

public class CommentMapping : Profile
{
    public CommentMapping()
    {
        CreateMap<Comment, CommentDto>().ReverseMap();
        CreateMap<CommentRequest, Comment>().ReverseMap();
        CreateMap<CommentResponse, Comment>().ReverseMap();
    }
}
