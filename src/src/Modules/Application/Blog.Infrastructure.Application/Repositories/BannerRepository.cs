using System;
using Blog.Domain.Application.Entities;
using Blog.Infrastructure.Application.Context;
using Blog.Infrastructure.Application.Interfaces;
using Blog.Infrastructure.Shared.Persistences.Repositories.Common;

namespace Blog.Infrastructure.Application.Repositories;

public class BannerRepository : GenericRepositoryAsync<Banner, Guid>, IBannerRepository
{
    public BannerRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
