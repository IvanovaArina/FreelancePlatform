namespace FreelancePlatformConnect.Api.Models.DTOs;
public record CreateSubscriptionRequest(
    string Name,
    string Description,
    string Price,
    List<string> WhatIncludes
);