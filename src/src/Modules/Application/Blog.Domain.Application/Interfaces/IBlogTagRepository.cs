using System;
using Blog.Domain.Application.Entities;
using Blog.Infrastructure.Shared.Interfaces;

namespace Blog.Domain.Application.Interfaces;

public interface IBlogTagRepository : IGenericRepository<BlogTag, Guid>
{

}
