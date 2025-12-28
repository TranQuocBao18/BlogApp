using System;
using Blog.Domain.Application.Entities;
using Blog.Infrastructure.Shared.Interfaces;

namespace Blog.Infrastructure.Application.Interfaces;

public interface IBlogLikeRepository : IGenericRepository<BlogLike, Guid>
{

}
