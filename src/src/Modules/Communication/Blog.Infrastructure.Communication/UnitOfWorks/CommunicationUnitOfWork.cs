using System;
using Blog.Infrastructure.Communication.Contexts;
using Blog.Domain.Communication.Interfaces;
using Blog.UnitOfWork;

namespace Blog.Infrastructure.Communication.UnitOfWorks;

public class CommunicationUnitOfWork : BaseUnitOfWork, ICommunicationUnitOfWork
{
    public INotificationMessageRepository NotificationMessageRepository { get; private set; }

    public CommunicationUnitOfWork(CommunicationDbContext context, INotificationMessageRepository notificationMessageRepository) : base(context)
    {
        NotificationMessageRepository = notificationMessageRepository;
    }
}
