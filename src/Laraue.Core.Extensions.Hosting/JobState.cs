﻿namespace Laraue.Core.Extensions.Hosting;

/// <summary>
/// Job state entity.
/// </summary>
public record JobState
{
    /// <summary>
    /// Unique job name.
    /// </summary>
    public required string JobName { get; init; }

    /// <summary>
    /// Last time when the job was executed.
    /// </summary>
    public System.DateTime? LastExecutionAt { get; init; }
    
    /// <summary>
    /// The next time when the job will be executed.
    /// </summary>
    public System.DateTime? NextExecutionAt { get; init; }
}