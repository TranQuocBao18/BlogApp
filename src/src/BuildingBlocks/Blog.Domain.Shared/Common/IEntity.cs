using System;
using System.ComponentModel.DataAnnotations;

namespace Blog.Domain.Shared.Common;

public interface IEntity<T>
{
    [Key]
    T Id { get; set; }
}
