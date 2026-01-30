using System;
using Blog.UnitOfWork;

namespace Blog.Domain.Communication.Interfaces;

public interface ICommunicationUnitOfWork : IUnitOfWork
{
    INotificationMessageRepository NotificationMessageRepository { get; }
}
