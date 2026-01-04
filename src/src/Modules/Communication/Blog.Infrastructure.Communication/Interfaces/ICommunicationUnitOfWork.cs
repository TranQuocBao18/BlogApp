using System;
using Blog.UnitOfWork;

namespace Blog.Infrastructure.Communication.Interfaces;

public interface ICommunicationUnitOfWork : IUnitOfWork
{
    INotificationMessageRepository NotificationMessageRepository { get; }
}
