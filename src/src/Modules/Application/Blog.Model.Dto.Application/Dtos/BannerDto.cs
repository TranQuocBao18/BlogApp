using System;

namespace Blog.Model.Dto.Application.Dtos;

public class BannerDto
{
    public Guid? Id { get; set; }
    public required string PublicId { get; set; }
    public required string Url { get; set; }
    public float Width { get; set; }
    public float Height { get; set; }
}
