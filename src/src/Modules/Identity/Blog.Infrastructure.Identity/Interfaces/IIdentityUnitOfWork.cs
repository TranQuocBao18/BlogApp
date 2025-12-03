using System;
using Blog.UnitOfWork;

namespace Blog.Infrastructure.Identity.Interfaces;

public interface IIdentityUnitOfWork : IUnitOfWork
{
    IUserRepository UserRepository { get; }
    IRoleRepository RoleRepository { get; }
}
