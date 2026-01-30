using System;
using Blog.Infrastructure.Application.Context;
using Blog.Domain.Application.Interfaces;
using Blog.UnitOfWork;

namespace Blog.Infrastructure.Application.UnitOfWorks;

public class ApplicationUnitOfWork : BaseUnitOfWork, IApplicationUnitOfWork
{
    public IBannerRepository BannerRepository { get; private set; }
    public IBlogRepository BlogRepository { get; private set; }
    public IBlogTagRepository BlogTagRepository { get; private set; }
    public ICategoryRepository CategoryRepository { get; private set; }
    public ICommentRepository CommentRepository { get; private set; }
    public IBlogLikeRepository BlogLikeRepository { get; private set; }
    public ICommentLikeRepository CommentLikeRepository { get; private set; }
    public ITagRepository TagRepository { get; private set; }

    public ApplicationUnitOfWork(
        ApplicationDbContext context,
        IBannerRepository bannerRepository,
        IBlogRepository blogRepository,
        IBlogTagRepository blogTagRepository,
        ICategoryRepository categoryRepository,
        ICommentRepository commentRepository,
        IBlogLikeRepository blogLikeRepository,
        ICommentLikeRepository commentLikeRepository,
        ITagRepository tagRepository
    ) : base(context)
    {
        BannerRepository = bannerRepository;
        BlogRepository = blogRepository;
        BlogTagRepository = blogTagRepository;
        CategoryRepository = categoryRepository;
        CommentRepository = commentRepository;
        BlogLikeRepository = blogLikeRepository;
        CommentLikeRepository = commentLikeRepository;
        TagRepository = tagRepository;
    }
}
