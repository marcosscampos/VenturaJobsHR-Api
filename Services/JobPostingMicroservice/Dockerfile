FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["VenturaJobsHR.Api/VenturaJobsHR.Api.csproj", "VenturaJobsHR.Api/"]
RUN dotnet restore "VenturaJobsHR.Api/VenturaJobsHR.Api.csproj"
COPY . .
WORKDIR "/src/VenturaJobsHR.Api"
RUN dotnet build "VenturaJobsHR.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VenturaJobsHR.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "VenturaJobsHR.ApiApi.dll"]
CMD ASPNETCORE_URLS=http://*:$PORT dotnet VenturaJobsHR.Api.dll