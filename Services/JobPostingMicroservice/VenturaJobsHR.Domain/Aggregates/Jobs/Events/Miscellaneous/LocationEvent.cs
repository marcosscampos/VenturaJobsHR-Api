﻿namespace VenturaJobsHR.Domain.Aggregates.Jobs.Events.Miscellaneous;

public class LocationEvent
{
    public string City { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
}
