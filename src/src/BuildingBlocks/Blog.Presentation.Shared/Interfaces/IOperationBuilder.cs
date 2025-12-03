using System;
using Blog.Model.Dto.Shared.Filters;

namespace Blog.Presentation.Shared.Interfaces;

public delegate Tuple<string, object[], string[]> OperationBuilder(FilterModel filter, Action<string[]> callback = null);
public interface IOperationBuilder
{
    Tuple<string, object[], string[]> Build(FilterModel filter, Action<string[]> callback = null);
}
