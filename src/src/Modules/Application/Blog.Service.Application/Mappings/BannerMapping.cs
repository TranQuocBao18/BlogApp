using System;
using AutoMapper;
using Blog.Domain.Application.Entities;
using Blog.Model.Dto.Application.Dtos;
using Blog.Model.Dto.Application.Requests;
using Blog.Model.Dto.Application.Responses;

namespace Blog.Service.Application.Mappings;

public class BannerMapping : Profile
{
    public BannerMapping()
    {
        CreateMap<Banner, BannerDto>().ReverseMap();
        CreateMap<BannerRequest, Banner>().ReverseMap();
        CreateMap<BannerResponse, Banner>().ReverseMap();
    }
}
