using System;
using Blog.UnitOfWork;

namespace Blog.Domain.Identity.Interfaces;

public interface IIdentityUnitOfWork : IUnitOfWork
{
    IUserRepository UserRepository { get; }
    IRoleRepository RoleRepository { get; }
}
