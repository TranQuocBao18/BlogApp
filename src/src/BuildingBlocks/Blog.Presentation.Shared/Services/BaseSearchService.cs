using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Blog.Domain.Shared.Common;
using Blog.Infrastructure.Shared.Interfaces;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Model.Dto.Shared.Constants;
using Blog.Model.Dto.Shared.Filters;
using Blog.Presentation.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Blog.Presentation.Shared.Services;

public abstract class BaseSearchService<TEntity, TKey, TCriteria, TResponse> : ISearchService<TEntity, TKey, TCriteria, TResponse>
                                                                                            where TCriteria : BaseCriteria
                                                                                            where TEntity : class, IEntity<TKey>
                                                                                            where TResponse : class, new()
{
    private readonly IServiceProvider _serviceProvider;

    protected Func<TEntity, TResponse> convertToModel { get; private set; }

    public BaseSearchService(Func<TEntity, TResponse> convertToModel, IServiceProvider serviceProvider)
    {
        this.convertToModel = convertToModel;
        _serviceProvider = serviceProvider;
    }

    public virtual async Task<BaseSearchResponse<TResponse>> SearchAsync(TCriteria criteria)
    {
        var start = DateTime.UtcNow;
        IEnumerable<TResponse> data = new List<TResponse>();
        var response = BaseSearchResponse<TResponse>.CreateFrom(data, criteria, 0);
        //var tasks = new Task[]{
        //   RetrieveDataAsync(criteria, response),
        //   CountAsync(criteria, response)
        //};
        //await Task.WhenAll(tasks);
        RetrieveDataAsync(criteria, response);
        CountAsync(criteria, response);
        return response;
    }

    protected virtual async Task RetrieveDataAsync(TCriteria criteria, BaseSearchResponse<TResponse> result)
    {
        var serviceScopeFactory = _serviceProvider.GetRequiredService<IServiceScopeFactory>();
        using (var scope = serviceScopeFactory.CreateScope())
        {
            var dbType = GetDbType();
            IGenericRepository<TEntity, TKey> repository = scope.ServiceProvider.GetService(dbType) as IGenericRepository<TEntity, TKey>;
            if (repository == null)
            {
                throw new System.Exception("The repository must be implemented the ISearchRepository");
            }
            IQueryable<TEntity> query = BuildFilter(criteria, repository);

            var idCriteria = JObject.FromObject(criteria).ToObject<TCriteria>();
            idCriteria.Fields = new string[] { "id" };
            IQueryable<TEntity> queryIds = BuildFilter(criteria, repository);
            var ids = await queryIds.AsNoTracking().Select(x => x.Id).Skip(criteria.PageNumber * criteria.PageSize).Take(criteria.PageSize).ToListAsync();
            var listResult = await query.Where(x => ids.Contains(x.Id)).AsNoTracking().ToListAsync();

            if (listResult.Any())
            {
                result.AddRangeData(listResult.Select(convertToModel));
            }
        }
    }

    protected virtual async Task CountAsync(TCriteria criteria, BaseSearchResponse<TResponse> result)
    {
        var serviceScopeFactory = _serviceProvider.GetRequiredService<IServiceScopeFactory>();
        using (var scope = serviceScopeFactory.CreateScope())
        {
            var dbType = GetDbType();
            IGenericRepository<TEntity, TKey> repository = scope.ServiceProvider.GetService(dbType) as IGenericRepository<TEntity, TKey>;
            if (repository == null)
            {
                throw new System.Exception("The repository must be implemented the ISearchRepository");
            }
            IQueryable<TEntity> query = BuildFilter(criteria, repository);
            result.TotalItems = await query.CountAsync();
        }
    }

    protected virtual IQueryable<TEntity> BuildFilter(TCriteria criteria, IGenericRepository<TEntity, TKey> repository)
    {
        // var query = IncludeProperties(repository.AsQueryable());
        var query = repository.AsQueryable().AsNoTracking();

        return BuildFilter(criteria, query);
    }

    protected virtual IQueryable<TEntity> BuildFilter(TCriteria criteria, IQueryable<TEntity> query)
    {
        var filterCompiler = _serviceProvider.GetRequiredService<IFilterCompiler>();

        if (IsValidFilter(criteria.Filter))
        {
            var filterObject = JsonConvert.DeserializeObject(criteria.Filter, JsonConstant.JsonSerializerSetting) as JObject;
            if (filterObject == null)
                throw new ArgumentException($"Filter string is not valid");
            int count = 0;
            var rs = filterCompiler.Compile(filterObject, ref count);
            query = query.Where(rs.Item1, rs.Item2);
        }

        if (!string.IsNullOrEmpty(criteria.Sorts))
        {
            var orders = criteria.Sorts.Split(';').Select(x => x.Replace("=", " "));
            query = query.OrderBy(string.Join(", ", orders));
        }

        if (criteria.Fields != null && criteria.Fields.Any())
        {
            query = query.Select<TEntity>($"new ({string.Join(",", criteria.Fields)})");
        }

        return query;
    }

    protected abstract System.Type GetDbType();

    protected IQueryable<TEntity> IncludeProperties(IQueryable<TEntity> queryable)
    {
        return queryable;
    }

    private bool IsValidFilter(string filter)
    {
        return !string.IsNullOrEmpty(filter) && filter != "{}" && filter != "[]";
    }
}
