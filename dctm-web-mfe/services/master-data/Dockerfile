# Master Data Service — multi-stage build, runs on Linux
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /source
COPY src/MasterData.Api/MasterData.Api.csproj src/MasterData.Api/
RUN dotnet restore src/MasterData.Api
COPY src/ src/
RUN dotnet publish src/MasterData.Api -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
USER app
# aspnet images ship no curl/wget; use bash's /dev/tcp to probe /healthz
HEALTHCHECK --interval=30s --timeout=5s --start-period=15s \
  CMD ["bash", "-c", "exec 3<>/dev/tcp/127.0.0.1/8080 && printf 'GET /healthz HTTP/1.0\\r\\n\\r\\n' >&3 && grep -q ' 200 ' <&3"]
ENTRYPOINT ["dotnet", "MasterData.Api.dll"]
