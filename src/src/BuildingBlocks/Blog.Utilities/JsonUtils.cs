using System;
using System.Text.Encodings.Web;
using System.Text.Json;
using Newtonsoft.Json;

namespace Blog.Utilities;

public static class JsonUtils
{
    public static string ToStringJson(this JsonDocument jsonDocument)
        {
            if (jsonDocument == null)
            {
                return null;
            }

            return jsonDocument.RootElement.ToString();
        }

        public static bool IsValidJsonString<T>(this string value)
        {
            try
            {
                var jsonObject = JsonConvert.DeserializeObject<T>(value);
                return jsonObject != null;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsInvalidJsonString<T>(this string value)
        {
            return !IsValidJsonString<T>(value);
        }

        public static string Serialize(object item)
        {
            if (item == null)
            {
                return null;
            }

            return System.Text.Json.JsonSerializer.Serialize(item, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            });
        }

        public static string Serialize(object item, JsonSerializerOptions jsonSerializerOptions)
        {
            if (item == null)
            {
                return null;
            }

            return System.Text.Json.JsonSerializer.Serialize(item, jsonSerializerOptions);
        }

        public static string PrettySerialize(object item, bool ignoreEncoding = false)
        {
            if (item == null)
            {
                return null;
            }

            if (ignoreEncoding)
            {
                return System.Text.Json.JsonSerializer.Serialize(item,
                new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    IgnoreNullValues = true,
                    WriteIndented = true
                });
            }
            return System.Text.Json.JsonSerializer.Serialize(item, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });
        }

        public static T Deserialize<T>(string json)
        {
            Preconditions.CheckNotNull(json);
            return System.Text.Json.JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            });
        }
}
