﻿//Ventura classes
global using VenturaJobsHR.Users.Domain.Abstractions.Repositories;
global using VenturaJobsHR.Users.Application.Services.Interface;
global using VenturaJobsHR.Users.Application.Services.Concrete;
global using VenturaJobsHR.Users.Repository.DatabaseSettings;
global using VenturaJobsHR.Users.Application.Records.User;
global using VenturaJobsHR.Users.Domain.Seedwork.Settings;
global using VenturaJobsHR.Users.Repository.Persistence;
global using VenturaJobsHR.Users.Repository.Mappings;
global using VenturaJobsHR.Users.Seedwork.Extensions;
global using VenturaJobsHR.Users.Repository.Context;
global using VenturaJobsHR.Users.Common.Middleware;
global using VenturaJobsHR.Users.Common.Exceptions;
global using VenturaJobsHR.Users.Seedwork.Swagger;
global using VenturaJobsHR.Users.Common.Response;
global using VenturaJobsHR.Users.Common.Mappings;
global using VenturaJobsHR.Users.Domain.Models;
global using VenturaJobsHR.Users.Repository;
global using VenturaJobsHR.Users.Common;


//Libs
global using JsonSerializer = System.Text.Json.JsonSerializer;
global using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using VenturaJobsHR.Users.Seedwork.Swagger.Filters;
global using Microsoft.AspNetCore.ResponseCompression;
global using Microsoft.AspNetCore.Authentication;
global using Microsoft.AspNetCore.Http.Features;
global using Microsoft.AspNetCore.HttpOverrides;
global using Swashbuckle.AspNetCore.SwaggerGen;
global using Swashbuckle.AspNetCore.Filters;
global using Microsoft.OpenApi.Models;
global using Microsoft.AspNetCore.Mvc;
global using System.IO.Compression;
global using System.Reflection;
global using System.Text.Json;
global using Newtonsoft.Json;
global using System.Net;