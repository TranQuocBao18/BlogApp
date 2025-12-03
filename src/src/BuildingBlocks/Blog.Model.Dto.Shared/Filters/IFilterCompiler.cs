using System;
using Newtonsoft.Json.Linq;

namespace Blog.Model.Dto.Shared.Filters;

public interface IFilterCompiler
{
    Tuple<string, object[], string[]> Compile(JObject filter, ref int count, Action<string[]> callback = null);
}
