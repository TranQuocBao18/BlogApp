using System;

namespace Blog.Shared;

public static class Const
{

}

public static class MaxLengthConst
{
    public const int ForTextField = 128;
    public const int ForEmail = 256;
    public const int ForUrlField = 1500;
    public const int MemberFullNameMaxLength = 501;
    public const int MemberEmailMaxLength = 250;
    public const int MemberPhoneMaxLength = 250;
}

public static class ServiceNameConst
{
    public const string IDENTITY = "identity";
    public const string APPLICATION = "application";
}
