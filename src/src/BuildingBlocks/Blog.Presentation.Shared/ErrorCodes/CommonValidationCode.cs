using System;
using System.ComponentModel;

namespace Blog.Presentation.Shared.ErrorCodes;

public enum CommonValidationCode
{
    [Description("{0} incorrect format. Please try again!")]
    MSG_001,
    [Description("{0} must be at least {1} characters long.")]
    MSG_002,
    [Description("{0} can be max {1} characters long.")]
    MSG_003,
    [Description("{0} you have entered is incorrect. Please try again!")]
    MSG_006,
    [Description("{0} and {1} do not match. Please try again!")]
    MSG_007,
    [Description("{0} does not exist. Please enter another one.")]
    MSG_009,
    [Description("{0} has already existed. Please enter another one.")]
    MSG_010,
    [Description("Please enter {0}.")]
    MSG_011,
    [Description("Please choose {0}.")]
    MSG_012,
    [Description("Incorrect {0}.")]
    MSG_013,
    [Description("You have exceeded the maximum number of permitted views. If you want to create a new view, please delete one first.")]
    MSG_014,
    [Description("Start date, end date of current user is not larger than start date, end date of all previous user.")]
    MSG_015,
    [Description("Use time of this user overlapped with use time of another user.")]
    MSG_016,
    [Description("End date is not larger than start date.")]
    MSG_017,
    [Description("Setting link is not supported")]
    MSG_018,
    [Description("Actor delete default app")]
    MSG_019,
    [Description("Actor delete the only version of default app")]
    MSG_020,
    [Description("Number must greater than {0}")]
    MSG_021,
    [Description("Date must greater than {0}")]
    MSG_022
}
