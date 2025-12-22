using System;
using Blog.Infrastructure.Application.Context;
using Blog.Infrastructure.Application.Interfaces;
using Blog.UnitOfWork;

namespace Blog.Infrastructure.Application.UnitOfWorks;

public class ApplicationUnitOfWork : BaseUnitOfWork, IApplicationUnitOfWork
{
    public IBannerRepository BannerRepository { get; private set; }
    public IBlogRepository BlogRepository { get; private set; }
    public IBlogTagRepository BlogTagRepository { get; private set; }
    public ICategoryRepository CategoryRepository { get; private set; }
    public ICommentRepository CommentRepository { get; private set; }
    public ILikeRepository LikeRepository { get; private set; }
    public ITagRepository TagRepository { get; private set; }

    public ApplicationUnitOfWork(
        ApplicationDbContext context,
        IBannerRepository bannerRepository,
        IBlogRepository blogRepository,
        IBlogTagRepository blogTagRepository,
        ICategoryRepository categoryRepository,
        ICommentRepository commentRepository,
        ILikeRepository likeRepository,
        ITagRepository tagRepository
    ) : base(context)
    {
        BannerRepository = bannerRepository;
        BlogRepository = blogRepository;
        BlogTagRepository = blogTagRepository;
        CategoryRepository = categoryRepository;
        CommentRepository = commentRepository;
        LikeRepository = likeRepository;
        TagRepository = tagRepository;
    }
}
