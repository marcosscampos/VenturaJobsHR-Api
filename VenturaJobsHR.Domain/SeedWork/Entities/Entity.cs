﻿using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace VenturaJobsHR.Domain.SeedWork.Entities;

public abstract class Entity
{
    public DateTime CreationDate { get; set; }
    public DateTime? LastUpdate { get; set; }
}
