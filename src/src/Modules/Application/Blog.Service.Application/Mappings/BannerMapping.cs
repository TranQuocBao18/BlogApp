using System;
using AutoMapper;
using Blog.Domain.Application.Entities;
using Blog.Domain.Application.Dtos;
using Blog.Domain.Application.Requests;
using Blog.Domain.Application.Responses;

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
