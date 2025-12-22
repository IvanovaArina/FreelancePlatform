namespace FreelancePlatformConnect.Api.Models.DTOs;

public record SubscriptionDto(
    int Id,
    string Name,
    string Description,
    string Price,
    bool IsActive,
    List<string> WhatIncludes,
    DateTime CreatedAt
);