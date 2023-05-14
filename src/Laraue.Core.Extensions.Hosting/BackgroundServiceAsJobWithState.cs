﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Laraue.Core.DateTime.Services.Abstractions;
using Microsoft.Extensions.Logging;

namespace Laraue.Core.Extensions.Hosting;

/// <summary>
/// Service that should execute some work and fall asleep for the some time and store the current state somewhere.
/// </summary>
public abstract class BackgroundServiceAsJobWithState : BackgroundServiceAsJob
{
    /// <summary>
    /// Unique job identifier.
    /// </summary>
    protected readonly string JobName;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<BackgroundServiceAsJobWithState> _logger;

    /// <summary>
    /// Current Job state.
    /// </summary>
    protected JobState? JobState { get; private set; }

    /// <inheritdoc />
    protected BackgroundServiceAsJobWithState(
        string jobName,
        IServiceProvider serviceProvider,
        IDateTimeProvider dateTimeProvider,
        ILogger<BackgroundServiceAsJobWithState> logger)
        : base(serviceProvider, logger)
    {
        JobName = jobName;
        _dateTimeProvider = dateTimeProvider;
        _logger = logger;
    }

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        OpenScope();
        
        JobState = await GetJobStateAsync(stoppingToken).ConfigureAwait(false);
        
        CloseScope();

        if (JobState?.NextExecutionAt is not null)
        {
            _logger.LogDebug("Waiting for the next execution at {ExecutionTime}", JobState.NextExecutionAt);
            
            var timeToWait = JobState.NextExecutionAt.Value - _dateTimeProvider.UtcNow;

            if (timeToWait > TimeSpan.Zero)
            {
                await Task.Delay(timeToWait, stoppingToken).ConfigureAwait(false);
            }
        }
        
        await base.ExecuteAsync(stoppingToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    protected override async Task<TimeSpan> ExecuteOnceAsync(CancellationToken stoppingToken)
    {
        OpenScope();

        var timeToWait = await ExecuteJobAsync(stoppingToken);
        var now = _dateTimeProvider.UtcNow;
        
        JobState = new JobState
        {
            JobName = JobName,
            NextExecutionAt = now + timeToWait,
            LastExecutionAt = now,
        };

        await SaveJobStateAsync(JobState, stoppingToken);
        
        CloseScope();

        return timeToWait;
    }

    /// <summary>
    /// Get the job state from the storage.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected abstract Task<JobState?> GetJobStateAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Sets the job state to the storage.
    /// </summary>
    /// <param name="state"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected abstract Task SaveJobStateAsync(JobState state, CancellationToken cancellationToken = default);
}