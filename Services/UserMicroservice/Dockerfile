FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["VenturaJobsHR.Users/VenturaJobsHR.Users.csproj", "VenturaJobsHR.Users/"]
RUN dotnet restore "VenturaJobsHR.Users/VenturaJobsHR.Users.csproj"
COPY . .
WORKDIR "/src/VenturaJobsHR.Users"
RUN dotnet build "VenturaJobsHR.Users.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VenturaJobsHR.Users.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "VenturaJobsHR.Users.dll"]
CMD ASPNETCORE_URLS=http://*:$PORT dotnet VenturaJobsHR.Users.dll