using System;
using AutoMapper;
using Blog.Domain.Application.Entities;
using Blog.Model.Dto.Application.Dtos;
using Blog.Model.Dto.Application.Requests;
using Blog.Model.Dto.Application.Responses;

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
