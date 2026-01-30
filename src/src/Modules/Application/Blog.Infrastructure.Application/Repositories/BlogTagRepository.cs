using System;
using Blog.Domain.Application.Entities;
using Blog.Infrastructure.Application.Context;
using Blog.Domain.Application.Interfaces;
using Blog.Infrastructure.Shared.Persistences.Repositories.Common;

namespace Blog.Infrastructure.Application.Repositories;

public class BlogTagRepository : GenericRepositoryAsync<BlogTag, Guid>, IBlogTagRepository
{
    public BlogTagRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
