﻿using Domain.Shared;

namespace Domain.Events;

public record NotificationFailed
{
    public Guid NotificationId { get; init; }
    public IEnumerable<Error> Errors { get; init; } = Enumerable.Empty<Error>();
}