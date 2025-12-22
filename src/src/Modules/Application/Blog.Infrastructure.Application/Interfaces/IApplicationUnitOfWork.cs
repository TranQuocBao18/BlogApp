using System;
using Blog.UnitOfWork;

namespace Blog.Infrastructure.Application.Interfaces;

public interface IApplicationUnitOfWork : IUnitOfWork
{
    IBannerRepository BannerRepository { get; }
    IBlogRepository BlogRepository { get; }
    IBlogTagRepository BlogTagRepository { get; }
    ICategoryRepository CategoryRepository { get; }
    ICommentRepository CommentRepository { get; }
    ILikeRepository LikeRepository { get; }
    ITagRepository TagRepository { get; }
}
