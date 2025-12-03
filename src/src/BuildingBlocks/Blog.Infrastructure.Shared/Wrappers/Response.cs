using System;
using System.ComponentModel;
using Blog.Infrastructure.Shared.ErrorCodes;
using Blog.Utilities;

namespace Blog.Infrastructure.Shared.Wrappers;

public class Response<T>
{
    public Response() { }

        public Response(T data, string message = null)
        {
            Succeeded = true;
            Message = message;
            Data = data;
        }

        public Response(string errorCode, string message)
        {
            Succeeded = false;
            Message = message;
            ErrorCode = string.IsNullOrWhiteSpace(errorCode) ? ErrorCodeEnum.COM_ERR_000.ToString() : errorCode;
        }

        public Response(ErrorCodeEnum errorCode, string v)
        {
            Succeeded = false;
            Message = errorCode.GetDescription();
            ErrorCode = errorCode.ToString();
        }

        public Response(ErrorCodeEnum errorCode, params object[] args)
        {
            Succeeded = false;
            Message = GetDescription(errorCode, args);
            ErrorCode = GetDescription(errorCode, args);
        }

        public static string GetDescription(ErrorCodeEnum errorCode, params object[] args)
        {
            var type = errorCode.GetType();
            var memInfo = type.GetMember(errorCode.ToString());
            if (memInfo.Length > 0)
            {
                var attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs.Length > 0)
                {
                    var description = ((DescriptionAttribute)attrs[0]).Description;
                    return string.Format(description, args);
                }
            }
            return errorCode.ToString();
        }

        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public string ErrorCode { get; set; } = null;
        public List<string> Errors { get; set; }
        public T Data { get; set; }
}
