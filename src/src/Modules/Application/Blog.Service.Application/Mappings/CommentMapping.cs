using System;
using AutoMapper;
using Blog.Domain.Application.Entities;
using Blog.Domain.Application.Dtos;
using Blog.Domain.Application.Requests;
using Blog.Domain.Application.Responses;

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
