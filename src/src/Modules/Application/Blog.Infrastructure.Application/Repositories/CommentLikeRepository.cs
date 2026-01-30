using System;
using Blog.Domain.Application.Entities;
using Blog.Infrastructure.Application.Context;
using Blog.Domain.Application.Interfaces;
using Blog.Infrastructure.Shared.Persistences.Repositories.Common;

namespace Blog.Infrastructure.Application.Repositories;

public class CommentLikeRepository : GenericRepositoryAsync<CommentLike, Guid>, ICommentLikeRepository
{
    public CommentLikeRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
