using System;
using System.ComponentModel;

namespace Blog.Infrastructure.Shared.ErrorCodes;

public enum ErrorCodeEnum
{
    #region Common
    [Description(@"Success")]
    COM_SUC_000,
    [Description(@"Unknown")]
    COM_ERR_000,
    [Description(@"Fail validation")]
    COM_ERR_001,
    [Description(@"Duplicate")]
    COM_ERR_002,
    [Description(@"Setting Fail")]
    COM_ERR_003,
    #endregion

    #region User
    [Description(@"User is not found.")]
    USE_ERR_001,
    [Description(@"Username is existing.")]
    USE_ERR_002,
    [Description(@"Email is existing.")]
    USE_ERR_003,
    [Description(@"User is locked.")]
    USE_ERR_004,
    [Description(@"User is deleted.")]
    USE_ERR_005,
    [Description(@"User is wrong password.")]
    USE_ERR_006,
    [Description(@"User is duplicate email.")]
    USE_ERR_007,
    [Description(@"User is duplicate username.")]
    USE_ERR_008,
    [Description(@"User is duplicate phonenumber.")]
    USE_ERR_009,
    [Description(@"Username is null or empty.")]
    USE_ERR_010,
    [Description(@"Username is in use.")]
    USE_ERR_011,
    [Description(@"User is exsiting.")]
    USE_ERR_012,
    #endregion

    #region Role Group
    [Description(@"Group is not found.")]
    ROG_ERR_001,
    [Description(@"Group is existing.")]
    ROG_ERR_002,
    [Description(@"Create Group is Fail.")]
    ROG_ERR_003,
    [Description(@"Update Group is Fail.")]
    ROG_ERR_004,
    [Description(@"Delete Group is Fail.")]
    ROG_ERR_005,
    [Description(@"Group is duplicate code.")]
    ROG_ERR_006,
    [Description(@"Group has users.")]
    ROG_ERR_007,
    #endregion

    #region Notification
    [Description(@"Notification Message is not found.")]
    NOTI_ERR_001,
    [Description(@"Notification Message is existing.")]
    NOTI_ERR_002,
    [Description(@"Create Notification Message is Fail.")]
    NOTI_ERR_003,
    [Description(@"Update Notification Message is Fail.")]
    NOTI_ERR_004,
    [Description(@"Delete Notification Message is Fail.")]
    NOTI_ERR_005,
    #endregion

    #region Banner
    [Description(@"Banner is not found.")]
    BAN_ERR_001,
    [Description(@"Banner is existing.")]
    BAN_ERR_002,
    [Description(@"Create Banner is Fail.")]
    BAN_ERR_003,
    [Description(@"Update Banner is Fail.")]
    BAN_ERR_004,
    [Description(@"Delete Banner is Fail.")]
    BAN_ERR_005,
    [Description(@"Banner width or height is duplicated.")]
    BAN_ERR_006,
    #endregion

    #region Blog
    [Description(@"Blog is not found.")]
    BLOG_ERR_001,
    [Description(@"Blog is existing.")]
    BLOG_ERR_002,
    [Description(@"Create Blog is Fail.")]
    BLOG_ERR_003,
    [Description(@"Update Blog is Fail.")]
    BLOG_ERR_004,
    [Description(@"Delete Blog is Fail.")]
    BLOG_ERR_005,
    [Description(@"Blog width or height is duplicated.")]
    BLOG_ERR_006,
    #endregion

    #region Categories
    #endregion

}
