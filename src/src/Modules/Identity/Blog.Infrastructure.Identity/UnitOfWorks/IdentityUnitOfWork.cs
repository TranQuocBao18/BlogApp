using System;
using Blog.Infrastructure.Identity.Contexts;
using Blog.Domain.Identity.Interfaces;
using Blog.UnitOfWork;

namespace Blog.Infrastructure.Identity.UnitOfWorks;

public class IdentityUnitOfWork : BaseUnitOfWork, IIdentityUnitOfWork
{
    public IUserRepository UserRepository { get; private set; }
    public IRoleRepository RoleRepository { get; private set; }

    public IdentityUnitOfWork(IdentityContext context, IUserRepository userRepository, IRoleRepository roleRepository) : base(context)
    {
        UserRepository = userRepository;
        RoleRepository = roleRepository;
    }
}
