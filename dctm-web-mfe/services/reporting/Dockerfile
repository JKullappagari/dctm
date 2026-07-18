FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /source
COPY src/Reporting.Api/Reporting.Api.csproj src/Reporting.Api/
RUN dotnet restore src/Reporting.Api
COPY src/ src/
RUN dotnet publish src/Reporting.Api -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
USER app
HEALTHCHECK --interval=30s --timeout=5s --start-period=15s \
  CMD ["bash","-c","exec 3<>/dev/tcp/127.0.0.1/8080 && printf 'GET /healthz HTTP/1.0\r\n\r\n' >&3 && grep -q ' 200 ' <&3"]
ENTRYPOINT ["dotnet", "Reporting.Api.dll"]
