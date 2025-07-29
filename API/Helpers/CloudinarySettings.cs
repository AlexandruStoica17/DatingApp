using System;

namespace API.Helpers;

public class CloudinarySettings
{
    public required string CloudName { get; set; } //key sensitive
    public required string ApiKey { get; set; }
    public required string ApiSecret { get; set; }
}
