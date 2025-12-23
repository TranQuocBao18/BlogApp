using System;
using Blog.Domain.Application.Entities;
using Blog.Infrastructure.Shared.Interfaces;

namespace Blog.Infrastructure.Application.Interfaces;

public interface IBlogRepository : IGenericRepository<BlogEntity, Guid>
{
    Task<BlogEntity> GetByBlogSlugAsync(string slug, CancellationToken cancellationToken, bool includedDeleted = false);
}
