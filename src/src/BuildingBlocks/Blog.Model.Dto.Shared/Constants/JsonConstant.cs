using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Blog.Model.Dto.Shared.Constants;

public static class JsonConstant
{
    public static string DefaultDateTimeFormat = "yyyy-MM-ddTHH:mm:ss:ffff";

        static JsonSerializerSettings jss = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Error,
            DateFormatString = DefaultDateTimeFormat,
            DateParseHandling = DateParseHandling.None,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
        public static JsonSerializerSettings JsonSerializerSetting
        {
            get
            {
                return jss;
            }
        }
        static JsonSerializer js = new JsonSerializer
        {
            NullValueHandling = NullValueHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Error,
            DateFormatString = DefaultDateTimeFormat,
            DateParseHandling = DateParseHandling.None,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
        public static JsonSerializer JsonSerializer
        {
            get
            {
                return js;
            }
        }
}
