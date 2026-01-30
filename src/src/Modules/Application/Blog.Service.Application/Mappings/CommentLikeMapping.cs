using System;
using AutoMapper;
using Blog.Domain.Application.Entities;
using Blog.Domain.Application.Dtos;
using Blog.Domain.Application.Requests;
using Blog.Domain.Application.Responses;

namespace Blog.Service.Application.Mappings;

public class CommentLikeMapping : Profile
{
    public CommentLikeMapping()
    {
        CreateMap<CommentLike, CommentLikeDto>().ReverseMap();
        CreateMap<CommentLikeRequest, CommentLike>().ReverseMap();
        CreateMap<CommentLikeResponse, CommentLike>().ReverseMap();
    }
}
