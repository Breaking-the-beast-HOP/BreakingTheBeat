using System;
using System.Collections.Generic;

namespace Gateway.Logging.Entities;

public partial class GatewayLog
{
    public int Id { get; set; }

    public string RequestId { get; set; } = null!;

    public DateTime Timestamp { get; set; }

    public string Message { get; set; } = null!;

    public string? RequestName { get; set; }

    public string? Path { get; set; }

    public string? Method { get; set; }

    public string? RequestData { get; set; }

    public string? LogLevel { get; set; }

    public string? Exception { get; set; }
}
