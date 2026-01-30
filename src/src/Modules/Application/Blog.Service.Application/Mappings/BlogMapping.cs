using System;
using AutoMapper;
using Blog.Domain.Application.Entities;
using Blog.Domain.Application.Dtos;
using Blog.Domain.Application.Requests;
using Blog.Domain.Application.Responses;

namespace Blog.Service.Application.Mappings;

public class BlogMapping : Profile
{
    public BlogMapping()
    {
        // BlogEntity → BlogResponse
        CreateMap<BlogEntity, BlogResponse>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
            .ForMember(dest => dest.Banner, opt => opt.MapFrom(src => src.Banner))
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src =>
                src.BlogTags.Select(bt => bt.Tag).ToList()))
            .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(src =>
                src.Comments.Count(c => !c.IsDeleted)))
            // LikeCount và IsLikeByCurrentUser sẽ set manually trong service
            .ForMember(dest => dest.LikeCount, opt => opt.Ignore())
            .ForMember(dest => dest.IsLikeByCurrentUser, opt => opt.Ignore());

        // Category → CategoryDto
        CreateMap<Category, CategoryDto>();

        // Banner → BannerDto
        CreateMap<Banner, BannerDto>();

        // Tag → TagDto
        CreateMap<Tag, TagDto>();

        // Reverse mappings cho Create/Update
        CreateMap<BlogDto, BlogEntity>()
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Category!.Id))
            .ForMember(dest => dest.BannerId, opt => opt.MapFrom(src => src.Banner != null ? src.Banner.Id : (Guid?)null))
            .ForMember(dest => dest.Category, opt => opt.Ignore())
            .ForMember(dest => dest.Banner, opt => opt.Ignore())
            .ForMember(dest => dest.BlogTags, opt => opt.Ignore())
            .ForMember(dest => dest.Comments, opt => opt.Ignore())
            .ForMember(dest => dest.Likes, opt => opt.Ignore());

        // BlogRequest → BlogEntity (for API requests)
        CreateMap<BlogRequest, BlogEntity>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id ?? Guid.NewGuid()))
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
            .ForMember(dest => dest.BannerId, opt => opt.MapFrom(src => src.BannerId))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
            .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => src.Slug))
            // .ForMember(dest => dest.MetaDescription, opt => opt.MapFrom(src => src.MetaDescription))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.Category, opt => opt.Ignore())
            .ForMember(dest => dest.Banner, opt => opt.Ignore())
            .ForMember(dest => dest.BlogTags, opt => opt.Ignore())
            .ForMember(dest => dest.Comments, opt => opt.Ignore())
            .ForMember(dest => dest.Likes, opt => opt.Ignore());
    }
}
