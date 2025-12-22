using System;
using Blog.Domain.Application.Entities;
using Blog.Infrastructure.Application.Interfaces;
using Blog.Infrastructure.Shared.Persistences.Repositories.Common;

namespace Blog.Infrastructure.Application.Repositories;

public class CategoryRepository : GenericRepositoryAsync<Category, Guid>, ICategoryRepository
{

}
