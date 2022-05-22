FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["VenturaJobsHR.Bff/VenturaJobsHR.Bff.csproj", "VenturaJobsHR.Bff/"]
RUN dotnet restore "VenturaJobsHR.Bff/VenturaJobsHR.Bff.csproj"
COPY . .
WORKDIR "/src/VenturaJobsHR.Bff"
RUN dotnet build "VenturaJobsHR.Bff.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VenturaJobsHR.Bff.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "VenturaJobsHR.Bff.dll"]
CMD ASPNETCORE_URLS=http://*:$PORT dotnet VenturaJobsHR.Bff.dll