namespace FreelancePlatformConnect.Api.Models.DTOs;
public record UpdateSubscriptionRequest(
    string Name,
    string Description,
    string Price,
    bool IsActive,
    List<string> WhatIncludes
);