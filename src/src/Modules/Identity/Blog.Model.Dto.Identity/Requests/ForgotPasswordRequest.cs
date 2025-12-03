using System;
using System.ComponentModel.DataAnnotations;

namespace Blog.Model.Dto.Identity.Requests;

public class ForgotPasswordRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}
