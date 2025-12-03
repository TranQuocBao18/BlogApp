using System;
using Blog.Domain.Shared.Common;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Model.Dto.Shared.Filters;

namespace Blog.Presentation.Shared.Interfaces;

public interface ISearchService<TEntity, TKey, TCriteria, TResponse> where TCriteria : BaseCriteria
                                                                     where TEntity : class, IEntity<TKey>
                                                                     where TResponse : class, new()
{
    Task<BaseSearchResponse<TResponse>> SearchAsync(TCriteria criteria);
}
